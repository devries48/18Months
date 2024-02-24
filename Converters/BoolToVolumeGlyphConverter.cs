namespace Months18.Converters;
public class BoolToVolumeGlyphConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string icon= value is bool boolValue && boolValue ? "IconVolumeOff" : "IconVolume";
        var mergedDictionaries = Application.Current?.Resources.MergedDictionaries ?? [];

        foreach (var mergedDictionary in mergedDictionaries)
        {
            if (mergedDictionary.TryGetValue(icon, out object style) && style is Style)
                return style;
        }

        return null; // Style not found
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

   

}
