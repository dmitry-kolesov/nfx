using System;
using System.Diagnostics;
using System.Windows.Forms;
using NFX.Media.PDF;

namespace PdfTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pdf = new PdfDocument();
            pdf.Draw(0, 0, "hell0 world", 20);
            pdf.Draw(100, 400, "testing....", 14);
            pdf.Draw(300, 400, "loc?", 14);
            pdf.DrawLine(40, 40, 140, 140);
            pdf.DrawLine(300, 300, 300, 500);
            pdf.Save("tmp2.pdf");
            Process.Start("tmp2.pdf");
        }
    }
}