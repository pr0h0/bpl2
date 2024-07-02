using BPL2.Lexer;
using BPL2.Parser;

namespace BPL2.Expr;

public class ObjectAccessExpr : Expression
{
    public Token Target { get; }
    public Expression Member { get; }
    public ObjectAccessExpr(Token target, Expression member) : base(ExprType.ObjectAccessExpr)
    {
        Target = target;
        Member = member;
    }

}

