using Antlr4.Runtime;

namespace compilador;

public class CustomErrorStrategy : DefaultErrorStrategy
{
    public override void Recover(Parser recognizer, RecognitionException e)
    {
        // En lugar de lanzar una excepción, registramos el error y continuamos
        NotifyErrorListeners(recognizer, e.OffendingToken, e);
    }

    public override IToken RecoverInline(Parser recognizer)
    {
        // Reportar error sin detener la ejecución con un mensaje más descriptivo
        IToken offendingToken = recognizer.CurrentToken;
        NotifyErrorListeners(recognizer, offendingToken, new InputMismatchException(recognizer));
        
        // Retornar el token actual para continuar con el parseo
        return offendingToken;
    }

    public override void Sync(Parser recognizer)
    {
        // No sincronizamos, lo dejamos continuar sin recuperación
    }

    private void NotifyErrorListeners(Parser recognizer, IToken offendingToken, RecognitionException e)
    {
        string expectedTokens = recognizer.GetExpectedTokens().ToString(recognizer.Vocabulary);
        string msg = $"Error: Se esperaba: {expectedTokens}";

        // Notificar con un mensaje detallado
        recognizer.NotifyErrorListeners(offendingToken, msg, e);
    }   
}
