using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class TypeDeclarationExpr : Expression
{
    public Token Name { get; }
    public List<Tuple<Token, Token>> Members { get; }

    public TypeDeclarationExpr(Token name, List<Tuple<Token, Token>> members) : base(ExprType.TypeDeclarationExpr)
    {
        Name = name;
        Members = members;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var values = new Dictionary<string, Token>();
        foreach (var member in Members)
        {
            values.Add(member.Item1.Value, member.Item2);
        }
        var value = new TypeValue(Name, Token.IDENTIFIER("OBJECT"), values, false);
        env.DefineType(Name, Token.IDENTIFIER("OBJECT"), value);
        return value;
    }


}

