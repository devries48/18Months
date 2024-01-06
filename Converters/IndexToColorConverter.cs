namespace Months18.Converters;

public class IndexToColorConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        string resColor = "Dark_PrimaryText";

        if((values != null) &&
            (values.Length == 2) &&
            (values[0] is int position) &&
            (values[1] is int currentIndex) &&
            (position == currentIndex + 1))
                resColor = "Dark_Accent";

        if(Application.Current != null && Application.Current.Resources.TryGetValue(resColor, out var colorvalue))
            return (Color)colorvalue;

        return Colors.Red;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    { throw new NotImplementedException(); }
}