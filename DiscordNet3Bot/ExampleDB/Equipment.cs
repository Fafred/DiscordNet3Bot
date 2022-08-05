namespace DiscordNet3Bot.ExampleDB
{
    using System.ComponentModel.DataAnnotations;

    public partial class Equipment
    {
        [Key]
        public int? Id { get; set; }

        public string? ItemDescription { get; set; }

        public int? CurrencyAmount { get; set; }

        public string? CurrencyType { get; set; }

        public int? WeightInCoins { get; set; }
    }
}
