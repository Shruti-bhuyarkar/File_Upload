using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentUpload
{
    public partial class UploadForm : Form
    {
        private string selectedExtension;
        private List<string> fileNames;
        public UploadForm(string extension)
        {
            InitializeComponent();
            selectedExtension = extension;
            fileNames = new List<string>();
        }

       

        private void UploadForm_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }
        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).Equals(selectedExtension, StringComparison.InvariantCultureIgnoreCase))
                {
                    SaveFileToDatabase(file);
                }
                else
                {
                    MessageBox.Show($"Only {selectedExtension} files are allowed.", "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"{selectedExtension} files|*{selectedExtension}|All files|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFileToDatabase(openFileDialog.FileName);
            }
        }
        private void SaveFileToDatabase(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string fileType = Path.GetFileName(filePath);

            
            string connectionString = "data source = DBSRV1\\TSSIPL2016; database = OFFICE_TRAINING; User ID = sa; password = sql@2016";
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO FileTable (FileName, FileType, FileData) VALUES (@FileName,@FileType, @FileData)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@FileData", fileData);
                    command.Parameters.AddWithValue("@FileType", fileType);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("File uploaded successfully.", "Upload Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadFileNames()
        {
            fileNames.Clear();

           
            string connectionString = "data source = DBSRV1\\TSSIPL2016; database = OFFICE_TRAINING; User ID = sa; password = sql@2016";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT FileName FROM Files";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fileNames.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int yOffset = 10;
            foreach (string fileName in fileNames)
            {
                g.DrawString(fileName, this.Font, Brushes.Black, new PointF(10, yOffset));
                yOffset += 20;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
