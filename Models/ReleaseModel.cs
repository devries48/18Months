using Months18.Controls;
using System.Text.Json.Serialization;

namespace Months18.Models;

public partial class ReleaseModel : ObservableObject
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public CountryCode CountryCode { get; set; } = CountryCode.None;
    public string Genre { get; set; } = string.Empty;
    public string SubGenres { get; set; } = string.Empty;
    public int Level { get; set; } = 0;
    public string FilenameCover { get; set; } = string.Empty;
    public List<TrackModel> Tracks { get; set; } = [];

    [ObservableProperty]
    [property:JsonIgnore]
    private byte[] _imageBytes = [];

    [ObservableProperty]
    [property: JsonIgnore]
    private bool _isSelected;

    public static async Task<ReleaseModel> Create(ReleaseModel r)
    {
        var release = new ReleaseModel()
        {
            Artist = r.Artist,
            Title = r.Title,
            CountryCode = r.CountryCode,
            Year = r.Year,
            Genre=r.Genre,
            SubGenres=r.SubGenres,
            Level = r.Level
        };
        var imagePath = Path.Combine(Prefernces.ImageDataPath, r.FilenameCover);
      
        await release.LoadImageBytesAsync(imagePath);

        foreach (var t in r.Tracks)
        {
            var trackPath = Path.Combine(Prefernces.MusicDataPath, t.FilenameMusic);
            release.AddTrack(trackPath, t.Title, t.Duration);
        }
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
            Nr = Tracks.Count + 1,
            Title = title,
            Duration = duration,
            Uri = filePath
    };

        Tracks.Add(track);
    }

    public string YearAndCountry
    {
        get
        {
            var country = CountryCode.Description().Replace("flag_of_", "").Replace(".png", "").Replace('_', ' ').ToTitleCase();
            return $"{Year}, {country}";
        }
    }

    public string TotalLength => CalculateTotalTime();

    private string CalculateTotalTime()
    {
        int totalMinutes = 0;
        int totalSeconds = 0;

        foreach (var track in Tracks)
        {
            var parts = track.Duration.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int seconds))
            {
                totalMinutes += minutes;
                totalSeconds += seconds;
            }
        }

        totalMinutes += totalSeconds / 60;
        totalSeconds %= 60;

        return $"Total length: {totalMinutes}:{totalSeconds:D2}";
    }
}
