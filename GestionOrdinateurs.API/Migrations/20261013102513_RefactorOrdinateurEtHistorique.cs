using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class RefactorOrdinateurEtHistorique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Processeur",
                table: "Ordinateurs",
                newName: "Caracteristiques_Processeur");

            migrationBuilder.RenameColumn(
                name: "Marque",
                table: "Ordinateurs",
                newName: "Caracteristiques_Marque");

            migrationBuilder.RenameColumn(
                name: "Ram",
                table: "Ordinateurs",
                newName: "Etat");

            migrationBuilder.RenameColumn(
                name: "EtatOrdinateur",
                table: "Ordinateurs",
                newName: "Caracteristiques_TypeOrdi");

            migrationBuilder.AddColumn<string>(
                name: "Technicien",
                table: "Reparations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Caracteristiques_RamGo",
                table: "Ordinateurs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Caracteristiques_StockageGo",
                table: "Ordinateurs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Technicien",
                table: "Reparations");

            migrationBuilder.DropColumn(
                name: "Caracteristiques_RamGo",
                table: "Ordinateurs");

            migrationBuilder.DropColumn(
                name: "Caracteristiques_StockageGo",
                table: "Ordinateurs");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_Processeur",
                table: "Ordinateurs",
                newName: "Processeur");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_Marque",
                table: "Ordinateurs",
                newName: "Marque");

            migrationBuilder.RenameColumn(
                name: "Etat",
                table: "Ordinateurs",
                newName: "Ram");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_TypeOrdi",
                table: "Ordinateurs",
                newName: "EtatOrdinateur");

            migrationBuilder.InsertData(
                table: "Ordinateurs",
                columns: new[] { "Id", "EtatOrdinateur", "Marque", "Nom", "Prix", "Processeur", "Ram", "Stock" },
                values: new object[,]
                {
                    { 1, 0, "Dell", "PC-SAFIA-01", 899.99m, "Intel Core i7", 16, 3 },
                    { 2, 0, "HP", "PC-COMPTA-01", 599.99m, "Intel Core i5", 8, 2 },
                    { 3, 0, "Lenovo", "PC-DIRECTION-01", 1199.99m, "AMD Ryzen 7", 32, 1 },
                    { 4, 1, "Apple", "PC-DESIGN-01", 1499.99m, "Apple M2", 32, 0 },
                    { 5, 2, "Asus", "PC-MOBILE-01", 749.99m, "AMD Ryzen 5", 16, 0 }
                });
        }
    }
}
