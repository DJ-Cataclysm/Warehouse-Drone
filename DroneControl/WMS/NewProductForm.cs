using System;
using System.Windows.Forms;

namespace WMS
{
    public partial class NewProductForm : Form
    {
        public NewProductForm()
        {
            InitializeComponent();
            dtpLastChecked.MaxDate = DateTime.Now; //Makes no sense to have checked in the future.
        }

        private void btnCreateNewProduct_Click(object sender, EventArgs e)
        {
            //Create new product.
            var product = new Product
            {
                Title = tbTitle.Text,
                Count = (int)nudCount.Value,
                Description = tbDescription.Text,
                LastCheck = dtpLastChecked.Value
            };

            //Add the product to the DB.
            using (ProductDBContext db = new ProductDBContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
            
            //Release resources used by this component.
            Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Release resources used by this component.
            Dispose();
        }
    }
}