namespace Months18.Controls;

public partial class FlagImage : Image
{
    public static readonly BindableProperty CountryCodeProperty =
        BindableProperty.Create(nameof(CountryCode), typeof(CountryCode), typeof(FlagImage), propertyChanged: OnCountryCodeChanged);

    public static readonly BindableProperty CountryCodeTextProperty =
        BindableProperty.Create(nameof(CountryCodeText), typeof(string), typeof(FlagImage), propertyChanged: OnCountryCodeTextChanged);

    public CountryCode CountryCode
    {
        get => (CountryCode)GetValue(CountryCodeProperty);
        set => SetValue(CountryCodeProperty, value);
    }

    public string CountryCodeText
    {
        get => (string)GetValue(CountryCodeTextProperty);
        set => SetValue(CountryCodeTextProperty, value);
    }

    private static void OnCountryCodeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FlagImage flagImage)
        {
            var code = newValue as CountryCode?;
            var image = code?.Description() ?? null;
            if (image != null)
                flagImage.SetImageSource(image);
        }
    }

    private static void OnCountryCodeTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FlagImage flagImage)
        {
            var text = newValue as string;
            if (Enum.TryParse<CountryCode>(text, out var result))
            {
                flagImage.CountryCode = result;
            }
        }
    }

    private void SetImageSource(string imageSource)
    {
        Source = ImageSource.FromFile(imageSource);
    }
}

