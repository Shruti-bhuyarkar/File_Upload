using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentUpload 
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenUploadForm("xml");
        }

        private void csv_Click(object sender, EventArgs e)
        {
            OpenUploadForm("csv");
        }

        private void json_Click(object sender, EventArgs e)
        {
            OpenUploadForm("json");
        }
        private void OpenUploadForm(string fileType)
        {
            UploadForm uploadForm = new UploadForm(fileType);
            uploadForm.ShowDialog();
        }
    }
}
