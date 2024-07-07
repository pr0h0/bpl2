using BPL2.Services;
using BPL2.Expr;
using BPL2.Exceptions;

namespace BPL2.REPL;

public class REPL
{
    private bool ShowTokens = true;
    private bool ShowExprs = true;
    private bool ShowValues = true;
    private bool IsRepl = false;
    private List<string> History = new();

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
            History.Add(command);
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
        else if (command.StartsWith("#file"))
        {
            var fileName = command.Replace("#file ", "");
            string content = "";
            try
            {
                content = File.ReadAllText(fileName);
            }
            catch (Exception ex)
            {
                LogService.Error(ex);
            }
            if (content.Length > 0)
            {
                History.Add(command);
                ParseFile(content);
            }
        }
        else if (command == "#history")
        {
            int index = 0;
            foreach (var item in History)
            {
                LogService.Log($"{++index}. {item}");
            }
        }
        else if (int.TryParse(command.Substring(1), out int historyIndex))
        {
            var localCommand = "";
            if (historyIndex < 0 && -historyIndex <= History.Count)
            {
                localCommand = (History[History.Count + historyIndex] ?? "");
            }
            else if (historyIndex > 0 && historyIndex <= History.Count)
            {
                localCommand = (History[historyIndex - 1]);
            }
            else
            {
                LogService.Error($"Index out of bounds {historyIndex} <!> {History.Count}");
            }

            if (localCommand.Length > 0)
            {
                if (localCommand.StartsWith("#"))
                {
                    ParseReplCommand(localCommand);
                }
                else
                {
                    ExecuteCommand(localCommand);
                }
            }
        }
    }

    public void ParseFile(string content)
    {
        if (IsRepl)
        {
            ParseReplCommand("#off");
        }
        ExecuteCommand(content);
    }

    public void ExecuteCommand(string command)
    {
        try
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
        catch (EndProgramException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogService.Error(ex);
            if (!IsRepl)
            {
                throw;
            }
        }
    }
}

