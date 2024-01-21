using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSpotifyPlaylistTracker.Domain.Migrations
{
    public partial class Add2023Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "ImageName" },
                values: new object[] { "113424562", "Manu Du Fromage", "mf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "ImageName" },
                values: new object[] { "12138108557", "Wildcard", "beer3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "113424562");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "12138108557");
        }
    }
}
