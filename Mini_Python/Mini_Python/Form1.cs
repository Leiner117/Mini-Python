using System.Diagnostics;
namespace Mini_Python
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            OpenFileDialog buscar = new OpenFileDialog
            {
                Filter = "Archivos Python (*.py)|*.py",
                Title = "Abrir archivo Python"
            };

            if (buscar.ShowDialog() == DialogResult.OK)
            {
                string filePath = buscar.FileName;

                // Verificar que el archivo tenga la extensión .py
                if (Path.GetExtension(filePath).ToLower() != ".py")
                {
                    MessageBox.Show("Solo se pueden abrir archivos .py", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si el archivo ya está abierto en alguna pestaña
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Tag != null && tab.Tag.ToString() == filePath)
                    {
                        // El archivo ya está abierto, seleccionar la pestaña y salir
                        tabControl1.SelectedTab = tab;
                        return;
                    }
                }

                // Si no se encuentra el archivo abierto, crear una nueva pestaña
                AddTabWithCustomName(Path.GetFileName(filePath), filePath);
            }
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

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            var rtb = sender as System.Windows.Forms.RichTextBox;

            int cursorPosition = rtb.SelectionStart;
            var index = rtb.GetLineFromCharIndex(cursorPosition);
            label1.Text = ("Fila: " + index.ToString());
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Obtener la pestaña actual
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

        private void AddTab(object sender, EventArgs e)
        {
            // Crear una nueva pestaña
            TabPage newTabPage = new TabPage("Nueva Pestaña");

            // Crear un nuevo RichTextBox
            RichTextBox richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill // Para que el RichTextBox ocupe todo el espacio de la pestaña
            };

            // Asignar el evento SelectionChanged al RichTextBox
            richTextBox.SelectionChanged += RichTextBox_SelectionChanged;

            // Agregar el RichTextBox a la nueva pestaña
            newTabPage.Controls.Add(richTextBox);

            // Agregar la nueva pestaña al TabControl
            tabControl1.TabPages.Add(newTabPage);

            // Opcional: Cambiar a la nueva pestaña automáticamente
            tabControl1.SelectedTab = newTabPage;
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            // Ejemplo de manejo del evento SelectionChanged
            var rtb = sender as System.Windows.Forms.RichTextBox;

            int cursorPosition = rtb.SelectionStart;
            var index = rtb.GetLineFromCharIndex(cursorPosition);
            label1.Text = ("Fila: " + index.ToString());
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Solicitar el nombre de la nueva pestaña al usuario
            string tabName = PromptForTabName();

            if (!string.IsNullOrEmpty(tabName))
            {
                AddTabWithCustomName(tabName, isNewFile: true);
            }
        }

        private string PromptForTabName()
        {
            // Crear un cuadro de diálogo simple para solicitar el nombre de la pestaña
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
                Dock = DockStyle.Fill
            };

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

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void correrToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
