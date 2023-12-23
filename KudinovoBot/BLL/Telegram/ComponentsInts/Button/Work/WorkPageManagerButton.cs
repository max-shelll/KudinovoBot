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

namespace KudinovoBot.BLL.Telegram.ComponentsInts.Button.Work
{
    public class WorkPageManagerButton
    {
        private readonly WorkRepository _workRepo;

        public WorkPageManagerButton(WorkRepository workRepo)
        {
            _workRepo = workRepo;
        }

        [InlineCallbackHandler<WorkTHeader>(WorkTHeader.PreviousPage, WorkTHeader.NextPage)]
        public async Task PreviousPage(ITelegramBotClient botClient, Update update)
        {
            var command = InlineCallback<PageTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            var header = (WorkTHeader)command.Data.Header;

            var works = (await _workRepo.GetAllAsync()).Select(w => w.Text).ToList();
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
            var backButton = (page > 0) ? new InlineCallback<PageTCommand>("⬅️", WorkTHeader.PreviousPage, new PageTCommand(page - 1, WorkTHeader.PreviousPage))
                : null;

            var createButton = new InlineCallback("Создать объявление", WorkTHeader.Create);

            var nextButton = (page < worksCount) ? new InlineCallback<PageTCommand>("➡️", WorkTHeader.NextPage, new PageTCommand(page + 1, WorkTHeader.NextPage))
                : null;

            return new List<IInlineContent> { backButton, createButton, nextButton }.Where(b => b != null).Select(b => b!).ToList();
        }

        private void UpdatePageNumber(WorkTHeader header, PageTCommand commandData, bool reachedLastPage)
        {
            if (header == WorkTHeader.PreviousPage)
            {
                commandData.Page -= 1;
            }
            else if (header == WorkTHeader.NextPage && !reachedLastPage)
            {
                commandData.Page += 1;
            }
        }
    }
}
