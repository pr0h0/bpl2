using BPL2.Services;

namespace BPL2;

class Program
{
    public static void Main(string[] args)
    {
        ArgsService.Init(args);
        // ArgsService.Args.Add("examples/test.bpl");

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
}