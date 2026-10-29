using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndVendu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Vendu",
                table: "Ordinateur",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Ordinateur",
                keyColumn: "Id",
                keyValue: 1,
                column: "Vendu",
                value: false);

            migrationBuilder.UpdateData(
                table: "Ordinateur",
                keyColumn: "Id",
                keyValue: 2,
                column: "Vendu",
                value: false);

            migrationBuilder.UpdateData(
                table: "Ordinateur",
                keyColumn: "Id",
                keyValue: 3,
                column: "Vendu",
                value: false);

            migrationBuilder.UpdateData(
                table: "Ordinateur",
                keyColumn: "Id",
                keyValue: 4,
                column: "Vendu",
                value: false);

            migrationBuilder.UpdateData(
                table: "Ordinateur",
                keyColumn: "Id",
                keyValue: 5,
                column: "Vendu",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "Vendu",
                table: "Ordinateur");
        }
    }
}
