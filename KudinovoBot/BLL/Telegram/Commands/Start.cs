using PRTelegramBot.Attributes;
using Telegram.Bot.Types;
using Telegram.Bot;
using Message = PRTelegramBot.Helpers.Message;

namespace KudinovoBot.BLL.Telegram.Commands
{
    [TelegramBotHandler]
    public class Start
    {
        [SlashHandler("start")]
        public async Task Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;

            string text =
            $"""
            Привет, {message.From.FirstName}! На связи бот Кудя 👋

            Тут ты можешь:
            ・найти интересные места в Кудиново, и взаимодействовать с ними прямо тут
            ・сохранять в избранное понравившиеся места
            ・участвовать в розыгрышах от наших партнёров

            🔎 Для поиска места напишите его название, например: шаурмечная, кафе, пятёрочка, магнит, пвз, озон, валдберис и так далее.
            
            """;

            await Message.Send(client, update, text);
        }
    }
}
