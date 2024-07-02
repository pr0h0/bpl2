using BPL2.Expr;

namespace BPL2.Exceptions;

public class InterpreterException : Exception
{
    public Expression? Expr;
    public InterpreterException(string message, Expression? expr = null) : base(message)
    {
        Expr = expr;
    }

    public override string ToString()
    {
        return $"INTERPRETER{(Expr != null ? $" <{Expr.Type}>" : "")} {Message}";
    }

}

