using Months18.Helpers;
using System.Globalization;

namespace Months18.Converters;
public class BoolToVolumeGlyphConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? IconFont.Volume_off : IconFont.Volume_up;

        return IconFont.Volume_up;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
