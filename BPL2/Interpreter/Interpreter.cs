using BPL2.Expr;
using BPL2.Values;
using Environment = BPL2.Env.Environment;

namespace BPL2.Interpreter;

public class Interpreter
{
    private readonly List<string> valubleValues = new() { "ANY", "NUMBER", "STRING", "BOOL", "NULL", "OBJECT", "ARRAY", "TUPLE" };
    public Environment Env;
    public Interpreter(Environment? env = null)
    {
        Env = env ?? new Environment();
    }

    public List<RuntimeValue> Interpret(List<Expression> expressions)
    {
        List<RuntimeValue> values = new();
        foreach (var expr in expressions)
        {
            var value = expr.Interpret(Env);
            if (valubleValues.Contains(value.Type()))
            {
                values.Add(value);
            }
        }

        return values;
    }

}

