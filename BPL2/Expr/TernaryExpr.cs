using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class TernaryExpr : Expression
{

    public Expression Condition;
    public Expression Then;
    public Expression Else;
    public TernaryExpr(Expression condition, Expression then, Expression _else) : base(ExprType.TernaryExpr)
    {
        Condition = condition;
        Then = then;
        Else = _else;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        if (Condition.Interpret(env) is BoolValue { Value: true })
        {
            return Then.Interpret(env);
        }
        return Else.Interpret(env);
    }
}

