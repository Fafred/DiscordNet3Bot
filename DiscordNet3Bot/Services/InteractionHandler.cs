namespace DiscordNet3Bot.Services
{
    using System.Reflection;
    using Discord.Interactions;
    using Discord.WebSocket;

    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _serviceProvider;

        public InteractionHandler(
            DiscordSocketClient client,
            InteractionService interactionService,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _interactionService = interactionService;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

            _client.InteractionCreated += HandleInteraction;

        }

        private async Task HandleInteraction(SocketInteraction socketInteraction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, socketInteraction);
                await _interactionService.ExecuteCommandAsync(context, _serviceProvider);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}