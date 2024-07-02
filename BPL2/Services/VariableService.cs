using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Values;

namespace BPL2.Services;

public static class VariableService
{
    public static void ValidateVariableDeclaration(Token typeOf, RuntimeValue value)
    {
        switch (typeOf.Value)
        {
            case "NUMBER":
                ValidateNumberValue(typeOf, value);
                return;
            case "STRING":
                ValidateString(typeOf, value);
                return;
            case "BOOL":
                ValidateBoolValue(typeOf, value);
                return;
            case "ARRAY":
                ValidateArrayValue(typeOf, value);
                return;
            case "TUPLE":
                ValidateTupleValue(typeOf, value);
                return;
            case "OBJECT":
                ValidateObjectValue(typeOf, value);
                return;
            case "FUNC":
                ValidateFuncValue(typeOf, value);
                return;
            default:
                throw new InterpreterException($"Unknown variable type {typeOf.Value}");
        }
    }

    private static void ValidateString(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "STRING" || value is not StringValue)
        {
            throw new InterpreterException($"Invalid variable type, expected STRING but got {value.Type()}");
        }
    }

    private static void ValidateNumberValue(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "NUMBER" || value is not NumberValue)
        {
            throw new InterpreterException($"Invalid variable type, expected NUMBER but got {value.Type()}");
        }
    }

    private static void ValidateBoolValue(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "BOOL" || value is not BoolValue)
        {
            throw new InterpreterException($"Invalid variable type, expected BOOL but got {value.Type()}");
        }
    }

    private static void ValidateArrayValue(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "ARRAY" || value is not ArrayValue)
        {
            throw new InterpreterException($"Invalid variable type, expected ARRAY but got {value.Type()}");
        }

        var arrValue = value as ArrayValue;
        arrValue?.Value.ForEach(value =>
        {
            ValidateVariableDeclaration(typeOf, value);
        });
    }

    private static void ValidateTupleValue(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "TUPLE" || value is not TupleValue)
        {
            throw new InterpreterException($"Invalid variable type, expected TUPLE but got {value.Type()}");
        }
    }

    private static void ValidateObjectValue(Token typeOf, RuntimeValue value)
    {
        if (typeOf.Value != "OBJECT" || value is not ObjectValue)
        {
            throw new InterpreterException($"Invalid variable type, expected OBJECT but got {value.Type()}");
        }
    }

    private static void ValidateFuncValue(Token typeOf, RuntimeValue value)
    {
        if (!typeOf.Value.Contains("FUNC") || (value is not FunctionValue && value is not STLFunctionValue && value is not CustomFunctionValue))
        {
            throw new InterpreterException($"Invalid variable type, expected FUNC but got {value.Type()}");
        }
    }
}

