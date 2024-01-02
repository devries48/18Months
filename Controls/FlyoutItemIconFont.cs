namespace Months18.Controls;

public class FlyoutItemIconFont : FlyoutItem
{
    public static readonly BindableProperty IconGlyphProperty = BindableProperty.Create(nameof(IconGlyphProperty), typeof(string), typeof(FlyoutItemIconFont), string.Empty);
    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set { SetValue(IconGlyphProperty, value); }
    }
}
