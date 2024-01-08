namespace Months18.Models;

public partial class TrackModel(ReleaseModel release) : ObservableObject
{
    // Position property is used in the playlist
    [ObservableProperty]
    private int _position = 0;

    public string TrackArtist { get => release.Artist; }
    public string ReleaseTitle { get => release.Title; }
    public byte[] ReleaseImage { get => release.ImageBytes; }
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;

    public AudioPlayerSource Source { get => AudioPlayerSource.FileSystem; }

    public string ArtistAndTrack { get => $"{TrackArtist} - {Title}"; }

    public byte[] GetReleaseImage()
    {
        return (byte[])release.ImageBytes.Clone();
    }
}
