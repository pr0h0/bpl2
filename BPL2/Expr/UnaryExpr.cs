using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class UnaryExpr : Expression
{

    public Expression Value;
    public Token Operator;
    private Env.Environment? Env = null;

    public UnaryExpr(Token op, Expression value) : base(ExprType.UnaryExpr)
    {
        Value = value;
        Operator = op;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        Env = env;
        if (Operator.Type == TokenType.Increment)
        {
            return InterpretIncrement();
        }
        if (Operator.Type == TokenType.Decrement)
        {
            return InterpretDecrement();
        }

        if (Operator.Type == TokenType.Not)
        {
            return InterpretNot();
        }
        if (Operator.Type == TokenType.Minus)
        {
            return InterpretMinus();
        }

        throw new InterpreterException($"Unary operator {Operator.Type} not implemented");
    }

    public override string ToString()
    {
        return base.ToString();
    }

    private RuntimeValue InterpretIncrement()
    {
        NumberValue value = Value.Interpret(Env!) as NumberValue ?? throw new InterpreterException("Increment target is null??");

        if (Value is IdentifierExpr identifierExpr)
        {
            var val = Env?.GetVariable(identifierExpr.Name);
            if (val != null)
            {
                val.Value = new NumberValue(((NumberValue)val.Value).Value + 1);
                Env?.SetValue(val.Name, val.Value);
                return val.Value;
            }
        }
        else
        {
            value.Value += 1;
        }
        return value;
    }

    private RuntimeValue InterpretDecrement()
    {
        NumberValue value = Value.Interpret(Env!) as NumberValue ?? throw new InterpreterException("Decrement target is null??");
        if (Value is IdentifierExpr identifierExpr)
        {
            var val = Env?.GetVariable(identifierExpr.Name);
            if (val != null)
            {
                val.Value = new NumberValue(((NumberValue)val.Value).Value - 1);
                Env?.SetValue(val.Name, val.Value);
                return val.Value;
            }
        }
        else
        {
            value.Value -= 1;
        }
        return value;
    }

    private RuntimeValue InterpretNot()
    {
        BoolValue? value = Value.Interpret(Env!) as BoolValue ?? throw new InterpreterException("Not target is null??");
        value.Value = !value.Value;
        return value;
    }

    private RuntimeValue InterpretMinus()
    {
        NumberValue? value = Value.Interpret(Env!) as NumberValue ?? throw new InterpreterException("Minus target is null??");
        value.Value = -value.Value;
        return value;
    }

}

