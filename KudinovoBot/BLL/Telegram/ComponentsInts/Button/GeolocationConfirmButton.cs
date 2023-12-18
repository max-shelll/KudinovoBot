using PRTelegramBot.Attributes;
using PRTelegramBot.Models.InlineButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using KudinovoBot.DAL.InclineCallbackHeaders;
using PRTelegramBot.Models.CallbackCommands;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button
{
    public class AcceptButton
    {
        [InlineCallbackHandler<GeolocationHeader>(GeolocationHeader.Accept)]
        public async Task Accept(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            if (command != null)
            {
                string msg = "Выполнена команда accept";

                await Message.Send(botClient, update, msg);
            }
        }

        [InlineCallbackHandler<GeolocationHeader>(GeolocationHeader.Deny)]
        public async Task Deny(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            if (command != null)
            {
                string msg = "Выполнена команда deny";

                await Message.Send(botClient, update, msg);
            }
        }
    }
}
