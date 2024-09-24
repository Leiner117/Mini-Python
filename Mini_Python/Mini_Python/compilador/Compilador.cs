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
        CommonTokenStream tokens = null;
        miniPythonLexer lexer = null;
        miniPythonParser parser = null;
        MyErrorListener myListener = new MyErrorListener();
        try
        {
            input = CharStreams.fromString(text);
            lexer = new miniPythonLexer(input);
            tokens = new CommonTokenStream(lexer);
            parser = new miniPythonParser(tokens);
            myListener.ErrorMsgs.Clear();
            parser.RemoveErrorListeners();
            lexer.RemoveErrorListeners(); 
            parser.ErrorHandler = new CustomErrorStrategy();
            lexer.AddErrorListener(myListener);
            parser.AddErrorListener(myListener);
            try
            {
                IParseTree tree = parser.program(); // Intenta analizar
                if (myListener.HasErrors())
                {
                    Console.WriteLine("Compilation failed");
                }
            }catch (RecognitionException e)
            {
                myListener.ErrorMsgs.Add($"Unhandled error: {e.Message}");
            }catch (Exception ex) // Captura excepciones generales
            {
               myListener.ErrorMsgs.Add($"General exception: {ex.Message}");
            }
        }catch (IOException e) {
            Console.WriteLine("No hay un archivo.");
            Console.WriteLine(e.Message);
        }
        return myListener;    
    }
    
}