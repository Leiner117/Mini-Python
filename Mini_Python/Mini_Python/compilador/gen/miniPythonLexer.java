// Generated from C:/Users/leine/OneDrive/Documentos/Github/Mini-Python/Mini_Python/Mini_Python/compilador/miniPythonLexer.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class miniPythonLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		NEWLINE=1, WS=2, INDENT=3, DEDENT=4, DEF=5, IF=6, ELSE=7, WHILE=8, FOR=9, 
		RETURN=10, PRINT=11, IN=12, LEN=13, PLUS=14, MINUS=15, MULT=16, DIV=17, 
		LT=18, GT=19, LE=20, GE=21, EQ=22, ASSIGN=23, COMMA=24, LPAREN=25, RPAREN=26, 
		LBRACKET=27, RBRACKET=28, LBRACE=29, RBRACE=30, COLON=31, INTEGER=32, 
		FLOAT=33, CHARCONST=34, STRING=35, IDENTIFIER=36;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"NEWLINE", "WS", "INDENT", "DEDENT", "DEF", "IF", "ELSE", "WHILE", "FOR", 
			"RETURN", "PRINT", "IN", "LEN", "PLUS", "MINUS", "MULT", "DIV", "LT", 
			"GT", "LE", "GE", "EQ", "ASSIGN", "COMMA", "LPAREN", "RPAREN", "LBRACKET", 
			"RBRACKET", "LBRACE", "RBRACE", "COLON", "INTEGER", "FLOAT", "CHARCONST", 
			"STRING", "IDENTIFIER"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, "'INDENT'", "'DEDENT'", "'def'", "'if'", "'else'", 
			"'while'", "'for'", "'return'", "'print'", "'in'", "'len'", "'+'", "'-'", 
			"'*'", "'/'", "'<'", "'>'", "'<='", "'>='", "'=='", "'='", "','", "'('", 
			"')'", "'['", "']'", "'{'", "'}'", "':'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "NEWLINE", "WS", "INDENT", "DEDENT", "DEF", "IF", "ELSE", "WHILE", 
			"FOR", "RETURN", "PRINT", "IN", "LEN", "PLUS", "MINUS", "MULT", "DIV", 
			"LT", "GT", "LE", "GE", "EQ", "ASSIGN", "COMMA", "LPAREN", "RPAREN", 
			"LBRACKET", "RBRACKET", "LBRACE", "RBRACE", "COLON", "INTEGER", "FLOAT", 
			"CHARCONST", "STRING", "IDENTIFIER"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


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
	        while (indentCount != getSavedIndent()) {
	            if (indentCount > getSavedIndent()) {
	                indentStack.Push(indentCount);
	                tokenQueue.AddLast(createToken(INDENT, "INDENT"+indentCount, next));
	            } else {
	                indentStack.Pop();
	                tokenQueue.AddLast(createToken(DEDENT, "DEDENT"+getSavedIndent(), next));
	            }
	        }
	        pendingDent = false;
	        tokenQueue.AddLast(next);
	        var dequeuedToken = tokenQueue.First.Value;
	        tokenQueue.RemoveFirst();
	        return dequeuedToken;
	    }


	public miniPythonLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "miniPythonLexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	@Override
	public void action(RuleContext _localctx, int ruleIndex, int actionIndex) {
		switch (ruleIndex) {
		case 0:
			NEWLINE_action((RuleContext)_localctx, actionIndex);
			break;
		case 1:
			WS_action((RuleContext)_localctx, actionIndex);
			break;
		case 2:
			INDENT_action((RuleContext)_localctx, actionIndex);
			break;
		case 3:
			DEDENT_action((RuleContext)_localctx, actionIndex);
			break;
		}
	}
	private void NEWLINE_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 0:

			    if (pendingDent) { Channel = Hidden; }
			    pendingDent = true;
			    indentCount = 0;
			    initialIndentToken = null;

			break;
		}
	}
	private void WS_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 1:

			    Channel = Hidden;
			    if (pendingDent) { indentCount += Text.Length; }

			break;
		}
	}
	private void INDENT_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 2:
			 Channel = Hidden; 
			break;
		}
	}
	private void DEDENT_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 3:
			 Channel = Hidden; 
			break;
		}
	}

	public static final String _serializedATN =
		"\u0004\u0000$\u00e0\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002\u0001"+
		"\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004"+
		"\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007"+
		"\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b"+
		"\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002"+
		"\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002"+
		"\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002"+
		"\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002"+
		"\u0018\u0007\u0018\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002"+
		"\u001b\u0007\u001b\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002"+
		"\u001e\u0007\u001e\u0002\u001f\u0007\u001f\u0002 \u0007 \u0002!\u0007"+
		"!\u0002\"\u0007\"\u0002#\u0007#\u0001\u0000\u0003\u0000K\b\u0000\u0001"+
		"\u0000\u0001\u0000\u0003\u0000O\b\u0000\u0001\u0000\u0001\u0000\u0001"+
		"\u0001\u0004\u0001T\b\u0001\u000b\u0001\f\u0001U\u0001\u0001\u0001\u0001"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003\u0001\u0003"+
		"\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003"+
		"\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0005\u0001\u0005"+
		"\u0001\u0005\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\b\u0001\b\u0001\b\u0001\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001"+
		"\t\u0001\t\u0001\t\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001"+
		"\u000b\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\f\u0001\f\u0001\r"+
		"\u0001\r\u0001\u000e\u0001\u000e\u0001\u000f\u0001\u000f\u0001\u0010\u0001"+
		"\u0010\u0001\u0011\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0013\u0001"+
		"\u0013\u0001\u0013\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0001\u0016\u0001\u0016\u0001\u0017\u0001\u0017\u0001"+
		"\u0018\u0001\u0018\u0001\u0019\u0001\u0019\u0001\u001a\u0001\u001a\u0001"+
		"\u001b\u0001\u001b\u0001\u001c\u0001\u001c\u0001\u001d\u0001\u001d\u0001"+
		"\u001e\u0001\u001e\u0001\u001f\u0004\u001f\u00be\b\u001f\u000b\u001f\f"+
		"\u001f\u00bf\u0001 \u0004 \u00c3\b \u000b \f \u00c4\u0001 \u0001 \u0004"+
		" \u00c9\b \u000b \f \u00ca\u0001!\u0001!\u0001!\u0001!\u0001\"\u0001\""+
		"\u0005\"\u00d3\b\"\n\"\f\"\u00d6\t\"\u0001\"\u0001\"\u0001#\u0001#\u0005"+
		"#\u00dc\b#\n#\f#\u00df\t#\u0001\u00d4\u0000$\u0001\u0001\u0003\u0002\u0005"+
		"\u0003\u0007\u0004\t\u0005\u000b\u0006\r\u0007\u000f\b\u0011\t\u0013\n"+
		"\u0015\u000b\u0017\f\u0019\r\u001b\u000e\u001d\u000f\u001f\u0010!\u0011"+
		"#\u0012%\u0013\'\u0014)\u0015+\u0016-\u0017/\u00181\u00193\u001a5\u001b"+
		"7\u001c9\u001d;\u001e=\u001f? A!C\"E#G$\u0001\u0000\u0004\u0002\u0000"+
		"\t\t  \u0001\u000009\u0003\u0000AZ__az\u0004\u000009AZ__az\u00e7\u0000"+
		"\u0001\u0001\u0000\u0000\u0000\u0000\u0003\u0001\u0000\u0000\u0000\u0000"+
		"\u0005\u0001\u0000\u0000\u0000\u0000\u0007\u0001\u0000\u0000\u0000\u0000"+
		"\t\u0001\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000\u0000\u0000\r"+
		"\u0001\u0000\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000\u0000\u0011"+
		"\u0001\u0000\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000\u0000\u0015"+
		"\u0001\u0000\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000\u0000\u0019"+
		"\u0001\u0000\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000\u0000\u001d"+
		"\u0001\u0000\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000\u0000!\u0001"+
		"\u0000\u0000\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%\u0001\u0000\u0000"+
		"\u0000\u0000\'\u0001\u0000\u0000\u0000\u0000)\u0001\u0000\u0000\u0000"+
		"\u0000+\u0001\u0000\u0000\u0000\u0000-\u0001\u0000\u0000\u0000\u0000/"+
		"\u0001\u0000\u0000\u0000\u00001\u0001\u0000\u0000\u0000\u00003\u0001\u0000"+
		"\u0000\u0000\u00005\u0001\u0000\u0000\u0000\u00007\u0001\u0000\u0000\u0000"+
		"\u00009\u0001\u0000\u0000\u0000\u0000;\u0001\u0000\u0000\u0000\u0000="+
		"\u0001\u0000\u0000\u0000\u0000?\u0001\u0000\u0000\u0000\u0000A\u0001\u0000"+
		"\u0000\u0000\u0000C\u0001\u0000\u0000\u0000\u0000E\u0001\u0000\u0000\u0000"+
		"\u0000G\u0001\u0000\u0000\u0000\u0001N\u0001\u0000\u0000\u0000\u0003S"+
		"\u0001\u0000\u0000\u0000\u0005Y\u0001\u0000\u0000\u0000\u0007b\u0001\u0000"+
		"\u0000\u0000\tk\u0001\u0000\u0000\u0000\u000bo\u0001\u0000\u0000\u0000"+
		"\rr\u0001\u0000\u0000\u0000\u000fw\u0001\u0000\u0000\u0000\u0011}\u0001"+
		"\u0000\u0000\u0000\u0013\u0081\u0001\u0000\u0000\u0000\u0015\u0088\u0001"+
		"\u0000\u0000\u0000\u0017\u008e\u0001\u0000\u0000\u0000\u0019\u0091\u0001"+
		"\u0000\u0000\u0000\u001b\u0095\u0001\u0000\u0000\u0000\u001d\u0097\u0001"+
		"\u0000\u0000\u0000\u001f\u0099\u0001\u0000\u0000\u0000!\u009b\u0001\u0000"+
		"\u0000\u0000#\u009d\u0001\u0000\u0000\u0000%\u009f\u0001\u0000\u0000\u0000"+
		"\'\u00a1\u0001\u0000\u0000\u0000)\u00a4\u0001\u0000\u0000\u0000+\u00a7"+
		"\u0001\u0000\u0000\u0000-\u00aa\u0001\u0000\u0000\u0000/\u00ac\u0001\u0000"+
		"\u0000\u00001\u00ae\u0001\u0000\u0000\u00003\u00b0\u0001\u0000\u0000\u0000"+
		"5\u00b2\u0001\u0000\u0000\u00007\u00b4\u0001\u0000\u0000\u00009\u00b6"+
		"\u0001\u0000\u0000\u0000;\u00b8\u0001\u0000\u0000\u0000=\u00ba\u0001\u0000"+
		"\u0000\u0000?\u00bd\u0001\u0000\u0000\u0000A\u00c2\u0001\u0000\u0000\u0000"+
		"C\u00cc\u0001\u0000\u0000\u0000E\u00d0\u0001\u0000\u0000\u0000G\u00d9"+
		"\u0001\u0000\u0000\u0000IK\u0005\r\u0000\u0000JI\u0001\u0000\u0000\u0000"+
		"JK\u0001\u0000\u0000\u0000KL\u0001\u0000\u0000\u0000LO\u0005\n\u0000\u0000"+
		"MO\u0005\r\u0000\u0000NJ\u0001\u0000\u0000\u0000NM\u0001\u0000\u0000\u0000"+
		"OP\u0001\u0000\u0000\u0000PQ\u0006\u0000\u0000\u0000Q\u0002\u0001\u0000"+
		"\u0000\u0000RT\u0007\u0000\u0000\u0000SR\u0001\u0000\u0000\u0000TU\u0001"+
		"\u0000\u0000\u0000US\u0001\u0000\u0000\u0000UV\u0001\u0000\u0000\u0000"+
		"VW\u0001\u0000\u0000\u0000WX\u0006\u0001\u0001\u0000X\u0004\u0001\u0000"+
		"\u0000\u0000YZ\u0005I\u0000\u0000Z[\u0005N\u0000\u0000[\\\u0005D\u0000"+
		"\u0000\\]\u0005E\u0000\u0000]^\u0005N\u0000\u0000^_\u0005T\u0000\u0000"+
		"_`\u0001\u0000\u0000\u0000`a\u0006\u0002\u0002\u0000a\u0006\u0001\u0000"+
		"\u0000\u0000bc\u0005D\u0000\u0000cd\u0005E\u0000\u0000de\u0005D\u0000"+
		"\u0000ef\u0005E\u0000\u0000fg\u0005N\u0000\u0000gh\u0005T\u0000\u0000"+
		"hi\u0001\u0000\u0000\u0000ij\u0006\u0003\u0003\u0000j\b\u0001\u0000\u0000"+
		"\u0000kl\u0005d\u0000\u0000lm\u0005e\u0000\u0000mn\u0005f\u0000\u0000"+
		"n\n\u0001\u0000\u0000\u0000op\u0005i\u0000\u0000pq\u0005f\u0000\u0000"+
		"q\f\u0001\u0000\u0000\u0000rs\u0005e\u0000\u0000st\u0005l\u0000\u0000"+
		"tu\u0005s\u0000\u0000uv\u0005e\u0000\u0000v\u000e\u0001\u0000\u0000\u0000"+
		"wx\u0005w\u0000\u0000xy\u0005h\u0000\u0000yz\u0005i\u0000\u0000z{\u0005"+
		"l\u0000\u0000{|\u0005e\u0000\u0000|\u0010\u0001\u0000\u0000\u0000}~\u0005"+
		"f\u0000\u0000~\u007f\u0005o\u0000\u0000\u007f\u0080\u0005r\u0000\u0000"+
		"\u0080\u0012\u0001\u0000\u0000\u0000\u0081\u0082\u0005r\u0000\u0000\u0082"+
		"\u0083\u0005e\u0000\u0000\u0083\u0084\u0005t\u0000\u0000\u0084\u0085\u0005"+
		"u\u0000\u0000\u0085\u0086\u0005r\u0000\u0000\u0086\u0087\u0005n\u0000"+
		"\u0000\u0087\u0014\u0001\u0000\u0000\u0000\u0088\u0089\u0005p\u0000\u0000"+
		"\u0089\u008a\u0005r\u0000\u0000\u008a\u008b\u0005i\u0000\u0000\u008b\u008c"+
		"\u0005n\u0000\u0000\u008c\u008d\u0005t\u0000\u0000\u008d\u0016\u0001\u0000"+
		"\u0000\u0000\u008e\u008f\u0005i\u0000\u0000\u008f\u0090\u0005n\u0000\u0000"+
		"\u0090\u0018\u0001\u0000\u0000\u0000\u0091\u0092\u0005l\u0000\u0000\u0092"+
		"\u0093\u0005e\u0000\u0000\u0093\u0094\u0005n\u0000\u0000\u0094\u001a\u0001"+
		"\u0000\u0000\u0000\u0095\u0096\u0005+\u0000\u0000\u0096\u001c\u0001\u0000"+
		"\u0000\u0000\u0097\u0098\u0005-\u0000\u0000\u0098\u001e\u0001\u0000\u0000"+
		"\u0000\u0099\u009a\u0005*\u0000\u0000\u009a \u0001\u0000\u0000\u0000\u009b"+
		"\u009c\u0005/\u0000\u0000\u009c\"\u0001\u0000\u0000\u0000\u009d\u009e"+
		"\u0005<\u0000\u0000\u009e$\u0001\u0000\u0000\u0000\u009f\u00a0\u0005>"+
		"\u0000\u0000\u00a0&\u0001\u0000\u0000\u0000\u00a1\u00a2\u0005<\u0000\u0000"+
		"\u00a2\u00a3\u0005=\u0000\u0000\u00a3(\u0001\u0000\u0000\u0000\u00a4\u00a5"+
		"\u0005>\u0000\u0000\u00a5\u00a6\u0005=\u0000\u0000\u00a6*\u0001\u0000"+
		"\u0000\u0000\u00a7\u00a8\u0005=\u0000\u0000\u00a8\u00a9\u0005=\u0000\u0000"+
		"\u00a9,\u0001\u0000\u0000\u0000\u00aa\u00ab\u0005=\u0000\u0000\u00ab."+
		"\u0001\u0000\u0000\u0000\u00ac\u00ad\u0005,\u0000\u0000\u00ad0\u0001\u0000"+
		"\u0000\u0000\u00ae\u00af\u0005(\u0000\u0000\u00af2\u0001\u0000\u0000\u0000"+
		"\u00b0\u00b1\u0005)\u0000\u0000\u00b14\u0001\u0000\u0000\u0000\u00b2\u00b3"+
		"\u0005[\u0000\u0000\u00b36\u0001\u0000\u0000\u0000\u00b4\u00b5\u0005]"+
		"\u0000\u0000\u00b58\u0001\u0000\u0000\u0000\u00b6\u00b7\u0005{\u0000\u0000"+
		"\u00b7:\u0001\u0000\u0000\u0000\u00b8\u00b9\u0005}\u0000\u0000\u00b9<"+
		"\u0001\u0000\u0000\u0000\u00ba\u00bb\u0005:\u0000\u0000\u00bb>\u0001\u0000"+
		"\u0000\u0000\u00bc\u00be\u0007\u0001\u0000\u0000\u00bd\u00bc\u0001\u0000"+
		"\u0000\u0000\u00be\u00bf\u0001\u0000\u0000\u0000\u00bf\u00bd\u0001\u0000"+
		"\u0000\u0000\u00bf\u00c0\u0001\u0000\u0000\u0000\u00c0@\u0001\u0000\u0000"+
		"\u0000\u00c1\u00c3\u0007\u0001\u0000\u0000\u00c2\u00c1\u0001\u0000\u0000"+
		"\u0000\u00c3\u00c4\u0001\u0000\u0000\u0000\u00c4\u00c2\u0001\u0000\u0000"+
		"\u0000\u00c4\u00c5\u0001\u0000\u0000\u0000\u00c5\u00c6\u0001\u0000\u0000"+
		"\u0000\u00c6\u00c8\u0005.\u0000\u0000\u00c7\u00c9\u0007\u0001\u0000\u0000"+
		"\u00c8\u00c7\u0001\u0000\u0000\u0000\u00c9\u00ca\u0001\u0000\u0000\u0000"+
		"\u00ca\u00c8\u0001\u0000\u0000\u0000\u00ca\u00cb\u0001\u0000\u0000\u0000"+
		"\u00cbB\u0001\u0000\u0000\u0000\u00cc\u00cd\u0005\'\u0000\u0000\u00cd"+
		"\u00ce\t\u0000\u0000\u0000\u00ce\u00cf\u0005\'\u0000\u0000\u00cfD\u0001"+
		"\u0000\u0000\u0000\u00d0\u00d4\u0005\"\u0000\u0000\u00d1\u00d3\t\u0000"+
		"\u0000\u0000\u00d2\u00d1\u0001\u0000\u0000\u0000\u00d3\u00d6\u0001\u0000"+
		"\u0000\u0000\u00d4\u00d5\u0001\u0000\u0000\u0000\u00d4\u00d2\u0001\u0000"+
		"\u0000\u0000\u00d5\u00d7\u0001\u0000\u0000\u0000\u00d6\u00d4\u0001\u0000"+
		"\u0000\u0000\u00d7\u00d8\u0005\"\u0000\u0000\u00d8F\u0001\u0000\u0000"+
		"\u0000\u00d9\u00dd\u0007\u0002\u0000\u0000\u00da\u00dc\u0007\u0003\u0000"+
		"\u0000\u00db\u00da\u0001\u0000\u0000\u0000\u00dc\u00df\u0001\u0000\u0000"+
		"\u0000\u00dd\u00db\u0001\u0000\u0000\u0000\u00dd\u00de\u0001\u0000\u0000"+
		"\u0000\u00deH\u0001\u0000\u0000\u0000\u00df\u00dd\u0001\u0000\u0000\u0000"+
		"\t\u0000JNU\u00bf\u00c4\u00ca\u00d4\u00dd\u0004\u0001\u0000\u0000\u0001"+
		"\u0001\u0001\u0001\u0002\u0002\u0001\u0003\u0003";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}