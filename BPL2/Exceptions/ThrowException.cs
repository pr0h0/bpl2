using BPL2.Values;

namespace BPL2.Exceptions;

public class ThrowException : Exception
{
    public RuntimeValue Value { get; }

    public ThrowException(RuntimeValue value) : base(value.ToString())
    {
        Value = value;
    }

}

