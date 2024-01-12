using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfflineSpotifyPlaylistTracker.Domain.Migrations
{
    public partial class AddScottUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "ImageName" },
                values: new object[] { "1252730340", "Scott Leah", "sl" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1252730340");
        }
    }
}
