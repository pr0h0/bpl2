using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class NumberLiteralExpr : Expression
{
    public string Value;
    public NumberLiteralExpr(string value) : base(ExprType.NumberLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        float value;
        try
        {
            value = float.Parse(Value);
        }
        catch
        {
            value = int.Parse(Value);
        }
        return new NumberValue(value);
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [{Value}]";
    }
}

