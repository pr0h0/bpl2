namespace BPL2.Services;

public static class ConsoleService
{
    public static string ReadLine(string? message = null)
    {
        if (message != null)
        {
            LogService.Log($"{message} ", false);
        }
        return Console.ReadLine() ?? "";
    }
}

