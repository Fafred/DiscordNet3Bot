using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotDatabases.Migrations
{
    public partial class InstallMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemDescription = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencyAmount = table.Column<int>(type: "INTEGER", nullable: true),
                    CurrencyType = table.Column<string>(type: "TEXT", nullable: true),
                    WeightInCoins = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
