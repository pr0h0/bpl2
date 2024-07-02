using BPL2.Exceptions;

namespace BPL2.Lexer
{
    public class Lexer
    {
        private string Content;
        private int Index;
        private int Line;
        private List<Token> Tokens;

        public Lexer(string content)
        {
            Line = 1;
            Index = 0;
            Tokens = new();
            Content = content;
        }

        public List<Token> Parse()
        {
            while (!IsEOF())
            {
                var token = ParseNext();
                if (token.Type == TokenType.Empty) continue;
                Tokens.Add(token);
            }

            Tokens.Add(new Token(TokenType.EOF, "EOF", ++Line));
            return Tokens;
        }

        private Token ParseNext()
        {
            switch (Content[Index])
            {
                case '+': return ParsePlus();
                case '-': return ParseMinus();
                case '*': return ParseMultiply();
                case '/': return ParseSlash();
                case '%': return ParseModulus();
                case '=': return ParseEqual();
                case '>': return ParseGreater();
                case '<': return ParseLess();
                case '!': return ParseBang();
                case '(': return new Token(TokenType.OpenParen, Content[Index++].ToString(), Line);
                case ')': return new Token(TokenType.CloseParen, Content[Index++].ToString(), Line);
                case '{': return new Token(TokenType.OpenCurly, Content[Index++].ToString(), Line);
                case '}': return new Token(TokenType.CloseCurly, Content[Index++].ToString(), Line);
                case '[': return new Token(TokenType.OpenBracket, Content[Index++].ToString(), Line);
                case ']': return new Token(TokenType.CloseBracket, Content[Index++].ToString(), Line);
                case '.': return new Token(TokenType.Dot, Content[Index++].ToString(), Line);
                case ',': return new Token(TokenType.Comma, Content[Index++].ToString(), Line);
                case ':': return new Token(TokenType.Colon, Content[Index++].ToString(), Line);
                case ';': return new Token(TokenType.Semicolon, Content[Index++].ToString(), Line);
                case '?': return new Token(TokenType.Question, Content[Index++].ToString(), Line);
            }

            if (Content[Index] == '&')
            {
                return ParseAnd();
            }
            if (Content[Index] == '|')
            {
                return ParseOr();
            }

            if ("\"'".Contains(Content[Index].ToString()))
            {
                return ParseString();
            }

            if (char.IsDigit(Content[Index]))
            {
                return ParseNumber();
            }

            if (char.IsLetter(Content[Index]) || Content[Index] == '_')
            {
                return ParseIdentifier();
            }

            if (Content[Index] == '`')
            {
                return ParseStringLiteral();
            }

            if (Content[Index] == '\n')
            {
                Line++;
                Index++;
                return new Token(TokenType.Empty, "", Line);
            }

            if ("\r\t ".Contains(Content[Index].ToString()))
            {
                Index++;
                return new Token(TokenType.Empty, "", Line);
            }

            throw new LexerException($"Unexpected token {Content[Index]}", Line);
        }

        private bool IsEOF()
        {
            return Index >= Content.Length;
        }

        private Token ParsePlus()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '+')
            {
                value += Content[Index++];
                return new Token(TokenType.Increment, value, Line);
            }
            else if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.PlusEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.Plus, value, Line);
            }
        }

        private Token ParseMinus()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '-')
            {
                value += Content[Index++];
                return new Token(TokenType.Decrement, value, Line);
            }
            else if (Content[Index] == '-')
            {
                value += Content[Index++];
                return new Token(TokenType.MinusEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.Minus, value, Line);
            }
        }

        private Token ParseMultiply()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.MultiplyEqual, value, Line);
            }
            else if (Content[Index] == '*')
            {
                value += Content[Index++];
                return new Token(TokenType.Exponent, value, Line);
            }
            else
            {
                return new Token(TokenType.Multiply, value, Line);
            }
        }

        private Token ParseModulus()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.ModulusEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.Modulus, value, Line);
            }
        }

        private Token ParseSlash()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.DivideEqual, value, Line);
            }
            else if (Content[Index] == '/')
            {
                while (Content[Index] != '\n')
                {
                    Index++;
                }
                Line++;
                return new Token(TokenType.Empty, "", Line);
            }
            else if (Content[Index] == '*')
            {
                while (Content[Index] != '*' || Content[Index + 1] != '/')
                {
                    Index++;
                    if (Content[Index] == '\n')
                    {
                        Line++;
                    }
                }
                Index += 2;
                return new Token(TokenType.Empty, "", Line);
            }
            else
            {
                return new Token(TokenType.Divide, value, Line);
            }
        }

        private Token ParseEqual()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.Equal, value, Line);
            }
            else if (Content[Index] == '>')
            {
                value += Content[Index++];
                return new Token(TokenType.Arrow, value, Line);
            }
            else
            {
                return new Token(TokenType.Assign, value, Line);
            }
        }

        private Token ParseGreater()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.GreaterThanEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.GreaterThan, value, Line);
            }
        }

        private Token ParseLess()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.LessThanEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.LessThan, value, Line);
            }
        }

        private Token ParseBang()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] == '=')
            {
                value += Content[Index++];
                return new Token(TokenType.NotEqual, value, Line);
            }
            else
            {
                return new Token(TokenType.Not, value, Line);
            }
        }

        private Token ParseStringLiteral()
        {
            var value = "";
            var quote = Content[Index++];
            while (!IsEOF() && (Content[Index] != quote || (Content[Index] == quote && Content[Index - 1] == '\\')))
            {
                if (Content[Index] == '\n')
                {
                    Line++;
                }
                value += Content[Index++];
            }

            if (IsEOF() && Content[Index - 1] != quote)
            {
                throw new LexerException("Unterminated string template", Line);
            }

            Index++;

            return new Token(TokenType.StringTemplate, value, Line);
        }

        private Token ParseString()
        {
            var value = "";
            var quote = Content[Index++];
            while (!IsEOF() && Content[Index] != quote && Content[Index - 1] != '\\')
            {
                if (Content[Index] == '\n')
                {
                    throw new LexerException("Unterminated string literal", Line);
                }
                value += Content[Index++];
            }

            if (IsEOF() && Content[Index] != quote)
            {
                throw new LexerException("Unterminated string literal", Line);
            }

            Index++;
            return new Token(TokenType.StringLiteral, value, Line);
        }

        private Token ParseNumber()
        {
            var value = "";
            while (!IsEOF() && (char.IsDigit(Content[Index]) || Content[Index] == '.'))
            {
                value += Content[Index++];
            }

            if (!float.TryParse(value, out _))
            {
                throw new LexerException($"Invalid number literal {value}", Line);
            }

            return new Token(TokenType.NumberLiteral, value, Line);
        }

        private Token ParseIdentifier()
        {
            var value = "";
            while (!IsEOF() && (char.IsLetterOrDigit(Content[Index]) || Content[Index] == '_'))
            {
                value += Content[Index++];
            }

            return new Token(TokenType.Identifier, value, Line);
        }

        private Token ParseAnd()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] != value[0])
            {
                throw new LexerException($"Unexpected token \"{Content[Index]}\", expected second \"&\" token", Line);
            }
            value += Content[Index++];
            return new Token(TokenType.And, value, Line);
        }

        private Token ParseOr()
        {
            var value = Content[Index++].ToString();
            if (Content[Index] != value[0])
            {
                throw new LexerException($"Unexpected token \"{Content[Index]}\", expected second \"|\" token", Line);
            }
            value += Content[Index++];
            return new Token(TokenType.Or, value, Line);
        }
    }
}
