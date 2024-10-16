﻿namespace Mini_Python.compilador.Checker;
using parser.generated;    
using Antlr4.Runtime;
public class ContextAnalizer : miniPythonParserBaseVisitor<object> {

    private TablaSimbolos TablaSimbolosProyecto;
    public List<string> errorList ;
    public ContextAnalizer() {
        TablaSimbolosProyecto = new TablaSimbolos();
        errorList =  new List<string>();
    }    
    private void reportError(String error, IToken offendingToken){
        var err = new System.Text.StringBuilder();
        err.Append(error).
            Append(" ").
            Append("line").
            Append(" ").
            Append(offendingToken.Line).
            Append(":").
            Append(offendingToken.Column+1 ).
            Append("");
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
     return base.VisitStatement(context);;
 }
    public override object VisitDefStatement(miniPythonParser.DefStatementContext context) {
        string nombreFuncion = context.IDENTIFIER().GetText();
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreFuncion) != null) {
            reportError($"CONTEXT ERROR  La funcion '{nombreFuncion}' ya esta definida en este scope.", context.IDENTIFIER().Symbol);
        } else
        {
            var nivel = TablaSimbolosProyecto.getNivelActual();
            if (nivel!= 0 && TablaSimbolosProyecto.BuscarEnNivelAnterior(nivel-1, nombreFuncion) != null) {
                var symbol = TablaSimbolosProyecto.BuscarEnNivelAnterior(nivel-1, nombreFuncion);
                if (symbol.Type == SymbolType.Function) {
                    var methodIdent = symbol as TablaSimbolos.MethodIdent;
                    if (methodIdent != null) {
                        int cantidadParametros = methodIdent.Params.Count;
                        int cantidadParametrosNueva = 0;
                        foreach (var param in context.argList().IDENTIFIER())
                        {
                            cantidadParametrosNueva++;
                        }
                        if (cantidadParametros == cantidadParametrosNueva) {
                            reportError($"CONTEXT ERROR  La funcion '{nombreFuncion}' esta siendo redefinida con los mismos {cantidadParametros} parametros.", context.IDENTIFIER().Symbol);
                            return null;
                        }
                    }
                }
            }
            
           // Extraemos la lista de parámetros de la función
           List<string> parametros = new List<string>();
           if (context.argList() != null) {
               // Asumimos que los identificadores de los parámetros están en el argList
               foreach (var param in context.argList().IDENTIFIER()){
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
           Visit(context.DOSPUNTOS());
           Visit(context.NEWLINE());
           Visit(context.sequence());
           TablaSimbolosProyecto.Imprimir();
           TablaSimbolosProyecto.CloseScope();
        }
       return null;
        //return base.VisitDefStatement(context);
    }
    public override object VisitArgList(miniPythonParser.ArgListContext context)
    {
        return base.VisitArgList(context);
    }
    public override object VisitIfStatement(miniPythonParser.IfStatementContext context)
    {
        // Visit the expression inside the if statement
        Visit(context.expression());
        Visit(context.DOSPUNTOS(0));
        Visit(context.NEWLINE(0));
        // Open a new scope for the if block
        TablaSimbolosProyecto.OpenScope();
        Visit(context.sequence(0)); // Visit the sequence of statements inside the if block
        TablaSimbolosProyecto.Imprimir();
        TablaSimbolosProyecto.CloseScope();
        // Check if there is an else block
        Visit(context.ELSE());
        // Open a new scope for the else block
        Visit(context.DOSPUNTOS(1));
        Visit(context.NEWLINE(1));
        TablaSimbolosProyecto.OpenScope();
        Visit(context.sequence(1)); // Visit the sequence of statements inside the else block
        TablaSimbolosProyecto.Imprimir();
        TablaSimbolosProyecto.CloseScope();
    
        return null;
       // return base.VisitIfStatement(context);
    }
    public override object VisitWhileStatement(miniPythonParser.WhileStatementContext context)
    {
        // Visit the expression inside the while statement
        Visit(context.expression());
        Visit(context.DOSPUNTOS());
        Visit(context.NEWLINE());
        // Open a new scope for the while block
        TablaSimbolosProyecto.OpenScope();
        Visit(context.sequence()); // Visit the sequence of statements inside the while block
        TablaSimbolosProyecto.Imprimir();
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
           Visit(context.expression());
        }
        return null;
        //return base.VisitReturnStatement(context);
    }

    public override object VisitForStatement(miniPythonParser.ForStatementContext context)
    {
            // Visit the expressions inside the for statement
            Visit(context.expression()); // Visit the variable in the for loop
            Visit(context.expressionList()); // Visit the list or range in the for loop
            Visit(context.DOSPUNTOS());
            Visit(context.NEWLINE());
            // Open a new scope for the for block
            TablaSimbolosProyecto.OpenScope();
            Visit(context.sequence()); // Visit the sequence of statements inside the for block
            TablaSimbolosProyecto.Imprimir();
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
            Visit(context.expression());
        }
        return null;
    //    return base.VisitPrintStatement(context);
    }
    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context)
    {
        string nombreVariable = context.IDENTIFIER().GetText();
        Visit(context.IDENTIFIER());
        if (TablaSimbolosProyecto.BuscarEnNivelActual(nombreVariable) != null) {
            reportError($"CONTEXT ERROR  La variable '{nombreVariable}' ya esta definida en este scope.", context.IDENTIFIER().Symbol);
        } else {
            Visit(context.ASSIGN());
            // Store the current error count
            int initialErrorCount = errorList.Count;
            Visit(context.expression());

            // Check if new errors were added
            if (errorList.Count > initialErrorCount){
                return null;
            }
            TablaSimbolosProyecto.InsertarVariable(context.IDENTIFIER().Symbol, SymbolType.Variable);
            Visit(context.NEWLINE());
        }
        return null;
         //return base.VisitAssignStatement(context);
    }
    public override object VisitFunctionCallStatement(miniPythonParser.FunctionCallStatementContext context)
    {
        // Check if the function is defined in the symbol table
        var functionName = context.IDENTIFIER().GetText();
        var functionSymbol = TablaSimbolosProyecto.Buscar(functionName);
        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function)
        {
            reportError($"CONTEXT ERROR  La funcion '{functionName}' no esta definida.", context.IDENTIFIER().Symbol);
        }
        else
        {
            // Check if the number of arguments matches the number of parameters
            var methodSymbol = functionSymbol as TablaSimbolos.MethodIdent;
            int numArguments = context.expressionList()?.expression().Length ?? 0;
            int numParameters = methodSymbol.Params.Count;

            if (numArguments != numParameters)
            {
                reportError($"CONTEXT ERROR  La funcion '{functionName}' espera {numParameters} argumentos, pero se pasaron {numArguments}.", context.IDENTIFIER().Symbol);
            }
        } 
        
        return null;
        //return base.VisitFunctionCallStatement(context);
    }
    public override object VisitSequence(miniPythonParser.SequenceContext context) {
        
        return base.VisitSequence(context) ;
    }
    public override object VisitExpression(miniPythonParser.ExpressionContext context) {
        // Visit the addition expression
        var ex1 = Visit(context.additionExpression());
        // Check if the addition expression contains a primitive expression and validate its existence
        var additionExpr = context.additionExpression();
        foreach (var multExpr in additionExpr.multiplicationExpression()) {
            foreach (var elemExpr in multExpr.elementExpression()) {
                if (elemExpr.primitiveExpression() is miniPythonParser.PrimitiveExpressionidentifierListASTContext identifierContext) {
                    string identifier = identifierContext.IDENTIFIER().GetText();
                    if (identifierContext.LPAREN() != null) {
                        // Function call case
                        var functionSymbol = TablaSimbolosProyecto.Buscar(identifier);
                        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function) {
                            reportError($"CONTEXT ERROR La funcion '{identifier}' no esta definida.", identifierContext.IDENTIFIER().Symbol);
                        } else {
                            // Check if the number of arguments matches the number of parameters
                            var methodSymbol = functionSymbol as TablaSimbolos.MethodIdent;
                            int numArguments = identifierContext.expressionList()?.expression().Length ?? 0;
                            int numParameters = methodSymbol.Params.Count;

                            if (numArguments != numParameters) {
                                reportError($"CONTEXT ERROR La funcion '{identifier}' espera {numParameters} argumentos, pero se pasaron {numArguments}.", identifierContext.IDENTIFIER().Symbol);
                            }
                        }
                    } else {
                        // Variable case
                        var symbol = TablaSimbolosProyecto.BuscarEnNivelActual(identifier);
                        if (symbol == null) {
                            symbol= TablaSimbolosProyecto.Buscar(identifier);
                            if (symbol == null) {
                                reportError($"CONTEXT ERROR La variable '{identifier}' no esta definida.", identifierContext.IDENTIFIER().Symbol);
                            }
                        }
                    }
                }
            }
        }    
        return ex1;
       // return base.VisitExpression(context);
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
        string identifier = context.IDENTIFIER().GetText();
        var symbol = TablaSimbolosProyecto.BuscarEnNivelActual(identifier);
        return symbol;
        //return base.VisitPrimitiveExpressionidentifierListAST(context);
    }

    public override object VisitListExpression(miniPythonParser.ListExpressionContext context)
    {
        return base.VisitListExpression(context);
    }
    public bool hasErrors(){
        return errorList.Count > 0;
    }
    public override string ToString ( )
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
