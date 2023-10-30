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
using CompressionLibrary.RLEI;
using CompressionLibrary.LZW;

namespace Lab_01
{
    public partial class CompressionPresenter : Form
    {
        // Function to check if a file has a rle extension
        bool IsRLEFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(extension) && extension.Equals($"{Extension}", StringComparison.OrdinalIgnoreCase);
        }

        public CompressionPresenter()
        {
            InitializeComponent();
            Graphics = CreateGraphics();

            comboBox1.Items.Add(
                new DataCompressionModel { 
                    Compression = new RLECompression(),
                    MethodName = "RLE",
                    Extension = ".rle"
                }
            );
            comboBox1.Items.Add(
                new DataCompressionModel { 
                    Compression = new RLEICompression(),
                    MethodName = "RLEI",
                    Extension = ".rlei"
                }
            );
            comboBox1.Items.Add(
                new DataCompressionModel { 
                    Compression = new LZWCompression(4094, true),
                    MethodName = "LZW 4k variable",
                    Extension = ".lzw4kv"
                }
            );
            comboBox1.Items.Add(
                new DataCompressionModel { 
                    Compression = new LZWCompression(16381, true),
                    MethodName = "LZW 16k variable",
                    Extension = ".lzw16kv"
                }
            );
            comboBox1.Items.Add(
                new DataCompressionModel { 
                    Compression = new LZWCompression(4094, false),
                    MethodName = "LZW 4k 9bit",
                    Extension = ".lzw4k9b"
                }
            );



            comboBox1.SelectedIndex = 3;
            ClearButton_Click(null, null);
        }
        DataCompressionModel CurrentModel => (DataCompressionModel)comboBox1.SelectedItem;
        DataCompression Compression => CurrentModel.Compression;
        string Extension => CurrentModel.Extension;

        Graphics Graphics { get; set; }
        Bitmap Bitmap { get; set; }
        string InputFile { get; set; }

        private string ShortFileName => ShortFileNameOf(InputFile);
        private string ShortFileNameOf(string file) => file.Substring(InputFile.LastIndexOf('\\') + 1);

        private void Compress_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog(); 
            dialog.Filter = $"Compressed files (*{Extension})|*{Extension}";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Compression.Compress(InputFile, dialog.FileName);
                textBox1.Text += $@"
Filename = {ShortFileName}
Compressing to = {ShortFileNameOf(dialog.FileName)}
Initial (bytes) = {Compression.Compressor.InitialSize}
Output (bytes) = {Compression.Compressor.ResultedSize}
Compression coefficient = {Compression.Compressor.CompressionCoefficient,2:F4}
Compression time = {Compression.Compressor.CompressionDuration.ToString()}
============================";
            }
        }

        private void Decompress_Click(object sender, EventArgs e)
        {
            var inputExtension = Path.GetExtension(InputFile);
            if (inputExtension != Extension && HasExtension(inputExtension) ) { MessageBox.Show($"File must be {Extension}, change the compression method"); return; }

            var dialog = new SaveFileDialog();
            dialog.Filter = "Files to save (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if(IsRLEFile(dialog.FileName))
                {
                    MessageBox.Show($"File cannot be of the type {Path.GetExtension(dialog.FileName)}");
                    return;
                }
                Compression.Decompress(InputFile, dialog.FileName);
                textBox1.Text += $@"
Filename = {ShortFileName}
Decompressing to = {ShortFileNameOf(dialog.FileName)}
Initial (bytes) = {Compression.Decompressor.InitialSize}
Output (bytes) = {Compression.Decompressor.ResultedSize}
Decompression coefficient = {Compression.Decompressor.CompressionCoefficient,2:F4}
Decompression time = {Compression.Decompressor.CompressionDuration.ToString()}
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
            dialog.Filter = $"Files to compress (*.*)|*.*|Compressed files (*{Extension})|*{Extension}";
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
                else if (HasExtension(Path.GetExtension(dialog.FileName)))
                {
                    MessageBox.Show($"File cannot be of the type {Path.GetExtension(dialog.FileName)}");
                    return;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text +=
$@"
Method changed to {CurrentModel.MethodName}
============================
";
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void GoUp_Click(object sender, EventArgs e)
        {
            textBox1.Select(0, 0);
            textBox1.ScrollToCaret();
        }

        private void GoDown_Click(object sender, EventArgs e)
        {
            textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);
            textBox1.ScrollToCaret();
        }

        public bool HasExtension(string extension) => comboBox1.Items.Cast<DataCompressionModel>().Select(x => x.Extension).Any((x) => x == extension);
    }
}
