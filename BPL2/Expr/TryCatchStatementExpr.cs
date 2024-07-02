using BPL2.Lexer;
using BPL2.Parser;

namespace BPL2.Expr;

public class TryCatchStatementExpr : Expression
{
    public Expression TryBlock { get; }
    public List<Tuple<Tuple<Token, Token>, Expression>> CatchBlocks { get; }
    public Expression? FinallyBlock { get; }

    public TryCatchStatementExpr(Expression tryBlock, List<Tuple<Tuple<Token, Token>, Expression>> catchBlocks, Expression? finallyBlock = null) : base(ExprType.TryCatchStatementExpr)
    {
        TryBlock = tryBlock;
        CatchBlocks = catchBlocks;
        FinallyBlock = finallyBlock;
    }

}

