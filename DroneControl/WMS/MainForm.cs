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
        static DateTime MINIMUMDATE = new DateTime(1753, 1, 1); //SQLServer DateTime1 has this as minimum value

        public MainForm()
        {
            InitializeComponent();
            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.AllowUserToAddRows = true;
            refreshDataGridView();
            pendingMutations = new List<Mutation>();
        }

        private void prepareMutations()
        {
            //Should be called when doing a full cycle scan or smart scan before drone scans anything
            //Creates empty mutations with the proper ID's and current stock
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
            //Used for filtering the list in such a way that only records remain where the OldCount and NewCount do not match
            //Should be used after finishing the full cycle count
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
            if(pendingMutations.Count == 0)
            {
                prepareMutations();
            }
            Mutation mutation = pendingMutations.Find(m => m.ID == id);
            if(mutation != null)
            {
                mutation.NewCount++;
            }
        }

        private void tsbAddProduct_Click(object sender, EventArgs e)
        {
            //Show the new product form as a dialog
            NewProductForm npf = new NewProductForm();
            npf.ShowDialog();
            refreshDataGridView();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            //Since the delete function deletes the selected item, an item must first be selected before it can be used
            if(dgvProducts.SelectedRows.Count > 0)
            {
                //Note: Once a row has been selected it is impossible to not have a selected row. That's why there's no disable case
                tsbDeleteProduct.Enabled = true;
            }
        }

        private void dgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Get selected, edited product
            if(dgvProducts.SelectedRows.Count != 0)
            {
                Product editedRecord = (Product)dgvProducts.SelectedRows[0].DataBoundItem; //Get data bound Product object to that row

                //Edit and save product
                using (ProductDBContext db = new ProductDBContext())
                {
                    var existingRecord = db.Products.Find(editedRecord.ID);
                    if (existingRecord == null)
                    {
                        return;
                    }
                    db.Entry(existingRecord).CurrentValues.SetValues(editedRecord);
                    db.SaveChanges();
                }
            }
        }

        private void tsbDeleteProduct_Click(object sender, EventArgs e)
        {
            //Make sure the user is not editing the selected product
            dgvProducts.CancelEdit();

            //Gather selected product
            Product product = (Product)dgvProducts.SelectedRows[0].DataBoundItem;

            //Ask for confirmation
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove " + product.Title +
                " (ID: " + product.ID + ")? \nThis operation cannot be undone.", 
                "Remove product", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //Remove product from db
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
                //Refresh
                refreshDataGridView();
            }
        }

        private void refreshDataGridView()
        {
            //Reset and set datasource, should be called each time the table changes
            dgvProducts.DataSource = typeof(Product);
            using (ProductDBContext db = new ProductDBContext())
            {
                dgvProducts.DataSource = db.Products.ToList();
            }
            dgvProducts.Columns[0].ReadOnly = true; //You may not modify the primary key cell
        }

        private void dgvProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //Make sure a row has been selected first.
            if (dgvProducts.SelectedRows.Count > 0)
            {
                //Find the edited column, trigger different behavior, depending on the column being edited
                //Note: Title and description is a string and can therefore not really be invalid
                string name = dgvProducts.Columns[e.ColumnIndex].Name;
                string input = (string)e.FormattedValue;
                if (name == "LastCheck")
                {
                    validateDate(input);
                }
                else if(name == "Count")
                {
                    validateInt(input, true);
                }  
                else if(name == "X" || name == "X" || name == "Z" || name == "Deviation")
                {
                    validateInt(input, false);
                }       
            }  
        }

        private void validateInt(string input, bool mustBeAboveZero)
        {
            int tempInt;
            if (!int.TryParse(input, out tempInt) || (mustBeAboveZero && tempInt < 0))
            {
                //Value is invalid, it is either not an int or it is <0
                MessageBox.Show("The value \"" + input +
                    "\" is not a valid number. Reverting input.");
                dgvProducts.CancelEdit();
            }
        }

        private void validateDate(string input)
        {
            //Must be a valid DateTime string
            DateTime tempDate;

            if (!DateTime.TryParse(input, out tempDate) || tempDate < MINIMUMDATE)
            {
                //DateTime string is invalid
                MessageBox.Show("The value \"" + input +
                    "\" is not valid, please input a valid date. Reverting input.");
                dgvProducts.CancelEdit();
            }
            else if (DateTime.Now < tempDate)
            {
                //DateTime string is invalid because it is in the future
                MessageBox.Show("The value \"" + input +
                    "\" is in the future, please input a valid date. Reverting input.");
                dgvProducts.CancelEdit();
            }
        }
    }
}