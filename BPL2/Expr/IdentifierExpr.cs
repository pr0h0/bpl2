using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class IdentifierExpr : Expression
{
    public string Name { get; private set; }
    public IdentifierExpr(string name) : base(ExprType.IdentifierExpr)
    {
        Name = name;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        try
        {
            return env.GetType(Name);
        }
        catch (InterpreterException)
        {
            try
            {
                return env.GetFunction(Name);
            }
            catch (InterpreterException)
            {
                return env.GetVariable(Name)!.Value;
            }
        }
    }

    public override string ToString()
    {
        return $"Exrepssion<{Type}> [{Name}]";
    }
}

