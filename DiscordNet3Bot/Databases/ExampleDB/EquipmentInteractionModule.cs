namespace DiscordNet3Bot.Databases.ExampleDB
{
    using Discord;
    using Discord.Interactions;
    using Microsoft.Extensions.DependencyInjection;
    using System.Text;

    [Group("eq", "Interact with the equipment database.")]
    public class EquipmentInteractionModules : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly EquipmentContext _equipmentContext;

        public EquipmentInteractionModules(IServiceProvider services)
        {
            _equipmentContext = services.GetRequiredService<EquipmentContext>();
        }


        [SlashCommand("add", "[Description] [# of coins] [coin type (pp/ep/gp/sp/cp)] [weight in coins]")]
        public async Task HandleAddEquipment([ComplexParameter] ComplexEquipmentParam param)
        {
            EquipmentModel equipment = new EquipmentModel();

            equipment.ItemDescription = param.Description.Trim();
            equipment.CurrencyAmount = param.AmountOfCoins;
            equipment.WeightInCoins = param.WeightInCoins;

            var coinType = param.CoinType.ToLower().Trim();

            switch (coinType)
            {
                case "pp":
                case "ep":
                case "gp":
                case "sp":
                case "cp":
                    equipment.CurrencyType = coinType;
                    break;
                default:
                    equipment.CurrencyType = "gp";
                    break;
            }

            _equipmentContext.Add(equipment);
            _equipmentContext.SaveChanges();

            await RespondAsync($"Added item:\n{param}", ephemeral: true);
        }

        [SlashCommand("list", "Lists equipment currently in the database.")]
        public async Task HandleListEquipment()
        {
            var stringBuilder = new StringBuilder();
            var embedBuilder = new EmbedBuilder();

            // If equipmentModels was null, then pull all the items from the db.  Either way, order alphabetically by ItemDescription.
            var equipments = _equipmentContext.Equipment.ToList<EquipmentModel>();
            equipments.OrderBy(x => x.ItemDescription);

            // This uses an extension method "EquipmentInfoListToField" defined in EmbedBuilderExtensions.
            embedBuilder
                .WithTitle("Equipment List")
                .WithColor(Color.Gold)
                .EquipmentInfoListToField(equipments);

            await RespondAsync(embed: embedBuilder.Build(), ephemeral: true);
        }

        [SlashCommand("remove", "Removes an item from the database.  [id] [description of item]")]
        public async Task HandleRemoveEquipment(int id, string description)
        {
            var equipments = _equipmentContext.Equipment.ToList<EquipmentModel>();

            var foundItem = equipments
                .Find(x => x.id == id && (x.ItemDescription ?? "").Equals(description, StringComparison.OrdinalIgnoreCase));

            if (foundItem is not null)
            {
                _equipmentContext.Remove(foundItem);
                _equipmentContext.SaveChanges();
                await RespondAsync("Item has been removed.", ephemeral: true);
            }
            else
            {
                await RespondAsync($"Item not found: `{{**id:** *{id}*\t**desc:** *{description}*}}`");
            }
        }

        [SlashCommand("modify", "Makes changes to an existing item in the database.  Tab through to the fields you want to change.")]
        public async Task HandleAddEquipment(int id, string? Description = null, int? AmountOfCoins = null, string? CoinType = null, int? WeightInCoins = null)
        {
            EquipmentModel equipment = new EquipmentModel();

            var equipments = _equipmentContext.Equipment.ToList<EquipmentModel>();

            var foundItem = equipments
                .Find(x => x.id == id);

            if (foundItem == null)
            {
                await RespondAsync($"Item not found: {{**id:** *{id}}}");
            }
            else
            {
                if (Description is null && AmountOfCoins is null && WeightInCoins is null && CoinType is null)
                {
                    // If all the inputs are null we'll create a modal.
                    //  This is purely to show an example of how to do this; the direct command interface is faster.

                }

                foundItem.ItemDescription = Description ?? foundItem.ItemDescription;
                foundItem.CurrencyAmount = AmountOfCoins ?? foundItem.CurrencyAmount;
                foundItem.WeightInCoins = WeightInCoins ?? foundItem.WeightInCoins;

                if (CoinType is not null)
                {
                    switch (CoinType = (CoinType.ToLower().Trim()))
                    {
                        case "pp":
                        case "ep":
                        case "gp":
                        case "sp":
                        case "cp":
                            foundItem.CurrencyType = CoinType;
                            break;
                    }
                }

                _equipmentContext.Update(foundItem);
                _equipmentContext.SaveChanges();

                await RespondAsync($"Item changed to:\n**Description:**\t`{foundItem.ItemDescription}`\n**Cost:**\t`{foundItem.CurrencyAmount} {foundItem.CurrencyType}`\n**Weight:**\t`{foundItem.WeightInCoins}`", ephemeral: true);
            }
        }
    }

    public class ModifyModal : IModal
    {
        public string Title => "Modify an item";
    }

    public class ComplexEquipmentParam
    {
        public string Description { get; }
        public int AmountOfCoins { get; }
        public string CoinType { get; }
        public int WeightInCoins { get; }

        [ComplexParameterCtor]
        public ComplexEquipmentParam(string description, int amountOfCoins, string coinType, int weightInCoins)
        {
            Description = description;
            AmountOfCoins = amountOfCoins;
            CoinType = coinType;
            WeightInCoins = weightInCoins;
        }

        public override string ToString()
        {
            return $"\n**Description:**\t`{Description}`\n**Cost:**\t`{AmountOfCoins} {CoinType}`\n**Weight:**\t`{WeightInCoins}`";
        }

    }
}
