namespace DiscordNet3Bot.Modules
{
    using Discord.Commands;
    using Discord;

    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        [Alias("hi", "greetings")]
        public async Task HandleHelloCommand()
        {
            await ReplyAsync("World.");
        }
    }
}