namespace DiscordNet3Bot.ExampleDB
{
    using Discord;
    using Discord.Interactions;
    using Discord.Net;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;

    public class EquipmentInteractionModules : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("eq", "Add, modify, remove, or list equipment.")] 
        public async Task HandleEqCommand(SocketSlashCommand slashCommand)
        {
            // TODO
        }
    }

}
