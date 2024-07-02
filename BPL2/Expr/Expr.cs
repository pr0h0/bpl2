using BPL2.Exceptions;
using BPL2.Values;

namespace BPL2.Expr;

public class Expression
{
    public string Type;
    public Expression(string type)
    {
        Type = type;
    }

    public virtual RuntimeValue Interpret(Env.Environment _)
    {
        throw new InterpreterException("Interpret not yet implemented", this);
    }

    public override string ToString()
    {
        return $"Expression<{Type}>";
    }
}

