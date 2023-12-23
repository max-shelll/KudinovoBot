using KudinovoBot.DAL.InclineCallbackHeaders;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button
{
    public class CreateWorkButton
    {
        [InlineCallbackHandler<WorkManagerHeader>(WorkManagerHeader.Create)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            if (command != null)
            {
                string msg = $"Выполнена команда от {update.Message.From.Username}";

                await Message.Send(botClient, update, msg);
            }
        }
    }
}
