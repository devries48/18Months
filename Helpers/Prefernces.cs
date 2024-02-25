namespace Months18.Helpers;

public static class Prefernces
{
    public static string DataFilePath
    {
        get { return Preferences.Get(nameof(DataFilePath), ""); }
        set { Preferences.Set(nameof(DataFilePath), value); }
    }

    public static int Level
    {
        get { return Preferences.Get(nameof(Level), 0); }
        set { Preferences.Set(nameof(Level), value); }
    }

    public static double VolumeMusic
    {
        get { return Preferences.Get(nameof(VolumeMusic), 1d); }
        set { Preferences.Set(nameof(VolumeMusic), value); }
    }

    public static double VolumeVideo
    {
        get { return Preferences.Get(nameof(VolumeVideo), 1d); }
        set { Preferences.Set(nameof(VolumeVideo), value); }
    }

    public static string DataPath
    {
        get
        {
            string? path = Preferences.Get(nameof(DataPath), null);
            if (string.IsNullOrWhiteSpace(path))
                return Path.Combine(AppContext.BaseDirectory, "Data");

            return path;
        }
        set
        {
            if (value == Path.Combine(AppContext.BaseDirectory, "Data"))
                value = string.Empty;

            Preferences.Set(nameof(DataPath), value);
        }
    }

    public static string MusicDataFilePath => Path.Combine(DataPath, "music.json");
    public static string VideoDataFilePath => Path.Combine(DataPath, "videos.json");
    public static string MusicDataPath => Path.Combine(DataPath, "Music");
    public static string ImageDataPath => Path.Combine(DataPath, "Images");
    public static string VideoDataPath => Path.Combine(DataPath, "Videos");
}
