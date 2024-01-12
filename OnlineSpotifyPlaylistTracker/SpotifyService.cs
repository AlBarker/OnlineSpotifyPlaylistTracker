using OnlineSpotifyPlaylistTracker.Domain.Models;
using SpotifyAPI.Web;
using System.Linq;
using System.Web;

namespace OnlineSpotifyPlaylistTracker
{
    public  class SpotifyService
    {
        private readonly RepositoryService repositoryService;
        public SpotifyService(RepositoryService repositorySerivce)
        {
            this.repositoryService = repositorySerivce;
        }

        public async Task<IEnumerable<SpotifyTrackModel>> GetPlaylistTracks()
        {
            var client = await GetClient();
            // 2020: 2CuhODa4xTTlemWopeXG71
            // 2021: 2rdxeAIgVMRLmNHiIAQqmV
            // 2022: 1goTZfOG1oahHtQCX2CzsA
            var playlistResponse = await client.Playlists.GetItems("1goTZfOG1oahHtQCX2CzsA");

            var users = await repositoryService.GetUsers();

            return playlistResponse.Items.Select((v, i) =>
            {
                var track = (FullTrack)v.Track;
                var artists = String.Join(", ", track.Artists.Select(x => x.Name));
                var fileName = (String.Join(" ", artists, track.Name));
                var sanitisedFileName = fileName.Replace("/", "").Replace("?", "").Replace("*", "");
                return new SpotifyTrackModel
                {
                    Id = i + 1,
                    Name = track.Name,
                    Artist = artists,
                    FileName = sanitisedFileName,
                    AlbumArt = track.Album.Images.FirstOrDefault()?.Url,
                    UserId = v.AddedBy.Id,
                };
            });

        }

        public async Task<IList<Track>> GetPlayedTracksDump()
        {
            var client = await GetClient();
            // 2017: 6SDuwhW8HekJQLZak0Kmmi
            // 2018: 3bdydssu6hXzOP4kLrI8cL
            // 2019: 6LJOm2SkbNEsmANfIGhemx
            // 2020: 2CuhODa4xTTlemWopeXG71
            // 2021: 2rdxeAIgVMRLmNHiIAQqmV
            // 2022: 1goTZfOG1oahHtQCX2CzsA
            var playlistResponse = await client.Playlists.GetItems("6SDuwhW8HekJQLZak0Kmmi");

            var users = await repositoryService.GetUsers();

            users.Add(new User
            {
                Id = "12138108557",
                DisplayName = "Wildcard",
                ImageName = "beer3",
            });

            return playlistResponse.Items.Select((v, i) =>
            {
                var track = (FullTrack)v.Track; 
                var artists = String.Join(", ", track.Artists.Select(x => x.Name));
                var fileName = (String.Join(" ", artists, track.Name));
                var sanitisedFileName = fileName.Replace("/", "").Replace("?", "").Replace("*", "");
                return new Track
                {
                    Name = track.Name,
                    Artist = artists,
                    FileName = sanitisedFileName,
                    AlbumArt = track.Album.Images.FirstOrDefault()?.Url,
                    UserId = v.AddedBy.Id,
                    User = users.FirstOrDefault(x => x.Id == v.AddedBy.Id),
                    TrackPosition = new TrackPosition
                    {
                        Position = i + 1,
                    }
                };
            }).ToList();

            //return playlistResponse.Items.Select((v, i) =>
            //{
            //    var track = (FullTrack)v.Track;
            //    var artists = String.Join(", ", track.Artists.Select(x => x.Name));
            //    var fileName = (String.Join(" ", artists, track.Name));
            //    var sanitisedFileName = fileName.Replace("/", "").Replace("?", "").Replace("*", "");
            //    return new SpotifyTrackModel
            //    {
            //        Id = i + 1,
            //        Name = track.Name,
            //        Artist = artists,
            //        FileName = sanitisedFileName,
            //        AlbumArt = track.Album.Images.FirstOrDefault()?.Url,
            //        UserId = v.AddedBy.Id
            //    };
            //});
        }

        private async Task<SpotifyClient> GetClient()
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest("28eeb98b2e194e34ba47b642f36c876d", "877cedf5541c42ff8f381951862b1f2d");
            var response = await new OAuthClient(config).RequestToken(request);

            return new SpotifyClient(config.WithToken(response.AccessToken));
        }
    }

    public class SpotifyTrackModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string FileName { get; set; }
        public string AlbumArt { get; set; }
        public string UserId { get; set; }
    }
}
