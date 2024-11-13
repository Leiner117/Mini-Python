using System.Diagnostics;

namespace compilador;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mini_Python;
using parser.generated;
using Mini_Python.compilador.Checker;
using Mini_Python.compilador.CodeGen;
using System.IO;
using System;
using static Mini_Python.Form1;
public class Compilador
{
    public static object compilador (string text)
    {
        ICharStream input = null;
        CommonTokenStream tokens = null;
        miniPythonLexer lexer = null;
        miniPythonParser parser = null;
        MyErrorListener myListener = new MyErrorListener();
        ContextAnalizer caVisitor = new ContextAnalizer();
        CodeGeneration cgVisitor = new CodeGeneration();
        myListener.ErrorMsgs.Clear();
        caVisitor.errorList.Clear();
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
            }else{
                Console.WriteLine("Compilation successful - Pasando al ContextAnalizer");
                caVisitor.Visit(tree);
                if (caVisitor.hasErrors()){
                    Console.WriteLine("ContextAnalizer failed");
                    Console.WriteLine(caVisitor.ToString());
                    return caVisitor;
                }
                Console.WriteLine("ContextAnalizer successful - Pasando al CodeGeneration");
                cgVisitor.Visit(tree);
                string byteCode = cgVisitor.ToString();
                Console.WriteLine(byteCode);
                Console.WriteLine("Compilation successful");
                
                try
                { 
                   string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
                   // Subir varios niveles hasta el directorio del proyecto
                   string projectDirectory = Directory.GetParent(binDirectory).Parent.Parent.Parent.FullName;
                    // Crear la ruta completa para el archivo dentro del directorio del proyecto
                    string filePath = Path.Combine(projectDirectory, "byteCodeProyecto.txt");
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(byteCode);
                    }
                    Console.WriteLine("bytecode generado.");
                    string exePath = Path.Combine(projectDirectory,"MiniPY.exe");
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = filePath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Console.WriteLine("Ejecutando el archivo bytecode generado");
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        string output = exeProcess.StandardOutput.ReadToEnd();
                        string error = exeProcess.StandardError.ReadToEnd();
                        exeProcess.WaitForExit();
                        string combinedOutput = output + Environment.NewLine + error;
                        if (!string.IsNullOrEmpty(error)|| output.Contains("Error:")) 
                            return new ProcessResult(false,"Error de Ejecucion:","",combinedOutput); 
                        return new ProcessResult(true,"Ejecucion Finalizada con exito:",output,"");
                    } 
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error al escribir el archivo bytecode.txt");
                    Console.WriteLine(e);
                    throw;
                }
            }
        }catch (NullReferenceException  ex){ }
        return myListener; 
    }
    
}