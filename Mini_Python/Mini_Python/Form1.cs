using System.Diagnostics;
using System.Net;
using System.Text;
using compilador;
namespace Mini_Python
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeTabControl();
            tabControl1.Appearance = TabAppearance.Normal;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(100, 30);
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);

            richTextBox1.MouseClick += richTextBox1_MouseClick;
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);


        }

        private void InitializeTabControl()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
            tabControl1.MouseDown += new MouseEventHandler(tabControl1_MouseDown);
        }




        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // Establece los colores

            Color backColor = Color.FromArgb(255, 30, 30, 30); // Color de fondo de la pesta�a
            Color foreColor = Color.WhiteSmoke;     // Color del texto de la pesta�a

            // Dibuja el fondo de la pesta�a
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            e.Graphics.DrawRectangle(Pens.Transparent, tabRect);

            // Dibuja el texto de la pesta�a
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font, tabRect, foreColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);


            Rectangle closeButton = new Rectangle(tabRect.Right - 15, tabRect.Top + 4, 10, 10);
            e.Graphics.DrawLine(Pens.White, closeButton.Left, closeButton.Top, closeButton.Right, closeButton.Bottom);
            e.Graphics.DrawLine(Pens.White, closeButton.Right, closeButton.Top, closeButton.Left, closeButton.Bottom);


        }


        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControl1.GetTabRect(i);
                Rectangle closeButton = new Rectangle(tabRect.Right - 15, tabRect.Top + 4, 10, 10);

                if (closeButton.Contains(e.Location))
                {
                    tabControl1.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Obtener la pestaña actual
            TabPage selectedTab = tabControl1.SelectedTab;

            if (selectedTab != null)
            {
                RichTextBox richTextBox = selectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox != null)
                {
                    if (selectedTab.Tag == null) // Archivo nuevo
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "Archivos Python (*.py)|*.py",
                            Title = "Guardar archivo Python"
                        };

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string savePath = saveFileDialog.FileName;
                            File.WriteAllText(savePath, richTextBox.Text);
                            selectedTab.Tag = savePath; // Ahora la pestaña está asociada a un archivo guardado
                            selectedTab.Text = Path.GetFileName(savePath); // Actualizar el nombre de la pestaña
                        }
                    }
                    else // Archivo existente
                    {
                        string filePath = selectedTab.Tag.ToString();
                        File.WriteAllText(filePath, richTextBox.Text);
                    }
                }
            }
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            // Ejemplo de manejo del evento SelectionChanged
            var rtb = sender as System.Windows.Forms.RichTextBox;

            int cursorPosition = rtb.SelectionStart;
            var index = rtb.GetLineFromCharIndex(cursorPosition);
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Solicitar el nombre de la nueva pesta�a al usuario
            string tabName = PromptForTabName();

            if (!string.IsNullOrEmpty(tabName))
            {
                AddTabWithCustomName(tabName, isNewFile: true);
            }
        }

        private string PromptForTabName()
        {
            // Crear un cuadro de di�logo simple para solicitar el nombre de la pesta�a
            using (Form prompt = new Form())
            {
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.Text = "Nuevo archivo";

                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Nuevo archivo:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
                Button confirmation = new Button() { Text = "Aceptar", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };

                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }

        private void AddTabWithCustomName(string tabName, string filePath = null, bool isNewFile = false)
        {
            TabPage newTabPage = new TabPage(tabName);

            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            RichTextBox lineNumberLabel = new RichTextBox
            {
                Dock = DockStyle.Left,
                Width = 50,
                BackColor = Color.FromArgb(255, 30, 30, 30),
                ForeColor = Color.White,
                Font = new Font("Consolas", 16, FontStyle.Regular),            
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true 
                
            };

            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 30, 30, 30),
                ForeColor = Color.White,
                Font = new Font("Consolas", 16, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                Top = 20
            };

            richTextBox.TextChanged += (sender, e) => UpdateLineNumbers(richTextBox, lineNumberLabel);
            richTextBox.VScroll += (sender, e) => UpdateLineNumbers(richTextBox, lineNumberLabel);
            richTextBox.PreviewKeyDown += RichTextBox_PreviewKeyDown;
            richTextBox.KeyDown += RichTextBox_KeyDown; // Añadir el evento KeyDown

            panel.Controls.Add(richTextBox);
            panel.Controls.Add(lineNumberLabel);
            newTabPage.Controls.Add(panel);

            newTabPage.Tag = isNewFile ? null : filePath;

            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectedTab = newTabPage;

            if (!string.IsNullOrEmpty(filePath))
            {
                richTextBox.Text = File.ReadAllText(filePath);
            }

            UpdateLineNumbers(richTextBox, lineNumberLabel);
        }


        private void RichTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
            }
        }

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;

            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                int selectionStart = richTextBox.SelectionStart;

                // Verificar si hay 4 espacios antes del cursor
                if (selectionStart >= 4 && richTextBox.Text.Substring(selectionStart - 4, 4) == "    ")
                {
                    richTextBox.Text = richTextBox.Text.Remove(selectionStart - 4, 4);
                    richTextBox.SelectionStart = selectionStart - 4;
                }

                e.SuppressKeyPress = true; // Evitar que el tabulador se propague
            }
            else if (e.KeyCode == Keys.Tab)
            {
                int selectionStart = richTextBox.SelectionStart;
                richTextBox.Text = richTextBox.Text.Insert(selectionStart, "    ");
                richTextBox.SelectionStart = selectionStart + 4;
                e.SuppressKeyPress = true; // Evitar que el tabulador se propague
            
        }
        }

        private void UpdateLineNumbers(RichTextBox richTextBox, RichTextBox lineNumberLabel)
        {
            lineNumberLabel.ScrollBars = RichTextBoxScrollBars.None;
            lineNumberLabel.WordWrap = false;
            lineNumberLabel.Multiline = true;

            int firstVisibleLine = richTextBox.GetLineFromCharIndex(richTextBox.GetCharIndexFromPosition(new Point(0, 0)));
            int lastVisibleLine = richTextBox.GetLineFromCharIndex(richTextBox.GetCharIndexFromPosition(new Point(0, richTextBox.Height)));

    
            
            StringBuilder lineNumbers = new StringBuilder();
            for (int i = firstVisibleLine; i <= lastVisibleLine + 1; i++)
            {
                lineNumbers.AppendLine((i + 1).ToString());
            }

            lineNumberLabel.Text = lineNumbers.ToString();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
                // Asegurarse de que hay una pestaña seleccionada
                if (tabControl1.SelectedTab != null)
                {
                    // Obtiene la pestaña seleccionada
                    TabPage selectedTab = tabControl1.SelectedTab;

                    // Encuentra el RichTextBox dentro de la pestaña seleccionada
                    RichTextBox richTextBox = selectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();
                    RichTextBox lineNumberLabel = selectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (richTextBox != null && lineNumberLabel != null)
                    {
                        // Eliminar todos los subrayados de colores
                        QuitarSubrayadosDeColores(richTextBox);

                        // Obtiene el texto del RichTextBox
                        string richText = richTextBox.Text;
                        ErrorConsole(Compilador.compilador(richText));

                    }
                    else
                    {
                        MessageBox.Show("No se encontró un RichTextBox en la pestaña seleccionada.");
                    }
                }
                else
                {
                    MessageBox.Show("No hay ninguna pestaña seleccionada.");
                }
        }


        private void QuitarSubrayadosDeColores(RichTextBox richTextBox)
        {
            // Guardar la posición actual del cursor
            int originalSelectionStart = richTextBox.SelectionStart;
            int originalSelectionLength = richTextBox.SelectionLength;

            // Seleccionar todo el texto
            richTextBox.SelectAll();

            // Restablecer el color de fondo y el color de texto
            richTextBox.SelectionBackColor = richTextBox.BackColor;
            richTextBox.SelectionColor = richTextBox.ForeColor;

            // Deseleccionar el texto
            richTextBox.Select(originalSelectionStart, originalSelectionLength);
        }


        private void abrirArchivoLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Obtener el �ltimo directorio abierto desde la configuraci�n
            string lastOpenedDirectory = Properties.Settings.Default.LastOpenedDirectory;

            OpenFileDialog buscar = new OpenFileDialog
            {
                Filter = "Archivos Python (*.py)|*.py",
                Title = "Abrir archivo Python",
                InitialDirectory = string.IsNullOrEmpty(lastOpenedDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : lastOpenedDirectory
            };

            if (buscar.ShowDialog() == DialogResult.OK)
            {
                string filePath = buscar.FileName;

                // Actualizar el �ltimo directorio abierto en la configuraci�n
                Properties.Settings.Default.LastOpenedDirectory = Path.GetDirectoryName(filePath);
                Properties.Settings.Default.Save();

                // Verificar que el archivo tenga la extensi�n .py
                if (Path.GetExtension(filePath).ToLower() != ".py")
                {
                    MessageBox.Show("Solo se pueden abrir archivos .py", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si el archivo ya est� abierto en alguna pesta�a
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Tag != null && tab.Tag.ToString() == filePath)
                    {
                        // El archivo ya est� abierto, seleccionar la pesta�a y salir
                        tabControl1.SelectedTab = tab;
                        return;
                    }
                }

                // Si no se encuentra el archivo abierto, crear una nueva pesta�a
                AddTabWithCustomName(Path.GetFileName(filePath), filePath);
            }
        }

        private void abrirArchivoExternoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crear un cuadro de diálogo simple para solicitar el enlace
            using (Form prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.Text = "Abrir archivo externo";

                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Pegar enlace:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                Button confirmation = new Button() { Text = "Aceptar", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.OK };

                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    string url = textBox.Text;

                    if (url.EndsWith(".py"))
                    {
                        try
                        {
                            string rawUrl = ConvertToRawGitHubUrl(url);
                            using (WebClient client = new WebClient())
                            {
                                string fileContent = client.DownloadString(rawUrl);
                                string fileName = Path.GetFileName(url);
                                AddTabWithCustomName(fileName, isNewFile: false);
                                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();
                                if (richTextBox != null)
                                {
                                    richTextBox.Text = fileContent;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al descargar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El enlace no apunta a un archivo .py", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private string ConvertToRawGitHubUrl(string url)
        {
            if (url.Contains("github.com"))
            {
                url = url.Replace("github.com", "raw.githubusercontent.com");
                url = url.Replace("/blob/", "/");
            }
            return url;
        }

        public void ErrorConsole(Object error)
        {
            // Limpiar y mostrar el RichTextBox de errores
            richTextBox1.Clear();
            richTextBox1.Visible = true;

            // Suponiendo que error.ToString() devuelve una cadena con múltiples errores separados por saltos de línea
            string[] errorLines = error.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in errorLines)
            {
                // Añadir la línea al RichTextBox de errores
                int startIndex = richTextBox1.TextLength;
                richTextBox1.AppendText(line + Environment.NewLine);
                int endIndex = richTextBox1.TextLength;

                // Buscar el patrón "line X:Y"
                var match = System.Text.RegularExpressions.Regex.Match(line, @"line (\d+:\d+)");
                if (match.Success)
                {
                    // Subrayar solo el número de línea y columna
                    int numberStartIndex = startIndex + match.Groups[1].Index;
                    int numberLength = match.Groups[1].Length;

                    richTextBox1.Select(numberStartIndex, numberLength);
                    richTextBox1.SelectionColor = Color.Aqua;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Underline);

                    // Añadir un enlace al número de línea y columna
                    richTextBox1.SelectionStart = numberStartIndex;
                    richTextBox1.SelectionLength = numberLength;
                    richTextBox1.SetSelectionLink(true);

                    // Subrayar la línea en rojo en el RichTextBox del TabControl
                    var lineColumnMatch = System.Text.RegularExpressions.Regex.Match(match.Groups[1].Value, @"(\d+):(\d+)");
                    if (lineColumnMatch.Success)
                    {
                        int lineNumber = int.Parse(lineColumnMatch.Groups[1].Value);
                        SubrayarLineaEnRojo(lineNumber);
                    }
                }
            }

            // Deseleccionar el texto
            richTextBox1.Select(0, 0);
        }


        private void SubrayarLineaEnRojo(int lineNumber)
        {
            // Asegurarse de que hay una pestaña seleccionada
            if (tabControl1.SelectedTab != null)
            {
                // Obtiene la pestaña seleccionada
                TabPage selectedTab = tabControl1.SelectedTab;

                // Encuentra el RichTextBox dentro de la pestaña seleccionada
                RichTextBox richTextBox = selectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox != null)
                {
                    // Desuscribir el evento TextChanged para evitar que se dispare durante el cambio de formato
                    richTextBox.TextChanged -= RichTextBox_TextChanged;

                    // Obtiene el índice del primer carácter de la línea
                    int lineIndex = richTextBox.GetFirstCharIndexFromLine(lineNumber - 1);

                    // Subrayar la línea en rojo
                    int lineLength = richTextBox.Lines[lineNumber - 1].Length;
                    richTextBox.Select(lineIndex, lineLength);
                    richTextBox.SelectionBackColor = Color.Red;

                    // Restablecer el color de fondo y el color de texto para el texto posterior
                    richTextBox.Select(lineIndex + lineLength, 0);
                    richTextBox.SelectionBackColor = richTextBox.BackColor;
                    richTextBox.SelectionColor = richTextBox.ForeColor;

                    // Deseleccionar el texto
                    richTextBox.Select(0, 0);

                    // Volver a suscribir el evento TextChanged
                    richTextBox.TextChanged += RichTextBox_TextChanged;
                }
            }
        }


        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox != null)
            {
                // Guardar la posición actual del cursor
                int selectionStart = richTextBox.SelectionStart;
                int selectionLength = richTextBox.SelectionLength;

                // Restablecer el color de fondo y el color de texto a los valores predeterminados
                richTextBox.SelectAll();
                richTextBox.SelectionBackColor = richTextBox.BackColor;
                richTextBox.SelectionColor = richTextBox.ForeColor;

                // Restaurar la posición del cursor
                richTextBox.Select(selectionStart, selectionLength);
            }
        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            RichTextBox richTextBox = e.TabPage.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (richTextBox != null)
            {
                richTextBox.TextChanged += RichTextBox_TextChanged;
            }
        }



        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox == null)
            {
                Debug.WriteLine("richTextBox es null");
                return;
            }

            int index = richTextBox.GetCharIndexFromPosition(e.Location);
            richTextBox.Select(index, 1);

            Debug.WriteLine($"Índice de carácter seleccionado: {index}");

            if (richTextBox.SelectionFont != null && richTextBox.SelectionFont.Underline)
            {
                string text = richTextBox.Text;
                var match = System.Text.RegularExpressions.Regex.Match(text, @"(\d+:\d+)", System.Text.RegularExpressions.RegexOptions.RightToLeft);

                while (match.Success)
                {
                    int matchStart = match.Index;
                    int matchEnd = match.Index + match.Length;

                    if (index >= matchStart && index < matchEnd)
                    {
                        richTextBox.Select(matchStart, match.Length);
                        string selectedText = richTextBox.SelectedText;
                        Debug.WriteLine($"Texto seleccionado: {selectedText}");

                        var lineColumnMatch = System.Text.RegularExpressions.Regex.Match(selectedText, @"(\d+):(\d+)");
                        if (lineColumnMatch.Success)
                        {
                            int lineNumber = int.Parse(lineColumnMatch.Groups[1].Value);
                            int columnNumber = int.Parse(lineColumnMatch.Groups[2].Value);

                            Debug.WriteLine($"Número de línea: {lineNumber}, Número de columna: {columnNumber}");

                            // Redirigir el cursor al RichTextBox en la pestaña seleccionada
                            TabPage selectedTab = tabControl1.SelectedTab;
                            RichTextBox targetRichTextBox = selectedTab.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<RichTextBox>().FirstOrDefault();
                            if (targetRichTextBox != null)
                            {
                                int lineIndex = targetRichTextBox.GetFirstCharIndexFromLine(lineNumber - 1);
                                targetRichTextBox.Select(lineIndex + columnNumber - 1, 0);
                                targetRichTextBox.Focus();

                                Debug.WriteLine($"Cursor redirigido a la línea {lineNumber}, columna {columnNumber} en el RichTextBox de la pestaña seleccionada.");
                            }
                            else
                            {
                                Debug.WriteLine("No se encontró un RichTextBox en la pestaña seleccionada.");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No se encontró un patrón de línea y columna en el texto seleccionado.");
                        }
                        return;
                    }
                    match = match.NextMatch();
                }
            }
            else
            {
                Debug.WriteLine("El texto seleccionado no está subrayado.");
            }
        }



        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void correrToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void abirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
