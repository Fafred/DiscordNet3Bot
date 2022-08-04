namespace DiscordNet3Bot.Services
{
    using System.Reflection;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;

    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commandService,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _client = client;
            _commandService = commandService;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)) return;

            if (message.Source != Discord.MessageSource.User) return;

            int argPos = 0;

            if (!message.HasStringPrefix(_configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            try
            {
                var context = new SocketCommandContext(_client, message);

                await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}