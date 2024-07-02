using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class EmptyExpr : Expression
{
    public EmptyExpr() : base(ExprType.EmptyExpr) { }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        return new VoidValue();
    }

    public override string ToString()
    {
        return $"Expr <{Type}> [EMPTY]";
    }
}

