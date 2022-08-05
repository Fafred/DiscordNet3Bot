namespace DiscordNet3Bot
{
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        // Don't forget to update the appsettings.json file with your bot's token and guild Id.

        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                services
                .AddSingleton(configuration)
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.All,
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 200,
                }))
                // Get our interactions in here.
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<Services.InteractionHandler>()
                // Get our commands in here.
                .AddSingleton(x => new CommandService())
                .AddSingleton<Services.CommandHandler>()
                // Hook up the databases.
                .AddDbContext<ExampleDB.EquipmentDBEntities>())
                .Build();

            using (host)
            {
                await RunASync(host);
            }
        }

        public async Task RunASync(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            var configuration = serviceProvider.GetRequiredService<IConfigurationRoot>();

            var slashCommands = serviceProvider.GetRequiredService<InteractionService>();
            await serviceProvider.GetRequiredService<Services.InteractionHandler>().InitializeAsync();

            var prefixCommands = serviceProvider.GetRequiredService<Services.CommandHandler>();
            await serviceProvider.GetRequiredService<Services.CommandHandler>().InitializeAsync();

            client.Log += async (LogMessage logMessage) => { Console.WriteLine(logMessage.Message); };
            slashCommands.Log += async (LogMessage logMessage) => { Console.WriteLine(logMessage.Message); };

            client.Ready += async () =>
            {
                await slashCommands.RegisterCommandsToGuildAsync(ulong.Parse(configuration["TestGuild"]));
                Console.WriteLine("Hello Discord.");
            };

            await client.LoginAsync(TokenType.Bot, configuration["Token"]);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}