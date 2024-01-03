namespace Months18.Models;

public class TrackModel(ReleaseModel release)
{
    public string TrackArtist { get => release.Artist; }
    public string ReleaseTitle { get => release.Title; }
    public byte[] ReleaseImage { get => release.ImageBytes; }
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;

    public AudioPlayerSource Source { get { return AudioPlayerSource.FileSystem; } }

    public byte[] GetReleaseImage()
    {
        return (byte[])release.ImageBytes.Clone();
    }
}
