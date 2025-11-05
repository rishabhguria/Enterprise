namespace PgpExtractor
{
    public class Logger : IDisposable
    {
        private readonly object _lock = new object();
        private readonly object _consoleLock = new object();

        private StreamWriter _logWriter;
        private readonly string _logDirectory;

        public Logger()
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs", currentDate);

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            var logFilePath = Path.Combine(_logDirectory, $"PGP_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            _logWriter = new StreamWriter(logFilePath) { AutoFlush = true };
        }

        public void Log(string message)
        {
            lock (_lock)
            {
                _logWriter?.WriteLine($"{DateTime.Now:[yy-MM-dd HH:mm:ss]} - {message}");
            }
        }

        public void ConsoleLog(string message, ConsoleColor? consoleColor = null)
        {
            lock (_consoleLock)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = consoleColor ?? originalColor;
                Console.WriteLine($"{DateTime.Now:[yy-MM-dd HH:mm:ss]} - {message}");
                Console.ForegroundColor = originalColor;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _logWriter?.Dispose();
            }
        }
    }
}
