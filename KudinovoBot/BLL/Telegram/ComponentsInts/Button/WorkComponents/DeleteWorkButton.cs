using KudinovoBot.DAL.Headers;
using KudinovoBot.DAL.Repositories;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using Telegram.Bot;
using Telegram.Bot.Types;
using KudinovoBot.DAL.Models;
using Message = PRTelegramBot.Helpers.Message;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Helpers;
using KudinovoBot.DAL.TCommand;
using PRTelegramBot.Models.TCommands;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button.WorkComponents
{
    public class DeleteWorkButton
    {
        private readonly WorkRepository _workRepo;

        public DeleteWorkButton(WorkRepository workRepo)
        {
            _workRepo = workRepo;
        }

        [InlineCallbackHandler<WorkTHeader>(WorkTHeader.Remove)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback<WorkTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            await _workRepo.DeleteById(command.Data.WorkId);

            await Message.Send(botClient: botClient, update: update, msg: "Объявление было успешно удалено!");
        }
    }
}
