using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class IfStatementExpr : Expression
{
    public Expression Condition { get; }
    public Expression Then { get; }
    public Expression? Else { get; }

    public IfStatementExpr(Expression condition, Expression then, Expression? _else = null) : base(ExprType.IfStatementExpr)
    {
        Condition = condition;
        Then = then;
        Else = _else;
    }

    public override RuntimeValue Interpret(Env.Environment parentEnv)
    {
        var env = new Env.Environment(parentEnv);

        BoolValue condition = (BoolValue)Condition.Interpret(env);
        if (condition.Value == true)
        {
            Then.Interpret(env);
        }
        else
        {
            Else?.Interpret(env);
        }

        return new VoidValue();
    }

    public override string ToString()
    {
        return $"Expr <{Type}> [IF]";
    }

}
