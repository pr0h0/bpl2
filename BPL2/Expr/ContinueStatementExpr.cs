using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ContinueStatementExpr : Expression
{
    public ContinueStatementExpr() : base(ExprType.ContinueStatementExpr) { }

    public override RuntimeValue Interpret(Env.Environment _)
    {
        throw new ContinueException();
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [CONTINUE]";
    }
}

