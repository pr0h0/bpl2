namespace BPL2.Values;

public class NumberValue : RuntimeValue
{
    public float Value;
    public override string Type() => "NUMBER";

    public NumberValue(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value <NUMBER> {Value}";
    }
}

