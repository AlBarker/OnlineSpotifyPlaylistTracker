using Microsoft.AspNetCore.Mvc;
using OnlineSpotifyPlaylistTracker;

namespace OnlineSpotifyPlaylistTracker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly RepositoryService repositoryService;
        private readonly SpotifyService spotifyService;

        public TrackController()
        {
            repositoryService = new RepositoryService();
            spotifyService = new SpotifyService(repositoryService);
        }

        [HttpGet("list", Name = "GetPlayedTracks")]
        public async Task<IList<TrackViewModel>> GetTracksAsync()
        {
            var tracks = await repositoryService.GetPlayedTracks();

            var mappedTracks = tracks
                .OrderBy(x => x.TrackPosition.Position)
                .Select(x => new TrackViewModel
            {
                Name = x.Name,
                Artist = x.Artist,
                Position = x.TrackPosition.Position,
                AddedByName = x.User.DisplayName,
                AddedByImage = x.User.ImageName,
                AlbumArt = "/assets/art/" + x.FileName + ".png",
                Popularity = x.Popularity
            }).ToList();

            return mappedTracks;
        }


        [HttpGet("usersummary", Name = "TracksByUser")]
        public async Task<IDictionary<string, int>> GetTracksByUserAsync()
        {
            var tracks = await repositoryService.GetPlayedTracks();

            var mappedTracks = tracks
                .GroupBy(x => x.User.DisplayName)
                .ToDictionary(x => x.Key, x => x.Count());

            return mappedTracks;
        }

        [HttpGet("dump", Name = "GetPlayedTrackDump")]
        public async Task<IList<TrackViewModel>> GetTrackDumpAsync()
        {
            var tracks = await spotifyService.GetPlayedTracksDump();

            var mappedTracks = tracks
                .OrderBy(x => x.TrackPosition.Position)
                .Select(x => new TrackViewModel
                {
                    Name = x.Name,
                    Artist = x.Artist,
                    Position = x.TrackPosition.Position,
                    AddedByName = x.User.DisplayName,
                    AddedByImage = x.User.ImageName,
                    AlbumArt = x.AlbumArt,
                    Popularity = x.Popularity
                }).ToList();

            return mappedTracks;
        }
    }

    public class TrackViewModel
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public int Position { get; set; }
        public string AddedByName { get; set; }
        public string AddedByImage { get; set; }
        public string AlbumArt { get; set; }
        public int Popularity { get; set; }
    }
}