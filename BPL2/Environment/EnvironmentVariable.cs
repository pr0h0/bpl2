using BPL2.Lexer;
using BPL2.Values;

namespace BPL2.Env;

public class EnvironmentVariable
{
    public Token Name;
    public Token TypeOf;
    public RuntimeValue Value;
    public bool IsConst;

    public EnvironmentVariable(
        Token name,
        Token typeOf,
        RuntimeValue value,
        bool isConst
    )
    {
        Name = name;
        TypeOf = typeOf;
        Value = value;
        IsConst = isConst;
    }

}
