namespace Months18.ViewModels;

public partial class MusicPageViewModel : ObservableObject
{
    public MusicPageViewModel(ISettingsService settingsService, IAudioPlayerService playerService)
    {
        _settingsService = settingsService;
        _playerService = playerService;
        _source = [];
        _playerService.SubscribeToMediaStateChanged(OnMediaStateChanged);

        _=GetLocalReleasesAsync();
    }

    private readonly ISettingsService _settingsService;
    private readonly IAudioPlayerService _playerService;
    private readonly List<ReleaseModel> _source;
    private ReleaseModel? _selectedRelease;

    public const int DefaultItemWidth = 200;
    // test
    [ObservableProperty]
    int span = 1;

    [RelayCommand]
    void ChangeSpan(int byAmount)
    {
        if (Span + byAmount <= 0)   //Prevent span from being <= 0.
        {
            Span = 1;
            return;
        }

        Span += byAmount;
    }
    // end test

    [ObservableProperty]
    private ObservableCollection<ReleaseModel> _releases = [];

    [ObservableProperty]
    private bool _isPlaying;

    [RelayCommand]
    private void ItemTap(ReleaseModel tappedItem)
    {
        if (_selectedRelease != null && _selectedRelease != tappedItem)
            _selectedRelease.IsSelected = false;

        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            _selectedRelease = null;
        }
        else
        {
            _selectedRelease = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void PlaySelectedRelease()
    {
        if (_selectedRelease == null || _playerService == null)
            return;

        if (_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if (_selectedRelease.Title == (_playerService.CurrentTrack?.ReleaseTitle ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayRelease(_selectedRelease);
    }

    public async Task GetLocalReleasesAsync()
    {
        if (_source.Count > 0)
            return;

        var release = await ReleaseModel.Create("Charles Mingus", "Blues & Roots", ImagePath("Blues Roots-front.jpg"), "USA", 1960).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Wednesday Night Prayer Meeting.mp3"), "Wednesday Night Prayer Meeting", "5:43");
        release.AddTrack(MusicPath("03. Moanin'.mp3"), "Moanin'", "8:03");
        release.AddTrack(MusicPath("06. E's Flat Ah's Flat Too.mp3"), "E's Flat Ah's Flat Too", "6:47");
        _source.Add(release);

        release = await ReleaseModel.Create("Herbie Hancock", "Head Hunters", ImagePath("Head Hunters-front.jpg"), "USA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Chameleon.mp3"), "Chameleon", "15:44");
        release.AddTrack(MusicPath("02. Watermelon Man.mp3"), "Watermelon Man", "6:31");
        release.AddTrack(MusicPath("03. Sly.mp3"), "Sly", "10:21");
        _source.Add(release);

        release = await ReleaseModel.Create("Miles Davis", "Bitches Brew", ImagePath("Bitches Brew-front.jpg"), "USA", 1970).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Miles Runs The Voodoo Down.mp3"), "Miles Runs The Voodoo Down", "14:01");
        release.AddTrack(MusicPath("03. Spanish Key.mp3"), "Spanish Key", "10:20");
        _source.Add(release);

        release = await ReleaseModel.Create("Jameszoo & Metropol Orkest", "Melkweg", ImagePath("Melkweg-front.jpg"), "NL", 2019).ConfigureAwait(false);
        release.AddTrack(MusicPath("03. (toots).mp3"), "(toots)", "5:24");
        release.AddTrack(MusicPath("04. (soup).mp3"), "(soup)", "6:36");
        release.AddTrack(MusicPath("07. (meat).mp3"), "(meat)", "8:54");
        _source.Add(release);

        release = await ReleaseModel.Create("Ozzy Osbourne", "No More Tears", ImagePath("No More Tears-front.jpg"), "UK", 1991).ConfigureAwait(false);
        release.AddTrack(MusicPath("03. Mama, I'm Coming Home.mp3"), "Mama, I'm Coming Home", "4:12");
        release.AddTrack(MusicPath("05. No More Tears.mp3"), "No More Tears", "7:24");
        release.AddTrack(MusicPath("11. Road To Nowhere.mp3"), "Road To Nowhere", "5:11");
        _source.Add(release);

        release = await ReleaseModel.Create("King Crimson", "Red", ImagePath("Red-front.jpg"), "UK", 1974).ConfigureAwait(false);
        release.AddTrack(MusicPath("02 - Fallen Angel.mp3"), "Fallen Angel", "6:01");
        release.AddTrack(MusicPath("05 - Starless.mp3"), "Starless", "12:18");
        _source.Add(release);

        release = await ReleaseModel.Create("Fifty Foot Hose", "Cauldron", ImagePath("Cauldron-front.jpg"), "USA", 1968).ConfigureAwait(false);
        release.AddTrack(MusicPath("02 If Not This Time.mp3"), "If Not This Time", "3:38");
        release.AddTrack(MusicPath("04 The Things That Concern You.mp3"), "The Things That Concern You", "3:29");
        release.AddTrack(MusicPath("06 Red The Sign Post.mp3"), "Red The Sign Post", "2:59");
        release.AddTrack(MusicPath("08 Rose.mp3"), "Rose", "5:09");
        release.AddTrack(MusicPath("10 God Bless The Child.mp3"), "God Bless The Child", "2:51");
        _source.Add(release);

        release = await ReleaseModel.Create("CAN", "Soundtracks", ImagePath("Soundtracks-front.jpg"), "D", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("02. Tango Whiskyman (From Deadlock).mp3"), "Tango Whiskyman (From Deadlock)", "4:03");
        release.AddTrack(MusicPath("04. Don't Turn The Light On, Leave Me Alone (From Cream).mp3"), "Don't Turn The Light On, Leave Me Alone (From Cream)", "3:42");
        release.AddTrack(MusicPath("06. Mother Sky (From Deep End).mp3"), "Mother Sky (From Deep End)", "14:28");
        release.AddTrack(MusicPath("07. She Brings The Rain (From Bottom - Ein Großer Graublauer Vogel).mp3"), "She Brings The Rain (From Bottom - Ein Großer Graublauer Vogel)", "4:05");
        _source.Add(release);

        release = await ReleaseModel.Create("Amon Düül II", "Yeti", ImagePath("Yeti-front.jpg"), "D", 1970).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Soap Shop Rock a) Burning Sister.mp3"), "Soap Shop Rock a) Burning Sister", "3:45");
        release.AddTrack(MusicPath("02. Soap Shop Rock b) Halluzination Guillotine.mp3"), "Soap Shop Rock b) Halluzination Guillotine", "3:10");
        release.AddTrack(MusicPath("03. Soap Shop Rock c) Gulp A Sonata.mp3"), "Soap Shop Rock c) Gulp A Sonata", "0:46");
        release.AddTrack(MusicPath("04. Soap Shop Rock d) Flesh-Coloured Anti-Aircraft Alarm.mp3"), "Soap Shop Rock d) Flesh-Coloured Anti-Aircraft Alarm", "6:04");
        _source.Add(release);

        release = await ReleaseModel.Create("CAN", "Ege Bamyasi", ImagePath("Ege Bamyasi-front.jpg"), "D", 1972).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Pinch.mp3"), "Pinch", "9:29");
        release.AddTrack(MusicPath("03. One More Night.mp3"), "One More Night", "5:35");
        release.AddTrack(MusicPath("04. Vitamin C.mp3"), "Vitamin C", "3:32");
        release.AddTrack(MusicPath("06. I'm So Green.mp3"), "I'm So Green", "3:05");
        _source.Add(release);

        release = await ReleaseModel.Create("Magma", "Kobaïa", ImagePath("Kobaia-front.jpg"), "FRA", 1970).ConfigureAwait(false);
        release.AddTrack(MusicPath("1.01. Kobaïa.mp3"), "Kobaïa", "10:09");
        release.AddTrack(MusicPath("1.02. Aïna.mp3"), "Aïna", "6:15");
        release.AddTrack(MusicPath("1.03. Malaria.mp3"), "Malaria", "4:21");
        release.AddTrack(MusicPath("2.04. Müh.mp3"), "Müh", "11:17");
        _source.Add(release);

        release = await ReleaseModel.Create("Catapilla", "Catapilla", ImagePath("Catapilla-front.jpg"), "UK", 1971).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Naked Death.mp3"), "Naked Death", "15:42");
        _source.Add(release);

        release = await ReleaseModel.Create("Magma", "Mekanïk Destruktïẁ Kommandöh", ImagePath("Mdk-front.jpg"), "FRA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Hortz Fur Dëhn Stekëhn West.mp3"), "Hortz Fur Dëhn Stekëhn West", "9:34");
        release.AddTrack(MusicPath("02. Ïma Sürï Dondaï.mp3"), "Ïma Sürï Dondaï", "4:28");
        release.AddTrack(MusicPath("03. Kobaïa Is De Hündïn.mp3"), "Kobaïa Is De Hündïn", "3:35");
        _source.Add(release);

        release = await ReleaseModel.Create("CAN", "Tago Mago", ImagePath("Tago Mago-front.jpg"), "D", 1971).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Paperhouse.mp3"), "Paperhouse", "7:28");
        release.AddTrack(MusicPath("04. Halleluhwah.mp3"), "Halleluhwah", "18:28");
        _source.Add(release);

        release = await ReleaseModel.Create("Amon Düül II", "Phallus Dei", ImagePath("Phallus-front.jpg"), "D", 1969).ConfigureAwait(false);
        release.AddTrack(MusicPath("03. Luzifers Ghilom.mp3"), "Luzifers Ghilom", "8:34");
        release.AddTrack(MusicPath("05. Phallus Dei.mp3"), "Phallus Dei", "20:46");
        _source.Add(release);

        release = await ReleaseModel.Create("Joe McPhee", "Nation Time", ImagePath("Nation Time-front.jpg"), "D", 1970).ConfigureAwait(false);
        release.AddTrack(MusicPath("01 - Nation Time.mp3"), "Nation Time", "18:36");
        release.AddTrack(MusicPath("02 - Shakey Jake.mp3"), "Shakey Jake", "13:39");
        _source.Add(release);

        release = await ReleaseModel.Create("Area", "Arbeit macht frei", ImagePath("Arbeit-front.jpg"), "ITA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Luglio, Agosto, Settembre (Nero).mp3"), "Luglio, Agosto, Settembre (Nero)", "4:27");
        release.AddTrack(MusicPath("02. Arbeit Macht Frei.mp3"), "Arbeit Macht Frei", "8:00");
        release.AddTrack(MusicPath("04. Le Labbra Del Tempo.mp3"), "Le Labbra Del Tempo", "6:04");
        _source.Add(release);

        release = await ReleaseModel.Create("King Crimson", "In the Court of the Crimson King", ImagePath("Court-front.jpg"), "UK", 1969).ConfigureAwait(false);
        release.AddTrack(MusicPath("02 - I Talk to the Wind.mp3"), "I Talk to the Wind", "6:03");
        release.AddTrack(MusicPath("03 - Epitaph - March for No Reason - Tomorrow and Tomorrow.mp3"), "Epitaph - March for No Reason - Tomorrow and Tomorrow", "8:51");
        _source.Add(release);

        release = await ReleaseModel.Create("Opeth", "Damnation", ImagePath("Damnation-front.jpg"), "SWE", 2003).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Windowpane.mp3"), "Windowpane", "7:44");
        release.AddTrack(MusicPath("02. In My Time Of Need.mp3"), "In My Time Of Need", "5:46");
        release.AddTrack(MusicPath("06. To Rid The Disease.mp3"), "To Rid The Disease", "6:21");
        release.AddTrack(MusicPath("08. Weakness.mp3"), "Weakness", "4:15");
        _source.Add(release);

        release = await ReleaseModel.Create("Opeth", "Blackwater Park", ImagePath("Blackwater-front.jpg"), "SWE", 2001).ConfigureAwait(false);
        release.AddTrack(MusicPath("02. Bleak.mp3"), "Bleak", "9:15");
        release.AddTrack(MusicPath("04. The Drapery Falls.mp3"), "The Drapery Falls", "10:53");
        _source.Add(release);

        release = await ReleaseModel.Create("Capitolo 6", "Frutti per Kagua", ImagePath("Frutti-front.jpg"), "ITA", 1972).ConfigureAwait(false);
        release.AddTrack(MusicPath("01.Frutti per Kagua.mp3"), "Frutti per Kagua", "18:28");
        release.AddTrack(MusicPath("04.L'ultima notte.mp3"), "L'ultima notte", "11:35");
        _source.Add(release);

        release = await ReleaseModel.Create("Deep Purple", "Who Do We Think We Are", ImagePath("Who-front.jpg"), "UK", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Woman From Tokyo.mp3"), "Woman From Tokyo", "5:52");
        release.AddTrack(MusicPath("05. Rat Bat Blue.mp3"), "Rat Bat Blue", "5:26");
        release.AddTrack(MusicPath("06. Place In Line.mp3"), "Place In Line", "6:33");
        _source.Add(release);

        release = await ReleaseModel.Create("Savatage", "Gutter Ballet", ImagePath("Gutter-front.jpg"), "USA", 1989).ConfigureAwait(false);
        release.AddTrack(MusicPath("02 Gutter Ballet.mp3"), "Gutter Ballet", "6:20");
        release.AddTrack(MusicPath("04 When The Crowds Are Gone.mp3"), "When The Crowds Are Gone", "5:46");
        release.AddTrack(MusicPath("07 Hounds.mp3"), "Hounds", "6:28");
        release.AddTrack(MusicPath("12 All That I Bleed (Bonus track).mp3"), "All That I Bleed (Piano Version)", "4:35");
        _source.Add(release);

        release = await ReleaseModel.Create("Bubu", "Anabelas", ImagePath("Anabelas-front.jpg"), "ARG", 1978).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. El Cortejo De Un Dia Amarillo.mp3"), "El Cortejo De Un Dia Amarillo", "19:23");
        release.AddTrack(MusicPath("02. El Viaje De Anabelas.mp3"), "El Viaje De Anabelas", "11:12");
        _source.Add(release);

        release = await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Banco Del Mutuo Soccorso", ImagePath("Banco-front.jpg"), "ITA", 1972).ConfigureAwait(false);
        release.AddTrack(MusicPath("01  In Volo .mp3"), "In Volo", "2:14");
        release.AddTrack(MusicPath("02  R.I.P. (Requiescant In Pace) .mp3"), "R.I.P. (Requiescant In Pace)", "6:41");
        release.AddTrack(MusicPath("03  Passaggio .mp3"), "Passaggio", "1:19");
        _source.Add(release);

        release = await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Io Sono Nato Libero", ImagePath("Io Sono-front.jpg"), "ITA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01  Canto nomade per un prigionero politico.mp3"), "Canto nomade per un prigionero politico", "15:47");
        release.AddTrack(MusicPath("02  Non mi rompete.mp3"), "Non mi rompete", "5:04");
        _source.Add(release);

        release = await ReleaseModel.Create("Osanna", "L'Uomo", ImagePath("Osanna-front.jpg"), "ITA", 1971).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Introduzione.mp3"), "Introduzione", "3:27");
        release.AddTrack(MusicPath("02. L'Uomo.mp3"), "L'Uomo", "3:33");
        release.AddTrack(MusicPath("06. In Un Vecchio Cieco.mp3"), "In Un Vecchio Cieco", "3:30");
        release.AddTrack(MusicPath("09. Lady Power.mp3"), "Lady Power", "3:55");
        _source.Add(release);

        release = await ReleaseModel.Create("Osanna", "Palepoli", ImagePath("Palepoli-front.jpg"), "ITA", 1973).ConfigureAwait(false);
        release.AddTrack(MusicPath("01. Oro Caldo.mp3"), "Oro Caldo", "18:31");
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

    //TODO: Check if file exists
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