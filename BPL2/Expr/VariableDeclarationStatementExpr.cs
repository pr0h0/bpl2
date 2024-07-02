using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Services;
using BPL2.Values;

namespace BPL2.Expr;

public class VariableDeclarationStatementExpr : Expression
{
    public bool IsConst { get; }
    public Token Name { get; }
    public Token TypeOf { get; }
    public Expression Value { get; }
    public VariableDeclarationStatementExpr(Token name, Token typeOf, Expression value, bool isConst) : base(ExprType.VariableDeclarationStatementExpr)
    {
        IsConst = isConst;
        Name = name;
        TypeOf = typeOf;
        Value = value;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        var value = Value.Interpret(env);
        VariableService.ValidateVariableDeclaration(TypeOf, value);
        if (TypeOf.Value == "FUNC" && value is FunctionValue funValue)
        {
            env.DefineFunction(Name, TypeOf, funValue);
        }
        else
        {
            env.DefineVariable(Name, TypeOf, value, IsConst);
        }
        return value;
    }

    public override string ToString()
    {
        return $"Expr <{Type}> [{(IsConst ? "const" : "var")} {Name.Value} : {TypeOf.Value}]";
    }

}

