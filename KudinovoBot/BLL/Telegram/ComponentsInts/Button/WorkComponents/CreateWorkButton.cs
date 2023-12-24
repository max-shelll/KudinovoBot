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
using KudinovoBot.DAL.Configs;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button.WorkComponents
{
    public class CreateWorkButton
    {
        private readonly WorkRepository _workRepo;
        private readonly Config _config;

        public CreateWorkButton(
            WorkRepository workRepo,
            Config config)
        {
            _workRepo = workRepo;
            _config = config;
        }

        [InlineCallbackHandler<WorkHeader>(WorkHeader.Create)]
        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback.GetCommandByCallbackOrNull(update.CallbackQuery.Data);

            string msg = 
            $"""
            Опишите вакансию на примере анкеты:
            1. Название вакансии
            2. Описание (обязанности, зарплата, адрес) 💸
            3. Контакты 📞
            """;

            update.RegisterNextStep(new StepTelegram(CreateWorkPost));

            await Message.Send(botClient, update, msg);
        }

        private async Task CreateWorkPost(ITelegramBotClient botClient, Update update, CustomParameters args)
        {
            var message = update.Message;

            string msg = $"Вы создали новый пост:\n{message.Text}\n@{message.From.Username} ©";

            var work = new Work()
            {
                Text = $"{message.Text}\n@{message.From.Username} ©",
                Author = (message.From.Id, message.From.Username)
            };
            await _workRepo.CreateAsync(work);

            update.ClearStepUser();

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                {
                    new InlineCallback<WorkTCommand>("Редактировать", WorkHeader.Edit, new WorkTCommand(work.Id)),
                    new InlineCallback<WorkTCommand>("Удалить", WorkHeader.Remove, new WorkTCommand(work.Id)),
                }),
            };

            await Message.Send(botClient: botClient, update: update, msg: msg, option: options);

            await SendToOwner(botClient, work);
        }

        private async Task SendToOwner(ITelegramBotClient botClient, Work work)
        {
            string msg = $"Создан новый пост:\n{work.Text}";

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(2, new()
                {
                    new InlineCallback<WorkTCommand>("Редактировать", WorkHeader.Edit, new WorkTCommand(work.Id)),
                    new InlineCallback<WorkTCommand>("Удалить", WorkHeader.Remove, new WorkTCommand(work.Id)),
                }),
            };
            await Message.Send(botClient: botClient, chatId: _config.OwnerId, msg: msg, option: options);
        }
    }
}
