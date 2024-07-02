using BPL2.Lexer;
using BPL2.Parser;

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

}

