using NetCoreAudio;
using OnlineSpotifyPlaylistTracker.Domain.Models;
using Vlc.DotNet.Core;

namespace OnlineSpotifyPlaylistTracker
{
    public interface IPlaybackService
    {
        void PlaySong(Track track);
        void PlayFillerSound(int trackNumber);
    }

    public class PlaybackService : IPlaybackService
    {
        public void PlaySong(Track track)
        {
            string audioFileName = $"C:\\Countdown\\playlist\\{track.FileName}.m4a";
            PlayAudioFile(audioFileName);
            Console.WriteLine($"Finished playing track {track.Name}");
        }

        public void PlayFillerSound(int trackNumber)
        {
            string hornFileName = "/Users/alexbarker/Countdown/filler/horn.m4a";
            PlayAudioFile(hornFileName);
            string audioFileName = $"/Users/alexbarker/Countdown/filler/{trackNumber}.m4a";
            PlayAudioFile(audioFileName);
            Console.WriteLine($"Finished playing filler sound {trackNumber}");
        }

        private void PlayAudioFile(string filePath)
        {
            // var libDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            //
            // var options = new string[]
            // {
            //     // VLC options can be given here. Please refer to the VLC command line documentation.
            // };
            //
            // var mediaPlayer = new VlcMediaPlayer(libDirectory);
            //
            // var mediaOptions = new string[]
            // {
            //     ":sout-keep"
            // };

            var mediaPlayer = new Player();
            mediaPlayer.Play(filePath);

            // mediaPlayer.SetMedia(new FileInfo(filePath), mediaOptions); ;

            bool playFinished = false;
            // mediaPlayer.PositionChanged += (sender, e) =>
            // {
            //     Console.Write("\r" + Math.Floor(e.NewPosition * 100) + "%");
            // };
            

            mediaPlayer.PlaybackFinished += (sender, e) =>
            {
                playFinished = true;
            };

            // mediaPlayer.Play();

            // Ugly, sorry, that's just an example...
            while (!playFinished)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            }

            if (playFinished)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                Console.WriteLine($"Finished playing file {filePath}");
                return;
            }
        }
    }
}
