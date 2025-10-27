
namespace BenScr.Debugging
{
    // Logger is a highlevel console logging class, used for efficient debugging
    // Usage example:
    // if (Item == null) Logger.Warning("Item", "Shouldn't be null.");
    // Output: [HH:mm:ss][Warning][Item] Shouldn't be null.

    public static class Logger
    {
        private enum LogType { Info, Message, Warning, Error }
        public static bool Tracing { get; set; }

        public static void WriteColored(string text, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
        public static void Message(string message) => LogInternal(LogType.Message, "", message);
        public static void Info(string message, string topic = "") => LogInternal(LogType.Info, topic, message);
        public static void Warning(string message, string topic = "") => LogInternal(LogType.Warning, topic, message);
        public static void Error(string message, string topic = "") => LogInternal(LogType.Error, topic, message);

        private static void LogInternal(LogType type, string message, string topic = "")
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string topicFormatted = topic == "" ? "" : $"[{topic}]";
            string formatted = $"[{timestamp}][{type}]{topicFormatted} {message}";

            switch (type)
            {
                case LogType.Info:
                    WriteColored(formatted, ConsoleColor.Green);
                    break;
                case LogType.Message:
                    WriteColored(formatted, ConsoleColor.Gray); break;
                case LogType.Warning:
                    WriteColored(formatted, ConsoleColor.Yellow);
                    break;
                case LogType.Error:
                    WriteColored(formatted, ConsoleColor.Red);
                    break;
            }
        }
    }
}
