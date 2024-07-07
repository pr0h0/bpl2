using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class BooleanLiteralExpr : Expression
{
    public string Value;
    public BooleanLiteralExpr(string value) : base(ExprType.BooleanLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        return new BoolValue(Value == "true");
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [{Value}]";
    }
}

