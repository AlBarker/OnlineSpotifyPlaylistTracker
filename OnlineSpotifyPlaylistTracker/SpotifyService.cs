using Newtonsoft.Json;
using OnlineSpotifyPlaylistTracker.Domain.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Linq;
using System.Web;
using static SpotifyAPI.Web.Scopes;


namespace OnlineSpotifyPlaylistTracker
{
    public  class SpotifyService
    {
        private readonly RepositoryService repositoryService;

        private const string CredentialsPath = "credentials.json";
        private static readonly string? clientId = "28eeb98b2e194e34ba47b642f36c876d";
        private static readonly EmbedIOAuthServer _server = new(new Uri("http://localhost:5543/callback"), 5543);

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
            // 2023: 4Bd2EKiv9HfAqfHIf5rMCM
            // 2024: 6Q2lmuK0ByfUmPDYPld2zI
            var playlistResponse = await client.Playlists.GetItems("6Q2lmuK0ByfUmPDYPld2zI");

            var users = await repositoryService.GetUsers();

            return playlistResponse.Items.Select((v, i) =>
            {
                var track = (FullTrack)v.Track;
                var artists = String.Join(", ", track.Artists.Select(x => x.Name));
                var fileName = (String.Join(" ", artists, track.Name));
                var sanitisedFileName = fileName.Replace("/", "").Replace("?", "").Replace("*", "").Replace("Ü", "U").Replace("è", "e").Replace("ü", "u");
                return new SpotifyTrackModel
                {
                    Id = i + 1,
                    Name = track.Name,
                    Artist = artists,
                    FileName = sanitisedFileName,
                    AlbumArt = track.Album.Images.FirstOrDefault()?.Url,
                    UserId = v.AddedBy.Id,
                    Uri = track.Uri,
                    Popularity = track.Popularity,
                    DurationMs = track.DurationMs,
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
            // 2023: 4Bd2EKiv9HfAqfHIf5rMCM
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
                var sanitisedFileName = fileName.Replace("/", "").Replace("?", "").Replace("*", "").Replace("Ü", "U").Replace("è", "e").Replace("ü", "u");
                return new Track
                {
                    Name = track.Name,
                    Artist = artists,
                    FileName = sanitisedFileName,
                    AlbumArt = track.Album.Images.FirstOrDefault()?.Url,
                    UserId = v.AddedBy.Id,
                    User = users.FirstOrDefault(x => x.Id == v.AddedBy.Id),
                    Popularity = track.Popularity,
                    TrackPosition = new TrackPosition
                    {
                        Position = i + 1,
                    }
                };
            }).ToList();
        }

        public async Task PlayTrack(string uri, int durationMs)
        {
            var client = await GetUserClient();
            await client.Player.ResumePlayback(new PlayerResumePlaybackRequest { Uris = new List<string> { uri } });

            Thread.Sleep(durationMs);

            var currentState = await client.Player.GetCurrentPlayback();

            // Probably don't need this but here it is
            if (currentState.IsPlaying)
            {
                Thread.Sleep(durationMs - currentState.ProgressMs);
            }

            return;
        }

        private async Task<SpotifyClient> GetClient()
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest("28eeb98b2e194e34ba47b642f36c876d", "");
            var response = await new OAuthClient(config).RequestToken(request);

            return new SpotifyClient(config.WithToken(response.AccessToken));
        }

        private async Task<SpotifyClient> GetUserClient()
        {
            var json = await File.ReadAllTextAsync(CredentialsPath);
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);

            var authenticator = new PKCEAuthenticator(clientId!, token!);
            authenticator.TokenRefreshed += (sender, token) => File.WriteAllText(CredentialsPath, JsonConvert.SerializeObject(token));

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            return new SpotifyClient(config);
        }

        public async Task Authenticate()
        {
            await StartAuthentication();
        }

        private static async Task Start()
        {
            var json = await File.ReadAllTextAsync(CredentialsPath);
            var token = JsonConvert.DeserializeObject<PKCETokenResponse>(json);

            var authenticator = new PKCEAuthenticator(clientId!, token!);
            authenticator.TokenRefreshed += (sender, token) => File.WriteAllText(CredentialsPath, JsonConvert.SerializeObject(token));

            var config = SpotifyClientConfig.CreateDefault()
              .WithAuthenticator(authenticator);

            var spotify = new SpotifyClient(config);

            var me = await spotify.UserProfile.Current();
            Console.WriteLine($"Welcome {me.DisplayName} ({me.Id}), you're authenticated!");

            var playlists = await spotify.PaginateAll(await spotify.Playlists.CurrentUsers().ConfigureAwait(false));
            Console.WriteLine($"Total Playlists in your Account: {playlists.Count}");

            _server.Dispose();
            Environment.Exit(0);
        }

        private static async Task StartAuthentication()
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();

            await _server.Start();
            _server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await _server.Stop();
                var token = await new OAuthClient().RequestToken(
                  new PKCETokenRequest(clientId!, response.Code, _server.BaseUri, verifier)
                );

                await File.WriteAllTextAsync(CredentialsPath, JsonConvert.SerializeObject(token));
                await Start();
            };

            var request = new LoginRequest(_server.BaseUri, clientId!, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challenge,
                CodeChallengeMethod = "S256",
                Scope = new List<string> { UserReadEmail, UserReadPrivate, PlaylistReadPrivate, PlaylistReadCollaborative, UserModifyPlaybackState, UserReadPlaybackState }
            };

            var uri = request.ToUri();
            try
            {
                BrowserUtil.Open(uri);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to open URL, manually open: {0}", uri);
            }
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
        public string Uri { get; set; }
        public int DurationMs { get; set; }
        public int Popularity { get; set; }
    }
}
