namespace Months18.Pages;

public partial class MusicPage : ContentPage
{
    public MusicPage(MusicPlayerService service, MusicPageViewModel viewModel)
    {
        InitializeComponent();

        _musicPlayerService = service;
        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    public MusicPageViewModel ViewModel;


    readonly MusicPlayerService _musicPlayerService;

    //void OnAddTrackClicked(object? sender, EventArgs e)
    //{
    //    if (string.IsNullOrWhiteSpace(CustomSourceEntry.Text))
    //    {
    //        DisplayAlert("Error Loading URL Source", "No value was found to load as a media source. " +
    //            "When you do enter a value, make sure it's a valid URL. No additional validation is done.",
    //            "OK");

    //        return;
    //    }
    //    _musicPlayerService.AddToPlaylist("Music/01. Naked Death.mp3", AudioPlayerSource.Embed);
    //}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        _ =  ViewModel.GetLocalReleases();
    }
}
