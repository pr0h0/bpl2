using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class StringLiteralExpr : Expression
{
    public string Value;
    public StringLiteralExpr(string value) : base(ExprType.StringLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        return new StringValue(Value);
    }

    public override string ToString()
    {
        return $"Expr <{Type}> [{Value}]";
    }
}

