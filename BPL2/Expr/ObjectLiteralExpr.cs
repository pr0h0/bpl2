using BPL2.Parser;

namespace BPL2.Expr;

public class ObjectLiteralExpr : Expression
{
    public Dictionary<string, Expression> Value;
    public ObjectLiteralExpr(Dictionary<string, Expression> value) : base(ExprType.ObjectLiteralExpr)
    {
        Value = value;
    }
}

