using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class AjouterDonneesOrdinateurs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ordinateurs",
                columns: new[] { "Id", "Caracteristiques_Marque", "Caracteristiques_Processeur", "Caracteristiques_RamGo", "Caracteristiques_StockageGo", "Caracteristiques_TypeOrdi", "Etat", "Nom", "Prix", "Stock" },
                values: new object[,]
                {
                    { 1, "Dell", "Intel Core i7", 16, 512, 1, 0, "PC-SAFIA-01", 899.99m, 3 },
                    { 2, "HP", "Intel Core i5", 8, 256, 1, 0, "PC-COMPTA-01", 599.99m, 2 },
                    { 3, "Lenovo", "AMD Ryzen 7", 32, 1000, 1, 0, "PC-DIRECTION-01", 1199.99m, 1 },
                    { 4, "Apple", "Apple M2", 32, 1000, 1, 2, "PC-DESIGN-01", 1499.99m, 0 },
                    { 5, "Asus", "AMD Ryzen 5", 16, 512, 0, 3, "PC-MOBILE-01", 749.99m, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ordinateurs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ordinateurs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ordinateurs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ordinateurs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ordinateurs",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
