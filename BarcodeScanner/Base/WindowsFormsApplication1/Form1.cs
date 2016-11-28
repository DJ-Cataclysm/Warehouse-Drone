using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            var barcodeBitmap = (Bitmap)Bitmap.FromFile("barcode2.png");
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result != null)
            {
                code.Text = result.Text;
            }
            barcodeBitmap = (Bitmap)Bitmap.FromFile("barcode.png");
            // detect and decode the barcode inside the bitmap
            result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result != null)
            {
                code2.Text = result.Text;
            }
            barcodeBitmap = (Bitmap)Bitmap.FromFile("EAN-13.png");
            // detect and decode the barcode inside the bitmap
            result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result != null)
            {
                code3.Text = result.Text;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }
    }
}
