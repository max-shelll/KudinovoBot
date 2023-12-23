using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.Commands
{
    public class Start
    {
        [SlashHandler("start")]
        public async Task Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            string msg =
            $"""
            Привет, {message.From.FirstName}! На связи бот Кудя 👋
            """;


            await Message.Send(botClient: client, update: update, msg: msg);
        }
    }
}
