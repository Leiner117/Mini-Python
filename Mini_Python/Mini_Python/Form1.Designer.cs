namespace Mini_Python
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            abirToolStripMenuItem = new ToolStripMenuItem();
            abrirArchivoExternoToolStripMenuItem = new ToolStripMenuItem();
            abrirArchivoLocalToolStripMenuItem = new ToolStripMenuItem();
            nuevoToolStripMenuItem = new ToolStripMenuItem();
            guardarToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            contextMenuStrip1 = new ContextMenuStrip(components);
            iconButton1 = new FontAwesome.Sharp.IconButton();
            richTextBox1 = new RichTextBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(30, 30, 30);
            menuStrip1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1636, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abirToolStripMenuItem, nuevoToolStripMenuItem, guardarToolStripMenuItem });
            archivoToolStripMenuItem.ForeColor = Color.White;
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(60, 20);
            archivoToolStripMenuItem.Text = "Archivo";
            archivoToolStripMenuItem.Click += archivoToolStripMenuItem_Click;
            // 
            // abirToolStripMenuItem
            // 
            abirToolStripMenuItem.BackColor = Color.FromArgb(30, 30, 30);
            abirToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abrirArchivoExternoToolStripMenuItem, abrirArchivoLocalToolStripMenuItem });
            abirToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            abirToolStripMenuItem.Name = "abirToolStripMenuItem";
            abirToolStripMenuItem.Size = new Size(116, 22);
            abirToolStripMenuItem.Text = "Abrir";
            abirToolStripMenuItem.Click += abirToolStripMenuItem_Click;
            // 
            // abrirArchivoExternoToolStripMenuItem
            // 
            abrirArchivoExternoToolStripMenuItem.BackColor = Color.FromArgb(30, 30, 30);
            abrirArchivoExternoToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            abrirArchivoExternoToolStripMenuItem.Name = "abrirArchivoExternoToolStripMenuItem";
            abrirArchivoExternoToolStripMenuItem.Size = new Size(185, 22);
            abrirArchivoExternoToolStripMenuItem.Text = "Abrir archivo externo";
            abrirArchivoExternoToolStripMenuItem.Click += abrirArchivoExternoToolStripMenuItem_Click;
            // 
            // abrirArchivoLocalToolStripMenuItem
            // 
            abrirArchivoLocalToolStripMenuItem.BackColor = Color.FromArgb(30, 30, 30);
            abrirArchivoLocalToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            abrirArchivoLocalToolStripMenuItem.Name = "abrirArchivoLocalToolStripMenuItem";
            abrirArchivoLocalToolStripMenuItem.Size = new Size(185, 22);
            abrirArchivoLocalToolStripMenuItem.Text = "Abrir archivo local";
            abrirArchivoLocalToolStripMenuItem.Click += abrirArchivoLocalToolStripMenuItem_Click;
            // 
            // nuevoToolStripMenuItem
            // 
            nuevoToolStripMenuItem.BackColor = Color.FromArgb(30, 30, 30);
            nuevoToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            nuevoToolStripMenuItem.Size = new Size(116, 22);
            nuevoToolStripMenuItem.Text = "Nuevo";
            nuevoToolStripMenuItem.Click += nuevoToolStripMenuItem_Click;
            // 
            // guardarToolStripMenuItem
            // 
            guardarToolStripMenuItem.BackColor = Color.FromArgb(30, 30, 30);
            guardarToolStripMenuItem.ForeColor = Color.WhiteSmoke;
            guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            guardarToolStripMenuItem.Size = new Size(116, 22);
            guardarToolStripMenuItem.Text = "Guardar";
            guardarToolStripMenuItem.Click += guardarToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Location = new Point(0, 56);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1636, 620);
            tabControl1.TabIndex = 2;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // iconButton1
            // 
            iconButton1.BackColor = Color.FromArgb(30, 30, 30);
            iconButton1.BackgroundImageLayout = ImageLayout.Stretch;
            iconButton1.FlatAppearance.BorderSize = 0;
            iconButton1.FlatStyle = FlatStyle.Flat;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Play;
            iconButton1.IconColor = Color.DarkGreen;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 20;
            iconButton1.Location = new Point(0, 27);
            iconButton1.Name = "iconButton1";
            iconButton1.Size = new Size(63, 27);
            iconButton1.TabIndex = 6;
            iconButton1.UseVisualStyleBackColor = false;
            iconButton1.Click += iconButton1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(30, 30, 30);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.DetectUrls = false;
            richTextBox1.ForeColor = Color.WhiteSmoke;
            richTextBox1.HideSelection = false;
            richTextBox1.ImeMode = ImeMode.Disable;
            richTextBox1.Location = new Point(12, 698);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(508, 173);
            richTextBox1.TabIndex = 7;
            richTextBox1.Text = "";
            richTextBox1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1636, 883);
            Controls.Add(richTextBox1);
            Controls.Add(iconButton1);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem abirToolStripMenuItem;
        private ToolStripMenuItem nuevoToolStripMenuItem;
        private TabControl tabControl1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem guardarToolStripMenuItem;
        private FontAwesome.Sharp.IconButton iconButton1;
        private ToolStripMenuItem abrirArchivoExternoToolStripMenuItem;
        private ToolStripMenuItem abrirArchivoLocalToolStripMenuItem;
        private RichTextBox richTextBox1;
    }
}
