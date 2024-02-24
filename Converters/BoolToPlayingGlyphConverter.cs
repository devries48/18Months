namespace Months18.Converters;
public class BoolToPlayingGlyphConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string icon = value is bool boolValue && boolValue ? "IconPauseSelected" : "IconPlaySelected";

        if (Application.Current != null && Application.Current.Resources.TryGetValue(icon, out var style))
            return style;

        return null;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
