parser grammar miniPythonParser;
options {
    tokenVocab = miniPythonLexer;
}

program: mainStatement* EOF;

mainStatement: defStatement| assignStatement ;

statement: defStatement
         | ifStatement
         | returnStatement
         | printStatement
         | whileStatement
         | assignStatement
         | forStatement
         | functionCallStatement;

defStatement: DEF IDENTIFIER LPAREN argList RPAREN DOSPUNTOS NEWLINE  sequence
            | DEF IDENTIFIER LPAREN argList RPAREN { NotifyErrorListeners("Expected ':' after function definition"); }
;
argList: (IDENTIFIER (COMMA IDENTIFIER)*)?;
ifStatement: IF expression DOSPUNTOS NEWLINE   sequence  ELSE DOSPUNTOS NEWLINE  sequence ? 
           |IF expression { NotifyErrorListeners("Expected ':' after 'expression' in If condition"); } NEWLINE sequence
            ELSE DOSPUNTOS NEWLINE  sequence ?
           | IF expression DOSPUNTOS NEWLINE sequence ELSE { NotifyErrorListeners("Expected ':' after 'else' in condition"); }     
           ;
whileStatement: WHILE expression DOSPUNTOS NEWLINE  sequence;
returnStatement: RETURN expression NEWLINE;
forStatement: FOR expression IN expressionList DOSPUNTOS NEWLINE sequence; 
printStatement: PRINT expression NEWLINE;
assignStatement: IDENTIFIER ASSIGN expression NEWLINE;
functionCallStatement: IDENTIFIER LPAREN expressionList RPAREN NEWLINE?;
sequence:  INDENT statement+ DEDENT ;
expression: additionExpression comparison?;
comparison: (LT | GT | LE | GE | EQ) additionExpression;
additionExpression: multiplicationExpression ((PLUS | MINUS) multiplicationExpression)*;
multiplicationExpression: elementExpression ((MULT | DIV) elementExpression)*;
elementExpression: primitiveExpression (LBRACKET expression RBRACKET)?;
                  
expressionList: (expression (COMMA expression)*)?;
primitiveExpression
    : LPAREN expression RPAREN
    | LEN LPAREN expression RPAREN
    | listExpression
    | (PLUS | MINUS)? (INTEGER | FLOAT | CHARCONST | STRING)
    | IDENTIFIER (LPAREN expressionList RPAREN)?
    ;
listExpression : LBRACKET expressionList RBRACKET
     | LBRACKET expressionList { NotifyErrorListeners("Expected ']' after list expression."); }
     ;
   
