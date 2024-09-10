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
            parser.RemoveErrorListeners();
            lexer.AddErrorListener(myListener);
            parser.AddErrorListener(myListener);
            try
            {
                IParseTree tree = parser.program();
                // Llama al punto de entrada principal de la gramática (en este caso, 'program')
                //IParseTree tree = parser.program();
                if (myListener.HasErrors())
                {
                    Console.WriteLine("Compilation failed:");
                    Console.WriteLine(myListener);
                    form.ErrorConsole(myListener);
                }else{
                    Console.WriteLine(myListener);
                    Console.WriteLine("Compilation succeeded");
                }
            }catch (RecognitionException e){
                Console.WriteLine("Error!!!!");
                Console.WriteLine(e.Message);
                form.ErrorConsole(myListener);
            }
        }catch (IOException e) {
            Console.WriteLine("No hay un archivo.");
            Console.WriteLine(e.Message);
            form.ErrorConsole(myListener);
        }
        return myListener;    
    }
    
}