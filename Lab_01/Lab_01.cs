using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CompressionLibrary;
using CompressionLibrary.RLE;

namespace Lab_01
{
    public partial class Lab_01 : Form
    {
        // Function to check if a file has a rle extension
        static bool IsRLEFile(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(extension) && extension.Equals(".rle", StringComparison.OrdinalIgnoreCase);
        }

        public Lab_01()
        {
            InitializeComponent();
            Graphics = CreateGraphics();
        }

        Graphics Graphics { get; set; }
        Bitmap Bitmap { get; set; }
        string InputFile { get; set; }
        FileCompressor Compressor = new RLECompressor();
        FileCompressor Decompressor = new RLEDecompressor();

        private string ShortFileName => ShortFileNameOf(InputFile);
        private string ShortFileNameOf(string file) => file.Substring(InputFile.LastIndexOf('\\') + 1);

        private void Compress_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog(); 
            dialog.Filter = "Compressed files (*.rle)|*.rle";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Compressor.Use(InputFile, dialog.FileName);
                textBox1.Text += $@"
Filename = {ShortFileName}
Compressing to = {ShortFileNameOf(dialog.FileName)}
Initial (bytes) = {Compressor.InitialSize}
Output (bytes) = {Compressor.ResultedSize}
Compression coefficient = {Compressor.CompressionCoefficient,2:F4}
Compression time = {Compressor.CompressionDuration.ToString()}
============================";
            }
        }

        private void Decompress_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Files to save (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if(IsRLEFile(dialog.FileName))
                {
                    MessageBox.Show("File cannot be of the type .rle");
                    return;
                }
                Decompressor.Use(InputFile, dialog.FileName);
                textBox1.Text += $@"
Filename = {ShortFileName}
Decompressing to = {ShortFileNameOf(dialog.FileName)}
Initial (bytes) = {Decompressor.InitialSize}
Output (bytes) = {Decompressor.ResultedSize}
Decompression coefficient = {Decompressor.CompressionCoefficient,2:F4}
Decompression time = {Decompressor.CompressionDuration.ToString()}
============================";
                Graphics.Clear(BackColor);
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }
                File.Copy(dialog.FileName, "background", true);
                Bitmap = new Bitmap("background");
                Graphics.DrawImage(Bitmap, 0, 0, Bitmap.Width, Bitmap.Height);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Files to compress (*.*)|*.*|Compressed files (*.rle)|*.rle";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Graphics.Clear(BackColor);
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }

                InputFile = dialog.FileName;
                if (IsRLEFile(dialog.FileName))
                {
                    Decompress.Enabled = true;
                    Compress.Enabled = false;
                }
                else
                {
                    File.Copy(dialog.FileName, "background", true);
                    Bitmap = new Bitmap("background");
                    Graphics.DrawImage(Bitmap, 0, 0, Bitmap.Width, Bitmap.Height);
                    Compress.Enabled = true;
                    Decompress.Enabled = false;
                }
                textBox1.Text += 
$@"
File {ShortFileName} opened
============================
";
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(Bitmap != null && (Width >= Bitmap.Width || Height >= Bitmap.Height) ) Graphics.DrawImage(Bitmap, 0, 0, Bitmap.Width, Bitmap.Height);
        }
    }
}
