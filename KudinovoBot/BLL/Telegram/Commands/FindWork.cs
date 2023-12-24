using KudinovoBot.DAL.Headers;
using KudinovoBot.DAL.Repositories;
using MongoDB.Driver.Core.WireProtocol.Messages;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
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
        private readonly WorkRepository _workRepo;

        public FindWork(WorkRepository workRepo)
        {
            _workRepo = workRepo;
        }

        [ReplyMenuHandler("работа в кудиново")]
        public async Task Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            var works = await _workRepo.GetAllAsync();

            string msg = "";
            if (works.Count == 0)
            {
                msg = "К сожалению, мы не нашли актуальных вакансий 🔎";
            }
            else
            {
                msg = works.First().Text;
            }

            var options = new OptionMessage()
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(3, new()
                {
                    new InlineCallback<PageTCommand>("Другие вакансии", WorkHeader.NextPage, new PageTCommand(0, WorkHeader.NextPage)),
                    new InlineCallback("Создать объявление", WorkHeader.Create),
                }),
            };

            await Message.Send(botClient: client, update: update, msg: msg, option: options);
        }
    }
}
