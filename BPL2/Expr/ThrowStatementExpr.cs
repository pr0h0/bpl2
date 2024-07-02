using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ThrowStatementExpr : Expression
{
    public Expression Value { get; }
    public ThrowStatementExpr(Expression value) : base(ExprType.ThrowStatementExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        throw new ThrowException(Value.Interpret(env));
    }

    public override string ToString()
    {
        return $"Expr <{Type}> [{Value}]";
    }

}

