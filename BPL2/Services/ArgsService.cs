namespace BPL2.Services;

public static class ArgsService
{
    public static readonly List<string> Args = new();

    public static void Init(string[] args)
    {
        Args.AddRange(args);
    }

    public static bool Has(string arg)
    {
        return Args.Contains(arg);
    }
}

