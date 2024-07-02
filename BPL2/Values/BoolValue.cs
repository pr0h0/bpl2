namespace BPL2.Values;

public class BoolValue : RuntimeValue
{
    public bool Value;
    public override string Type() => "BOOL";

    public BoolValue(bool value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value <BOOL> {Value}";
    }
}

