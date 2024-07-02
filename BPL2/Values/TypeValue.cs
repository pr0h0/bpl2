using System;
using BPL2.Lexer;

namespace BPL2.Values
{
    public class TypeValue : RuntimeValue
    {
        public Token Name;
        public Token TypeOf;
        public Dictionary<string, Token> Types;
        private readonly bool IsArray;


        public override string Type() => "TYPE";

        public TypeValue(Token name, Token typeOf, Dictionary<string, Token> types, bool isArray)
        {
            Name = name;
            TypeOf = typeOf;
            Types = types;
            IsArray = isArray;
        }
    }
}

