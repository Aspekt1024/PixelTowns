using Godot;

namespace PixelTowns.Units;

public class AiOverseer
{
    public enum LogMode
    {
        None = 0,
        ErrorOnly = 1000,
        Normal = 2000,
        Verbose = 3000,
    }

    private LogMode logMode;

    public void SetLogMode(LogMode mode)
    {
        logMode = mode;
    }

    public static void Log(string message)
    {
        LogMode mode = GameManager.Ai.logMode;
        if (mode is LogMode.Normal or LogMode.Verbose)
        {
            GD.Print(CreateMessage(message));
        }
    }

    public static void LogInfo(string message)
    {
        LogMode mode = GameManager.Ai.logMode;
        if (mode is LogMode.Verbose)
        {
            GD.Print(CreateMessage(message));
        }
    }

    public static void LogError(string message)
    {
        LogMode mode = GameManager.Ai.logMode;
        if (mode is LogMode.None) return;
        
        GD.PrintErr(CreateMessage(message));
    }

    private static string CreateMessage(string message)
    {
        return $"{Time.GetTimeStringFromSystem()} [AI Overseer] {message}";
    }
}