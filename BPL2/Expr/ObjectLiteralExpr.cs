using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ObjectLiteralExpr : Expression
{
    public Dictionary<string, Expression> Value;
    public ObjectLiteralExpr(Dictionary<string, Expression> value) : base(ExprType.ObjectLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var values = new Dictionary<string, RuntimeValue>();
        foreach (var member in Value)
        {
            values.Add(member.Key, member.Value.Interpret(env));
        }
        return new ObjectValue(values);
    }
}

