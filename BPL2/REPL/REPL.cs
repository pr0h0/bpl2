using BPL2.Services;
using BPL2.Expr;

namespace BPL2.REPL;

public class REPL
{
    private bool ShowTokens = true;
    private bool ShowExprs = true;
    private bool ShowValues = true;
    private bool IsRepl = false;
    private readonly Interpreter.Interpreter Interpreter = new();

    public REPL() { }

    public void Start()
    {
        IsRepl = true;

        LogService.Log("==================================================");
        LogService.Log("======== THE BESTEST PROGRAMMING LANGUAGE ========");
        LogService.Log("==================================================");

        while (true)
        {
            var command = ConsoleService.ReadLine("BPL2> ").Trim();

            if (command == "#exit") break;
            if (command.StartsWith("#"))
            {
                ParseReplCommand(command);
                continue;
            }

            ExecuteCommand(command);
        }

        LogService.Log("Exiting...");
    }

    private void ParseReplCommand(string command)
    {
        if (command == "#clear")
        {
            Console.Clear();
        }
        else if (command == "#tokens")
        {
            ShowTokens = !ShowTokens;
        }
        else if (command == "#exprs")
        {
            ShowExprs = !ShowExprs;
        }
        else if (command == "#values")
        {
            ShowValues = !ShowValues;
        }
        else if (command == "#on")
        {
            ShowTokens = true;
            ShowExprs = true;
            ShowValues = true;
        }
        else if (command == "#off")
        {
            ShowTokens = false;
            ShowExprs = false;
            ShowValues = false;
        }
    }

    public void ParseFile(string content)
    {
        IsRepl = true;
        ParseReplCommand("#off");
        ExecuteCommand(content);
    }

    public void ExecuteCommand(string command)
    {
        var tokens = new Lexer.Lexer(command).Parse();

        if (IsRepl && ShowTokens)
        {
            foreach (var token in tokens)
            {
                LogService.Log(token);
            }
        }

        List<Expression> exprs = new Parser.Parser(tokens).Parse();

        if (IsRepl && ShowExprs)
        {
            foreach (Expression expr in exprs)
            {
                LogService.Log(expr.ToString());
            }
        }

        var values = Interpreter.Interpret(exprs);
        if (IsRepl && ShowValues)
        {
            foreach (var val in values)
            {
                if (val != null)
                {
                    LogService.Log(val.ToString()!);
                }
            }
        }
    }
}

