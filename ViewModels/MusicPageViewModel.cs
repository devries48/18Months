namespace Months18.ViewModels;

public partial class MusicPageViewModel : ObservableObject
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Class instantiated by Dependency Injection.")]
    public MusicPageViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _source = [];
    }

    private readonly ISettingsService _settingsService;
    private readonly List<ReleaseModel> _source;

    [ObservableProperty]
    private ObservableCollection<ReleaseModel> _releases = [];

    private ReleaseModel? _selectedRelease;

    public ReleaseModel? SelectedRelease
    {
        get => _selectedRelease;
        set
        {
            if (_selectedRelease != value)
            {
                // Reset the IsSelected property for the previously selected release
                if (_selectedRelease != null)
                    _selectedRelease.IsSelected = false;

                _selectedRelease = value;

                // Set the IsSelected property for the newly selected release
                if (_selectedRelease != null)
                    _selectedRelease.IsSelected = true;

                OnPropertyChanged(nameof(SelectedRelease));
            }
        }
    }

    [RelayCommand]
    private void OnItemTapped(ReleaseModel tappedItem)
    {
        if (tappedItem == SelectedRelease)
        {
            SelectedRelease = null; // Deselect the item
        }
     }

    public async Task GetLocalReleases()
    {
        if (_source.Count > 0)
            return;

        var release = await ReleaseModel.Create("Charles Mingus", "Blues & Roots", ImagePath("Blues Roots-front.jpg")).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Wednesday Night Prayer Meeting.mp3"), "Wednesday Night Prayer Meeting","5:43");
        release.AddTrack(MusicPath("03. Moanin'.mp3"), "Moanin'", "8:03");
        release.AddTrack(MusicPath("06. E's Flat Ah's Flat Too"), "E's Flat Ah's Flat Too", "6:47");
        _source.Add(release);

        _source.Add(await ReleaseModel.Create("Herbie Hancock", "Head Hunters", ImagePath("Head Hunters-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("Miles Davis", "Bitches Brew", ImagePath("Bitches Brew-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("Ozzy Osbourne", "No More Tears", ImagePath("No More Tears-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("King Crimson", "Red", ImagePath("Red-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("Can", "Soundtracks", ImagePath("Soundtracks-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("Amon Düül II", "Yeti", ImagePath("Yeti-front.jpg")).ConfigureAwait(false));

        _source.Add(await ReleaseModel.Create("Catapilla", "Catapilla", ImagePath("Catapilla-front.jpg")).ConfigureAwait(false));
        _source.Add(await ReleaseModel.Create("Koenjihyakkei", "Angherr Shisspa", ImagePath("Angherr Shisspa-front.jpg")).ConfigureAwait(false));

        Releases = new ObservableCollection<ReleaseModel>(_source);
    }

    private static string ImagePath(string filename) =>
        // TODO: Get from MusicMateData.json
        Path.Combine("C:/Users/rvrie/source/repos/18Months", "Data/Images", filename);
    private static string MusicPath(string filename) =>
    // TODO: Get from MusicMateData.json
    Path.Combine("C:/Users/rvrie/source/repos/18Months", "Data/Music", filename);


}

//2022
//Aug: Miles, Herbie, Charles, Can, Ozzy(no more), yeti
//Sep: Can, Miles
//Okt: Can, Miles, yeti, king crimson(red)
//Nov: Can, Magma(kobaia), catapilla
//Dec: Can, Magma(kobaia)

//2023
//jan: Magma, Can
//feb: Magma, Can, Amon(Phal)
//mar: Magma, Can, Amon(yet), Area
//apr: Area, Joe Mcphee, Opeth (black), Mingus (blues, ah hum)
//mei: Joe McPhee, Dun, 50ft
//jun: fruttu, Joe, Area, Opeth(damnation), crimson(1e)
//jul: Savatage, Frutti, deep purple(machine), Bubu, mingus(ahum)
//aug: Osanna, Quella, PFM, Bubu, banco
//sep: Quella, Osanna, Mercifull,
//okt: Koen, Banco,Camel
//nov: Koen, Banco, KollekTiv, brainstorm
//dec: Koen, Dun, Kollektiv, brainstorm, Mingus(children)

//in top: Ys,PFM, merciful,