namespace BPL2.Values;

public class TupleValue : RuntimeValue
{
    public List<RuntimeValue> Value;
    public override string Type() => "TUPLE";

    public TupleValue(List<RuntimeValue> value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value <TUPLE> {string.Join("", Value.Select(v => v.ToString()))}";
    }
}

