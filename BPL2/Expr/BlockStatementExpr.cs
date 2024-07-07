using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class BlockStatementExpr : Expression
{
    public List<Expression> Body { get; }
    public BlockStatementExpr(List<Expression> body) : base(ExprType.BlockStatementExpr)
    {
        Body = body;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var localEnv = new Env.Environment(env);
        foreach (var expr in Body)
        {
            expr.Interpret(localEnv);
        }

        return new VoidValue();
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [BLOCK]";
    }
}

