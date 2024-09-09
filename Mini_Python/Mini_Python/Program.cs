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
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }catch (IOException e)
            {
                Console.WriteLine("No hay un archivo.");
                Console.WriteLine(e.Message);
            }
        }
    }
}