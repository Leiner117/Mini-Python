using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using parser.generated;

namespace Mini_Python
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Crea un input stream desde el archivo de prueba
            ICharStream input = null;
            try {
                input = CharStreams.fromPath("C:\\Users\\leine\\OneDrive\\Documentos\\Github\\Mini-Python\\Mini_Python\\Mini_Python\\test.txt");
            } catch (IOException e) {
                throw new Exception(e.Message);
            }
            // Crea un lexer para el input
            var lexer = new miniPythonLexer(input);
            // Crea un buffer de tokens con el lexer
            var tokens = new CommonTokenStream(lexer);
            // Crea un parser con el buffer de tokens
            var parser = new miniPythonParser(tokens);
            // Llama al punto de entrada principal de la gramática (en este caso, 'program')
            IParseTree tree = parser.program();
            // Imprime el árbol de análisis (para fines de depuración)
            Console.WriteLine(tree.ToStringTree(parser));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

        }
    }
}