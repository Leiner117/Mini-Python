using System.Text;

namespace compilador.codeGen;
using parser.generated;  

public class codeGeneration : miniPythonParserBaseVisitor<object> {
    
    private class Instruction
    {
        private string instr=null;
        private string value=null;
        public Instruction(string instr, string value)
        {
            this.instr = instr;
            this.value = value;
        }
        public Instruction(string instr)
        {
            this.instr = instr;
        }
      
        public override string ToString() {
            if (value != null)
                return instr + " " + value;
            else
                return instr;
        }
    }

    private List<Instruction> bytecode;
    public codeGeneration() {
        bytecode = new List<Instruction>();
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

    public override object VisitAssignStatement(miniPythonParser.AssignStatementContext context)
    {
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
        StringBuilder sb = new StringBuilder();
        int cont = 0;
        foreach (Instruction i in bytecode) {
            sb.Append((cont++) + " " + i + "\n");
        }
        return sb.ToString();
    }


}