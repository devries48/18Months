namespace Months18.Helpers;

public static class Extenstions
{
    public static void DispatchIfRequired(this IDispatcher dispatcher, Action action)
    {
        if (dispatcher.IsDispatchRequired)
            dispatcher.Dispatch(action);
        else
            action();
    }

    public static string Description(this Enum enumValue)
    {
        var descriptionAttribute = enumValue.GetType()?
            .GetField(enumValue.ToString())?
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute;

        return descriptionAttribute?.Description ?? string.Empty;
    }

    public static bool IsApproximatelyEqual(this double value1, double value2, double epsilon = 0.000001)
    {
        return Math.Abs(value1 - value2) < epsilon;
    }

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }
}
