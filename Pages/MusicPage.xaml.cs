namespace Months18.Pages
{
    public partial class MusicPage : ContentPage
    {
        public MusicPage()
        {
            InitializeComponent();
        }

        void OnAddTrackClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomSourceEntry.Text))
            {
                DisplayAlert("Error Loading URL Source", "No value was found to load as a media source. " +
                    "When you do enter a value, make sure it's a valid URL. No additional validation is done.",
                    "OK");

                return;
            }
        }

    }
}
