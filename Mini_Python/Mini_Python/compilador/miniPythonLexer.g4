lexer grammar miniPythonLexer;

@lexer::members {
    private bool pendingDent = true;
    private int indentCount = 0;
    private LinkedList<IToken> tokenQueue = new LinkedList<IToken>();
    private Stack<int> indentStack = new Stack<int>();
    private IToken initialIndentToken = null;
    private int getSavedIndent() {
        return indentStack.Count == 0 ? 0 : indentStack.Peek();
    }
    private CommonToken createToken(int type, string text, IToken next) {
        CommonToken token = new CommonToken(type, text);
        if (initialIndentToken != null) {
            token.StartIndex = initialIndentToken.StartIndex;
            token.Line = initialIndentToken.Line;
            token.Column = initialIndentToken.Column;
            token.StopIndex = next.StartIndex - 1;
        }
        return token;
    }
    public override IToken NextToken() {
        if (tokenQueue.Count > 0) {
            var firstToken = tokenQueue.First.Value;
            tokenQueue.RemoveFirst();
            return firstToken;
        }
        IToken next = base.NextToken();
        if (pendingDent && initialIndentToken == null && next.Type != NEWLINE) {
            initialIndentToken = next;
        }
        if (next == null || next.Channel == Hidden || next.Type == NEWLINE) {
            return next;
        }
        if (next.Type == TokenConstants.EOF) {
                            indentCount = 0;
                            if (!pendingDent) {
                                initialIndentToken = next;
                                tokenQueue.AddLast(createToken(NEWLINE, "NEWLINE", next));
                            }
                        }
        // Ajustar las diferencias entre el conteo actual de indentaciones y el último guardado
        while (indentCount != getSavedIndent()) {
            int difference = Math.Abs(indentCount - getSavedIndent());
            if (difference == 0|| difference == 4 || indentStack.Contains(difference) || difference == getSavedIndent()) {
                if (indentCount > getSavedIndent()) {
                    indentStack.Push(indentCount);
                    tokenQueue.AddLast(createToken(INDENT, "INDENT", next));
                } else {
                    indentStack.Pop();
                    tokenQueue.AddLast(createToken(DEDENT, "DEDENT", next));
                }
            }else
            {
                return next;
            }               
        }
        pendingDent = false;
        tokenQueue.AddLast(next);
        var dequeuedToken = tokenQueue.First.Value;
        tokenQueue.RemoveFirst();
        return dequeuedToken;
    }
}

NEWLINE : ('\r'? '\n' | '\r') {
    if (pendingDent){  Channel = Hidden;} 
    pendingDent = true;
    indentCount = 0;
    initialIndentToken = null;
} ;
WS : [ \t]+ {
    Channel = Hidden;
    if (pendingDent) { indentCount += Text.Length; }
} ;
INDENT : 'INDENT' { Channel = Hidden; }; 
DEDENT : 'DEDENT' { Channel = Hidden; };
BlockComment : '\'\'\'' ( . | NEWLINE )*? '\'\'\'' -> channel(HIDDEN) ;
ComillasDoblesComment: '"""' ( . | NEWLINE )*? '"""' -> channel(HIDDEN) ;
LineComment : '#' ~[\r\n]* -> channel(HIDDEN) ;

// Palabras clave
DEF:            'def';
IF:             'if';
ELSE:           'else';
WHILE:          'while';
FOR:            'for';
RETURN:         'return';
PRINT:          'print';
IN:             'in';
LEN:            'len';

// Operadores
PLUS:           '+';
MINUS:          '-';
MULT:           '*';
DIV:            '/';
LT:             '<';
GT:             '>';
LE:             '<=';
GE:             '>=';
EQ:             '==';
OR:             'or';
AND:            'and';
MODULAR:        '%';
ASSIGN:         '=';
COMMA:          ',';
LPAREN:         '(';
RPAREN:         ')';
LBRACKET:       '[';
RBRACKET:       ']';
LBRACE:         '{';
RBRACE:         '}';
DOSPUNTOS:      ':';

// Tipos de datos
INTEGER:        [0-9]+;
FLOAT:          [0-9]+ '.' [0-9]+;
CHARCONST:      '\'' .*? '\'';
STRING:         '"' .*? '"';
IDENTIFIER:     [a-zA-Z_][a-zA-Z_0-9]*;