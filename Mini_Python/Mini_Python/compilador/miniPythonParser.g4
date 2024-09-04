parser grammar miniPythonParser;
options {
    tokenVocab = miniPythonLexer;
}

program: mainStatement* EOF;

mainStatement: statement;

statement: defStatement
         | ifStatement
         | returnStatement
         | printStatement
         | whileStatement
         | assignStatement
         | functionCallStatement;

defStatement: DEF IDENTIFIER LPAREN argList RPAREN DOSPUNTOS NEWLINE  sequence ;
argList: (IDENTIFIER (COMMA IDENTIFIER)*)?;
ifStatement: IF expression DOSPUNTOS NEWLINE   sequence  ELSE DOSPUNTOS NEWLINE  sequence ?;
whileStatement: WHILE expression DOSPUNTOS NEWLINE  sequence ;
returnStatement: RETURN expression NEWLINE;
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
    : LPAREN (expression | LEN expression) RPAREN
    | listExpression
    | (PLUS | MINUS)? (INTEGER | FLOAT | CHARCONST | STRING)
    | IDENTIFIER (LPAREN expressionList RPAREN)?
    ;
listExpression: LBRACKET expressionList RBRACKET;