using System;
namespace BPL2.Lexer
{
    public class Token
    {
        public string Type;
        public string Value;
        public int Line;

        public Token(string type, string value, int line = 0)
        {
            Type = type;
            Value = value;
            Line = line;
        }

        public override string ToString()
        {
            return $"Token<{Type}> [#{Line}] {Value}";
        }
    }
}

