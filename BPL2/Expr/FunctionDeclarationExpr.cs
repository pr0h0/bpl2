using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class FunctionDeclarationExpr : Expression
{
    public Token Name { get; }
    public List<Tuple<Token, string>> Args { get; }
    public BlockStatementExpr Body { get; }
    public Token ReturnType { get; }

    public FunctionDeclarationExpr(Token name, List<Tuple<Token, string>> args, BlockStatementExpr body, Token returnType) : base(ExprType.FunctionDeclarationExpr)
    {
        Name = name;
        Args = args;
        Body = body;
        ReturnType = returnType;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {

        var func = new CustomFunctionValue(Name, Args, Body, env);
        return env.DefineFunction(Name, ReturnType, func);
    }

}

