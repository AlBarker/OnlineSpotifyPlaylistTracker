using Microsoft.EntityFrameworkCore;
using OnlineSpotifyPlaylistTracker.Domain.Models;

namespace OnlineSpotifyPlaylistTracker.Domain
{
    public class SpotifyPlaylistTrackerContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackPosition> TrackPositions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionString = "server=localhost;database=spotify-playlist-tracker;user=root;password=admin";
            optionsBuilder.UseMySql(
                connectionString, ServerVersion.AutoDetect(connectionString))
                // The following three options help with debugging, but should
                // be changed or removed for production.
                //.LogTo(Console.WriteLine, LogLevel.Information)
                //.EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var users = new List<User>
            {
                new User
                {
                    Id = "karnage11i",
                    DisplayName = "Alex Karney",
                    ImageName = "ak"
                },
                new User
                {
                    Id = "magsatire",
                    DisplayName = "Jack McGrath",
                    ImageName = "jm"
                },
                new User
                {
                    Id = "1232101260",
                    DisplayName = "Chris Quigley",
                    ImageName = "cq"
                },
                new User
                {
                    Id = "1238290776",
                    DisplayName = "Joshua Landy",
                    ImageName = "jl"
                },
                new User
                {
                    Id = "1233033915",
                    DisplayName = "Alex Barker",
                    ImageName = "ab"
                },
                new User
                {
                    Id = "1244598275",
                    DisplayName = "Daniel Hornblower",
                    ImageName = "dh"
                },
                new User
                {
                    Id = "genjamon1234",
                    DisplayName = "Josh Anderson",
                    ImageName = "ja"
                },
                new User
                {
                    Id = "braeden.wilson",
                    DisplayName = "Braeden Wilson",
                    ImageName = "bw"
                },
                new User
                {
                    Id = "1278556031",
                    DisplayName = "Matt Knightbridge",
                    ImageName = "mk"
                },
                new User
                {
                    Id = "griffkyn22",
                    DisplayName = "Griffyn Heels",
                    ImageName = "gh"
                },
                new User
                {
                    Id = "1252730340",
                    DisplayName = "Scott Leah",
                    ImageName = "sl"
                },
                new User
                {
                    Id = "113424562",
                    DisplayName = "Manu Du Fromage",
                    ImageName = "mf"
                },
                 new User
                {
                    Id = "12138108557",
                    DisplayName = "Wildcard",
                    ImageName = "beer3",
                },

            };

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasData(users);
            });
        }
    }
}