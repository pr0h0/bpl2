using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class NullLiteralExpr : Expression
{
    public NullLiteralExpr() : base(ExprType.NullLiteralExpr) { }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        return new NullValue();
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [null]";
    }
}

