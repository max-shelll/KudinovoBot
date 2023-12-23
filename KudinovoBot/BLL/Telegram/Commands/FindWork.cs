using KudinovoBot.DAL.Headers;
using PRTelegramBot.Attributes;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.TCommands;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.Commands
{
    public class FindWork
    {
        [ReplyMenuHandler("работа в кудиново")]
        public async Task Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            string msg = "🔎 Добро пожаловать в сервис по поиску работы.";

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(3, new()
                {
                    new InlineCallback("Создать объявление", WorkTHeader.Create),
                    new InlineCallback<PageTCommand>("➡️", WorkTHeader.NextPage, new PageTCommand(0, WorkTHeader.NextPage)),
                }),
            };

            await Message.Send(botClient: client, update: update, msg: msg, option: options);
        }
    }
}
