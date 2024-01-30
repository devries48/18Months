using static Microsoft.Maui.Controls.AnimationExtensions;

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

    #region BindableProperties
    public static readonly BindableProperty TextProperty =
       BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MarqueeLabel),
        string.Empty,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marquee && marquee._label != null)
            {
                marquee._label.Text = (string)newValue;
                marquee.InitLabelText();
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
            if(bindable is MarqueeLabel marqueeLabel && marqueeLabel._label != null)
                marqueeLabel._label.TextColor = (Color)newValue;
        });

    public static readonly BindableProperty DefaultTextColorProperty =
    BindableProperty.Create(
        nameof(DefaultTextColor),
        typeof(Color),
        typeof(MarqueeLabel),
        default(Color),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel.UpdateTextColor();
        });

    public static readonly BindableProperty SelectedTextColorProperty =
        BindableProperty.Create(
        nameof(SelectedTextColor),
        typeof(Color),
        typeof(MarqueeLabel),
        default(Color),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel.UpdateTextColor();
        });

    public static readonly BindableProperty FontFamilyProperty =
    BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(MarqueeLabel),
        default(string),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if((bindable is MarqueeLabel marqueeLabel) && (marqueeLabel._label != null))
                marqueeLabel._label.FontFamily = (string)newValue;
        });


    public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(MarqueeLabel),
        14.0,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel._label.FontSize = (double)newValue;
        });

    public static readonly BindableProperty FontAttributesProperty =
    BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(MarqueeLabel),
        FontAttributes.None,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel._label.FontAttributes = (FontAttributes)newValue;
        });


    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
        nameof(IsActive),
        typeof(bool),
        typeof(MarqueeLabel),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
            {
                if((bool)newValue)
                {
                    marqueeLabel._label.LineBreakMode = LineBreakMode.NoWrap;
                    marqueeLabel.AnimateMarquee(); // Start animations when IsActive is true
                } else
                {
                    marqueeLabel.StopAnimation();
                }
            }
        });

    /// <summary>
    /// Select and Activate marquee effect and change color to SelectedColor if available.
    /// </summary>
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(MarqueeLabel),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
            {
                marqueeLabel.UpdateTextColor();
                marqueeLabel.IsActive = marqueeLabel.IsSelected;
                //marqueeLabel.UpdateIsActive();
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
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel.OnPropertyChanged(nameof(DurationAnimation));
        });

    public static readonly BindableProperty DurationPauseProperty =
        BindableProperty.Create(
        nameof(DurationPause),
        typeof(double),
        typeof(MarqueeLabel),
        2.0, // Default duration for the pause
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if(bindable is MarqueeLabel marqueeLabel)
                marqueeLabel.OnPropertyChanged(nameof(DurationPause));
        });

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set
        {
            SetValue(TextProperty, value);
            OnPropertyChanged(nameof(Text));
        }
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

    public Color DefaultTextColor
    {
        get => (Color)GetValue(DefaultTextColorProperty);
        set => SetValue(DefaultTextColorProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set
        {
            SetValue(FontSizeProperty, value);
            OnPropertyChanged(nameof(FontSize));
        }
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set
        {
            SetValue(FontAttributesProperty, value);
            OnPropertyChanged(nameof(FontAttributes));
        }
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set
        {
            SetValue(IsActiveProperty, value);
            OnPropertyChanged(nameof(IsActive));
        }
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set
        {
            SetValue(IsSelectedProperty, value);
            OnPropertyChanged(nameof(IsSelected));
        }
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

    readonly Label _label;
    CancellationTokenSource cancelTokenSource = new();
    bool _stopAnimationFlag = false;
    bool _isAnimationRunning = false;
    double _totalTextWidth = 0;
    double _translationX = 0;

    private async void AnimateMarquee()
    {
        if(_stopAnimationFlag)
        {
            _stopAnimationFlag = false;
            return;
        }

        if(NotAnimated())
            return;

        _isAnimationRunning = true;

        // Pause for 'DurationPause' seconds before a new animation
        var delayTaskCompletionSource = new TaskCompletionSource<bool>();

        using(CancelToken.Register(() => delayTaskCompletionSource.TrySetCanceled()))
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(DurationPause), CancelToken);
            } catch
            {
                return;
            }
        }

        if(CancelToken.IsCancellationRequested)
        {
            _isAnimationRunning = false;
            return;
        }

        // Set up the first animation from 0 to -totalTextWidth
        var firstAnimation = new Animation(
            x =>
            {
                _translationX = x;
                _label.TranslationX = _translationX;
            },
            0,
            -_totalTextWidth,
            Easing.Linear);

        // Set up the second animation from Width to 0
        var secondAnimation = new Animation(
            x =>
            {
                _translationX = x;
                _label.TranslationX = _translationX;
            },
            _totalTextWidth,
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


    /// <summary>
    /// Initialize MarqueeLabel, abort possible active animation from previous usage.
    /// </summary>
    private void InitLabelText()
    {
        if(_isAnimationRunning)
        {
            this.AbortAnimation("MarqueeAnimation");

            cancelTokenSource.Cancel();
            cancelTokenSource = new CancellationTokenSource(); // Set a new CancellationToken for the upcoming animation
        }

        SizeRequest labelSize = _label.Measure(double.PositiveInfinity, double.PositiveInfinity);
        _totalTextWidth = labelSize.Request.Width;
        _label.TranslationX = 0;

        AnimateMarquee();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>'True' when the 'marquee requirements' are not met.</returns>
    private bool NotAnimated() => _stopAnimationFlag ||
        !IsActive ||
        !IsEnabled ||
        _totalTextWidth <= 0 ||
        Width <= 0 ||
        _totalTextWidth <= Width;

    /// <summary>
    /// When a label is initialised or invisible the width = -1, so wait for the size change
    /// </summary>
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        //SizeChanged -= OnSizeChanged;
        if(Width > 0)
            InitLabelText();
    }

    private void SetupLabel()
    {
        _label.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
        _label.SetBinding(Label.TextColorProperty, new Binding(nameof(TextColor), source: this));
        _label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(FontFamily), source: this));
        _label.SetBinding(Label.FontSizeProperty, new Binding(nameof(FontSize), source: this));
        _label.SetBinding(Label.FontAttributesProperty, new Binding(nameof(FontAttributes), source: this));
        _label.SetBinding(Label.StyleProperty, new Binding(nameof(Style), source: this));

        // Add other property bindings as needed
    }

    /// <summary>
    /// The animation completes and stops
    /// </summary>
    private void StopAnimation()
    {
        if(_isAnimationRunning)
            _stopAnimationFlag = true;
        else
            _label.LineBreakMode = LineBreakMode.TailTruncation;
    }


    private void UpdateTextColor() => TextColor =
        IsSelected && SelectedTextColor != default(Color) ? SelectedTextColor : DefaultTextColor;

    CancellationToken CancelToken => cancelTokenSource.Token;
}