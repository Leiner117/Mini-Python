namespace compilador
{
    using System.Collections.Generic;
    using Antlr4.Runtime;
    using parser.generated;

    public class MyErrorListener:BaseErrorListener , IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        public List<string> ErrorMsgs;

        public MyErrorListener()
        {
            ErrorMsgs = new List<string>();
        }
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (recognizer is miniPythonParser)
            {
                ErrorMsgs.Add($"PARSER ERROR - line {line}:{charPositionInLine+1} {msg}");
            }
            else if (recognizer is miniPythonLexer)
            {
                ErrorMsgs.Add($"SCANNER ERROR - line {line}:{charPositionInLine+1} {msg}");
            }
            else
            {
                ErrorMsgs.Add("Other Error");
            }
        }
        // Syntax error handling for character-level errors (lexer)
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            ErrorMsgs.Add($"LEXER ERROR - line {line}:{charPositionInLine+1} {msg}");
        }

        public bool HasErrors()
        {
            return ErrorMsgs.Count > 0;
        }

        public override string ToString()
        {
            if (!HasErrors())
            {
                return "0 errors";
            }
            else
            {
                var builder = new System.Text.StringBuilder();
                foreach (var error in ErrorMsgs)
                {
                    builder.AppendLine(error);
                }
                return builder.ToString();
            }
        }
    }
}