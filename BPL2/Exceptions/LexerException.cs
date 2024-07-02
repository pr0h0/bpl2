namespace BPL2.Exceptions;

public class LexerException : Exception
{
    public int Line;
    public LexerException(string message, int line) : base(message)
    {
        Line = line;
    }

    public override string ToString()
    {
        return $"LEXER <Line {Line}> {Message}";
    }
}

