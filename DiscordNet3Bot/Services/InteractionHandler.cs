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

            // In order to deal with errors and exceptions, should probably:
            // Implement handling for post slash command execution:
            //      _client.SlashCommandExecuted += HandleSlashCommandExecuted
            //      ...HandleSlashCommandExecuted(SlashCommandInfo arg1, DiscordIInteractionContext arg2, IResult arg3)
            // And post context command execution:
            //      _client.ContextCommandExecuted += HandleContextCommandExecuted
            // And post component command execution:
            //      _client.ComponentCommandExecuted += HandleComponentCommandExecuted.
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

                // Delete the offending interaction if there was an error and send an ephemeral
                //  message with the error.
                if (socketInteraction.Type == Discord.InteractionType.ApplicationCommand) {
                    await socketInteraction.RespondAsync($"Error: {e}", ephemeral: true);
                    await socketInteraction.GetOriginalResponseAsync()
                        .ContinueWith(async (message) => await message.Result.DeleteAsync());
                }
            }
        }
    }
}