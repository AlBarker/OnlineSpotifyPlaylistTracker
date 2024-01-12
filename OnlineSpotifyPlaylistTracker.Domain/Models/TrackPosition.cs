using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpotifyPlaylistTracker.Domain.Models
{
    public class TrackPosition
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int TrackId { get; set; }
        public bool IsPlayed { get; set; }
        public virtual Track Track { get; set; }
    }
}
