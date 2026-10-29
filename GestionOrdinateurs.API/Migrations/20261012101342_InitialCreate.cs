using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ordinateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Marque = table.Column<string>(type: "TEXT", nullable: false),
                    Ram = table.Column<int>(type: "INTEGER", nullable: false),
                    Processeur = table.Column<string>(type: "TEXT", nullable: false),
                    EtatOrdinateur = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordinateurs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Ordinateurs",
                columns: new[] { "Id", "EtatOrdinateur", "Marque", "Nom", "Processeur", "Ram" },
                values: new object[,]
                {
                    { 1, 0, "Dell", "PC-SAFIA-01", "Intel Core i7", 16 },
                    { 2, 0, "HP", "PC-COMPTA-01", "Intel Core i5", 8 },
                    { 3, 0, "Lenovo", "PC-DIRECTION-01", "AMD Ryzen 7", 32 },
                    { 4, 1, "Apple", "PC-DESIGN-01", "Apple M2", 32 },
                    { 5, 2, "Asus", "PC-MOBILE-01", "AMD Ryzen 5", 16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordinateurs");
        }
    }
}
