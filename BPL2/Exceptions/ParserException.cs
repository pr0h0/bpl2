using BPL2.Lexer;

namespace BPL2.Exceptions;

public class ParserException : Exception
{
    public Token? Token;
    public ParserException(string message, Token? token = null) : base(message)
    {
        Token = token;
    }

    public override string ToString()
    {
        return $"PARSER{(Token != null ? $" <{Token.Type} | {Token.Value}>" : "")} {Message}";
    }
}

