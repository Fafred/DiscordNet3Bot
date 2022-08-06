namespace DiscordNet3Bot.Databases
{
    using System.ComponentModel.DataAnnotations;

    public partial class DBEntity
    {
        [Key]
        public int id { get; set; }
    }
}
