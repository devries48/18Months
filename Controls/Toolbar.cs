using Microsoft.Maui.Controls.Shapes;

namespace Months18.Controls;

public partial class Toolbar : Border
{
    public Toolbar()
    {
        Margin = new Thickness(20, 0);
        HorizontalOptions = LayoutOptions.Fill;
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 0, 2, 2) };
        StrokeThickness = 3;
        VerticalOptions = LayoutOptions.Start;
        ZIndex = 1;

        _expandButton = new ImageButton
        {
            Margin = new Thickness(0),
            Aspect = Aspect.AspectFit,
            BackgroundColor = BackgroundColor,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            Style = GetExpandButtonStyle(true)
        };

        _initialHeight = 0;

        SetContent();

        SizeChanged += OnSizeChanged;
        PropertyChanged += OnPropertyChanged;
        _expandButton.Clicked += HandleImageButtonClick;
    }

    public static readonly BindableProperty HeaderContentProperty =
         BindableProperty.Create(
        nameof(HeaderContent),
        typeof(View),
        typeof(Toolbar),
        propertyChanged: OnHeaderContentPropertyChanged);

    public static readonly BindableProperty ExpandedContentProperty =
        BindableProperty.Create(
        nameof(ExpandedContent),
        typeof(View),
        typeof(Toolbar),
        propertyChanged: OnExpandedContentPropertyChanged);

    public static readonly BindableProperty ExpandedHeightProperty =
    BindableProperty.Create(nameof(ExpandedHeight), typeof(double), typeof(Toolbar), defaultValue: 0.0);

    public static readonly BindableProperty IsExpandedProperty =
      BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(Toolbar),
        defaultValue: false,
        propertyChanged: OnIsExpandedPropertyChanged);

    public static readonly BindableProperty AccentColorProperty =
     BindableProperty.Create(
        nameof(AccentColor),
        typeof(Color),
        typeof(Toolbar),
        default(Color),
        propertyChanged: OnAccentColorPropertyChanged);

    public static readonly BindableProperty HeaderColorProperty =
 BindableProperty.Create(
        nameof(HeaderColor),
        typeof(Color),
        typeof(Toolbar),
        default(Color),
        propertyChanged: OnHeaderColorPropertyChanged);

    public View HeaderContent
    {
        get => (View)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public View ExpandedContent
    {
        get => (View)GetValue(ExpandedContentProperty);
        set => SetValue(ExpandedContentProperty, value);
    }

    public double ExpandedHeight
    {
        get => (double)GetValue(ExpandedHeightProperty);
        set => SetValue(ExpandedHeightProperty, value);
    }

    public bool IsExpanded { get => (bool)GetValue(IsExpandedProperty); set => SetValue(IsExpandedProperty, value); }

    public Color AccentColor
    {
        get => (Color)GetValue(AccentColorProperty);
        set => SetValue(AccentColorProperty, value);
    }

    public Color HeaderColor
    {
        get => (Color)GetValue(HeaderColorProperty);
        set => SetValue(HeaderColorProperty, value);
    }

    private readonly ImageButton _expandButton;
    private double _initialHeight;

    private static void OnHeaderContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Toolbar toolbar)
        {
            toolbar.SetContent();
            toolbar.UpdateHeaderColor();
        }
    }

    private static void OnExpandedContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Toolbar toolbar)
            toolbar.SetContent();
    }

    private static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Toolbar toolbar)
        {
            toolbar.UpdateBorderHeight();
            toolbar.UpdateHeaderColor();
        }
    }

    private static void OnAccentColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Toolbar toolbar)
        {
            toolbar.UpdateAccentColor();
            toolbar.UpdateHeaderColor();
        }
    }

    private static void OnHeaderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Toolbar toolbar && toolbar.IsExpanded)
            toolbar.UpdateHeaderColor();
    }

    private static Color? GetAccentColorForTheme()
    {
        if (Application.Current == null)
            return null;

        var theme = Application.Current?.UserAppTheme ?? AppTheme.Dark;
        var colorKey = theme == AppTheme.Dark ? "Dark_Accent" : "Light_Accent";

        if (Application.Current!.Resources.TryGetValue(colorKey, out var colorValue) && colorValue is Color accentColor)
            return accentColor;

        return null;
    }

    private static Style? GetExpandButtonStyle(bool accent)
    {
        string icon = accent ? "IconExpandAccent" : "IconExpand";

        if (Application.Current != null && Application.Current.Resources.TryGetValue(icon, out var style))
            return (Style?)style;

        return null;
    }

    private void SetContent()
    {
        var grid = new Grid
        {
            RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                },
            ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
        };

        var headerContentView = new ContentView
        {
            Content = HeaderContent,
            GestureRecognizers = { new TapGestureRecognizer { Command = new Command(() => IsExpanded = !IsExpanded) } }
        };

        var expandButton = new ContentView { Content = _expandButton };
        //var expandContentView = new ContentView { Content = ExpandedContent };

        var expandContentView = new ContentView
        {
            Content = ExpandedContent,
            GestureRecognizers = { new TapGestureRecognizer { Command = new Command(() => IsExpanded = !IsExpanded) } }
        };

        grid.Add(headerContentView, 0, 0);
        grid.Add(expandButton, 1, 0);
        grid.Add(expandContentView);
        grid.SetRow(expandContentView, 1);
        grid.SetColumnSpan(expandContentView, 2);
        Content = grid;
    }

    private async void UpdateHeaderColor(View? view = null)
    {
        if (HeaderContent == null)
            return;

        view ??= HeaderContent;

        if (view is Label label)
        {
            await label.TextColorTo(IsExpanded ? HeaderColor : AccentColor);
        }
        else
        {
            foreach (Label lbl in view.GetVisualTreeDescendants().Where(e => e.GetType() == typeof(Label)).Cast<Label>())
            {
                UpdateHeaderColor(lbl);
            }
        }
    }

    private void UpdateBorderHeight()
    {
        double startHeight, targetHeight;
        Easing easing;

        if (IsExpanded)
        {
            easing = Easing.CubicInOut;
            startHeight = _initialHeight;
            targetHeight = ExpandedHeight;
            Focus();
        }
        else
        {
            easing = Easing.SpringIn;
            startHeight = ExpandedHeight;
            targetHeight = _initialHeight;
        }

        this.Animate(
            "HeightAnimation",
            animation =>
            {
                HeightRequest = animation;
            },
            startHeight,
            targetHeight,
            length: 250,
            easing: easing);

        // Animate the rotation of the ImageButton
        _expandButton.RotateTo(IsExpanded ? 180 : 0, 250, easing);
    }

    private void UpdateAccentColor()
    {
        var accentColor = GetAccentColorForTheme();
        bool isAccent = AccentColor == accentColor;

        _expandButton.Style = GetExpandButtonStyle(isAccent);

    }

      //if (AccentColor== )
    //((FontImageSource) _expandButton.Source).Color = AccentColor;

    /// <summary>
    /// When a Toolbar is initialised or invisible the height = -1, so wait for the size change
    /// </summary>
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (Height > 0 && !IsExpanded)
        {
            SizeChanged -= OnSizeChanged;
            SizeRequest toolbarSize = Measure(double.PositiveInfinity, double.PositiveInfinity);
            _initialHeight = toolbarSize.Request.Height;
        }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BackgroundColor))
        {
            _expandButton.BackgroundColor = BackgroundColor;
        }
    }

    private void HandleImageButtonClick(object? sender, System.EventArgs e) { IsExpanded = !IsExpanded; }
}