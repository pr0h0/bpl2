namespace BPL2.Values;

public class ObjectValue : RuntimeValue
{
    public Dictionary<string, RuntimeValue> Value;
    public override string Type() => "OBJECT";

    public ObjectValue(Dictionary<string, RuntimeValue> value)
    {
        Value = value;
    }
}
