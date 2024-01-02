namespace Months18.Models;

public partial class ReleaseModel : ObservableObject
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<TrackModel> Tracks { get; set; } = [];

    [ObservableProperty]
    private byte[] _imageBytes = [];

    [ObservableProperty]
    private bool _isSelected;

    public static async Task<ReleaseModel> Create(string artist, string title, string imagePath)
    {
        var release = new ReleaseModel() { Artist = artist, Title = title };
        await release.LoadImageBytesAsync(imagePath);
        return release;
    }

    private async Task LoadImageBytesAsync(string imagePath)
    {
        try
        {
            // Use Task.Run to offload the file reading to a background thread
            ImageBytes = await Task.Run(() => File.ReadAllBytes(imagePath));
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., file not found, permissions issue, etc.)
            // Log or handle the exception according to your application's needs
            Console.WriteLine($"Error loading image bytes: {ex.Message}");
        }
    }

    public void AddTrack(string filePath, string title, string duration)
    {
        var track = new TrackModel(this)
        {
            Title = title,
            Duration = duration,
            FilePath = filePath
        };
        Tracks.Add(track);
    }
}
