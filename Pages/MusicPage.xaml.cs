
namespace Months18.Pages;

public partial class MusicPage : ContentPage
{
    public MusicPage(IMusicPlayerService service, MusicPageViewModel viewModel)
    {
        InitializeComponent();

        _musicPlayerService = service;
        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    public MusicPageViewModel ViewModel;


    readonly IMusicPlayerService _musicPlayerService;

    bool isrelease1 = true;
     async void OnAddTrackClickedAsync(object? sender, EventArgs e)
    {
        var release1 =  await ReleaseModel.Create(
      "Charles Mingus",
      "Blues & Roots",
      ImagePath("Blues Roots-front.jpg"),
      "USA",
      1960)
      ;
        release1.AddTrack(MusicPath("01. Wednesday Night Prayer Meeting.mp3"), "Wednesday Night Prayer Meeting", "5:43");
        release1.AddTrack(MusicPath("03. Moanin'.mp3"), "Moanin'", "8:03");
        release1.AddTrack(MusicPath("06. E's Flat Ah's Flat Too"), "E's Flat Ah's Flat Too", "6:47");

        var release2 =  await  ReleaseModel.Create(
            "Herbie Hancock",
            "Head Hunters",
            ImagePath("Head Hunters-front.jpg"),
            "USA",
            1973)
            ;
        release2.AddTrack(MusicPath("01. Chameleon.mp3"), "Chameleon", "15:44");
        release2.AddTrack(MusicPath("02. Watermelon Man.mp3"), "Watermelon Man", "6:31");
        release2.AddTrack(MusicPath("03. Sly.mp3"), "Sly", "10:21");

        ReleaseModel model;
        if (isrelease1)
            model = release1;
        else model = release2;

        _musicPlayerService.PlayRelease(model);
        isrelease1=!isrelease1;
    }

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
        _ =  ViewModel.GetLocalReleases();
    }
}
