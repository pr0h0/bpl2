using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class TupleDeclarationExpr : Expression
{
    public Token Name { get; }
    public List<Token> Types { get; }

    public TupleDeclarationExpr(Token name, List<Token> types) : base(ExprType.TupleDeclarationExpr)
    {
        Name = name;
        Types = types;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var mappedType = new Dictionary<string, Token>();
        int index = 0;
        foreach (var type in Types)
        {
            mappedType.Add(index.ToString(), type);
            index++;
        }
        var typeValue = new TypeValue(Name, Token.IDENTIFIER("TUPLE"), mappedType, false);
        env.DefineType(Name, Token.IDENTIFIER("TUPLE"), typeValue);
        return typeValue;
    }

}

