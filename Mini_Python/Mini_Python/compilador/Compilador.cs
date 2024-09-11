namespace compilador;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mini_Python;
using parser.generated;
using static Mini_Python.Form1;
public class Compilador
{
    public static MyErrorListener compilador (string text)
    {
        Form1 form = new Form1();
        ICharStream input = null;
        MyErrorListener myListener = new MyErrorListener();
        try
        {
            input = CharStreams.fromString(text);
            var lexer = new miniPythonLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new miniPythonParser(tokens);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(myListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(myListener);
            IParseTree tree = parser.program();
        }catch (IOException e) {
            Console.WriteLine("No hay un archivo.");
            Console.WriteLine(e.Message);
        }
        return myListener;    
    }
    
}