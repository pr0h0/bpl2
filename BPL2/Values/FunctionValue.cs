using BPL2.Expr;
using BPL2.Lexer;

namespace BPL2.Values;

public abstract class FunctionValue : RuntimeValue
{
    public override string Type() => "FUNCTION";

    public FunctionValue() { }
}


public class CustomFunctionValue : FunctionValue
{
    public Token Name;
    public List<Tuple<Token, string>> Args;
    public BlockStatementExpr Body;
    public Env.Environment Scope;

    public override string Type() => "CUSTOM_FUNCTION";

    public CustomFunctionValue(Token name, List<Tuple<Token, string>> args, BlockStatementExpr body, Env.Environment env)
    {
        Name = name;
        Args = args;
        Body = body;
        Scope = env;
    }

}

public class STLFunctionValue : FunctionValue
{
    public Token Name;
    public List<Tuple<Token, string>> Args;
    public Func<List<RuntimeValue>, RuntimeValue> Func;

    public override string Type() => "STL_FUNCTION";

    public STLFunctionValue(Token name, List<Tuple<Token, string>> args, Func<List<RuntimeValue>, RuntimeValue> func)
    {
        Name = name;
        Args = args;
        Func = func;
    }

}

