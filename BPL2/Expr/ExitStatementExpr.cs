using BPL2.Exceptions;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ExitStatementExpr : Expression
{
    public Expression Code { get; }
    public Expression Message { get; }

    public ExitStatementExpr(Expression code, Expression message) : base(ExprType.ExitStatementExpr)
    {
        Code = code;
        Message = message;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var code = Code.Interpret(env);
        if (code is not NumberValue)
        {
            throw new RuntimeException($"Exit takes parameter of type NUMBER, but got {code.Type()}");
        }
        var message = (StringValue)Message.Interpret(env);
        throw new EndProgramException((int)((NumberValue)code).Value, message.Value);
    }

}

