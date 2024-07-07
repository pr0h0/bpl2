using System.Text;

namespace BPL2.Values;

public class ObjectValue : RuntimeValue
{
    public Dictionary<string, RuntimeValue> Value;
    public override string Type() => "OBJECT";

    public ObjectValue(Dictionary<string, RuntimeValue> value)
    {
        Value = value;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("{");
        foreach (var member in Value)
        {
            sb.Append(member.Key);
            sb.Append(": ");
            sb.Append(member.Value.ToString());
            sb.Append(", ");
        }
        sb.Append("}");
        return sb.ToString();
    }
}
