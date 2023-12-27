using CommunityToolkit.Maui.Core.Primitives;
using System.Globalization;

namespace Months18.Converters;

public class CurrentStateToEnabledConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MediaElementState currentState)
            return currentState is not MediaElementState.None and not MediaElementState.Failed;

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}