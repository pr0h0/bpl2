using BPL2.Exceptions;
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
        var value = Value.Interpret(env);
        if (Target is ObjectAccessExpr objectAccess)
        {
            var target = objectAccess.Target.Value;
            var targetVariable = env.GetVariable(target ?? "");
            var key = objectAccess.Member.Value;
            var objectValue = (targetVariable.Value as ObjectValue);
            if (objectValue == null || objectValue?.Value.ContainsKey(key) == false)
            {
                throw new InterpreterException($"Object doesn't contains member: {key}");
            }

            objectValue!.Value[key] = value;
            return objectValue.Value[key];
        }
        else if (Target is ArrayAccessExpr arrayAccess)
        {
            var index = arrayAccess.Index.Interpret(env);
            if (index is not NumberValue)
            {
                throw new InterpreterException($"Index must be of type NUMBER, but got type {index.Type()}");
            }

            var target = arrayAccess.Target.Value;
            var targetVariable = env.GetVariable(target ?? "");

            if (targetVariable.Value is TupleValue tupleValue)
            {
                var indexAsInt = (int)((NumberValue)index).Value;
                if (indexAsInt >= tupleValue.Value.Count)
                {
                    throw new InterpreterException($"Index out of range: {indexAsInt}");
                }
                tupleValue.Value[indexAsInt] = value;
                return tupleValue.Value[indexAsInt];
            }
            else if (targetVariable.Value is ArrayValue arrayValue)
            {
                var indexAsInt = (int)((NumberValue)index).Value;
                if (indexAsInt >= arrayValue.Value.Count)
                {
                    throw new InterpreterException($"Index out of range: {indexAsInt}");
                }
                arrayValue.Value[indexAsInt] = value;
                return arrayValue.Value[indexAsInt];
            }
            else
            {
                throw new InterpreterException($"Target must be of type TUPLE or ARRAY, but got type {targetVariable.Value.Type()}");
            }
        }
        else
        {
            var targetValue = env.GetVariable((Target as IdentifierExpr)!.Name ?? "");
            targetValue.Value = value;
            return targetValue.Value;
        }
    }

}

