namespace BPL2.Values;

public abstract class RuntimeValue
{
    public RuntimeValue() { }

    public virtual string Type() => "RUNTIME";

    public override string ToString()
    {
        return "Runtime Value";
    }
}

