namespace OnlineSpotifyPlaylistTracker.Domain.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string FileName { get; set; }
        public string AlbumArt { get; set; }
        public string UserId { get; set; }
        public string Uri { get; set; }
        public int DurationMs { get; set; }
        public User User { get; set; }

        public virtual TrackPosition TrackPosition { get; set; }
    }
}
