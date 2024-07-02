using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class AssignmentStatementExpr : Expression
{
    public Expression Target { get; }
    public Expression Value { get; }

    public AssignmentStatementExpr(Expression target, Expression value) : base(ExprType.AssignmentStatementExpr)
    {
        Target = target;
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var target = (Target as IdentifierExpr)!.Name;
        var targetVariable = env.GetVariable(target);
        targetVariable.Value = Value.Interpret(env);
        return targetVariable.Value;
    }

}

