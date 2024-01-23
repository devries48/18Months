namespace Months18.Models;

public partial class TrackModel(ReleaseModel release) : ObservableObject
{
    // Position property is used in the playlist
    [ObservableProperty]
    private int _playlistPosition = 0;

    // Display Button panel in DetailView
    [ObservableProperty]
    private bool _isSelected;

    public bool PlaylistSingleArtist { get; set; }

    public string TrackArtist { get => release.Artist; }
    public string ReleaseTitle { get => release.Title; }
    public byte[] ReleaseImage { get => release.ImageBytes; }

    public int Nr { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;

    public AudioPlayerSource Source { get => AudioPlayerSource.FileSystem; }

    public string PlaylistTrack
    {
        get
        {
            if (PlaylistSingleArtist)
                return Title;

            return $"{TrackArtist}  -  {Title}";
        }
    }

    public byte[] GetReleaseImage()
    {
        return (byte[])release.ImageBytes.Clone();
    }
}
