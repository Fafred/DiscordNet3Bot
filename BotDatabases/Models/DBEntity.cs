namespace BotDatabases.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class DBEntity
    {
        [Key]
        public int id { get; set; }
    }
}
