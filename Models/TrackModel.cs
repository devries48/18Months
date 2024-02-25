using System.Text.Json.Serialization;

namespace Months18.Models;

public partial class TrackModel : ObservableObject
{
    public TrackModel() { }
    public TrackModel(ReleaseModel release) => _release = release;

    readonly ReleaseModel? _release;

    // Position property is used in the playlist
    [ObservableProperty]
    [property: JsonIgnore]
    private int _playlistPosition = 0;

    // Display Button panel in DetailView
    [ObservableProperty]
    [property: JsonIgnore]
    private bool _isSelected;

    [JsonIgnore]
    public bool PlaylistSingleArtist { get; set; }

    public string TrackArtist { get => _release?.Artist ?? string.Empty; }

    public string ReleaseTitle { get => _release?.Title ?? string.Empty; }

    public byte[] ReleaseImage { get => _release?.ImageBytes ?? []; }

    [JsonIgnore]
    public int Nr { get; set; } = 0;

    public string Title { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public string FilenameMusic { get; set; } = string.Empty;

    [JsonIgnore]
    public string Uri { get; set; } = string.Empty;

    public MediaPlayerSource Source { get => MediaPlayerSource.FileSystem; }

    public string PlaylistTrack
    {
        get
        {
            if(PlaylistSingleArtist)
                return Title;

            return $"{TrackArtist}  -  {Title}";
        }
    }

    public byte[] GetReleaseImage()
    {
        if(_release == null)
            return [];

        return (byte[])_release.ImageBytes.Clone();
    }
}
