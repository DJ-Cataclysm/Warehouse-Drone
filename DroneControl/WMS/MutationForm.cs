using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WMS
{
    public partial class MutationForm : Form
    {
        List<Mutation> pendingMutations;

        public MutationForm(List<Mutation> pendingMutations)
        {
            this.pendingMutations = pendingMutations;
            InitializeComponent();
            refreshDataGridView();
        }

        private void refreshDataGridView()
        {
            //Reset and set datasource, should be called each time the table changes.
            dgvProducts.DataSource = typeof(Mutation);
            dgvProducts.DataSource = pendingMutations;

            //You may only modify the NewCount (2nd) column.
            dgvProducts.Columns[0].ReadOnly = true;
            dgvProducts.Columns[1].ReadOnly = true;
            dgvProducts.Columns[3].ReadOnly = true;
        }

        private void dgvProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //Make sure a row has been selected first.
            if (dgvProducts.SelectedRows.Count > 0)
            {
                if (dgvProducts.Columns[e.ColumnIndex].Name == "NewCount")
                {
                    int tempInt;
                    if (!int.TryParse((string)e.FormattedValue, out tempInt) || tempInt < 0)
                    {
                        //Count is invalid, it is either not an int or it is <0.
                        MessageBox.Show("The value \"" + (string)e.FormattedValue + "\" is not a valid count number. Reverting input.");
                        dgvProducts.CancelEdit();
                    }
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //Confirmation prompt
            DialogResult result = MessageBox.Show("Are you sure you wish to accept and save these mutations?", "Are you sure?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //Remove mutations with no changes.
                pendingMutations.RemoveAll(m => m.NewCount == m.OldCount);

                using (var db = new ProductDBContext())
                {
                    //Store mutations in databse
                    db.Mutations.AddRange(pendingMutations);

                    foreach (Mutation m in pendingMutations)
                    {
                        //Update the product table with new count
                        var existingRecord = db.Products.Find(m.ID);
                        if (existingRecord != null)
                        {
                            existingRecord.Count = m.NewCount;
                        }
                    }

                    db.SaveChanges();
                }

                Close();
            }

        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            pendingMutations.Clear();
            base.OnClosed(e);
        }
    }
}
