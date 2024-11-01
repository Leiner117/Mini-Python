namespace Mini_Python.compilador.CodeGen;
using System.Text;
using Antlr4.Runtime;
using parser.generated;  

public class CodeGeneration : miniPythonParserBaseVisitor<object> {
    
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

    public override object VisitDefStatement(miniPythonParser.DefStatementContext context)
    {
        bytecode.Add(new Instruction("DEF", context.IDENTIFIER().GetText()));
        Visit(context.sequence());
        bytecode.Add(new Instruction("END"));
        return null;
        //return base.VisitDefStatement(context);
    }

    public override object VisitArgList(miniPythonParser.ArgListContext context)
    {
        return base.VisitArgList(context);
    }

    public override object VisitIfStatement(miniPythonParser.IfStatementContext context)
    {
        // Visitar la condición del if
        Visit(context.expression());
        // Agregar la instrucción JUMP_IF_FALSE con un valor temporal
        bytecode.Add(new Instruction("JUMP_IF_FALSE", ""));
        int jumpIfFalsePos = bytecode.Count - 1;
        // Visitar el bloque de código del if
        Visit(context.sequence(0));
        // Agregar la instrucción JUMP_ABSOLUTE con un valor temporal
        bytecode.Add(new Instruction("JUMP_ABSOLUTE", ""));
        int jumpAbsolutePos = bytecode.Count - 1;
        // Backpatching: actualizar JUMP_IF_FALSE para saltar al bloque else
        bytecode[jumpIfFalsePos].Value = bytecode.Count.ToString();

        // Viitar el bloque de código del else
        Visit(context.sequence(1));

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
        Visit(context.sequence());
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
        return base.VisitForStatement(context);
    }

    public override object VisitPrintStatement(miniPythonParser.PrintStatementContext context)
    {
        Visit(context.expression());
        bytecode.Add(new Instruction("CALL_FUNCTION", "print"));
        return null;
        //return base.VisitPrintStatement(context);
    }

    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context)
    {
        if (context.firstDefinition)
            bytecode.Add(new Instruction("PUSH_LOCAL", context.IDENTIFIER().GetText()));
        Visit(context.expression());
        bytecode.Add(new Instruction("STORE_FAST", context.IDENTIFIER().GetText()));
        return null;
        //return base.VisitAssignStatement(context);
    }

    public override object VisitFunctionCallStatement(miniPythonParser.FunctionCallStatementContext context)
    {
        Visit(context.expressionList());
        bytecode.Add(new Instruction("CALL_FUNCTION", context.IDENTIFIER().GetText()));
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

        bytecode.Add(new Instruction("COMPARE_OP", context.GetChild(1).GetText()));
        return null;
       // return base.VisitComparison(context);
    }

    public override object VisitAdditionExpression(miniPythonParser.AdditionExpressionContext context)
    {
        for (int i = 0; i < context.multiplicationExpression().Length; i++) {
            Visit(context.multiplicationExpression(i));
            if (i > 0) {
                bytecode.Add(new Instruction("BINARY_ADD"));
            }
        }
        return null;
        //return base.VisitAdditionExpression(context);
    }

    public override object VisitMultiplicationExpression(miniPythonParser.MultiplicationExpressionContext context)
    {
        for (int i = 0; i < context.elementExpression().Length; i++) {
            Visit(context.elementExpression(i));
            if (i > 0) {
                bytecode.Add(new Instruction("BINARY_MULTIPLY"));
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
 
    public override string ToString() {
        var sb = new StringBuilder();
        int cont = 0;
        foreach (Instruction i in bytecode) {
            sb.Append((cont++) + " " + i + "\n");
        }
        return sb.ToString();
    }


}