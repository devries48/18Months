
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

    private static string ImagePath(string filename) =>
// TODO: Get from MusicMateData.json
CombinePaths("C:/Users/rvrie/source/repos/18Months", "Data/Images", filename);

    private static string MusicPath(string filename) =>
 // TODO: Get from MusicMateData.json
 CombinePaths("C:/Users/rvrie/source/repos/18Months", "Data/Music", filename).Replace("/", "\\");

    private static string CombinePaths(params string[] paths)
    {
        string result = string.Empty;

        foreach (var path in paths)
        {
            if (result.Length != 0 && !result.EndsWith('/'))
                result += "/";

            result += path;
        }
        return result;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        _ =  ViewModel.GetLocalReleasesAsync().ConfigureAwait(false);
    }
}
