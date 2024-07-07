using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class TryCatchStatementExpr : Expression
{
    public Expression TryBlock;
    public List<Tuple<Tuple<Token, Token>, Expression>> CatchBlocks;
    public Expression? FinallyBlock;

    public TryCatchStatementExpr(Expression tryBlock, List<Tuple<Tuple<Token, Token>, Expression>> catchBlocks, Expression? finallyBlock = null) : base(ExprType.TryCatchStatementExpr)
    {
        TryBlock = tryBlock;
        CatchBlocks = catchBlocks;
        FinallyBlock = finallyBlock;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var tryEnv = new Env.Environment(env);
        try
        {
            TryBlock.Interpret(tryEnv);
        }
        catch (ThrowException ex)
        {
            var catchEnv = new Env.Environment(env);
            var matchingCatch = CatchBlocks.FirstOrDefault(x => CheckIfTypeMatch(ex, x));

            if (matchingCatch == null)
            {
                throw;
            }
            catchEnv.DefineVariable(Token.IDENTIFIER(matchingCatch.Item1.Item2.Value), Token.IDENTIFIER(ex.Value.Type()), ex.Value, true);
            matchingCatch.Item2.Interpret(catchEnv);
        }

        if (FinallyBlock != null && FinallyBlock is not EmptyExpr)
        {
            var finallyEnv = new Env.Environment(env);
            FinallyBlock.Interpret(finallyEnv);
        }
        return new VoidValue();
    }

    private bool CheckIfTypeMatch(ThrowException ex, Tuple<Tuple<Token, Token>, Expression> x)
    {
        // TODO: Check object type if matches
        var contains = new List<string>() { ex.Value.Type(), "ANY" }.Contains(x.Item1.Item1.Value);

        if(!contains) return false;

        return true;
    }
}

