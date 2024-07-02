using BPL2.Parser;

namespace BPL2.Expr;

public class TupleLiteralExpr : Expression
{
    public List<Expression> Value;
    public TupleLiteralExpr(List<Expression> value) : base(ExprType.TupleLiteralExpr)
    {
        Value = value;
    }
}

