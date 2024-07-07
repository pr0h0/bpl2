using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class TupleLiteralExpr : Expression
{
    public List<Expression> Value;
    public TupleLiteralExpr(List<Expression> value) : base(ExprType.TupleLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var tupleValue = new TupleValue(Value.Select(x => x.Interpret(env)).ToList());
        return tupleValue;
    }
}

