using System.Diagnostics;
using System.Net;
using compilador;
namespace Mini_Python
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.Appearance = TabAppearance.Normal;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(100, 30);
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
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



        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Obtener la pesta�a actual
            TabPage selectedTab = tabControl1.SelectedTab;

            if (selectedTab != null)
            {
                RichTextBox richTextBox = selectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

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
                            selectedTab.Tag = savePath; // Ahora la pesta�a est� asociada a un archivo guardado
                            selectedTab.Text = Path.GetFileName(savePath); // Actualizar el nombre de la pesta�a
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

        private void AddTab(object sender, EventArgs e)
        {
            // Crear una nueva pesta�a
            TabPage newTabPage = new TabPage("Nueva Pesta�a");

            // Crear un nuevo RichTextBox
            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill // Para que el RichTextBox ocupe todo el espacio de la pesta�a
            };

            // Asignar el evento SelectionChanged al RichTextBox
            richTextBox.SelectionChanged += RichTextBox_SelectionChanged;

            // Agregar el RichTextBox a la nueva pesta�a
            newTabPage.Controls.Add(richTextBox);

            // Agregar la nueva pesta�a al TabControl
            tabControl1.TabPages.Add(newTabPage);

            // Cambiar a la nueva pesta�a autom�ticamente
            tabControl1.SelectedTab = newTabPage;
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            // Ejemplo de manejo del evento SelectionChanged
            var rtb = sender as System.Windows.Forms.RichTextBox;

            int cursorPosition = rtb.SelectionStart;
            var index = rtb.GetLineFromCharIndex(cursorPosition);
            label1.Text = ("Fila: " + (index+1).ToString());
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

            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 30, 30, 30), // Establecer el color de fondo del RichTextBox a rgba(30,30,30,255)
                ForeColor = Color.White // Establecer el color del texto a blanco
            };

            // Suscribir los eventos PreviewKeyDown y KeyDown
            richTextBox.PreviewKeyDown += RichTextBox_PreviewKeyDown;
            richTextBox.KeyDown += RichTextBox_KeyDown;
            richTextBox.SelectionChanged += RichTextBox_SelectionChanged;
            newTabPage.Controls.Add(richTextBox);

            // Si es un archivo nuevo, Tag es null; si es un archivo existente, Tag contiene la ruta
            newTabPage.Tag = isNewFile ? null : filePath;

            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectedTab = newTabPage;

            if (!string.IsNullOrEmpty(filePath))
            {
                richTextBox.Text = File.ReadAllText(filePath);
            }
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
            if (e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true; // Evita que el Tab cambie el enfoque
                RichTextBox richTextBox = sender as RichTextBox;
                if (richTextBox != null)
                {
                    int selectionStart = richTextBox.SelectionStart;
                    richTextBox.Text = richTextBox.Text.Insert(selectionStart, "\t");
                    richTextBox.SelectionStart = selectionStart + 1; // Mueve el cursor despu�s del tabulador
                }
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

        private void iconButton1_Click(object sender, EventArgs e)
        {
            // Asegurarse de que hay una pesta�a seleccionada
            if (tabControl1.SelectedTab != null)
            {
                // Obtiene la pesta�a seleccionada
                TabPage selectedTab = tabControl1.SelectedTab;

                // Encuentra el RichTextBox dentro de la pesta�a seleccionada
                // Asumimos que el RichTextBox es el primer (y �nico) control en la pesta�a
                RichTextBox richTextBox = selectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                if (richTextBox != null)
                {
                    // Obtiene el texto del RichTextBox
                    string richText = richTextBox.Text;

                    Compilador.myListenerError(richText);
                }
                else
                {
                    MessageBox.Show("No se encontr� un RichTextBox en la pesta�a seleccionada.");
                }
            }
            else
            {
                MessageBox.Show("No hay ninguna pesta�a seleccionada.");
            }
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
            // Crear un cuadro de di�logo simple para solicitar el enlace
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
                                RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
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


    }
}
