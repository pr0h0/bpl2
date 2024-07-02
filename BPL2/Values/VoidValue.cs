namespace BPL2.Values;

public class VoidValue : RuntimeValue
{
    public RuntimeValue? Value = null;
    public override string Type() => "VOID";

    public VoidValue() { }

    public override string ToString()
    {
        return $"Value <VOID> ";
    }
}

