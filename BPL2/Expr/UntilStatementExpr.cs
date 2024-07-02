﻿using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class UntilStatementExpr : Expression
{
    public Expression Condition;
    public Expression Body;
    public Expression? Failsafe;

    public UntilStatementExpr(Expression condition, Expression body, Expression? failsafe = null) : base(ExprType.UntilStatementExpr)
    {
        Condition = condition;
        Body = body;
        Failsafe = failsafe;
    }

    public override RuntimeValue Interpret(Env.Environment parentEnv)
    {
        var env = new Env.Environment(parentEnv);

        var i = 0;
        RuntimeValue? failsafe = Failsafe?.Interpret(env);
        if (Failsafe != null && failsafe is not NumberValue)
        {
            throw new InterpreterException("failsafe value must be number", this);
        }
        var failsafeValue = failsafe != null ? ((NumberValue)failsafe).Value : 0;
        var failSafeExists = Failsafe != null;

        var InterpretCondition = (Expression condition) =>
        {
            var conditionValue = condition.Interpret(env);
            if (conditionValue is not BoolValue)
            {
                throw new InterpreterException("condition must evaluate to bool value", this);
            }
            return ((BoolValue)conditionValue).Value == false;
        };

        while (InterpretCondition(Condition) && (failSafeExists == false || (failSafeExists && i < failsafeValue)))
        {
            try
            {
                Body.Interpret(env);
                i++;
            }
            catch (ContinueException)
            {
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
        return $"Expr <{Type}> [UNITL]";
    }
}
