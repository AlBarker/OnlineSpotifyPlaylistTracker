using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSpotifyPlaylistTracker.Domain.Migrations
{
    public partial class FixBillyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "billy.sakalis");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "ImageName" },
                values: new object[] { "billysakalis", "Billy Sakalis", "bs" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "billysakalis");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "ImageName" },
                values: new object[] { "billy.sakalis", "Billy Sakalis", "bs" });
        }
    }
}
