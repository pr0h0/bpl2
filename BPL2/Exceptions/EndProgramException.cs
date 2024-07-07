namespace BPL2.Exceptions;

public class EndProgramException : Exception
{
    public new string Message { get; }
    public int Code;
    public EndProgramException(int code = 0, string message = "") : base(message)
    {
        Code = code;
        Message = message;
    }

}

