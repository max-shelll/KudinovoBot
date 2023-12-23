using KudinovoBot.DAL.Headers;
using PRTelegramBot.Attributes;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.TCommands;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;
using Telegram.Bot.Types.ReplyMarkups;

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

            var options = new OptionMessage()
            {
                MenuReplyKeyboardMarkup = MenuGenerator.ReplyKeyboard(1, new List<KeyboardButton>()
                {
                   new KeyboardButton("Работа в Кудиново"),
                }),
            };

            await Message.Send(botClient: client, update: update, msg: msg, option: options);
        }
    }
}
