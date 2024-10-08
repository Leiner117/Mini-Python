﻿namespace Mini_Python.compilador.Checker;
using parser.generated;    
using Antlr4.Runtime;
public class ContextAnalizer : miniPythonParserBaseVisitor<object> {

    private TablaSimbolos TablaSimbolosProyecto;
    private List<string> errorList ;
    public ContextAnalizer() {
        TablaSimbolosProyecto = new TablaSimbolos();
        errorList =  new List<string>();
    }    
    private void reportError(String error, IToken offendingToken){
        var err = new System.Text.StringBuilder();
        //StringBuffer err = new System.Text.StringBuffer();
        err.Append(error).
            Append(" ").
            Append("(").
            Append(offendingToken.Line).
            Append(":").
            Append(offendingToken.Column ).
            Append(")");
        errorList.Add(err.ToString());
    }
    public override object VisitProgram(miniPythonParser.ProgramContext context)
    {
        TablaSimbolosProyecto.OpenScope();
        
        //return null; 
        return base.VisitProgram(context);
    }

    public override object VisitMainStatement(miniPythonParser.MainStatementContext context)
    {
        return base.VisitMainStatement(context);
    }
 public override object VisitStatement(miniPythonParser.StatementContext context) {
     //TablaSimbolosProyecto.CloseScope();
     //TablaSimbolosProyecto.Imprimir();
     
     return base.VisitStatement(context);;
 }
    public override object VisitDefStatement(miniPythonParser.DefStatementContext context) {
        string nombreFuncion = context.IDENTIFIER().GetText();
        //Visit(context.IDENTIFIER());
       // Visit(context.LPAREN());
      //  Visit(context.RPAREN()); 
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreFuncion) != null) {
            reportError($"Error: La funcion '{nombreFuncion}' ya esta definida en este scope.", context.IDENTIFIER().Symbol);
            //errorList.Add($"Error: La funcion '{nombreFuncion}' ya esta definida en este scope.");
        } else
        {
           // Extraemos la lista de parámetros de la función
           List<string> parametros = new List<string>();
         //  Visit(context.argList());
           if (context.argList() != null) {
               // Asumimos que los identificadores de los parámetros están en el argList
               foreach (var param in context.argList().IDENTIFIER()){
                       // Insertamos cada parámetro en la tabla de símbolos
                       parametros.Add(param.GetText()); // Guardamos el parámetro
                       
               }
           }
           // Insertamos la función en la tabla de símbolos con su lista de parámetros
           TablaSimbolosProyecto.InsertarFuncion(context.IDENTIFIER().Symbol, SymbolType.Function, parametros);
           // Insert parameters into the new scope
           TablaSimbolosProyecto.OpenScope();
           if (context.argList() != null) {
               foreach (var param in context.argList().IDENTIFIER()) {
                   TablaSimbolosProyecto.InsertarVariable(param.Symbol, SymbolType.Parameter);
               }
           }
           //Visit(context.DOSPUNTOS());
          // Visit(context.NEWLINE());
           //Visit(context.sequence());
           // Imprimimos la tabla de símbolos para depuración
          // TablaSimbolosProyecto.Imprimir();
           // Cerramos el scope después de procesar la función
           //TablaSimbolosProyecto.CloseScope();
        }
        TablaSimbolosProyecto.Imprimir();
        //return null;
        return base.VisitDefStatement(context);
    }
    public override object VisitArgList(miniPythonParser.ArgListContext context)
    {
        return base.VisitArgList(context);
    }

    public override object VisitIfStatement(miniPythonParser.IfStatementContext context)
    {
        // Visit the expression inside the if statement
        Visit(context.expression());
        // Open a new scope for the if block
        TablaSimbolosProyecto.OpenScope();
        Visit(context.sequence(0)); // Visit the sequence of statements inside the if block
        TablaSimbolosProyecto.CloseScope();

        // Check if there is an else block
        if (context.ELSE() != null)
        {
            // Open a new scope for the else block
            TablaSimbolosProyecto.OpenScope();
            Visit(context.sequence(1)); // Visit the sequence of statements inside the else block
            TablaSimbolosProyecto.CloseScope();
        }
      //  return null;
        return base.VisitIfStatement(context);
    }

    public override object VisitWhileStatement(miniPythonParser.WhileStatementContext context)
    {
        // Visit the expression inside the while statement
 //       Visit(context.expression());

        // Open a new scope for the while block
        TablaSimbolosProyecto.OpenScope();
       // Visit(context.sequence()); // Visit the sequence of statements inside the while block
        TablaSimbolosProyecto.CloseScope();

        return null;
       // return base.VisitWhileStatement(context);
    }

    public override object VisitReturnStatement(miniPythonParser.ReturnStatementContext context)
    {
        // Visit the expression inside the print statement
     //   Visit(context.expression());

        // Check if the expression is a simple identifier (variable)
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText) && !expressionText.Contains("("))
        {
            var expressionSymbol = TablaSimbolosProyecto.BuscarEnNivelActual(expressionText);
           
            if (expressionSymbol == null)
            {
                reportError($"Error: La variable '{expressionText}' no está definida.", context.expression().Start);
               // errorList.Add($"Error: La variable '{expressionText}' no está definida.");
            }
        }
        // For complex expressions, you can add additional validation if needed

        return null;
        //return base.VisitReturnStatement(context);
    }

    public override object VisitForStatement(miniPythonParser.ForStatementContext context)
    {
            // Visit the expressions inside the for statement
         //   Visit(context.expression()); // Visit the variable in the for loop
          //  Visit(context.expressionList()); // Visit the list or range in the for loop

            // Open a new scope for the for block
            TablaSimbolosProyecto.OpenScope();
            Visit(context.sequence()); // Visit the sequence of statements inside the for block
            TablaSimbolosProyecto.CloseScope();

            return null;
       // return base.VisitForStatement(context);
    }

    public override object VisitPrintStatement(miniPythonParser.PrintStatementContext context)
    {
        // Visit the expression inside the print statement
       // Visit(context.expression());

        // Check if the expression is a simple identifier (variable)
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText) && !expressionText.Contains("("))
        {
            var expressionSymbol = TablaSimbolosProyecto.BuscarEnNivelActual(expressionText);
           
            if (expressionSymbol == null)
            {
                reportError($"Error: La variable '{expressionText}' no está definida.",context.expression().Start);
              //  errorList.Add($"Error: La variable '{expressionText}' no está definida.");
            }
        }
        // For complex expressions, you can add additional validation if needed

        return null;
    
    //    return base.VisitPrintStatement(context);
    }
    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context)
    {
        string nombreVariable = context.IDENTIFIER().GetText();
        Visit(context.IDENTIFIER());
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreVariable) != null) {
            reportError($"Error: La variable '{nombreVariable}' ya esta definida en este scope.", context.IDENTIFIER().Symbol);
            //errorList.Add($"Error: La variable '{nombreVariable}' ya esta definida en este scope.");
        } else {
            Visit(context.expression());
            TablaSimbolosProyecto.InsertarVariable(context.IDENTIFIER().Symbol, SymbolType.Variable);
            Visit(context.ASSIGN());
          // 
            Visit(context.NEWLINE());
        }
        TablaSimbolosProyecto.Imprimir();
        //return null;
         return base.VisitAssignStatement(context);
    }
    public override object VisitFunctionCallStatement(miniPythonParser.FunctionCallStatementContext context)
    {
        // Visit the function identifier
    //    Visit(context.IDENTIFIER());
        // Visit the list of expressions (arguments) passed to the function
      //  Visit(context.expressionList());
        // Check if the function is defined in the symbol table
        var functionName = context.IDENTIFIER().GetText();
        var functionSymbol = TablaSimbolosProyecto.BuscarEnNivelActual(functionName);
        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function)
        {
            reportError($"Error: La función '{functionName}' no está definida.", context.IDENTIFIER().Symbol);
          //  errorList.Add($"Error: La función '{functionName}' no está definida.");
        }
        else
        {
            // Check if the number of arguments matches the number of parameters
            var methodSymbol = functionSymbol as TablaSimbolos.MethodIdent;
            int numArguments = context.expressionList()?.expression().Length ?? 0;
            int numParameters = methodSymbol.Params.Count;

            if (numArguments != numParameters)
            {
                reportError($"Error: La función '{functionName}' espera {numParameters} argumentos, pero se pasaron {numArguments}.", context.IDENTIFIER().Symbol);
                //errorList.Add($"Error: La función '{functionName}' espera {numParameters} argumentos, pero se pasaron {numArguments}.");
            }
        } 
        
        return null;
        //return base.VisitFunctionCallStatement(context);
    }
    public override object VisitSequence(miniPythonParser.SequenceContext context) {
        return base.VisitSequence(context) ;
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

    public override object VisitPrimitiveExpressionparenthesisExprAST(miniPythonParser.PrimitiveExpressionparenthesisExprASTContext context)
    {
        return base.VisitPrimitiveExpressionparenthesisExprAST(context);
    }

    public override object VisitPrimitiveExpressionlenAST(miniPythonParser.PrimitiveExpressionlenASTContext context)
    {
        return base.VisitPrimitiveExpressionlenAST(context);
    }

    public override object VisitPrimitiveExpressionlistAST(miniPythonParser.PrimitiveExpressionlistASTContext context)
    {
        return base.VisitPrimitiveExpressionlistAST(context);
    }

    public override object VisitPrimitiveExpressionliteralAST(miniPythonParser.PrimitiveExpressionliteralASTContext context)
    {
        return base.VisitPrimitiveExpressionliteralAST(context);
    }

    public override object VisitPrimitiveExpressionidentifierListAST(miniPythonParser.PrimitiveExpressionidentifierListASTContext context)
    {
        return base.VisitPrimitiveExpressionidentifierListAST(context);
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
