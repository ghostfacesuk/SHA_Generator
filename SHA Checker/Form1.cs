using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SHA_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select a directory to scan files.";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string directoryPath = folderBrowser.SelectedPath;

                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        saveFileDialog.FileName = "file_hashes.txt";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string outputFile = saveFileDialog.FileName;
                            CalculateAndSaveHashes(directoryPath, outputFile);
                            label1.Text = "File hashes saved to " + outputFile;
                        }
                    }
                }
            }
        }

        private void CalculateAndSaveHashes(string directoryPath, string outputFile)
        {
            var fileInfos = new System.Collections.Generic.List<string>();

            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                if (File.Exists(filePath))
                {
                    string sha256Hash = CalculateSHA256(filePath);
                    FileInfo fileInfo = new FileInfo(filePath);

                    string formattedFileInfo = $"Name: {Path.GetFileName(filePath)}\n" +
                                              $"File Size: {fileInfo.Length} bytes\n" +
                                              $"SHA256: {sha256Hash.ToUpper()}\n";

                    fileInfos.Add(formattedFileInfo);
                }
            }

            File.WriteAllLines(outputFile, fileInfos);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // This event handler can be left empty or implement functionality if needed.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // This event handler can be left empty or implement functionality if needed.
        }

        private string CalculateSHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha256.ComputeHash(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = Application.ProductVersion;
            string message = "SHA Checker\n\nVersion: " + version + "\n\nAuthor: Stevie G";

            MessageBox.Show(message, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}