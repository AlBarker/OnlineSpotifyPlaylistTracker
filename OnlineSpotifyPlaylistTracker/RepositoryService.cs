using Microsoft.EntityFrameworkCore;
using OnlineSpotifyPlaylistTracker.Domain;
using OnlineSpotifyPlaylistTracker.Domain.Models;

namespace OnlineSpotifyPlaylistTracker
{
    public class RepositoryService
    {
        public async Task<Track> GetTrackFromTrackPosition(int trackPosition)
        {
            using var context = new SpotifyPlaylistTrackerContext();
            var trackPos = await context.TrackPositions
                .Include(x => x.Track)
                .Include(x => x.Track.User)
                .FirstOrDefaultAsync(x => x.Position == trackPosition);

            if (trackPos == null)
            {
                Console.WriteLine($"Couldn't find track at position {trackPosition}");
                return null;
            }

            trackPos.IsPlayed = true;
            await context.SaveChangesAsync();

            return trackPos.Track;
        }

        public async Task<IList<Track>> GetPlayedTracks()
        {
            using var context = new SpotifyPlaylistTrackerContext();
            return await context.Tracks
                .Include(x => x.TrackPosition)
                .Include(x => x.User)
                .Where(x => x.TrackPosition.IsPlayed)
                .ToListAsync();
        }

        public async Task<IList<Track>> GetTracks()
        {
            using var context = new SpotifyPlaylistTrackerContext();
            return await context.Tracks.ToListAsync();
        }

        public async Task AddTracks(IEnumerable<SpotifyTrackModel> tracks)
        {
            using var context = new SpotifyPlaylistTrackerContext();
            var users = await GetUsers();
            
            var tracksToAdd = tracks.Select(x => new Track
            {
                Id = x.Id,
                Name = x.Name,
                Artist = x.Artist,
                AlbumArt = x.AlbumArt,
                FileName = x.FileName,
                UserId = users.First(u => u.Id == x.UserId)?.Id,
                Uri = x.Uri,
                DurationMs = x.DurationMs,
                Popularity = x.Popularity,
            });
            await context.Tracks.AddRangeAsync(tracksToAdd);
            await context.SaveChangesAsync();
        }

        public async Task<IList<User>> GetUsers()
        {
            using var context = new SpotifyPlaylistTrackerContext();
            return await context.Users.ToListAsync();
        }

        public async Task ClearAllTracks()
        {
            using var context = new SpotifyPlaylistTrackerContext();
            context.Tracks.RemoveRange(context.Tracks.ToList());

            await context.SaveChangesAsync();
        }

        public async Task ClearAndSaveTrackPositons(IList<TrackPosition> positonsToAdd)
        {
            using var context = new SpotifyPlaylistTrackerContext();
            var currentTrackPositions = context.TrackPositions;

            if (currentTrackPositions != null)
            {
                Console.WriteLine("Clearing old track positions");
                context.TrackPositions.RemoveRange(currentTrackPositions);
                Console.WriteLine($"Cleared {currentTrackPositions.Count()}");
            }

            context.TrackPositions.AddRange(positonsToAdd);

            await context.SaveChangesAsync();
            Console.WriteLine($"Successfully saved {positonsToAdd.Count()} new positions");
        }

        public async Task DownloadAlbumArt(IEnumerable<SpotifyTrackModel> tracks)
        {
            Console.WriteLine("Starting to download album art. Clearing directory...");
           
            System.IO.DirectoryInfo di = Directory.CreateDirectory("assets//art");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            Console.WriteLine("Directory cleared");
            foreach (var track in tracks)
            {
                using (HttpClient client = new HttpClient())
                {
                    using (Stream streamToReadFrom = await client.GetStreamAsync(track.AlbumArt))
                    {
                        string fileToWriteTo = Path.GetFullPath($"assets//art//{track.FileName}.png");
                        //Console.WriteLine($"Downloading album art from {track.AlbumArt} and writing to {track.FileName}");
                        using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
                        {
                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        }

                        Console.WriteLine($"Finished downloading album art for {track.Name} {track.Artist}");
                    }
                }
            }

            return;
        }

        public async Task ValidateTrackNamesAreCorrect()
        {
            var tracks = await GetTracks();
            DirectoryInfo d = new DirectoryInfo(@"C:\Countdown\playlist");

            FileInfo[] files = d.GetFiles("*.m4a");
            var fileNames = files.Select(f => f.Name);

            foreach (var track in tracks)
            {
                if (fileNames.Contains(track.FileName + ".m4a"))
                    continue;
                else
                    Console.WriteLine($"ERROR: MISSING TRACK {track.FileName}");
            }
        }

        public async Task ValidateFillerSoundbytes()
        {
            var expectedFileNames = new string[]
            {
                "100", "95", "90", "85", "80", "75", "70", "65", "60", "55", "50", "45", "40", "35", "30", "25", "20", "15", "10", "5", "4", "3", "2", "1"
            };
            DirectoryInfo d = new DirectoryInfo(@"C:\Countdown\filler");

            FileInfo[] files = d.GetFiles("*.m4a");
            var fileNames = files.Select(f => f.Name);

            foreach (var expectedFilename in expectedFileNames)
            {
                if (fileNames.Contains(expectedFilename + ".m4a"))
                    continue;
                else
                    Console.WriteLine($"ERROR: MISSING FILLER SNIPPER {expectedFilename}");
            }
        }

        public async Task<int> GetCurrentPositionToPlay()
        {
            using var context = new SpotifyPlaylistTrackerContext();
            return await context.TrackPositions.Where(x => x.IsPlayed == false).Select(x => x.Position).MaxAsync();
        }
    }
}
