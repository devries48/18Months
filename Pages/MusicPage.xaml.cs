
namespace Months18.Pages;

public partial class MusicPage : ContentPage
{
    public MusicPage(IAudioPlayerService service, MusicPageViewModel viewModel)
    {
        InitializeComponent();

        _audioPlayerService = service;
        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    public MusicPageViewModel ViewModel;

    readonly IAudioPlayerService _audioPlayerService;

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        _ =  ViewModel.GetLocalReleasesAsync().ConfigureAwait(false);
    }
}
