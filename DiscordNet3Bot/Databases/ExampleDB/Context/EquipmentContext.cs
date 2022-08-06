namespace DiscordNet3Bot.Databases.ExampleDB
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public partial class EquipmentContext : DbContext
    {
        private IConfiguration _configuration;

        // This variable needs to be the same name as the table.
        public virtual DbSet<EquipmentModel> Equipment { get; set; }

        public EquipmentContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = _configuration["EquipmentDBDataSource"],
                Mode = SqliteOpenMode.ReadWrite,
            };

            var connectionString = connectionStringBuilder.ToString();

            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
