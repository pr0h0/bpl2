using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ForStatementExpr : Expression
{
    public Expression Init;
    public Expression Condition;
    public Expression After;

    public Expression Body;
    public Expression? Failsafe;

    public ForStatementExpr(Expression init, Expression condition, Expression after, Expression body, Expression? failsafe = null) : base(ExprType.ForStatementExpr)
    {
        Init = init;
        Condition = condition;
        After = after;
        Body = body;
        Failsafe = failsafe;
    }

    public override RuntimeValue Interpret(Env.Environment parentEnv)
    {
        var env = new Env.Environment(parentEnv);
        var i = 0;
        var failsafe = Failsafe != null ? ((NumberValue)Failsafe.Interpret(env)).Value : 0;
        var failSafeExists = Failsafe != null;

        var InterpretCondition = (Expression condition) =>
        {
            if (condition is EmptyExpr) return true;

            var conditionValue = condition.Interpret(env);
            if (conditionValue is not BoolValue)
            {
                throw new InterpreterException("Condition must evaluate to bool value", this);
            }
            return ((BoolValue)conditionValue).Value == true;
        };

        Init.Interpret(env);

        while (InterpretCondition(Condition) && (!failSafeExists || (failsafe < i)))
        {
            try
            {
                Body.Interpret(env);
                After.Interpret(env);
                i++;
            }
            catch (ContinueException)
            {
                After.Interpret(env);
                i++;
                continue;
            }
            catch (BreakException)
            {
                break;
            }
        }

        return new VoidValue();

    }

    public override string ToString()
    {
        return $"Expr <{Type}> [FOR]";
    }

}

