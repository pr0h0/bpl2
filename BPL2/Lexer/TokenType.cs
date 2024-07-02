namespace BPL2.Lexer;

public static class TokenType
{
    public static readonly string NumberLiteral = "NUMBER_LITERAL";
    public static readonly string StringLiteral = "STRING_LITERAL";
    public static readonly string StringTemplate = "STRING_TEMPLATE";

    public static readonly string Identifier = "IDENTIFIER";
    public static readonly string EOF = "EOF";

    public static readonly string Plus = "PLUS";
    public static readonly string PlusEqual = "PLUS_EQUAL";
    public static readonly string Minus = "MINUS";
    public static readonly string MinusEqual = "MINUS_EQUAL";
    public static readonly string Multiply = "MULTIPLY";
    public static readonly string MultiplyEqual = "MULTIPLY_EQUAL";
    public static readonly string Divide = "DIVIDE";
    public static readonly string DivideEqual = "DIVIDE_EQUAL";
    public static readonly string Modulus = "MODULUS";
    public static readonly string ModulusEqual = "MODULUS_EQUAL";
    public static readonly string Exponent = "EXPONENT";

    public static readonly string GreaterThan = "GREATER_THAN";
    public static readonly string GreaterThanEqual = "GREATER_THAN_EQUAL";
    public static readonly string LessThan = "LESS_THAN";
    public static readonly string LessThanEqual = "LESS_THAN_EQUAL";

    public static readonly string OpenParen = "OPEN_PAREN";
    public static readonly string CloseParen = "CLOSE_PAREN";
    public static readonly string OpenCurly = "OPEN_CURLY";
    public static readonly string CloseCurly = "CLOSE_CURLY";
    public static readonly string OpenBracket = "OPEN_BRACKET";
    public static readonly string CloseBracket = "CLOSE_BRACKET";

    public static readonly string Comma = "COMMA";
    public static readonly string Semicolon = "SEMICOLON";
    public static readonly string Colon = "COLON";
    public static readonly string Dot = "DOT";

    public static readonly string Assign = "ASSIGN";
    public static readonly string Equal = "EQUAL";
    public static readonly string NotEqual = "NOT_EQUAL";

    public static readonly string And = "AND";
    public static readonly string Or = "OR";
    public static readonly string Not = "NOT";

    public static readonly string Increment = "INCREMENT";
    public static readonly string Decrement = "DECREMENT";

    public static readonly string Arrow = "ARROW";

    public static readonly string Empty = "EMPTY";

    public static readonly string Question = "QUESTION";
}

