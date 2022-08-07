namespace DiscordNet3Bot.Databases.ExampleDB
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    // This should be fine for personal bot use, but all of this should really
    //  be in another library with a Data Access Layer.  That's more involved
    //  and a bit more work and this is servicible.  I'll probably branch off
    //  this master and do it a bit more properly.
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
