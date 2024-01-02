namespace Months18.Models;

public class TrackModel(ReleaseModel release)
{
    public string ReleaseArtist { get => release.Artist; }
    public string ReleaseTitle { get => release.Title; }

    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;

    public byte[] GetReleaseImage()
    {
        return (byte[])release.ImageBytes.Clone();
    }
}
