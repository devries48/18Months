using System.Text.Json.Serialization;

namespace Months18.Models;

public  partial class VideoModel :ObservableObject
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;

    [JsonIgnore]
    public string Uri { get; set; } = string.Empty;

    public string FilenameVideo { get; set; } = string.Empty;

    public string FilenameThumbnail { get; set; } = string.Empty;

    [ObservableProperty]
    [property: JsonIgnore]
    private byte[] _imageBytes = [];

    [ObservableProperty]
    [property: JsonIgnore]
    private bool _isSelected;

    public MediaPlayerSource Source { get => MediaPlayerSource.FileSystem; }

    public static async Task<VideoModel> Create(VideoModel v)
    {
        var video = new VideoModel() {  Title = v.Title, Description= v.Description, Duration = v.Duration };
        var imagePath = Path.Combine(Prefernces.VideoDataPath, v.FilenameThumbnail); 
        video.Uri =Path.Combine(Prefernces.VideoDataPath, v.FilenameVideo);

        await video.LoadImageBytesAsync(imagePath);

        return video;
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

}
