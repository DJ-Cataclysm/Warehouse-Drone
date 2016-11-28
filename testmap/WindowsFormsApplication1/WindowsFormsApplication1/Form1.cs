using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.Navigate("C:/Users/carroto/Documents/Visual Studio 2013/Projects/WindowsFormsApplication1/WindowsFormsApplication1/testpage.html");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flySec = Convert.ToInt32(textBox1.Text);

            object[] o = new object[1];
            o[0] = flySec;

            webBrowser1.Document.InvokeScript("fly", o);

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = new WebBrowser();
            wb.AllowNavigation = true;

            wb.Navigate("testpage.html");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
