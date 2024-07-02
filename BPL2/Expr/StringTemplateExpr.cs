using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class StringTemplateExpr : Expression
{
    public List<Expression> Value;
    public StringTemplateExpr(List<Expression> value) : base(ExprType.StringTemplateExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var values = Value.Select(v => v.Interpret(env)).ToList();

        return new StringValue(string.Join("", values.Select(v => ((dynamic)v).Value)));
    }
}

