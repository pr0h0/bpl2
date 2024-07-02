using System;
using BPL2.Values;

namespace BPL2.Exceptions
{
    public class RuntimeException : ThrowException
    {
        public RuntimeException(string message) : base(new StringValue(message)) { }
    }
}

