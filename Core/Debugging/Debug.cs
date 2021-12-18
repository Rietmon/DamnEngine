using System;
using System.Windows.Forms;

namespace DamnEngine
{
    public static class Debug
    {
        public static Action OnCrash { get; set; }
        
        // Not working with Rider debug mode...
        // static Debug()
        // {
        //     AppDomain.CurrentDomain.FirstChanceException += (obj, e) =>
        //     {
        //         Crash($"Ooops...\n{e.Exception.Message}\n{e.Exception.StackTrace}");
        //     };
        // }
        
        public static void Log(object message) => WriteMessage(message, ConsoleColor.White);
        public static bool LogAssert(bool prediction, object message = null) { if (!prediction) Log($"Assert! {message}"); return !prediction; }

        public static void LogWarning(object message) => WriteMessage(message, ConsoleColor.Yellow);
        public static bool LogWarningAssert(bool prediction, object message = null) { if (!prediction) LogWarning($"Assert warning! {message}"); return !prediction; }

        public static void LogError(object message) => WriteMessage(message, ConsoleColor.Red);
        public static bool LogErrorAssert(bool prediction, object message = null) { if (!prediction) LogError($"Assert error! {message}"); return !prediction; }

         public static void MessageBox(object message) => 
             ShowMessageBox(message, "Message", MessageBoxIcon.Information);
        public static bool AssertMessageBox(bool prediction, object message) { if (!prediction) MessageBox($"Assert! {message}"); return !prediction; }

         public static void MessageBoxWarning(object message) =>
             ShowMessageBox(message, "Warning", MessageBoxIcon.Warning);
        public static bool AssertMessageBoxWarning(bool prediction, object message) { if (!prediction) MessageBoxWarning($"Assert warning! {message}"); return !prediction; }

         public static void MessageBoxError(object message) =>
             ShowMessageBox(message, "Error", MessageBoxIcon.Error);
        public static bool AssertMessageBoxError(bool prediction, object message) { if (!prediction) MessageBoxError($"Assert error! {message}"); return !prediction; }

        public static void Crash(object message)
        {
            ShowMessageBox(message, "Crash", MessageBoxIcon.Error);
            OnCrash?.Invoke();
            Environment.Exit(1);
        }

        public static bool CrashAssert(bool prediction, object message = null) { if (!prediction) Crash($"Assert error!\n{message}"); return !prediction; }

        private static void WriteMessage(object message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }

        private static void ShowMessageBox(object message, object title, MessageBoxIcon icon)
        {
            System.Windows.Forms.MessageBox.Show(message.ToString(), title.ToString(), MessageBoxButtons.OK, icon);
        }
    }
}