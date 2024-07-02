using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ReturnStatementExpr : Expression
{
    public Expression Value { get; }
    public ReturnStatementExpr(Expression value) : base(ExprType.ReturnStatementExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        throw new ReturnException(Value.Interpret(env));
    }

}

