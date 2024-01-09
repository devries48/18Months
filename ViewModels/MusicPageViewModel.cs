namespace Months18.ViewModels;

public partial class MusicPageViewModel : ObservableObject
{
    public MusicPageViewModel(ISettingsService settingsService, IAudioPlayerService playerService)
    {
        _settingsService = settingsService;
        _playerService = playerService;
        _source = [];
        _playerService.SubscribeToMediaStateChanged(OnMediaStateChanged);
    }

    private readonly ISettingsService _settingsService;
    private readonly IAudioPlayerService _playerService;
    private readonly List<ReleaseModel> _source;
    private ReleaseModel? _selectedRelease;

    [ObservableProperty]
    private ObservableCollection<ReleaseModel> _releases = [];

    [ObservableProperty]
    private bool _isPlaying;

    [RelayCommand]
    private void ItemTap(ReleaseModel tappedItem)
    {
        if(_selectedRelease != null && _selectedRelease != tappedItem)
            _selectedRelease.IsSelected = false;

        if(tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            _selectedRelease = null;
        } else
        {
            _selectedRelease = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void PlaySelectedRelease()
    {
        if(_selectedRelease == null || _playerService == null)
            return;

        if(_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if(_selectedRelease.Title == (_playerService.CurrentTrack?.ReleaseTitle ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayRelease(_selectedRelease);
    }

    public async Task GetLocalReleasesAsync()
    {
        if(_source.Count > 0)
            return;

        var release = await ReleaseModel.Create(
            "Charles Mingus",
            "Blues & Roots",
            ImagePath("Blues Roots-front.jpg"),
            "USA",
            1960)
            .ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Wednesday Night Prayer Meeting.mp3"), "Wednesday Night Prayer Meeting", "5:43");
        release.AddTrack(MusicPath("03. Moanin'.mp3"), "Moanin'", "8:03");
        release.AddTrack(MusicPath("06. E's Flat Ah's Flat Too"), "E's Flat Ah's Flat Too", "6:47");
        _source.Add(release);

        release = await ReleaseModel.Create(
            "Herbie Hancock",
            "Head Hunters",
            ImagePath("Head Hunters-front.jpg"),
            "USA",
            1973)
            .ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Chameleon.mp3"), "Chameleon", "15:44");
        release.AddTrack(MusicPath("02. Watermelon Man.mp3"), "Watermelon Man", "6:31");
        release.AddTrack(MusicPath("03. Sly.mp3"), "Sly", "10:21");
        _source.Add(release);

        release = await ReleaseModel.Create(
            "Miles Davis",
            "Bitches Brew",
            ImagePath("Bitches Brew-front.jpg"),
            "USA",
            1970)
            .ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Miles Runs The Voodoo Down.mp3"), "Miles Runs The Voodoo Down", "14:01");
        release.AddTrack(MusicPath("03. Spanish Key.mp3"), "Spanish Key", "10:20");
        _source.Add(release);

        release = await ReleaseModel.Create(
            "Jameszoo & Metropol Orkest",
            "Melkweg",
            ImagePath("Melkweg-front.jpg"),
            "NL",
            2019)
            .ConfigureAwait(false);
        release.AddTrack(MusicPath("03. (toots).mp3"), "(toots)", "5:24");
        release.AddTrack(MusicPath("04. (soup).mp3"), "(soup)", "6:36");
        release.AddTrack(MusicPath("07. (meat).mp3"), "(meat)", "8:54");
        _source.Add(release);

        release = await ReleaseModel.Create(
            "Ozzy Osbourne",
            "No More Tears",
            ImagePath("No More Tears-front.jpg"),
            "UK",
            1991)
            .ConfigureAwait(false);
        release.AddTrack(MusicPath("03. Mama, I'm Coming Home.mp3"), "Mama, I'm Coming Home", "4:12");
        release.AddTrack(MusicPath("05. No More Tears.mp3"), "No More Tears", "7:24");
        release.AddTrack(MusicPath("11. Road To Nowhere.mp3"), "Road To Nowhere", "5:11");
        _source.Add(release);

        _source.Add(
            await ReleaseModel.Create("King Crimson", "Red", ImagePath("Red-front.jpg"), "UK", 1974)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("Fifty Foot Hose", "Cauldron", ImagePath("Cauldron-front.jpg"), "USA", 1968)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("CAN", "Soundtracks", ImagePath("Soundtracks-front.jpg"), "D", 1973)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("Amon Düül II", "Yeti", ImagePath("Yeti-front.jpg"), "D", 1970)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("CAN", "Ege Bamyasi", ImagePath("Ege Bamyasi-front.jpg"), "D", 1972)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("Magma", "Kobaïa", ImagePath("Kobaia-front.jpg"), "FRA", 1970)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("Catapilla", "Catapilla", ImagePath("Catapilla-front.jpg"), "UK", 1971)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("Magma", "Mekanïk Destruktïẁ Kommandöh", ImagePath("Mdk-front.jpg"), "FRA", 1973)
                .ConfigureAwait(false));
        _source.Add(
            await ReleaseModel.Create("CAN", "Tago Mago", ImagePath("Tago Mago-front.jpg"), "D", 1971)
                .ConfigureAwait(false));
      
        release =await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Banco Del Mutuo Soccorso", ImagePath("Banco-front.jpg"), "ITA", 1972).ConfigureAwait(false);
        release.AddTrack(MusicPath("01  In Volo .mp3"), "In Volo", "2:14");
        release.AddTrack(MusicPath("02  R.I.P. (Requiescant In Pace) .mp3"), "R.I.P. (Requiescant In Pace)", "6:41");
        release.AddTrack(MusicPath("03  Passaggio .mp3"), "Passaggio", "1:19");
        _source.Add(release);

        release = await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Io Sono Nato Libero", ImagePath("Io Sono-front.jpg"), "ITA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01  Canto nomade per un prigionero politico.mp3"), "Canto nomade per un prigionero politico", "15:47");
        release.AddTrack(MusicPath("02  Non mi rompete.mp3"), "Non mi rompete", "5:04");
        _source.Add(release);


        _source.Add(
            await ReleaseModel.Create(
                "Koenjihyakkei",
                "Angherr Shisspa",
                ImagePath("Angherr Shisspa-front.jpg"),
                "JP",
                2005)
                .ConfigureAwait(false));

        Releases = new ObservableCollection<ReleaseModel>(_source);
    }

    private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e)
    { IsPlaying = e.NewState == MediaElementState.Playing; }

    private static string ImagePath(string filename) =>
 // TODO: Get from MusicMateData.json
 CombinePaths("C:/Users/rvrie/source/repos/18Months", "Data/Images", filename);

    private static string MusicPath(string filename) =>
 // TODO: Get from MusicMateData.json
 CombinePaths("C:/Users/rvrie/source/repos/18Months", "Data/Music", filename).Replace("/", "\\");

    private static string CombinePaths(params string[] paths)
    {
        string result = string.Empty;

        foreach(var path in paths)
        {
            if(result.Length != 0 && !result.EndsWith('/'))
                result += "/";

            result += path;
        }
        return result;
    }
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