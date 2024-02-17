using System.Diagnostics;

namespace Months18.Pages;

public partial class VideoPage : ContentPage
{
    public VideoPage(VideoPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    // The value we want the slider to increment each time it updates
    readonly double sliderIncrement = 1;

    // The value for the slider we will be using.
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Recognize the sender as a Slider object.
        Slider slider = (Slider)sender;


        // Check if the value is valid, based on our increment.
        if (slider.Value % sliderIncrement != 0)
        {
            var sliderCorrectValue =  Math.Round(slider.Value, MidpointRounding.AwayFromZero); 
            slider.Value = sliderCorrectValue;
            // Update label text (optional)
            Debug.WriteLine( sliderCorrectValue.ToString());
        }
    }
}