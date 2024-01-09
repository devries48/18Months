namespace Months18.Controls;

public class MarqueeLabel : ScrollView
{
    public MarqueeLabel()
    {
        Orientation = ScrollOrientation.Horizontal;
        Padding = new Thickness(0);
        Margin = new Thickness(0);
        BackgroundColor = Colors.Transparent;
        HorizontalScrollBarVisibility = ScrollBarVisibility.Never;

        _label = new Label { LineBreakMode = LineBreakMode.NoWrap };
       
        SetupLabel();
        
        Content = _label;    
        SizeChanged += OnSizeChanged;
    }

    private void SetupLabel()
    {
        _label.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
        _label.SetBinding(Label.TextColorProperty, new Binding(nameof(TextColor), source: this));
        _label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(FontFamily), source: this));
        _label.SetBinding(Label.FontSizeProperty, new Binding(nameof(FontSize), source: this));
        // Add other property bindings as needed
    }


    double translationX = 0;
    double totalTextWidth = 0;
    bool isAnimationRunning = false;
    bool stopAnimationFlag = false;
    readonly Label _label;

    #region BindableProperties
    public static readonly BindableProperty TextProperty =
       BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MarqueeLabel),
        string.Empty,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marquee && marquee._label != null)
            {
                marquee._label.Text = (string)newValue;

                SetTotalTextWidth(marquee);
                marquee.AnimateMarquee(); // Start animations when IsActive is true
            }
        });

    public static readonly BindableProperty TextColorProperty =
     BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(MarqueeLabel),
        default(Color),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                if (marqueeLabel._label != null)
                    marqueeLabel._label.TextColor = (Color)newValue;
            }
        });

    public static readonly BindableProperty SelectedTextColorProperty =
        BindableProperty.Create(
        nameof(SelectedTextColor),
        typeof(Color),
        typeof(MarqueeLabel),
        default(Color),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel.UpdateTextColor();
            }
        });

    public static readonly BindableProperty FontFamilyProperty =
    BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(MarqueeLabel),
        default(string),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                if (marqueeLabel._label != null)
                    marqueeLabel._label.FontFamily = (string)newValue;
            }
        });


    public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(MarqueeLabel),
        14.0,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel._label.FontSize = (double)newValue;
            }
        });

    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
        nameof(IsActive),
        typeof(bool),
        typeof(MarqueeLabel),
        true,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                if ((bool)newValue)
                {
                    marqueeLabel._label.LineBreakMode = LineBreakMode.NoWrap;
                    marqueeLabel.AnimateMarquee(); // Start animations when IsActive is true
                }
                else
                {
                    marqueeLabel.StopAnimation();
                }
            }
        });

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(MarqueeLabel),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel.UpdateTextColor();
            }
        });

    public static readonly BindableProperty DurationAnimationProperty =
       BindableProperty.Create(
        nameof(DurationAnimation),
        typeof(double),
        typeof(MarqueeLabel),
        8.0, // Default duration for the animation
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel.OnPropertyChanged(nameof(DurationAnimation));
            }
        });

    public static readonly BindableProperty DurationPauseProperty =
        BindableProperty.Create(
        nameof(DurationPause),
        typeof(double),
        typeof(MarqueeLabel),
        2.0, // Default duration for the pause
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel.OnPropertyChanged(nameof(DurationPause));
            }
        });

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set
        {
            SetValue(TextColorProperty, value);
            OnPropertyChanged(nameof(TextColor));
        }
    }

    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public double DurationAnimation
    {
        get => (double)GetValue(DurationAnimationProperty);
        set => SetValue(DurationAnimationProperty, value);
    }

    public double DurationPause
    {
        get => (double)GetValue(DurationPauseProperty);
        set => SetValue(DurationPauseProperty, value);
    }
    #endregion

    private async void AnimateMarquee()
    {
        isAnimationRunning = false;
        if (stopAnimationFlag)
        {
            stopAnimationFlag = false;
            return;
        }

        if (totalTextWidth <= Width)
            return; // Skip animation if the content fits

        await Task.Delay(TimeSpan.FromSeconds(DurationPause));

        // Set up the first animation from 0 to -totalTextWidth
        var firstAnimation = new Animation(
            x =>
            {
                translationX = x;
                _label.TranslationX = translationX;
            },
            0,
            -totalTextWidth,
            Easing.Linear);

        // Set up the second animation from Width to 0
        var secondAnimation = new Animation(
            x =>
            {
                translationX = x;
                _label.TranslationX = translationX;
            },
            totalTextWidth,
            0,
            Easing.Linear);

        // Create a sequence of animations
        var sequence = new Animation
        {
            { 0, 0.5, firstAnimation }, // Run the first animation for the first half of the sequence
            { 0.5, 1, secondAnimation } // Run the second animation for the second half of the sequence
        };

        // Callback for the end of the sequence
        sequence.Commit(
            this,
            "MarqueeAnimation",
            16,
            (uint)(DurationAnimation * 1000),
            Easing.Linear,
            (value, cancelled) => AnimateMarquee());
    }

    private void StopAnimation()
    {
        if (isAnimationRunning)
            stopAnimationFlag = true;
        else
            _label.LineBreakMode = LineBreakMode.TailTruncation;
    }

    private void UpdateTextColor() => TextColor = IsSelected && SelectedTextColor != default(Color) ? SelectedTextColor : TextColor;

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        SetTotalTextWidth(this);
        AnimateMarquee();
        SizeChanged -= OnSizeChanged;
    }

    private static void SetTotalTextWidth(MarqueeLabel marquee)
    {
        SizeRequest labelSize = marquee._label.Measure(double.PositiveInfinity, double.PositiveInfinity);
        marquee.totalTextWidth = labelSize.Request.Width;
    }
}