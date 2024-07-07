using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ArrayLiteralExpr : Expression
{

    public List<Expression> Value;
    public ArrayLiteralExpr(List<Expression> value) : base(ExprType.ArrayLiteralExpr)
    {
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var values = Value.Select(v => v.Interpret(env)).ToList();
        var length = values.Count;
        ArrayValue value = new(new());

        if (length > 0)
        {
            var firstType = (values.ElementAt(0) as NumberValue)?.Type();
            if (values.Where(v => v.Type() != firstType).Any())
            {
                throw new InterpreterException("All elements in ARRAY must be of same type");
            }
            values.ForEach(value.Value.Add);
        }

        return value;
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [ARRAY LITERAL]";
    }
}

