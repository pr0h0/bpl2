namespace BPL2.Values;

public class StringValue : RuntimeValue
{
    public string Value;

    public override string Type() => "STRING";

    public StringValue(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value <STRING> {Value}";
    }
}

