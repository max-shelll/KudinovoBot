using KudinovoBot.DAL.InclineCallbackHeaders;
using PRTelegramBot.Attributes;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
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
            string msg =
            $"""
            🔎 Похоже, вы посмотрели все актуальные объявления. 

            Если все вышеперечисленные варианты не подошли — ожидайте появления новых 🕒
            """;

            var menu = MenuGenerator.InlineKeyboard(1, new()
            {
                new InlineCallback("Создать объявление", WorkManagerHeader.Create),
            });

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = menu,
            };

            await Message.Send(botClient: client, update: update, msg: msg, option: options);
        }
    }
}
