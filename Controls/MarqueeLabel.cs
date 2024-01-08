namespace Months18.Controls;

public class MarqueeLabel : Frame
{
    public MarqueeLabel()
    {
        Padding = new Thickness(0);
        Margin = new Thickness(0);
        BackgroundColor = Colors.Transparent;

        innerFrame = new Frame { Padding = new Thickness(0), Margin = new Thickness(0), BackgroundColor = Colors.Transparent, Content = new Label() };

        marqueeLabel = (Label)innerFrame.Content;
        marqueeLabel.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
        Content = innerFrame;

        SizeChanged += OnSizeChanged;
    }

    private double translationX = 0;
    private readonly double durationInSeconds = 10; // Adjust as needed

    readonly Frame innerFrame;
    readonly Label marqueeLabel;

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MarqueeLabel),
        string.Empty,
        propertyChanged: OnTextChanged);

    public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

    private async void AnimateMarquee()
    {
        await Task.Delay(TimeSpan.FromSeconds(1)); // Adjust the delay duration as needed

        // Set up the first animation from 0 to -Width
        var firstAnimation = new Animation(x =>
            {
                translationX = x;
                innerFrame.TranslationX = translationX;
            }, 0, -Width);

        // Set up the second animation from Width to 0
        var secondAnimation = new Animation(x =>
            {
                translationX = x;
                innerFrame.TranslationX = translationX;
            }, Width, 0, Easing.Linear);

        // Create a sequence of animations
        var sequence = new Animation
        {
            { 0, 0.5, firstAnimation }, // Run the first animation for the first half of the sequence
            { 0.5, 1, secondAnimation } // Run the second animation for the second half of the sequence
        };

        // Callback for the end of the sequence
        sequence.Commit(this, "MarqueeAnimation", 16, (uint)(durationInSeconds * 1000), Easing.Linear, (value, cancelled) =>
            {
                AnimateMarquee(); // Initialize a new set of animations for the next iteration
            });
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable is MarqueeLabel marqueeFrame && marqueeFrame.marqueeLabel != null)
        {
            marqueeFrame.marqueeLabel.Text = (string)newValue;
        }
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        // SizeChanged event triggered, initialize components and unsubscribe from the event
        AnimateMarquee();
        SizeChanged -= OnSizeChanged;
    }
}