namespace Months18.Converters;

public class AccentColorToDisabledConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string resColor = value is bool isEnabled && isEnabled 
            ? "Dark_Accent" 
            : "Dark_Disabled";

        if (Application.Current != null && Application.Current.Resources.TryGetValue(resColor, out var colorvalue))
            return (Color)colorvalue;

        return Colors.Red;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
