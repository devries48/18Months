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
}
