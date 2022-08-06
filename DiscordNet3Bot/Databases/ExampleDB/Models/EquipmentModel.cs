namespace DiscordNet3Bot.Databases.ExampleDB
{
    using System.ComponentModel.DataAnnotations;

    public partial class EquipmentModel : DBEntity
    {
        public string? ItemDescription { get; set; }

        public int? CurrencyAmount { get; set; }

        public string? CurrencyType { get; set; }

        public int? WeightInCoins { get; set; }
    }
}
