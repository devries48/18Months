﻿namespace Months18.Converters;
public class CurrentStateToPlayingGlyphConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MediaElementState currentState)
            return currentState == MediaElementState.Playing ? IconFont.Pause : IconFont.Play_arrow;
        
        return IconFont.Play_arrow;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}