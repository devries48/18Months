namespace Months18.Converters;

public class AccentColorToDisabledConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var theme = Application.Current?.PlatformAppTheme ?? AppTheme.Dark;
        
        string resColor = value is bool isEnabled && isEnabled
            ? theme.ToString() + "_Accent"
            : theme.ToString() + "_Disabled";

        if (Application.Current != null && Application.Current.Resources.TryGetValue(resColor, out var colorvalue))
            return (Color)colorvalue;

        return Colors.Red;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
