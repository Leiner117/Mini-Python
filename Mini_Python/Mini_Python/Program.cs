using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using compilador;
using parser.generated;

namespace Mini_Python
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Crea un input stream desde el archivo de prueba
                ICharStream input = null;
                input = CharStreams.fromPath("C:\\Users\\Walter\\Documents\\IIS2024\\Compiladores\\ProyectoMini-Python\\ProyectoRider\\Mini_Python\\Mini_Python\\test.txt");
                var lexer = new miniPythonLexer(input);
                var tokens = new CommonTokenStream(lexer);
                var parser = new miniPythonParser(tokens);
                MyErrorListener myListener = new MyErrorListener();
                lexer.RemoveErrorListeners();
                parser.RemoveErrorListeners();
                lexer.AddErrorListener(myListener);
                parser.AddErrorListener(myListener);
                try
                {
                    // Llama al punto de entrada principal de la gramática (en este caso, 'program')
                    IParseTree tree = parser.program();
                    if (myListener.HasErrors())
                    {
                        Console.WriteLine("Compilation failed:");
                        Console.WriteLine(myListener);
                    }
                    else
                    {
                        Console.WriteLine(myListener);
                        Console.WriteLine("Compilation succeeded");
                    }
                }catch (RecognitionException e)
                {
                    Console.WriteLine("Error!!!!");
                    Console.WriteLine(e.Message);
                }
                // Imprime el árbol de análisis (para fines de depuración)
                //Console.WriteLine(tree.ToStringTree(parser));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
               // Application.Run(new Form1());
            }catch (IOException e)
            {
                Console.WriteLine("No hay un archivo.");
                Console.WriteLine(e.Message);
            }
        }
    }
}