using KudinovoBot.BLL.Telegram.Handlers;
using KudinovoBot.BLL.Telegram.Services;
using KudinovoBot.DAL.Configs;
using KudinovoBot.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace KudinovoBot
{
    public class Program
    {
        private readonly IServiceProvider _service;
        private readonly PRBot _client;

        private readonly Config _config;

        public Program()
        {
            string json = File.ReadAllText("config.json");
            _config = JsonConvert.DeserializeObject<Config>(json) ?? throw new InvalidOperationException("Failed to load configuration.");

            _service = new ServiceCollection()
                 .AddBotHandlers()
                 .AddSingleton(_config)
                 /// Repositories
                 .AddSingleton<WorkRepository>()
                 .BuildServiceProvider();

            _client = new PRBot(options =>
            {
                options.Token = _config.BotToken;
                options.ClearUpdatesOnStart = true;
                options.WhiteListUsers = new List<long>();
                options.Admins = new List<long>();
                options.BotId = 0;
            }, _service);
        }

        static void Main(string[] args)
            => new Program().RunAsync().GetAwaiter().GetResult();

        private async Task RunAsync()
        {
            _client.OnLogError += LoggerService.LogErrorAsync;
            _client.OnLogCommon += LoggerService.LogCommonAsync;

            HandlerInts();

            await _client.Start();;

            await Task.Delay(Timeout.Infinite);
        }

        private void HandlerInts()
        {
            if (_client == null)
                return;

            _client.Handler.Router.OnMissingCommand += MissingCommandHandler.Execute;
        }
    }
}