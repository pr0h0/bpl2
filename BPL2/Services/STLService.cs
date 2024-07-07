using BPL2.Expr;
using BPL2.Lexer;
using BPL2.Values;
using BPL2.Exceptions;

namespace BPL2.Services;

public static class STLService
{
    public static void PopulateSTLVariables(Env.Environment env)
    {
        env.DefineVariable(Token.IDENTIFIER("PI"), Token.IDENTIFIER("NUMBER"), new NumberValue((float)Math.PI), true);
        env.DefineVariable(Token.IDENTIFIER("version"), Token.IDENTIFIER("STRING"), new StringValue("0.0.1"), true);
    }

    public static void PopulateSTLFunctions(Env.Environment environment)
    {
        // print FUNCTION 
        environment.DefineFunction(Token.IDENTIFIER("print"), Token.IDENTIFIER("VOID"), new STLFunctionValue(
            Token.IDENTIFIER("print"),
            new List<Tuple<Token, string>>() { Tuple.Create(Token.IDENTIFIER("message"), "STRING") },
            (args) =>
            {
                args.Select(arg =>
                {
                    // TODO: Move to respective classes ToString method
                    if (arg is TypeValue typeValue)
                    {
                        var isObject = typeValue.TypeOf.Value == "OBJECT";
                        var value = isObject ? "{\n" : "[";
                        var prefix = "    ";
                        foreach (var type in typeValue.Types)
                        {
                            if (isObject)
                            {
                                value += prefix + type.Key + ": ";
                                value += type.Value.Value + ",\n";
                            }
                            else
                            {
                                value += type.Value.Value + ", ";
                            }
                        }
                        value += isObject ? "}" : "]";
                        return value;
                    }
                    if (arg is ObjectValue objectValue)
                    {
                        var value = "{\n";
                        var prefix = "    ";
                        foreach (var type in objectValue.Value)
                        {
                            value += prefix + type.Key + ": ";
                            value += type.Value.ToString() + ",\n";
                        }
                        value += "}";
                        return value;
                    }
                    if (arg is TupleValue tupleValue)
                    {
                        var value = "[";
                        foreach (var type in tupleValue.Value)
                        {
                            value += type.ToString() + ", ";
                        }
                        value += "]";
                        return value;
                    }
                    if (arg is ArrayValue arrayValue)
                    {
                        var value = "[";
                        foreach (var type in arrayValue.Value)
                        {
                            value += type.ToString() + ", ";
                        }
                        value += "]";
                        return value;
                    }
                    return (arg as dynamic).Value.ToString();
                }).ToList().ForEach(arg =>
            {
                Console.WriteLine(arg);
            });
                return new VoidValue();
            }
       ));

        // time FUNCTION
        environment.DefineFunction(Token.IDENTIFIER("time"), Token.IDENTIFIER("NUMBER"), new STLFunctionValue(
            Token.IDENTIFIER("time"),
            new List<Tuple<Token, string>>() { },
            (args) =>
            {
                return new NumberValue(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
       ));

        // input FUNCTION
        environment.DefineFunction(Token.IDENTIFIER("input"), Token.IDENTIFIER("STRING"), new STLFunctionValue(
             Token.IDENTIFIER("input"),
            new List<Tuple<Token, string>>() { Tuple.Create(Token.IDENTIFIER("message"), "STRING") },
            (args) =>
            {
                if (args.Any())
                {
                    Console.Write(((dynamic)args[0])?.Value);
                }
                var value = Console.ReadLine();
                return new StringValue(value ?? "");
            }
       ));

        // convert FUNCTION
        environment.DefineFunction(Token.IDENTIFIER("convert"), Token.IDENTIFIER("ANY"), new STLFunctionValue(
             Token.IDENTIFIER("convert"),
            new List<Tuple<Token, string>>() {
                Tuple.Create( Token.IDENTIFIER( "value1"), "ANY"),
                Tuple.Create( Token.IDENTIFIER( "value2"), "TYPE")
            },
            (args) =>
            {
                var value = args[0];
                var type = (args[1] as TypeValue)!.TypeOf.Value;

                if (value is NumberValue numValue && type == "STRING")
                {
                    return new StringValue(numValue.Value.ToString());
                }
                if (value is StringValue strValue && type == "NUMBER")
                {
                    return new NumberLiteralExpr(strValue.Value).Interpret(null!);
                }

                throw new InterpreterException($"Unable to convert {value.Type()} to {type}");
            }
       ));

        // convert FUNCTION
        environment.DefineFunction(Token.IDENTIFIER("typeof"), Token.IDENTIFIER("ANY"), new STLFunctionValue(
            Token.IDENTIFIER("typeof"),
            new List<Tuple<Token, string>>() {
                Tuple.Create(Token.IDENTIFIER( "value1"), "ANY"),
            },
            (args) =>
            {
                return new StringValue(args[0].Type());
            }
       ));

        // exit FUNCTION
        environment.DefineFunction(Token.IDENTIFIER("exit"), Token.IDENTIFIER("VOID"), new STLFunctionValue(
            Token.IDENTIFIER("exit"),
            new List<Tuple<Token, string>>() { },
            (args) =>
            {
                // placeholder so it can't be redefined;
                return new VoidValue();
            }
       ));
    }

    public static void PopulateSTLTypes(Env.Environment environment)
    {
        var types = new List<string>() {
            "NUMBER",
            "STRING",
            "BOOL",
            "OBJECT",
            "TUPLE",
        };

        types.ForEach(tokenString =>
        {
            var token = Token.IDENTIFIER(tokenString);
            environment.DefineType(
                token,
                token,
                new TypeValue(
                    token,
                    token,
                    new Dictionary<string, Token>(),
                    false
                )
            );
        });
    }
}
