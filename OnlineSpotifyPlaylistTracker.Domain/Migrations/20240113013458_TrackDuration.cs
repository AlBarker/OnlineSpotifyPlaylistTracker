using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSpotifyPlaylistTracker.Domain.Migrations
{
    public partial class TrackDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMs",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMs",
                table: "Tracks");
        }
    }
}
