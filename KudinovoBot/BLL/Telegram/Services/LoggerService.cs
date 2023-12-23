using Pastel;
using System.Drawing;
using static PRTelegramBot.Core.PRBot;

namespace KudinovoBot.BLL.Telegram.Services
{
    public class LoggerService
    {
        public static void LogErrorAsync(Exception exception, long? botId)
        {
            string currentTime = DateTime.Now.ToString("dd.MM.yy | HH:mm");

            Console.WriteLine(
                   $"[{currentTime.Pastel(Color.DarkOrange)}] | " +
                   $"[{"API Error".Pastel(Color.Red)}] > " +
                   $"\n{exception.Message.Pastel(Color.GhostWhite)}");
        }

        public static void LogCommonAsync(string msg, TelegramEvents typeEvent, ConsoleColor color)
        {
            string currentTime = DateTime.Now.ToString("dd.MM.yy | HH:mm");

            Console.WriteLine(
                   $"[{currentTime.Pastel(Color.DarkOrange)}] | " +
                   $"[{"Info".Pastel(Color.Cyan)} | {typeEvent.ToString().Pastel(color)}] > " +
                   $"{msg.Pastel(Color.GhostWhite)}");
        }
    }
}
