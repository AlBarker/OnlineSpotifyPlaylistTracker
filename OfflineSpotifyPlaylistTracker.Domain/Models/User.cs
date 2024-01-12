using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineSpotifyPlaylistTracker.Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string ImageName { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
