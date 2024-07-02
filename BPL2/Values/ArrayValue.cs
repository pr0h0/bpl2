namespace BPL2.Values;

public class ArrayValue : RuntimeValue
{
    public List<RuntimeValue> Value;
    public override string Type() => "ARRAY";

    public ArrayValue(List<RuntimeValue> value)
    {
        Value = value;
    }


    public override string ToString()
    {
        return $"Value <ARRAY> {string.Join("", Value.Select(v => v.ToString()))}";
    }
}

