using BPL2.Lexer;
using BPL2.Parser;

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

}

