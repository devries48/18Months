namespace Months18.Converters;

public class PlaylistIndexToBoolConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((values?.Length == 2) && (values[0] is int position) && (values[1] is int currentIndex) && (position == currentIndex + 1))
            return true;

        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    { throw new NotImplementedException(); }
}