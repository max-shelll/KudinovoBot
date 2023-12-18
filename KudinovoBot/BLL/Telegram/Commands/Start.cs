using PRTelegramBot.Attributes;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;
using PRTelegramBot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Extensions;
using KudinovoBot.DAL.Parameters;
using System.Net.Http.Json;
using KudinovoBot.DAL.Configs;
using KudinovoBot.DAL.InclineCallbackHeaders;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.Interface;

namespace KudinovoBot.BLL.Telegram.Commands
{
    public class Start
    {
        [SlashHandler("start")]
        public async Task Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            string msg = 
            $"""
            Привет, {message.From.FirstName}! На связи бот Кудя 👋

            Пожалуйста, уточните ваш город 🔎
            Например: Ногинск, Электроугли, село Кудиново и т.п.
            """;

            update.RegisterNextStep(new StepTelegram(GetLocation, new StartParams()));
            await Message.Send(client, update, msg);
        }

        private async Task GetLocation(ITelegramBotClient botClient, Update update, CustomParameters args)
        {
            var message = update.Message;
            update.ClearStepUser();

            string cityName = message.Text;
            string baseUrl = "https://nominatim.openstreetmap.org/search?format=json&q=";
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "KudinovoBot");
                var response = await client.GetAsync(baseUrl + cityName);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<GeolocationData>>();
                    if (content?.Any() == true)
                    {
                        var displayAddress = content[0].display_name;
                        await SendLocationInfo(botClient, update, displayAddress);
                    }
                    else
                    {
                        await Message.Send(botClient, update, "Город не найден.");
                    }
                }
                else
                {
                    await Message.Send(botClient, update, "Произошла ошибка при получении данных о местоположении.");
                }
            }
            catch (HttpRequestException)
            {
                await Message.Send(botClient, update, "Ошибка при обращении к серверу.");
            }
            catch (Exception)
            {
                await Message.Send(botClient, update, "Произошла непредвиденная ошибка.");
            }
        }

        private async Task SendLocationInfo(ITelegramBotClient botClient, Update update, string displayAddress)
        {
            List<IInlineContent> buttons = new()
            {
                new InlineCallback("✅", GeolocationHeader.Accept),
                new InlineCallback("❌", GeolocationHeader.Deny),
            };

            var generateMenu = MenuGenerator.InlineKeyboard(2, buttons);
            var options = new OptionMessage { MenuInlineKeyboardMarkup = generateMenu };

            await Message.Send(botClient, update, $"Город: {displayAddress}", options);
        }
    }
}
