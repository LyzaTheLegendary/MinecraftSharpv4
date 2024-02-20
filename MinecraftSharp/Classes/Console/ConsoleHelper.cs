namespace MinecraftSharp.Classes.Display
{
    public static class ConsoleHelper
    {
        private static string GetTimeStamp { get { return DateTime.Now.ToString("ddd HH:mm"); } }
        public static void WriteInfo(string text)
        {
            Write($"[{GetTimeStamp}] {text}");
        }
        private static void Write(string text)
        {
            Console.WriteLine(text);
            Logging.Write(text + Environment.NewLine);
        }
    }
}
