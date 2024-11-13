namespace Mini_Python.compilador.CodeGen;

public class ProcessResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }

    public ProcessResult(bool isSuccess, string message = "", string output = "", string error = "")
    {
        IsSuccess = isSuccess;
        Message = message;
        Output = output;
        Error = error;
    }

    public override string ToString()
    {
        return IsSuccess ? Output : Error;
    }
    
}