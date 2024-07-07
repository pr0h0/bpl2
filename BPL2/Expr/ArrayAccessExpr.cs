using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ArrayAccessExpr : Expression
{
    public Token Target { get; }
    public Expression Index { get; }
    public ArrayAccessExpr(Token target, Expression index) : base(ExprType.ArrayAccessExpr)
    {
        Target = target;
        Index = index;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var index = Index.Interpret(env);
        var array = env.GetVariable(Target.Value);

        if (array?.Value is ArrayValue || array?.Value is TupleValue || array?.Value is ObjectValue)
        {
            if (array.Value is ArrayValue arrValue)
            {
                return InterpretArrayAccess(index, arrValue);
            }
            if (array.Value is TupleValue tupleValue)
            {
                return InterpretTupleAccess(index, tupleValue);
            }
            //if(array.Value is ObjectValue)
            //{
            return InterpretObjectAccess(index, (ObjectValue)array.Value);
            //}
        }
        else
        {
            throw new InterpreterException($"Unable to access {array?.TypeOf.Value} using index");
        }
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [ARRAY ACCESS]";
    }

    private RuntimeValue InterpretArrayAccess(RuntimeValue index, ArrayValue array)
    {
        if (index is not NumberValue || index.Type() != "NUMBER")
        {
            throw new InterpreterException($"ARRAY can be indexed only with NUMBER, but got {index.Type()}");
        }
        var indexNumber = index as NumberValue;

        var arrValue = array.Value;
        if (arrValue.Count <= indexNumber?.Value)
        {
            throw new InterpreterException($"ARRAY index out of bounds, ARRAY length is {arrValue.Count} but got {indexNumber.Value}");
        }
        return arrValue.ElementAt((int)(indexNumber!.Value));
    }

    private RuntimeValue InterpretTupleAccess(RuntimeValue index, TupleValue array)
    {
        if (index is not NumberValue || index.Type() != "NUMBER")
        {
            throw new InterpreterException($"TUPLE can be indexed only with NUMBER, but got {index.Type()}");
        }
        var indexNumber = index as NumberValue;

        var arrValue = array.Value;
        if (arrValue.Count <= indexNumber?.Value)
        {
            throw new InterpreterException($"TUPLE index out of bounds, TUPLE length is {arrValue.Count} but got {indexNumber.Value}");
        }
        return arrValue.ElementAt((int)(indexNumber!.Value));
    }

    private RuntimeValue InterpretObjectAccess(RuntimeValue index, ObjectValue array)
    {
        var indexValue = ((StringValue)index).Value;
        if (!array.Value.ContainsKey(indexValue))
        {
            throw new InterpreterException($"OBJECT doesn't containt key: {indexValue}");
        }
        var retValue = array.Value.GetValueOrDefault(indexValue);
        return retValue ?? throw new InterpreterException($"OBJECT value not found for key: {indexValue}");
    }
}

