using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class FunctionCallExpr : Expression
{
    public Token Name { get; }
    public List<Expression> Args { get; }
    public Env.Environment? Env;

    public FunctionCallExpr(Token name, List<Expression> args) : base(ExprType.FunctionCallExpr)
    {
        Name = name;
        Args = args;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        Env = env;
        var func = env.GetFunction(Name.Value)!;

        // TODO: Check if we can use func overloading to call same func but two implementations depending on type
        // And check if we cam pass env as arg instead of assigning it to attribute
        return func is STLFunctionValue stlFunc ? InterpretSTLFunc(stlFunc) : InpterpretCustomFunction((CustomFunctionValue)func);
    }

    public RuntimeValue InterpretSTLFunc(STLFunctionValue func)
    {
        var env = new Env.Environment(Env);
        var args = Args.Select(arg => arg.Interpret(env)).ToList();

        var index = 0;
        func.Args.ForEach((arg) =>
        {
            var type = arg.Item1;
            var name = arg.Item2;

            env.DefineVariable(type, new Token("", name, 0), args.ElementAt(index), false);
            index++;
        });

        return func.Func(args);
    }

    public RuntimeValue InpterpretCustomFunction(CustomFunctionValue func)
    {
        var argEnv = new Env.Environment(Env);
        var closureEnv = new Env.Environment(func.Scope);

        var args = Args.Select(arg => arg.Interpret(argEnv));

        var index = 0;
        func.Args.ForEach((arg) =>
        {
            var type = arg.Item1;
            var name = arg.Item2;

            closureEnv.DefineVariable(type, new Token("", name, 0), args.ElementAt(index), false);
            index++;
        });

        try
        {
            return func.Body.Interpret(closureEnv);
        }
        catch (ReturnException ex)
        {
            if (func.Type() != "VOID")
            {
                return ex.Value;
            }
            throw ex;
        }
    }
}
