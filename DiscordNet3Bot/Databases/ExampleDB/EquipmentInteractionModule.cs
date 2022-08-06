namespace DiscordNet3Bot.Databases.ExampleDB
{
    using Discord;
    using Discord.Interactions;
    using Discord.Net;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Text;

    public class EquipmentInteractionModules : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IConfiguration _configuration;
        private readonly EquipmentContext _equipmentContext;

        public EquipmentInteractionModules(IServiceProvider services)
        {
            _configuration = services.GetRequiredService<IConfiguration>();
            _equipmentContext = services.GetRequiredService<EquipmentContext>();
        }

        public enum HandleEqCommandChoice
        {
            Add,
            Remove,
            Modify,
            List
        }

        [SlashCommand("eq", "/eq [Add/Remove/Modify/List]")]
        public async Task HandleEqCommand(HandleEqCommandChoice choice)
        {
            switch (choice)
            {
                case HandleEqCommandChoice.Add:
                    await RespondAsync("Add", ephemeral: true);
                    break;
                case HandleEqCommandChoice.Remove:
                    await RespondAsync("Remove", ephemeral: true);
                    break;
                case HandleEqCommandChoice.Modify:
                    await RespondAsync("Modify", ephemeral: true);
                    break;
                case HandleEqCommandChoice.List:
                    await RespondAsync(embed: EqCommandListEmbed()/*, ephemeral: true*/);
                    break;
                default:
                    await RespondAsync("Usage: /eq [Add/Remove/Modify/List]", ephemeral: true);
                    break;
            }   
        }

        // Create the embed for listing the equipment.
        private Embed EqCommandListEmbed()
        {
            var stringBuilder = new StringBuilder();
            var embedBuilder = new EmbedBuilder();

            // Grabbing all the equipment from our database.
            var equipments = _equipmentContext.Equipment.ToList<EquipmentModel>();

            // This uses LINQ to go through and find the longest string in the ItemDescription field.
            //  We take the length of the longest string and add 4 to it to ensure decent looking spacing.
            int descriptionPadLength = equipments
                .Select(x => (x.ItemDescription ?? "").Length)
                .Max();
            descriptionPadLength += 4;


            // Nows the boring stuff - user interface.
            stringBuilder.Append("`Description".PadRight(descriptionPadLength, ' '));
            stringBuilder.Append("Cost".PadRight(8, ' '));
            stringBuilder.Append("Weight (in coins)`\n");

            string fieldName = stringBuilder.ToString();
            stringBuilder.Clear();
            stringBuilder.AppendLine("`".PadRight(fieldName.Length, '-'));
            

            foreach(var equipment in equipments)
            {
                stringBuilder.Append((equipment.ItemDescription ?? "").PadRight(descriptionPadLength, ' '));
                stringBuilder.Append($"{(equipment.CurrencyAmount ?? 0)} {(equipment.CurrencyType ?? "")}".PadRight(8, ' '));
                stringBuilder.AppendLine($"{(equipment.WeightInCoins ?? 0)}".PadRight(5,' '));
            }
            stringBuilder.AppendLine("`".PadLeft(fieldName.Length, '-'));

            embedBuilder
                .WithTitle("Equipment List")
                .AddField(fieldName, stringBuilder.ToString(), inline : false);

            return embedBuilder.Build();
        }
    }

}
