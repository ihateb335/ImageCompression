using System.IO;

namespace Lab_01
{
    partial class CompressionPresenter
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            Graphics.Dispose();
            if (Bitmap != null) Bitmap.Dispose();
            File.Delete("background");
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Compress = new System.Windows.Forms.Button();
            this.Decompress = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GoUp = new System.Windows.Forms.Button();
            this.GoDown = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(200, 399);
            this.textBox1.TabIndex = 0;
            // 
            // Compress
            // 
            this.Compress.Enabled = false;
            this.Compress.Location = new System.Drawing.Point(10, 13);
            this.Compress.Name = "Compress";
            this.Compress.Size = new System.Drawing.Size(75, 23);
            this.Compress.TabIndex = 1;
            this.Compress.Text = "Compress";
            this.Compress.UseVisualStyleBackColor = true;
            this.Compress.Click += new System.EventHandler(this.Compress_Click);
            // 
            // Decompress
            // 
            this.Decompress.Enabled = false;
            this.Decompress.Location = new System.Drawing.Point(91, 13);
            this.Decompress.Name = "Decompress";
            this.Decompress.Size = new System.Drawing.Size(75, 23);
            this.Decompress.TabIndex = 2;
            this.Decompress.Text = "Decompress";
            this.Decompress.UseVisualStyleBackColor = true;
            this.Decompress.Click += new System.EventHandler(this.Decompress_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ClearButton);
            this.panel1.Controls.Add(this.GoDown);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.GoUp);
            this.panel1.Controls.Add(this.Compress);
            this.panel1.Controls.Add(this.Decompress);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 399);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 51);
            this.panel1.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "RLE";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(434, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "RLE";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(722, 13);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 4;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(600, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 399);
            this.panel2.TabIndex = 5;
            // 
            // GoUp
            // 
            this.GoUp.Location = new System.Drawing.Point(600, 13);
            this.GoUp.Name = "GoUp";
            this.GoUp.Size = new System.Drawing.Size(53, 23);
            this.GoUp.TabIndex = 5;
            this.GoUp.Text = "Up";
            this.GoUp.UseVisualStyleBackColor = true;
            this.GoUp.Click += new System.EventHandler(this.GoUp_Click);
            // 
            // GoDown
            // 
            this.GoDown.Location = new System.Drawing.Point(659, 13);
            this.GoDown.Name = "GoDown";
            this.GoDown.Size = new System.Drawing.Size(53, 23);
            this.GoDown.TabIndex = 6;
            this.GoDown.Text = "Down";
            this.GoDown.UseVisualStyleBackColor = true;
            this.GoDown.Click += new System.EventHandler(this.GoDown_Click);
            // 
            // Lab_01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Lab_01";
            this.Text = "Lab_01";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Compress;
        private System.Windows.Forms.Button Decompress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button GoDown;
        private System.Windows.Forms.Button GoUp;
    }
}

