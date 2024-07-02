namespace BPL2.Values;

public class NullValue : RuntimeValue
{
    public RuntimeValue? Value = null;
    public override string Type() => "NULL";

    public NullValue() { }

    public override string ToString()
    {
        return $"Value <NULL> {Value}";
    }
}

