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
            webBrowser1.Navigate("testpage.html"); //geen idee of dit het doet

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flySec = 0;
            int.TryParse(textBox1.Text, out flySec);

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

        private void btnForward_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void btnBackward_Click(object sender, EventArgs e)
        {

        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {

        }

        private void btnHoger_Click(object sender, EventArgs e)
        {

        }

        private void btnLager_Click(object sender, EventArgs e)
        {

        }

        private void btnLand_Click(object sender, EventArgs e)
        {

        }
    }
}
