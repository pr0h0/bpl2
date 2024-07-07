using BPL2.Exceptions;
using BPL2.Services;

namespace BPL2;

class Program
{
    public static void Main(string[] args)
    {
        try
        {
            ArgsService.Init(args);

            var REPL = new REPL.REPL();
            if (ArgsService.Args.Any())
            {
                var content = File.ReadAllText(ArgsService.Args.First(), System.Text.Encoding.Default);
                REPL.ParseFile(content);
            }
            else
            {
                REPL.Start();
            }
        }
        catch (EndProgramException ex)
        {
            if (ex.Code != 0)
            {
                LogService.Error($"Program exited with code {ex.Code}");
                if (ex.Message.Length > 0)
                {
                    LogService.Error(ex.Message);
                }
            }
            Environment.Exit(ex.Code);
        }
    }
}