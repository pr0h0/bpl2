using BPL2.Exceptions;
using BPL2.Lexer;
using BPL2.Services;
using BPL2.Values;

namespace BPL2.Env;

public class Environment
{
    public Environment? Parent { get; }
    public Dictionary<string, EnvironmentVariable> variables = new();
    public Dictionary<string, FunctionValue> funcs = new();
    public Dictionary<string, TypeValue> types = new();
    public List<string> all = new();

    public Environment(Environment? parent = null)
    {
        if (parent != null)
        {
            Parent = parent;
        }
        else
        {
            STLService.PopulateSTLTypes(this);
            STLService.PopulateSTLVariables(this);
            STLService.PopulateSTLFunctions(this);
        }
    }

    #region VARIABLES
    public bool CheckVariableExist(string name)
    {
        if (!variables.ContainsKey(name) && (Parent == null || Parent.CheckVariableExist(name) == false))
        {
            throw new InterpreterException($"Variable {name} not defined");
        }

        return true;
    }

    public EnvironmentVariable GetVariable(string name)
    {
        CheckVariableExist(name);

        return (variables.GetValueOrDefault(name) ?? Parent?.GetVariable(name))!;
    }

    public EnvironmentVariable DefineVariable(Token name, Token typeOf, RuntimeValue value, bool isConst = false)
    {
        if (all.Contains(name.Value))
        {
            throw new InterpreterException($"Variable {name.Value} is already defined");
        }

        variables.Add(name.Value, new EnvironmentVariable(name, typeOf, value, isConst));
        all.Add(name.Value);

        return variables.GetValueOrDefault(name.Value)!;
    }

    public EnvironmentVariable SetValue(Token name, RuntimeValue value)
    {
        var variable = GetVariable(name.Value);
        if (variable.IsConst)
        {
            throw new InterpreterException($"Variable {name.Value} is constant and can't br reassigned");
        }

        variable.Value = value;
        variables[name.Value].Value = value;


        return variable;
    }
    #endregion

    #region FUNCTIONS
    public bool CheckFuncExist(string name)
    {
        if (!funcs.ContainsKey(name) && (Parent == null || Parent.CheckFuncExist(name) == false))
        {
            throw new InterpreterException($"Function {name} not defined");
        }

        return true;
    }
    public FunctionValue GetFunction(string name)
    {
        CheckFuncExist(name);

        funcs.TryGetValue(name, out var func);
        return (func ?? Parent?.GetFunction(name))!;
    }

    public FunctionValue DefineFunction(Token name, Token typeOf, FunctionValue value)
    {
        if (all.Contains(name.Value))
        {
            throw new InterpreterException($"Function {name.Value} is already defined");
        }


        funcs.Add(name.Value, value);
        all.Add(name.Value);

        return funcs.GetValueOrDefault(name.Value)!;
    }
    #endregion

    #region TYPES
    public bool CheckTypeExist(string name)
    {
        if (!types.ContainsKey(name) && (Parent == null || Parent.CheckTypeExist(name) == false))
        {
            throw new InterpreterException($"Type {name} not defined");
        }

        return true;
    }
    public TypeValue GetType(string name)
    {
        CheckTypeExist(name);

        types.TryGetValue(name, out var type);
        return (type ?? Parent?.GetType(name))!;
    }

    public TypeValue DefineType(Token name, Token typeOf, TypeValue value)
    {
        if (all.Contains(name.Value))
        {
            throw new InterpreterException($"Type {name.Value} is already defined");
        }


        types.Add(name.Value, value);
        all.Add(name.Value);

        return types.GetValueOrDefault(name.Value)!;
    }
    #endregion
}
