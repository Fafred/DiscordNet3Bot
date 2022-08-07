namespace BotDatabases.Context
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public partial class EquipmentContext : DbContext
    {

        // This variable needs to be the same name as the table.
        public virtual DbSet<Models.EquipmentModel> Equipment { get; set; }

        public EquipmentContext(DbContextOptions contextOptions)
            : base(contextOptions)
        {
            //_configuration = configuration;
        }

        /*
        private IConfiguration _configuration;

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
        */
    }
}
