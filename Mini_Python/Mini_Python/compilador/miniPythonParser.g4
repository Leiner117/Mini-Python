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
defStatement: DEF IDENTIFIER LPAREN argList RPAREN DOSPUNTOS NEWLINE  sequence;
argList: (IDENTIFIER (COMMA IDENTIFIER)*)?;
ifStatement: IF expression DOSPUNTOS NEWLINE sequence  ELSE DOSPUNTOS NEWLINE  sequence;
whileStatement: WHILE expression DOSPUNTOS NEWLINE  sequence;
returnStatement: RETURN expression NEWLINE;
forStatement: FOR expression IN expressionList DOSPUNTOS NEWLINE sequence; 
printStatement: PRINT expression NEWLINE;
assignStatement locals [bool firstDefinition= false] : IDENTIFIER ASSIGN expression NEWLINE;
functionCallStatement: IDENTIFIER LPAREN expressionList RPAREN NEWLINE?;
sequence:  INDENT statement+ DEDENT ;
expression: additionExpression comparison?;
comparison: (LT | GT | LE | GE | EQ) additionExpression;
additionExpression: multiplicationExpression ((PLUS | MINUS| OR | AND|MODULAR) multiplicationExpression)*;
multiplicationExpression: elementExpression ((MULT | DIV) elementExpression)*;
elementExpression: primitiveExpression (LBRACKET expression RBRACKET)?;

expressionList: (expression (COMMA expression)*)?;
primitiveExpression                                                 
    : LPAREN expression RPAREN                                        #primitiveExpressionparenthesisExprAST                               
    | LEN LPAREN expression RPAREN                                    #primitiveExpressionlenAST
    | listExpression                                                  #primitiveExpressionlistAST                      
    | (PLUS | MINUS)? (INTEGER | FLOAT | CHARCONST | STRING)          #primitiveExpressionliteralAST
    | IDENTIFIER (LPAREN expressionList RPAREN)?                      #primitiveExpressionidentifierListAST
    ;
listExpression : LBRACKET expressionList RBRACKET;
   