using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class ObjectAccessExpr : Expression
{
    public Token Target { get; }
    public Token Member { get; }

    public ObjectAccessExpr(Token target, Token member) : base(ExprType.ObjectAccessExpr)
    {
        Target = target;
        Member = member;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var obj = env.GetVariable(Target.Value);
        var type = env.GetType(obj.TypeOf.Value);

        var target = Target.Value;
        var member = Member.Value;

        if (obj.Value.Type() != "OBJECT")
        {
            throw new RuntimeException($"Variable [{target}] is not an object, but a [{obj.Value.Type()}]");
        }

        if (!type.Types.TryGetValue(member, out _))
        {
            throw new RuntimeException($"Member <{Member.Value}> not found on [{target}]");
        }

        if (((ObjectValue)obj.Value).Value.TryGetValue(member, out RuntimeValue? val))
        {
            return val;
        }

        throw new RuntimeException($"Unable to get object value [{target}]");
    }
}
