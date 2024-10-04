namespace compilador;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mini_Python;
using parser.generated;
using Mini_Python.compilador.Checker;

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
        
        myListener.ErrorMsgs.Clear();
        input = CharStreams.fromString(text);
        lexer = new miniPythonLexer(input);
        lexer.RemoveErrorListeners(); 
        lexer.AddErrorListener(myListener);
        tokens = new CommonTokenStream(lexer);
        parser = new miniPythonParser(tokens);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(myListener);
        try
        {
            IParseTree tree = parser.program(); 
            if (myListener.HasErrors())
            {
                Console.WriteLine("Compilation failed");
            }
            else
            {
                Console.WriteLine("Compilation successful - Pasando al ContextAnalizer");
                ContextAnalizer caVisitor = new ContextAnalizer();
                caVisitor.Visit(tree);
                if (caVisitor.hasErrors()){
                    Console.WriteLine("ContextAnalizer failed");
                    Console.WriteLine(caVisitor.toString());
                    Console.WriteLine(caVisitor);
                }
                else
                {
                    Console.WriteLine(myListener);
                    Console.WriteLine("ContextAnalizer passed");
                }
                
            }
        }catch (NullReferenceException  ex){ }
        return myListener;    
    }
    
}