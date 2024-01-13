using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSpotifyPlaylistTracker.Domain.Migrations
{
    public partial class TrackUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Tracks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Tracks");
        }
    }
}
