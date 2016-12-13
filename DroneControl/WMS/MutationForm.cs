using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //You may only modify the NewCount column.
            dgvProducts.Columns[0].ReadOnly = true;
            dgvProducts.Columns[1].ReadOnly = true;
            dgvProducts.Columns[3].ReadOnly = true;
        }

        private void dgvProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //Make sure a row has been selected first.
            if (dgvProducts.SelectedRows.Count > 0)
            {
                if(dgvProducts.Columns[e.ColumnIndex].Name == "NewCount")
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
    }
}
