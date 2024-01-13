using OnlineSpotifyPlaylistTracker.Domain.Models;
using OnlineSpotifyPlaylistTracker.Extensions;

namespace OnlineSpotifyPlaylistTracker
{
    public class PlaylistManager
    {
        private IPlaybackService playbackService;
        private readonly RepositoryService repositoryService;
        private readonly SpotifyService spotifyService;

        const int CountSongs = 100;

        public PlaylistManager(RepositoryService repositorySerivce)
        {
            playbackService = new PlaybackService();
            repositoryService = repositorySerivce;
            spotifyService = new SpotifyService(repositoryService);
        }

        public async Task StartPlaylist()
        {
            var currentSong = await repositoryService.GetCurrentPositionToPlay();
            
            for (int i = currentSong; i > 0; i--)
            {
                var track = await repositoryService.GetTrackFromTrackPosition(i);
                if (i % 5 == 0 || i < 5)
                {
                    playbackService.PlayFillerSound(i);

                }
                Console.WriteLine($"Playing #{i}, {track.Name} by {track.Artist}. Added by {track.User.DisplayName}");
                playbackService.PlaySong(track);
            }
        }

        public async Task StartPlaylistLive()
        {
            var currentSong = await repositoryService.GetCurrentPositionToPlay();

            for (int i = currentSong; i > 0; i--)
            {
                var track = await repositoryService.GetTrackFromTrackPosition(i);
                if (i % 5 == 0 || i < 5)
                {
                    playbackService.PlayFillerSound(i);

                }
                Console.WriteLine($"Playing #{i}, {track.Name} by {track.Artist}. Added by {track.User.DisplayName}");

                await spotifyService.PlayTrack(track.Uri, track.DurationMs);
            }
        }

        public async Task ShufflePlaylist()
        {
            var tracks = await repositoryService.GetTracks();
            tracks.Shuffle();

            var trackPositions = new List<TrackPosition>();
            foreach (var (track, i) in tracks.WithIndex())
            {
                trackPositions.Add(new TrackPosition
                {
                    Position = i + 1,
                    TrackId = track.Id
                });
            }

            await repositoryService.ClearAndSaveTrackPositons(trackPositions);
        }
    }
}
