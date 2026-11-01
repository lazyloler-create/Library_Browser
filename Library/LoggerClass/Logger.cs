public class Logger : ILogger
{
    private readonly string logFolderPath;
    private readonly string logFilePath;

    public Logger()
    {
        logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        Directory.CreateDirectory(logFolderPath);
        logFilePath = Path.Combine(logFolderPath, "LibraryBrowser.log");
    }

    public void Log(string message)
    {
        string logEntry = $"{DateTime.Now:dd-MM-yyyy HH:mm} - {message}";
        try
        {
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }

    public void LogError(string message)
    {
        Log($"[ERROR]: {message}");
    }

    public void LogInfo(string message)
    {
        Log($"[INFO]: {message}");
    }

    public void LogWarning(string message)
    {
        Log($"[WARNING]: {message}");
    }

    public void LogSuccess(string message)
    {
        Log($"[SUCCESS]: {message}");
    }
}
