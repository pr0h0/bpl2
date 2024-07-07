using System.Linq;
using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Parser;
using BPL2.Values;

namespace BPL2.Expr;

public class BinaryExpr : Expression
{
    public Expression Left;
    public Token Operator;
    public Expression Right;

    public BinaryExpr(Expression left, Token op, Expression right) : base(ExprType.BinaryExpr)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public override RuntimeValue Interpret(Env.Environment env)
    {
        if (Operator.Type == TokenType.Equal) return EvaluateEqualOperator(env);
        if (Operator.Type == TokenType.NotEqual) return EvaluateNotEqualOperator(env);
        if (Operator.Type == TokenType.Plus) return EvaluatePlusOperator(env);
        if (Operator.Type == TokenType.Minus) return EvaluateMinusOperator(env);
        if (Operator.Type == TokenType.Multiply) return EvaluateMultiplyOperator(env);
        if (Operator.Type == TokenType.Divide) return EvaluateDivideOperator(env);
        if (Operator.Type == TokenType.Exponent) return EvaluateExponentOperator(env);
        if (Operator.Type == TokenType.Modulus) return EvaluateModulusOperator(env);
        if (Operator.Type == TokenType.GreaterThan) return EvaluateGreaterThanOperator(env);
        if (Operator.Type == TokenType.GreaterThanEqual) return EvaluateGreaterThanEqualOperator(env);
        if (Operator.Type == TokenType.LessThan) return EvaluateLessThanOperator(env);
        if (Operator.Type == TokenType.LessThanEqual) return EvaluateLessThanEqualOperator(env);
        if (Operator.Type == TokenType.And) return EvaluateAndOperator(env);
        if (Operator.Type == TokenType.Or) return EvaluateOrOperator(env);

        throw new InterpreterException($"Operator not implemented <{Operator.Type}|{Operator.Value}>");
    }

    public override string ToString()
    {
        return base.ToString();
    }

    private RuntimeValue EvaluateEqualOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        // BELOW ARE SAME TYPE OPERANDS
        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        return ((dynamic)left).Value == ((dynamic)right).Value ? new BoolValue(true) : new BoolValue(false);
    }

    private RuntimeValue EvaluateNotEqualOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        // BELOW ARE SAME TYPE OPERANDS
        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        return ((dynamic)left).Value != ((dynamic)right).Value ? new BoolValue(true) : new BoolValue(false);
    }

    private RuntimeValue EvaluatePlusOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        // HERE WE HANDLE FEW CASES THAT WE CAN ADD DIFFERENT TYPE OPERANDS
        if (leftType == "ARRAY" && rightType != "ARRAY")
        {
            // TODO: Check if RHS is of same type as ARRAY
            return new ArrayValue(((ArrayValue)left).Value.Append(right).ToList());
        }

        // BELOW ARE SAME TYPE OPERANDS
        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new NumberValue(((NumberValue)left).Value + ((NumberValue)right).Value);
        }

        if (leftType == "STRING")
        {
            return new StringValue(((StringValue)left).Value + ((StringValue)right).Value);
        }

        if (leftType == "ARRAY")
        {
            // TODO: Check types of both arrays
            return new ArrayValue(((ArrayValue)left).Value.Concat(((ArrayValue)right).Value).ToList());
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateMinusOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new NumberValue(((NumberValue)left).Value - ((NumberValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateLessThanEqualOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new BoolValue(((NumberValue)left).Value <= ((NumberValue)right).Value);
        }
        if (leftType == "STRING")
        {
            var leftValue = ((StringValue)left).Value;
            var rightValue = ((StringValue)right).Value;

            var leftArray = leftValue.Split(string.Empty);
            var rightArray = rightValue.Split(string.Empty);

            var length = Math.Min(leftArray.Length, rightArray.Length);
            for (var i = 0; i < length; i++)
            {
                if (leftArray[i] == rightArray[i]) continue;
                if (leftArray[i][0] <= rightArray[i][0]) return new BoolValue(true);
                return new BoolValue(false);
            }
            if (leftValue == rightValue) return new BoolValue(true);
            if (leftArray.Length <= rightArray.Length) return new BoolValue(true);
            return new BoolValue(false);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateLessThanOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new BoolValue(((NumberValue)left).Value < ((NumberValue)right).Value);
        }
        if (leftType == "STRING")
        {
            var leftValue = ((StringValue)left).Value;
            var rightValue = ((StringValue)right).Value;

            var leftArray = leftValue.Split(string.Empty);
            var rightArray = rightValue.Split(string.Empty);

            var length = Math.Min(leftArray.Length, rightArray.Length);
            for (var i = 0; i < length; i++)
            {
                if (leftArray[i] == rightArray[i]) continue;
                if (leftArray[i][0] < rightArray[i][0]) return new BoolValue(true);
                return new BoolValue(false);
            }
            if (leftValue == rightValue) return new BoolValue(false);
            if (leftArray.Length < rightArray.Length) return new BoolValue(true);
            return new BoolValue(false);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateGreaterThanEqualOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new BoolValue(((NumberValue)left).Value >= ((NumberValue)right).Value);
        }
        if (leftType == "STRING")
        {
            var leftValue = ((StringValue)left).Value;
            var rightValue = ((StringValue)right).Value;

            var leftArray = leftValue.Split(string.Empty);
            var rightArray = rightValue.Split(string.Empty);

            var length = Math.Min(leftArray.Length, rightArray.Length);
            for (var i = 0; i < length; i++)
            {
                if (leftArray[i] == rightArray[i]) continue;
                if (leftArray[i][0] > rightArray[i][0]) return new BoolValue(true);
                return new BoolValue(false);
            }
            if (leftValue == rightValue) return new BoolValue(true);
            if (leftArray.Length > rightArray.Length) return new BoolValue(true);
            return new BoolValue(false);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateGreaterThanOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new BoolValue(((NumberValue)left).Value > ((NumberValue)right).Value);
        }
        if (leftType == "STRING")
        {
            var leftValue = ((StringValue)left).Value;
            var rightValue = ((StringValue)right).Value;

            var leftArray = leftValue.Split(string.Empty);
            var rightArray = rightValue.Split(string.Empty);

            var length = Math.Min(leftArray.Length, rightArray.Length);
            for (var i = 0; i < length; i++)
            {
                if (leftArray[i] == rightArray[i]) continue;
                if (leftArray[i][0] > rightArray[i][0]) return new BoolValue(true);
                return new BoolValue(false);
            }
            if (leftValue == rightValue) return new BoolValue(false);
            if (leftArray.Length > rightArray.Length) return new BoolValue(true);
            return new BoolValue(false);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateModulusOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new NumberValue(((NumberValue)left).Value % ((NumberValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateExponentOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new NumberValue((float)Math.Pow(((NumberValue)left).Value, ((NumberValue)right).Value));
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateDivideOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            if (((NumberValue)right).Value == 0)
            {
                throw new RuntimeException("Zero divison occurred");
            }
            return new NumberValue(((NumberValue)left).Value / ((NumberValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateMultiplyOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "NUMBER")
        {
            return new NumberValue(((NumberValue)left).Value * ((NumberValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateAndOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "BOOL")
        {
            return new BoolValue(((BoolValue)left).Value && ((BoolValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }

    private RuntimeValue EvaluateOrOperator(Env.Environment env)
    {
        var left = Left.Interpret(env);
        var right = Right.Interpret(env);

        var leftType = left.Type();
        var rightType = right.Type();

        if (leftType != rightType)
        {
            throw new InterpreterException($"Different LHS and RHS types <{leftType}> <{Operator.Type}> <{rightType}>");
        }

        if (leftType == "BOOL")
        {
            return new BoolValue(((BoolValue)left).Value || ((BoolValue)right).Value);
        }

        throw new InterpreterException($"There is no defined behaviour for {Operator.Type} between {leftType} and {rightType}");
    }
}

