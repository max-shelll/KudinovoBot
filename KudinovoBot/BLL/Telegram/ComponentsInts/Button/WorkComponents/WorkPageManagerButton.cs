using PRTelegramBot.Attributes;
using PRTelegramBot.Models.InlineButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using KudinovoBot.DAL.Repositories;
using PRTelegramBot.Models.CallbackCommands;
using KudinovoBot.DAL.Headers;
using PRTelegramBot.Models.TCommands;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models;
using PRTelegramBot.Helpers;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models.Interface;
using MongoDB.Driver;

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button.WorkComponents
{
    public class WorkPageManagerButton
    {
        private readonly WorkRepository _workRepo;

        public WorkPageManagerButton(WorkRepository workRepo)
        {
            _workRepo = workRepo;
        }

        [InlineCallbackHandler<WorkHeader>(WorkHeader.PreviousPage, WorkHeader.NextPage)]
        public async Task ChangePage(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback<PageTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            var header = (WorkHeader)command.Data.Header;

            var works = (await _workRepo.GetAllAsync()).Select(w => w.Text).ToList();

            if (works.Count() <= 0)
            {
                await Message.NotifyFromCallBack(botClient, update.CallbackQuery.Id, "К сожалению, мы не нашли актуальных вакансий 🔎");
                return;
            }

            string msg = GetMessage(works, command.Data.Page, out bool reachedLastPage);

            var buttons = GetInlineButtons(command.Data.Page, works.Count, reachedLastPage);
            var options = new OptionMessage
            {
                MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(3, buttons),
            };

            UpdatePageNumber(header, command.Data, reachedLastPage);

            await Message.Edit(botClient: botClient, update: update, msg: msg, option: options);
        }

        private string GetMessage(List<string> works, int page, out bool reachedLastPage)
        {
            reachedLastPage = (page == works.Count);

            if (reachedLastPage)
            {
                return """
                    🔎 Похоже, вы посмотрели все актуальные объявления. 

                    Если все вышеперечисленные варианты не подошли — ожидайте появления новых 🕒
                    """;
            }
            else
            {
                return works.ElementAt(page);
            }
        }

        private List<IInlineContent> GetInlineButtons(int page, int worksCount, bool reachedLastPage)
        {
            var backButton = (page > 0) ? new InlineCallback<PageTCommand>("⬅️", WorkHeader.PreviousPage, new PageTCommand(page - 1, WorkHeader.PreviousPage))
                : null;

            var createButton = new InlineCallback("Создать объявление", WorkHeader.Create);

            var nextButton = (page < worksCount) ? new InlineCallback<PageTCommand>("Другие вакансии", WorkHeader.NextPage, new PageTCommand(page + 1, WorkHeader.NextPage))
                : null;

            return new List<IInlineContent> { backButton, createButton, nextButton }.Where(b => b != null).Select(b => b!).ToList();
        }

        private void UpdatePageNumber(WorkHeader header, PageTCommand commandData, bool reachedLastPage)
        {
            if (header == WorkHeader.PreviousPage)
            {
                commandData.Page -= 1;
            }
            else if (header == WorkHeader.NextPage && !reachedLastPage)
            {
                commandData.Page += 1;
            }
        }
    }
}
