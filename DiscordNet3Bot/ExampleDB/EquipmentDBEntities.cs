namespace DiscordNet3Bot.ExampleDB
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public partial class EquipmentDBEntities : DbContext
    {
        public virtual DbSet<Equipment> Equipment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "./ExampleDB/SampleDB.db",
                Mode = SqliteOpenMode.ReadWrite,
            };

            var connectionString = connectionStringBuilder.ToString();

            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
