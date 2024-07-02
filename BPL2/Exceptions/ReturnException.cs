using BPL2.Values;

namespace BPL2.Exceptions;

public class ReturnException : Exception
{
    public RuntimeValue Value { get; }
    public ReturnException(RuntimeValue value) : base()
    {
        Value = value;
    }

}

