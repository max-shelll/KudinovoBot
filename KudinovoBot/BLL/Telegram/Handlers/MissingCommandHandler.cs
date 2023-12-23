using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.Handlers
{
    public static class MissingCommandHandler
    {
        public static async Task Execute(ITelegramBotClient botclient, Update update)
        {
            string msg = "Введённая вами команда не найдена ";
            await Message.Send(botclient, update, msg);
        }
    }
}
