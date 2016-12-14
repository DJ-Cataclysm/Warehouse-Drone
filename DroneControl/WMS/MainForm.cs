using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WMS
{
    public partial class MainForm : Form
    {
        List<Mutation> pendingMutations;
        MutationForm mutationForm;

        public MainForm()
        {
            InitializeComponent();
            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.AllowUserToAddRows = true;
            refreshDataGridView();
            pendingMutations = new List<Mutation>();
        }

        private void btnExampleMutation_Click(object sender, EventArgs e)
        {
            prepareMutations();
            showMutations();
        }

        public void prepareMutations()
        {
            //Should be called when doing a full cycle scan or smart scan before drone scans anything.
            //Creates empty mutations with the proper ID's and current stock.
            pendingMutations.Clear();
            using (ProductDBContext db = new ProductDBContext())
            {
                List<Product> products = db.Products.ToList();
                foreach(Product p in products)
                {
                    pendingMutations.Add(new Mutation(p));
                }
            }
        }

        private void filterMutations()
        {
            //Used for filtering the list in such a way that only records remain where the OldCount and NewCount do not match.
            //Should be used after finishing the full cycle count.
            pendingMutations.RemoveAll(m => m.NewCount == m.OldCount);
        }

        public void showMutations()
        {
            filterMutations();
            mutationForm = new MutationForm(pendingMutations);
            mutationForm.ShowDialog();
            refreshDataGridView();
        }

        public void productScanned(int id)
        {
            Mutation mutation = pendingMutations.Find(m => m.ID == id);
            if(mutation != null)
            {
                mutation.NewCount++;
            }
        }

        private void tsbAddProduct_Click(object sender, EventArgs e)
        {
            //Show the new product form as a dialog.
            NewProductForm npf = new NewProductForm();
            npf.ShowDialog();
            refreshDataGridView();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            //Since the delete function deletes the selected item, an item must first be selected before it can be used.
            if(dgvProducts.SelectedRows.Count > 0)
            {
                //Note: Once a row has been selected it is impossible to not have a selected row. That's why there's no disable case.
                tsbDeleteProduct.Enabled = true;
            }
        }

        private void dgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Get selected, edited product
            Product editedRecord = (Product)dgvProducts.SelectedRows[0].DataBoundItem; //Get data bound Product object to that row
 
            //Edit and save product
            using (ProductDBContext db = new ProductDBContext())
            {
                var existingRecord = db.Products.Find(editedRecord.ID);
                if(existingRecord == null)
                {
                    return;
                }
                db.Entry(existingRecord).CurrentValues.SetValues(editedRecord);
                db.SaveChanges();
            }
        }

        private void tsbDeleteProduct_Click(object sender, EventArgs e)
        {
            //Gather selected product.
            Product product = (Product)dgvProducts.SelectedRows[0].DataBoundItem;

            //Ask for confirmation.
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove " + product.Title +" (ID: " + product.ID + ")? \nThis operation cannot be undone.", 
                "Remove product", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //Remove product from db.
                using (ProductDBContext db = new ProductDBContext())
                {
                    db.Products.Attach(product);
                    db.Products.Remove(product);
                    db.SaveChanges();
                    if(db.Products.Count() == 0)
                    {
                        tsbDeleteProduct.Enabled = false;
                    }
                }
                //Refresh.
                refreshDataGridView();
            }
        }

        private void refreshDataGridView()
        {
            //Reset and set datasource, should be called each time the table changes.
            dgvProducts.DataSource = typeof(Product);
            using (ProductDBContext db = new ProductDBContext())
            {
                dgvProducts.DataSource = db.Products.ToList();
            }
            dgvProducts.Columns[0].ReadOnly = true; //You may not modify the primary key cell.
        }

        private void dgvProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //Make sure a row has been selected first.
            if(dgvProducts.SelectedRows.Count > 0)
            {
                //Find the edited column, trigger different behavior, depending on the column being edited.
                //Note: Title and description is a string and can therefore not really be invalid.
                switch (dgvProducts.Columns[e.ColumnIndex].Name)
                {
                    case "LastCheck":
                        //Must be a valid DateTime string
                        DateTime tempDate;
                        DateTime minimumDate = new DateTime(1753, 1, 1); //SQLServer DateTime1 has this as minimum value.
                        if(!DateTime.TryParse((string) e.FormattedValue, out tempDate) || tempDate < minimumDate)
                        {
                            //DateTime string is invalid.
                            MessageBox.Show("The value \"" + (string)e.FormattedValue + "\" is not valid, please input a valid date. Reverting input.");
                            dgvProducts.CancelEdit();
                        }
                        else if(DateTime.Now < tempDate)
                        {
                            //DateTime string is invalid because it is in the future.
                            MessageBox.Show("The value \"" + (string)e.FormattedValue + "\" is in the future, please input a valid date. Reverting input.");
                            dgvProducts.CancelEdit();
                        }
                        break;
                    case "Count":
                        //Must be an integer bigger or equal to 0.
                        int tempInt;
                        if (!int.TryParse((string)e.FormattedValue, out tempInt) || tempInt < 0)
                        {
                            //Count is invalid, it is either not an int or it is <0.
                            MessageBox.Show("The value \"" + (string)e.FormattedValue + "\" is not a valid count number. Reverting input.");
                            dgvProducts.CancelEdit();
                        }
                        break;
                }
            }
        }
    }
}