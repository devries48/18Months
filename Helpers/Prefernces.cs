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

    public static double Volume
    {
        get { return Preferences.Get(nameof(Volume), 1); }
        set { Preferences.Set(nameof(Volume), value); }
    }
}
