using Antlr4.Runtime.Tree;

namespace Mini_Python.compilador.CodeGen;
using System.Text;
using Antlr4.Runtime;
using parser.generated;  

public class CodeGeneration : miniPythonParserBaseVisitor<object>
{
    private int nivelActual = 0;
    private List<Dictionary<string, string>> scopeStack = new List<Dictionary<string, string>>();

    private class Instruction{
        private string instr;
        private string value;
        public Instruction(string instruc, string valor)
        {
            instr = instruc;
            value = valor;
        }
        public Instruction(string instruc)
        {
            instr = instruc;
        }
        public string Value
        {
            set { this.value = value; }
        }
        public override string ToString() {
            return value != null ? instr + " " + value : instr;
           
        }
    }
    private List<Instruction> bytecode; 
 
    public CodeGeneration() {
        bytecode =new List<Instruction>();
        scopeStack.Add(new Dictionary<string, string>());
        
    }
    public override object VisitProgram(miniPythonParser.ProgramContext context)
    {
        // Visitar todas las declaraciones dentro del programa
        foreach (var statement in context.mainStatement())
        {
            Visit(statement);
        }
        // Agregar la instrucción END al final del programa
        bytecode.Add(new Instruction("END"));
        return null;
        //return base.VisitProgram(context);
    }
    public override object VisitMainStatement(miniPythonParser.MainStatementContext context)
    {
        return base.VisitMainStatement(context);
    }
    public override object VisitStatement(miniPythonParser.StatementContext context)
    {
        return base.VisitStatement(context);
    }
    public override object VisitDefStatement(miniPythonParser.DefStatementContext context)
    {
        bytecode.Add(new Instruction("DEF", context.IDENTIFIER().GetText()));
        nivelActual++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.argList());
        Visit(context.sequence());
        nivelActual--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        return null;
        //return base.VisitDefStatement(context);
    }
    public override object VisitArgList(miniPythonParser.ArgListContext context)
    {
        if (context.IDENTIFIER() != null)
        {
            foreach (var identifier in context.IDENTIFIER())
            {
                var name = identifier.GetText()+"_"+nivelActual;
                bytecode.Add(new Instruction("PUSH_LOCAL", name));
                scopeStack[nivelActual].Add(identifier.GetText(), name);
            }
        }
        return null;
       // return base.VisitArgList(context);
    }
    public override object VisitIfStatement(miniPythonParser.IfStatementContext context)
    {
        // Visitar la condición del if
        Visit(context.expression());
        // Agregar la instrucción JUMP_IF_FALSE con un valor temporal
        bytecode.Add(new Instruction("JUMP_IF_FALSE", ""));
        int jumpIfFalsePos = bytecode.Count - 1;
        // Visitar el bloque de código del if
        nivelActual++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence(0));
        nivelActual--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        // Agregar la instrucción JUMP_ABSOLUTE con un valor temporal
        bytecode.Add(new Instruction("JUMP_ABSOLUTE", ""));
        int jumpAbsolutePos = bytecode.Count - 1;
        // Backpatching: actualizar JUMP_IF_FALSE para saltar al bloque else
        bytecode[jumpIfFalsePos].Value = bytecode.Count.ToString();
        // Viitar el bloque de código del else
        nivelActual++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence(1));
        nivelActual--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        // Backpatching: actualizar JUMP_ABSOLUTE para saltar al final del else
        bytecode[jumpAbsolutePos].Value = bytecode.Count.ToString();
       
        return null;
        //return base.VisitIfStatement(context);
    }
    public override object VisitWhileStatement(miniPythonParser.WhileStatementContext context)
    {
        int startPos = bytecode.Count;
        Visit(context.expression());
        bytecode.Add(new Instruction("JUMP_IF_FALSE", ""));
        int jumpIfFalsePos = bytecode.Count - 1;
        nivelActual++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence());
        nivelActual--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        bytecode.Add(new Instruction("JUMP_ABSOLUTE", startPos.ToString()));
        bytecode[jumpIfFalsePos].Value = bytecode.Count.ToString();
        return null;
        
        //return base.VisitWhileStatement(context);
    }
    public override object VisitReturnStatement(miniPythonParser.ReturnStatementContext context)
    {
        Visit(context.expression());
        bytecode.Add(new Instruction("RETURN_VALUE"));
        return null;
        //return base.VisitReturnStatement(context);
    }
    public override object VisitForStatement(miniPythonParser.ForStatementContext context)
    {
        Visit(context.expressionList()); // Iteración sobre la lista de expresiones
        string varName = context.expression().GetText()+"_"+nivelActual;
        bytecode.Add(new Instruction("STORE_FAST", varName));
        nivelActual++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence());
        nivelActual--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        return null;
    }
    public override object VisitPrintStatement(miniPythonParser.PrintStatementContext context)
    { // Determinar el número de expresiones manualmente
        int numArgs = 0;
        if (context.expression() != null)
        {
            foreach (var expr in context.expression().children)
            {
                Visit(expr);
                numArgs++;
            }
        }
        // Cargar la función global `print`
        bytecode.Add(new Instruction("LOAD_GLOBAL", context.PRINT().GetText()));
        bytecode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        return null;
        //return base.VisitPrintStatement(context);
    }
    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context)
    {
        var variableName = context.IDENTIFIER().GetText()+"_"+nivelActual;
        if (context.firstDefinition)
        {
            if (nivelActual == 0) {
                scopeStack[nivelActual].Add(context.IDENTIFIER().GetText(), variableName);
                bytecode.Add(new Instruction("PUSH_GLOBAL", variableName));    
                
            }else{
            scopeStack[nivelActual].Add(context.IDENTIFIER().GetText(), variableName);
            bytecode.Add(new Instruction("PUSH_LOCAL", variableName));
            }
        }    
        Visit(context.expression());
        if (nivelActual == 0)
        {
            bytecode.Add(new Instruction("STORE_GLOBAL", variableName));
        }else
        {
            bytecode.Add(new Instruction("STORE_FAST", variableName));    
        }
        
        return null;
        //return base.VisitAssignStatement(context);
    }
    public override object VisitFunctionCallStatement(miniPythonParser.FunctionCallStatementContext context)
    {
        var numArgs = 0;
        foreach (var expr in context.expressionList().expression())
        {
            Visit(expr);
            numArgs++;
        }
        bytecode.Add(new Instruction("LOAD_GLOBAL", context.IDENTIFIER().GetText()));
        bytecode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        return null;
     //   return base.VisitFunctionCallStatement(context);
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
        
        Visit(context.additionExpression());

        bytecode.Add(new Instruction("COMPARE_OP", context.GetChild(0).GetText()));
        return null;
       // return base.VisitComparison(context);
    }
    public override object VisitAdditionExpression(miniPythonParser.AdditionExpressionContext context)
    {
        Visit(context.multiplicationExpression(0));
        for (int i = 1; i < context.multiplicationExpression().Length; i++) {
            Visit(context.multiplicationExpression(i));
            var operatorToken = context.GetChild((i * 2) - 1).GetText();
            if (operatorToken == "+") {
                bytecode.Add(new Instruction("BINARY_ADD"));
            } else if (operatorToken == "-") {
                bytecode.Add(new Instruction("BINARY_SUBTRACT"));
            }
            else if (operatorToken == "or") {
                bytecode.Add(new Instruction("BINARY_OR"));
            }
            else if (operatorToken == "and") {
                bytecode.Add(new Instruction("BINARY_AND"));
            }
            else if (operatorToken == "%") {
                bytecode.Add(new Instruction("BINARY_MODULO"));
            }
        }
        return null;
        //return base.VisitAdditionExpression(context);
    }
    public override object VisitMultiplicationExpression(miniPythonParser.MultiplicationExpressionContext context)
    {
        Visit(context.elementExpression(0));
        for (int i = 1; i < context.elementExpression().Length; i++) {
            Visit(context.elementExpression(i));
            var operatorToken = context.GetChild((i * 2) - 1).GetText();
            if (operatorToken == "*") {
                bytecode.Add(new Instruction("BINARY_MULTIPLY"));
            } else if (operatorToken == "/") {
                bytecode.Add(new Instruction("BINARY_DIVIDE"));
            }
        }
        return null;
        //return base.VisitMultiplicationExpression(context);
    }
    public override object VisitElementExpression(miniPythonParser.ElementExpressionContext context)
    {
        Visit(context.primitiveExpression());
        if (context.LBRACKET() != null && context.expression() != null)
        {
            Visit(context.expression());
            bytecode.Add(new Instruction("BINARY_SUBSCR"));
        }
        return null;
        //return base.VisitElementExpression(context);
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
        Visit(context.expression());
        bytecode.Add(new Instruction("LOAD_GLOBAL", "len"));
        bytecode.Add(new Instruction("CALL_FUNCTION", "1"));
        return null;
    }
    public override object VisitPrimitiveExpressionlistAST(miniPythonParser.PrimitiveExpressionlistASTContext context)
    {
        return base.VisitPrimitiveExpressionlistAST(context);
    }
    public override object VisitPrimitiveExpressionliteralAST(miniPythonParser.PrimitiveExpressionliteralASTContext context)
    {
        string literal = context.GetText();
        bytecode.Add(new Instruction("LOAD_CONST", literal));
        return null;
    }
    public override object VisitPrimitiveExpressionidentifierListAST(miniPythonParser.PrimitiveExpressionidentifierListASTContext context)
    {
        var identifier = context.IDENTIFIER().GetText();
        string variableName = null;
        var contadorNivel = -1;
        // Buscar la variable en los scopes, desde el nivel actual hacia atrás
        for (int i = nivelActual; i >= 0; i--)
        {
            if (scopeStack[i].ContainsKey(identifier))
            {
                variableName = scopeStack[i][identifier];
                contadorNivel = i;       
                break;
            }
        } 
        if (context.expressionList() != null)
        {
            var numArgs = 0;
            foreach (var expr in context.expressionList().expression())
            {
                Visit(expr);
                numArgs++;
            }
            bytecode.Add(new Instruction("LOAD_GLOBAL", identifier));
            bytecode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        }
        else
        {
            if (contadorNivel == 0) {
                bytecode.Add(new Instruction("LOAD_GLOBAL", variableName));
            }
            else
            {
            bytecode.Add(new Instruction("LOAD_FAST", variableName));
            }
        }
        return null;
    }
    public override object VisitListExpression(miniPythonParser.ListExpressionContext context)
    {
        var numElements = 0;
        foreach (var expr in context.expressionList().expression())
        {
            Visit(expr);
            numElements++;
        }
        bytecode.Add(new Instruction("BUILD_LIST", numElements.ToString()));
        return null;
        //return base.VisitListExpression(context);
    }
    public override string ToString() {
        var sb = new StringBuilder();
        int cont = 0;
        foreach (Instruction i in bytecode) {
            sb.AppendLine($"{cont++} {i}");
        }
        return sb.ToString();
    }
}