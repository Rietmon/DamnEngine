using System;

namespace DamnEngine
{
    public static class Debug
    {
        public static void Log(object message) => WriteMessage(message, ConsoleColor.White);
        public static void LogAssert(bool prediction, object message = null) { if (!prediction) Log($"Assert error! {message}"); }

        public static void LogWarning(object message) => WriteMessage(message, ConsoleColor.Yellow);
        public static void LogWarningAssert(bool prediction, object message = null) { if (!prediction) LogWarning($"Assert error! {message}"); }

        public static void LogError(object message) => WriteMessage(message, ConsoleColor.Red);
        public static void LogErrorAssert(bool prediction, object message = null) { if (!prediction) LogError($"Assert error! {message}"); }

         public static void MessageBox(object message) => 
             ShowMessageBox(message, "Message", MessageBoxIcon.Information);
        public static void AssertMessageBox(bool prediction, object message) { if (!prediction) MessageBox($"Assert error! {message}"); }

         public static void MessageBoxWarning(object message) =>
             ShowMessageBox(message, "Warning", MessageBoxIcon.Warning);
        public static void AssertMessageBoxWarning(bool prediction, object message) { if (!prediction) MessageBoxWarning($"Assert error! {message}"); }

         public static void MessageBoxError(object message) =>
             ShowMessageBox(message, "Error", MessageBoxIcon.Error);
        public static void AssertMessageBoxError(bool prediction, object message) { if (!prediction) MessageBoxError($"Assert error! {message}"); }

        public static void Crash(object message)
        {
            ShowMessageBox(message, "Crash", MessageBoxIcon.Error);
            Environment.Exit(1);
        }

        public static void CrashAssert(bool prediction, object message = null) { if (!prediction) Crash($"Assert error!\n{message}"); }

        private static void WriteMessage(object message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }

        private static void ShowMessageBox(object message, object title, MessageBoxIcon icon)
        {
            DamnEngine.MessageBox.Show(message.ToString(), title.ToString(), MessageBoxButtons.OK, icon);
        }
    }
}