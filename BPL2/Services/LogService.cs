namespace BPL2.Services;

public static class LogService
{
    public static void Log(string message, bool newLine = true)
    {
        Console.ForegroundColor = ConsoleColor.White;
        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }
    }

    public static void Log(object message, bool newLine = true)
    {
        Console.ForegroundColor = ConsoleColor.White;
        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }
    }

    public static void Error(string messsage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(messsage);
    }

    public static void Error(Exception error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error.Message);

        if (ArgsService.Has("--debug"))
        {
            Console.WriteLine(error.StackTrace);
        }
    }
}

