namespace Months18.Helpers;

public static class Prefernces
{
    public static bool DarkTheme
    {
        get => Preferences.Get(nameof(DarkTheme), true);
        set { Preferences.Set(nameof(DarkTheme), value); }
    }

    public static string DataFilePath
    {
        get => Preferences.Get(nameof(DataFilePath), "");
        set { Preferences.Set(nameof(DataFilePath), value); }
    }

    public static int Level
    {
        get => Preferences.Get(nameof(Level), 0);
        set { Preferences.Set(nameof(Level), value); }
    }

    public static double VolumeMusic
    {
        get => Preferences.Get(nameof(VolumeMusic), 1d);
        set { Preferences.Set(nameof(VolumeMusic), value); }
    }

    public static double VolumeVideo
    {
        get => Preferences.Get(nameof(VolumeVideo), 1d);
        set { Preferences.Set(nameof(VolumeVideo), value); }
    }

    public static string MediaPath
    {
        get
        {
            string? path = Preferences.Get(nameof(MediaPath), null);
            if (string.IsNullOrWhiteSpace(path))
                return Path.Combine(AppContext.BaseDirectory, "Media");

            return path;
        }
        set
        {
            if (value == Path.Combine(AppContext.BaseDirectory, "Media"))
                value = string.Empty;

            Preferences.Set(nameof(MediaPath), value);
        }
    }

    public static string TriviaPath
    {
        get
        {
            string? path = Preferences.Get(nameof(TriviaPath), null);
            if (string.IsNullOrWhiteSpace(path))
                return Path.Combine(AppContext.BaseDirectory, "Trivia");

            return path;
        }
        set
        {
            if (value == Path.Combine(AppContext.BaseDirectory, "Trivia"))
                value = string.Empty;

            Preferences.Set(nameof(TriviaPath), value);
        }
    }

    public static string MusicDataFilePath => Path.Combine(MediaPath, "music.json");
    public static string VideoDataFilePath => Path.Combine(MediaPath, "videos.json");
    public static string DocumentDataFilePath => Path.Combine(MediaPath, "documents.json");
    public static string MusicDataPath => Path.Combine(MediaPath, "Music");
    public static string ImageDataPath => Path.Combine(MediaPath, "Covers");
    public static string VideoDataPath => Path.Combine(MediaPath, "Videos");
    public static string DocumentDataPath => Path.Combine(MediaPath, "Documents");
}
