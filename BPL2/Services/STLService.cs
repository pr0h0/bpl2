using BPL2.Expr;
using BPL2.Lexer;
using BPL2.Values;
using BPL2.Exceptions;

namespace BPL2.Services;

public static class STLService
{
    public static void PopulateSTLVariables(Env.Environment env)
    {
        env.DefineVariable(new Token("", "PI", 0), new Token(TokenType.Identifier, "NUMBER", 0), new NumberValue((float)Math.PI), true);
        env.DefineVariable(new Token("", "version", 0), new Token(TokenType.Identifier, "STRING", 0), new StringValue("0.0.1"), true);
    }

    public static void PopulateSTLFunctions(Env.Environment environment)
    {
        // print FUNCTION 
        environment.DefineFunction(new Token("", "print"), new Token("", "VOID"), new STLFunctionValue(
            new Token("", "print"),
            new List<Tuple<Token, string>>() { Tuple.Create(new Token("", "message"), "STRING") },
            (args) =>
            {
                args.Select(arg =>
                {
                    return (arg as dynamic).Value.ToString();
                }).ToList().ForEach(arg =>
            {
                Console.WriteLine(arg);
            });
                return new VoidValue();
            }
       ));

        // time FUNCTION
        environment.DefineFunction(new Token("", "time"), new Token("", "NUMBER"), new STLFunctionValue(
            new Token("", "time"),
            new List<Tuple<Token, string>>() { },
            (args) =>
            {
                return new NumberValue(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
       ));

        // input FUNCTION
        environment.DefineFunction(new Token("", "input"), new Token("", "STRING"), new STLFunctionValue(
            new Token("", "input"),
            new List<Tuple<Token, string>>() { Tuple.Create(new Token("", "message"), "STRING") },
            (args) =>
            {
                if (args.Any())
                {
                    Console.Write(((dynamic)args[0])?.Value);
                }
                var value = Console.ReadLine();
                LogService.Error(value ?? "");
                return new StringValue(value ?? "");
            }
       ));

        // convert FUNCTION
        environment.DefineFunction(new Token("", "convert"), new Token("", "ANY"), new STLFunctionValue(
            new Token("", "convert"),
            new List<Tuple<Token, string>>() {
                Tuple.Create(new Token("", "value1"), "ANY"),
                Tuple.Create(new Token("", "value2"), "TYPE")
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
        environment.DefineFunction(new Token("", "typeof"), new Token("", "ANY"), new STLFunctionValue(
            new Token("", "typeof"),
            new List<Tuple<Token, string>>() {
                Tuple.Create(new Token("", "value1"), "ANY"),
            },
            (args) =>
            {
                return new StringValue(args[0].Type());
            }
       ));
    }

    public static void PopulateSTLTypes(Env.Environment environment)
    {
        environment.DefineType(
            new Token("", "NUMBER"),
            new Token("", "NUMBER"),
            new TypeValue(
                new Token("", "NUMBER"),
                new Token("", "NUMBER"),
                new Dictionary<string, Token>(),
                false
            )
        );

        environment.DefineType(
            new Token("", "STRING"),
            new Token("", "STRING"),
            new TypeValue(
                new Token("", "STRING"),
                new Token("", "STRING"),
                new Dictionary<string, Token>(),
                false
            )
        );

        environment.DefineType(
            new Token("", "BOOL"),
            new Token("", "BOOL"),
            new TypeValue(
                new Token("", "BOOL"),
                new Token("", "BOOL"),
                new Dictionary<string, Token>(),
                false
            )
        );

        environment.DefineType(
            new Token("", "OBJECT"),
            new Token("", "OBJECT"),
            new TypeValue(
                new Token("", "OBJECT"),
                new Token("", "OBJECT"),
                new Dictionary<string, Token>(),
                false
            )
        );

        environment.DefineType(
            new Token("", "TUPLE"),
            new Token("", "TUPLE"),
            new TypeValue(
                new Token("", "TUPLE"),
                new Token("", "TUPLE"),
                new Dictionary<string, Token>(),
                false
            )
        );
    }
}
