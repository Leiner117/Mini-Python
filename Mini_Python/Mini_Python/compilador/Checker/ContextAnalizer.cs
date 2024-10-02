namespace Mini_Python.compilador.Checker;
using parser.generated;    
public class ContextAnalizer : miniPythonParserBaseVisitor<object> {

    private TablaSimbolos TablaSimbolosProyecto;
    private List<string> errorList ;
    public ContextAnalizer() {
        TablaSimbolosProyecto = new TablaSimbolos();
        errorList =  new List<string>();
    }

    public override object VisitProgram(miniPythonParser.ProgramContext context)
    {
        return base.VisitProgram(context);
    }

    public override object VisitMainStatement(miniPythonParser.MainStatementContext context)
    {
        return base.VisitMainStatement(context);
    }

    public override object VisitStatement(miniPythonParser.StatementContext context)
    {
        return base.VisitStatement(context);
    }

    public override object VisitDefStatement(miniPythonParser.DefStatementContext context) {
        string nombreFuncion = context.IDENTIFIER().GetText();
        
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreFuncion) != null) {
            errorList.Add($"Error: La funcion '{nombreFuncion}' ya esta definida en este scope.");
        } else
        {
           TablaSimbolosProyecto.OpenScope();
            Visit(context.IDENTIFIER());
           Visit(context.LPAREN());
           Visit(context.argList());
           Visit(context.RPAREN());
            Visit(context.DOSPUNTOS());
           Visit(context.NEWLINE());
           Visit(context.sequence());
           // Extraemos la lista de parámetros de la función
           List<string> parametros = new List<string>();
           if (context.argList() != null)
           {
               // Asumimos que los identificadores de los parámetros están en el argList
               foreach (var param in context.argList().IDENTIFIER())
               {
                   string paramName = param.GetText();

                   // Verificar si el parámetro ya está definido en este scope
                   if (TablaSimbolosProyecto.BuscarEnNivelActual(paramName) != null)
                   {
                       errorList.Add($"Error: El parametro '{paramName}' ya esta definido en este scope.");
                   }
                   else
                   {
                       // Insertamos cada parámetro en la tabla de símbolos
                       TablaSimbolosProyecto.InsertarVariable(param.Symbol, SymbolType.Parameter);
                       parametros.Add(paramName); // Guardamos el parámetro
                   }
               }
           }

           // Insertamos la función en la tabla de símbolos con su lista de parámetros
           TablaSimbolosProyecto.InsertarFuncion(context.IDENTIFIER().Symbol, SymbolType.Function, parametros);
           // Imprimimos la tabla de símbolos para depuración
           TablaSimbolosProyecto.Imprimir();
           // Cerramos el scope después de procesar la función
           TablaSimbolosProyecto.OpenScope();
        }

        //return null;
        return base.VisitDefStatement(context);
    }

    public override object VisitArgList(miniPythonParser.ArgListContext context)
    {
        return base.VisitArgList(context);
    }

    public override object VisitIfStatement(miniPythonParser.IfStatementContext context)
    {
        return base.VisitIfStatement(context);
    }

    public override object VisitWhileStatement(miniPythonParser.WhileStatementContext context)
    {
        return base.VisitWhileStatement(context);
    }

    public override object VisitReturnStatement(miniPythonParser.ReturnStatementContext context)
    {
        return base.VisitReturnStatement(context);
    }

    public override object VisitForStatement(miniPythonParser.ForStatementContext context)
    {
        return base.VisitForStatement(context);
    }

    public override object VisitPrintStatement(miniPythonParser.PrintStatementContext context)
    {
        return base.VisitPrintStatement(context);
    }


    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context) {
        string nombreVariable = context.IDENTIFIER().GetText();
        
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreVariable) != null) {
            errorList.Add($"Error: La variable '{nombreVariable}' ya esta definida en este scope.");
        } else {
            TablaSimbolosProyecto.InsertarVariable(context.IDENTIFIER().Symbol, SymbolType.Variable);
            Visit(context.IDENTIFIER());
            Visit(context.ASSIGN());
            Visit(context.expression());
            Visit(context.NEWLINE());
            
            
            TablaSimbolosProyecto.Imprimir();
          
        }

        //return null;
         return base.VisitAssignStatement(context);
    }

    public override object VisitFunctionCallStatement(miniPythonParser.FunctionCallStatementContext context)
    {
        return base.VisitFunctionCallStatement(context);
    }

    public override object VisitSequence(miniPythonParser.SequenceContext context)
    {
        return base.VisitSequence(context);
    }

    public override object VisitExpression(miniPythonParser.ExpressionContext context)
    {
        return base.VisitExpression(context);
    }

    public override object VisitComparison(miniPythonParser.ComparisonContext context)
    {
        return base.VisitComparison(context);
    }

    public override object VisitAdditionExpression(miniPythonParser.AdditionExpressionContext context)
    {
        return base.VisitAdditionExpression(context);
    }

    public override object VisitMultiplicationExpression(miniPythonParser.MultiplicationExpressionContext context)
    {
        return base.VisitMultiplicationExpression(context);
    }

    public override object VisitElementExpression(miniPythonParser.ElementExpressionContext context)
    {
        return base.VisitElementExpression(context);
    }

    public override object VisitExpressionList(miniPythonParser.ExpressionListContext context)
    {
        return base.VisitExpressionList(context);
    }

    public override object VisitPrimitiveExpression(miniPythonParser.PrimitiveExpressionContext context)
    {
        return base.VisitPrimitiveExpression(context);
    }

    public override object VisitListExpression(miniPythonParser.ListExpressionContext context)
    {
        return base.VisitListExpression(context);
    }

    public bool hasErrors(){
        return errorList.Count > 0;
    }
    
    public String toString ( )
    {
        if ( !hasErrors() ) return "0 type/scope errors";
        var builder = new System.Text.StringBuilder();
        foreach (var error in errorList)
        {
            builder.AppendLine(error);
        }
        return builder.ToString();
    }
    
    
}
