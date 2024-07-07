using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class BreakStatementExpr : Expression
{
    public BreakStatementExpr() : base(ExprType.BreakStatementExpr) { }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        throw new BreakException();
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [BREAK]";
    }
}

