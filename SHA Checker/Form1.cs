using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Choose the directory to scan
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select a directory to scan files.";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string directoryPath = folderBrowser.SelectedPath;
                    string outputFile = "file_hashes.txt";
                    CalculateAndSaveHashes(directoryPath, outputFile);
                    label1.Text = "File hashes saved to " + outputFile;
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
                    string fileInfo = Path.GetFileName(filePath) + ": " + sha256Hash;
                    fileInfos.Add(fileInfo);
                }
            }

            File.WriteAllLines(outputFile, fileInfos);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
