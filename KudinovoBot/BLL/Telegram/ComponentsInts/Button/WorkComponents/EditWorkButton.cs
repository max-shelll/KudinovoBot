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
using KudinovoBot.DAL.Parameters;
using KudinovoBot.DAL.Configs;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button.WorkComponents
{
    public class EditWorkButton
    {
        private readonly WorkRepository _workRepo;
        private readonly Config _config;

        public EditWorkButton(
            WorkRepository workRepo,
            Config config)
        {
            _workRepo = workRepo;
            _config = config;
        }

        [InlineCallbackHandler<WorkTHeader>(WorkTHeader.Edit)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback<WorkTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            var msg = "Введите новое сообщение для того чтобы заменить старое";

            update.RegisterNextStep(new StepTelegram(CreateWorkPost, new WorkParameters() { WorkId = command.Data.WorkId}));

            await Message.Send(botClient: botClient, update: update, msg: msg);
        }

        private async Task CreateWorkPost(ITelegramBotClient botClient, Update update, CustomParameters args)
        {
            var message = update.Message;
            var data = args as WorkParameters;

            string msg = $"Вы обновили пост:\n {message.Text}\n @{message.From.Username} ©";

            var workDb = await _workRepo.GetById(data.WorkId);
            workDb.Text =
            $"""
            {message.Text}
                
            @{message.From.Username} ©
            """;

            await _workRepo.UpdateAsync(workDb);

            update.ClearStepUser();

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                {
                    new InlineCallback<WorkTCommand>("Редактировать", WorkTHeader.Edit, new WorkTCommand(workDb.Id)),
                    new InlineCallback<WorkTCommand>("Удалить", WorkTHeader.Remove, new WorkTCommand(workDb.Id)),
                }),
            };

            await Message.Send(botClient: botClient, update: update, msg: msg, option: options);

            await SendToOwner(botClient, workDb);
        }

        private async Task SendToOwner(ITelegramBotClient botClient, Work work)
        {
            string msg = $"Был обновлен пост:\n {work.Text}";

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                {
                    new InlineCallback<WorkTCommand>("Редактировать", WorkTHeader.Edit, new WorkTCommand(work.Id)),
                    new InlineCallback<WorkTCommand>("Удалить", WorkTHeader.Remove, new WorkTCommand(work.Id)),
                }),
            };
            await Message.Send(botClient: botClient, chatId: _config.OwnerId, msg: msg, option: options);
        }
    }
}
