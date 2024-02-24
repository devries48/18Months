using System.Diagnostics;

namespace Months18.Converters;
public class PlayButtonStyleConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return null;

        var currentState = (MediaElementState)values[0];
        var isEnabled = (bool)values[1];

        string styleResourceKey = DetermineStyleResourceKey(currentState, isEnabled);
        if (Application.Current != null && Application.Current.Resources.TryGetValue(styleResourceKey, out var style))
            return style;
        
        return null;
    }

    private static string DetermineStyleResourceKey(MediaElementState currentState, bool isEnabled)
    {
        return currentState == MediaElementState.Playing
            ? isEnabled ? "IconPauseAccent" : "IconPauseDisabled"
            : isEnabled ? "IconPlayAccent" : "IconPlayDisabled";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}