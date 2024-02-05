using Months18.Controls;

namespace Months18.Models;

public partial class ReleaseModel : ObservableObject
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public CountryCode CountryCode { get; set; } = CountryCode.None;
    public List<TrackModel> Tracks { get; set; } = [];
    public string Genre { get; set; } = string.Empty;
    public string SubGenres { get; set; } = string.Empty;
    public int Level { get; set; } = 0;

    [ObservableProperty]
    private byte[] _imageBytes = [];

    [ObservableProperty]
    private bool _isSelected;

    public static async Task<ReleaseModel> Create(string artist, string title, string imagePath, CountryCode origin, int year,int level)
    {
        var release = new ReleaseModel() { Artist = artist, Title = title, CountryCode = origin, Year = year.ToString(), Level=level };
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
            Nr = Tracks.Count + 1,
            Title = title,
            Duration = duration,
            Uri = filePath
        };

        Tracks.Add(track);
    }

    public void AddGenres(params string[] genres)
    {
        Genre = genres.First();
        if (genres.Length > 1)
            SubGenres = string.Join(", ", genres.Skip(1));
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
