// See https://aka.ms/new-console-template for more information
using OfflineSpotifyPlaylistTracker;

var repositorySerivce = new RepositoryService();
var playlistManager = new PlaylistManager(repositorySerivce);
var spotifyService = new SpotifyService(repositorySerivce);

//var args = Environment.GetCommandLineArgs();

if (args.Length == 0)
{
	Console.WriteLine("At least one argument is required");
	return;
}

var command = args[0];

switch (command)
{
	case "start":
		await playlistManager.StartPlaylist();
		break;
	case "shuffle":
		await playlistManager.ShufflePlaylist();
		break;
	case "init-playlist":
        await repositorySerivce.ClearAllTracks();
        var tracks = await spotifyService.GetPlaylistTracks();
        await repositorySerivce.AddTracks(tracks);
        await repositorySerivce.DownloadAlbumArt(tracks);
		break;
	case "validate":
		await repositorySerivce.ValidateTrackNamesAreCorrect();
		await repositorySerivce.ValidateFillerSoundbytes();
		break;
	default:
		Console.WriteLine("Invalid command");
		break;
}
