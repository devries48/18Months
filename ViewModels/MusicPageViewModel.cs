using Months18.Controls;

namespace Months18.ViewModels;

public partial class MusicPageViewModel : ObservableObject
{
    public MusicPageViewModel(ISettingsService settingsService, IAudioPlayerService playerService)
    {
        _settingsService = settingsService;
        _playerService = playerService;
        _source = [];
        _playerService.SubscribeToMediaStateChanged(OnMediaStateChanged);
        LevelValue = Prefernces.Level;
    }

    private readonly ISettingsService _settingsService;
    private readonly IAudioPlayerService _playerService;
    private readonly List<ReleaseModel> _source;
    private TrackModel? _selectedTrack;

    public const int DefaultItemWidth = 200;

    [ObservableProperty]
    private int _span = 1;

    [ObservableProperty]
    private ObservableCollection<ReleaseModel> _releases = [];

    [ObservableProperty]
    private ReleaseModel? _selectedRelease;

    private double _levelValue = -1;
    public double LevelValue
    {
        get => _levelValue;
        set
        {
            if (!EqualityComparer<double>.Default.Equals(_levelValue, value))
            {
                _levelValue = value;
                OnPropertyChanged(nameof(LevelValue));

                var lvl = (int)Math.Round( _levelValue);
                if (SelectedLevel == null || lvl != SelectedLevel.Level)
                {
                    SelectedLevel = LevelModel.Create(lvl);
                    _ = GetLocalReleasesAsync().ConfigureAwait(false);
                }
            }
        }
    }

    [ObservableProperty]
    private LevelModel? _selectedLevel;

    [ObservableProperty]
    private bool _isPlaying;

    [ObservableProperty]
    private bool _isDetailViewVisible;

    [ObservableProperty]
    private AnimationType _detailsVisibleAnimation = AnimationType.Fading;

    [ObservableProperty]
    private AnimationType _releasesVisibleAnimation = AnimationType.Scaling;

    [RelayCommand]
    private void ChangeSpan(int byAmount)
    {
        if (Span + byAmount <= 0)   //Prevent span from being <= 0.
        {
            Span = 1;
            return;
        }

        Span += byAmount;
    }

    [RelayCommand]
    private void ReleaseTap(ReleaseModel tappedItem)
    {
        if (SelectedRelease != null && SelectedRelease != tappedItem)
            SelectedRelease.IsSelected = false;

        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            SelectedRelease = null;
        }
        else
        {
            SelectedRelease = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void TrackTap(TrackModel tappedItem)
    {
        if (_selectedTrack != null && _selectedTrack != tappedItem)
            _selectedTrack.IsSelected = false;

        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            _selectedTrack = null;
        }
        else
        {
            _selectedTrack = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void PlaySelectedRelease()
    {
        if (SelectedRelease == null || _playerService == null)
            return;

        if (_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if (SelectedRelease.Title == (_playerService.CurrentTrack?.ReleaseTitle ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayRelease(SelectedRelease);
    }

    [RelayCommand]
    private void PlaySelectedTrack()
    {
        if (SelectedRelease == null || _playerService == null)
            return;

        if (_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if (_selectedTrack?.Title == (_playerService.CurrentTrack?.Title ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayTrack(_selectedTrack);
    }

    [RelayCommand]
    private void ShowSelectedRelease()
    {
        if (SelectedRelease == null || _playerService == null)
            return;

        IsDetailViewVisible = true;
    }

    [RelayCommand] private void HideSelectedRelease() => IsDetailViewVisible = false;

    public async Task GetLocalReleasesAsync()
    {
        if (_source.Count == 0)
        {
            var release = await ReleaseModel.Create("Charles Mingus", "Blues & Roots", ImagePath("Blues Roots-front.jpg"), CountryCode.USA, 1960, 2).ConfigureAwait(false);
            release.AddGenres("Post-Bop", "Hard Bop", "Avant-Garde Jazz");
            release.AddTrack(MusicPath("01. Wednesday Night Prayer Meeting.mp3"), "Wednesday Night Prayer Meeting", "5:43");
            release.AddTrack(MusicPath("03. Moanin'.mp3"), "Moanin'", "8:03");
            release.AddTrack(MusicPath("06. E's Flat Ah's Flat Too.mp3"), "E's Flat Ah's Flat Too", "6:47");
            _source.Add(release);

            release = await ReleaseModel.Create("Herbie Hancock", "Head Hunters", ImagePath("Head Hunters-front.jpg"), CountryCode.USA, 1973, 2).ConfigureAwait(false);
            release.AddGenres("Jazz Fusion", "Jazz-Funk", "Post-Bop", "Hard Bop", "Electro", "Jazz");
            release.AddTrack(MusicPath("01. Chameleon.mp3"), "Chameleon", "15:44");
            release.AddTrack(MusicPath("02. Watermelon Man.mp3"), "Watermelon Man", "6:31");
            release.AddTrack(MusicPath("03. Sly.mp3"), "Sly", "10:21");
            _source.Add(release);

            release = await ReleaseModel.Create("Miles Davis", "Bitches Brew", ImagePath("Bitches Brew-front.jpg"), CountryCode.USA, 1970, 3).ConfigureAwait(false);
            release.AddGenres("Jazz Fusion", "Avant-Garde Jazz", "Jazz-Rock", "Jazz-Funk");
            release.AddTrack(MusicPath("01. Spanish Key.mp3"), "Spanish Key", "17:29");
            release.AddTrack(MusicPath("03. Miles Runs The Voodoo Down.mp3"), "Miles Runs The Voodoo Down", "14:04");
            _source.Add(release);

            release = await ReleaseModel.Create("Jameszoo & Metropol Orkest", "Melkweg", ImagePath("Melkweg-front.jpg"), CountryCode.NLD, 2019, 3).ConfigureAwait(false);
            release.AddGenres("Experimental Big Band", "Nu Jazz", "Jazz Fusion");
            release.AddTrack(MusicPath("03. (toots).mp3"), "(toots)", "5:24");
            release.AddTrack(MusicPath("04. (soup).mp3"), "(soup)", "6:36");
            release.AddTrack(MusicPath("07. (meat).mp3"), "(meat)", "8:54");
            _source.Add(release);

            release = await ReleaseModel.Create("Ozzy Osbourne", "No More Tears", ImagePath("No More Tears-front.jpg"), CountryCode.GBR, 1991, 1).ConfigureAwait(false);
            release.AddGenres("Heavy Metal", "Hard Rock");
            release.AddTrack(MusicPath("03. Mama, I'm Coming Home.mp3"), "Mama, I'm Coming Home", "4:12");
            release.AddTrack(MusicPath("05. No More Tears.mp3"), "No More Tears", "7:24");
            release.AddTrack(MusicPath("11. Road To Nowhere.mp3"), "Road To Nowhere", "5:11");
            _source.Add(release);

            release = await ReleaseModel.Create("King Crimson", "Red", ImagePath("Red-front.jpg"), CountryCode.GBR, 1974, 2).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Art Rock", "Symphonic Prog", "Jazz-Rock", "Hard Rock");
            release.AddTrack(MusicPath("02 - Fallen Angel.mp3"), "Fallen Angel", "6:01");
            release.AddTrack(MusicPath("05 - Starless.mp3"), "Starless", "12:18");
            _source.Add(release);

            release = await ReleaseModel.Create("Fifty Foot Hose", "Cauldron", ImagePath("Cauldron-front.jpg"), CountryCode.USA, 1968, 1).ConfigureAwait(false);
            release.AddGenres("Psychedelic Rock", "Experimental Rock", "Acid Rock");
            release.AddTrack(MusicPath("02 If Not This Time.mp3"), "If Not This Time", "3:38");
            release.AddTrack(MusicPath("04 The Things That Concern You.mp3"), "The Things That Concern You", "3:29");
            release.AddTrack(MusicPath("06 Red The Sign Post.mp3"), "Red The Sign Post", "2:59");
            release.AddTrack(MusicPath("08 Rose.mp3"), "Rose", "5:09");
            release.AddTrack(MusicPath("10 God Bless The Child.mp3"), "God Bless The Child", "2:51");
            _source.Add(release);

            release = await ReleaseModel.Create("CAN", "Soundtracks", ImagePath("Soundtracks-front.jpg"), CountryCode.DEU, 1973, 2).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Psychedelic Rock");
            release.AddTrack(MusicPath("02. Tango Whiskyman (From Deadlock).mp3"), "Tango Whiskyman (From Deadlock)", "4:03");
            release.AddTrack(MusicPath("04. Don't Turn The Light On, Leave Me Alone (From Cream).mp3"), "Don't Turn The Light On, Leave Me Alone (From Cream)", "3:42");
            release.AddTrack(MusicPath("06. Mother Sky (From Deep End).mp3"), "Mother Sky (From Deep End)", "14:28");
            release.AddTrack(MusicPath("07. She Brings The Rain (From Bottom - Ein Großer Graublauer Vogel).mp3"), "She Brings The Rain (From Bottom - Ein Großer Graublauer Vogel)", "4:05");
            _source.Add(release);

            release = await ReleaseModel.Create("Amon Düül II", "Yeti", ImagePath("Yeti-front.jpg"), CountryCode.DEU, 1970, 3).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Psychedelic Rock", "Progressive Rock", "Heavy Psych", "Experimental Rock");
            release.AddTrack(MusicPath("01. Soap Shop Rock a) Burning Sister.mp3"), "Soap Shop Rock a) Burning Sister", "3:45");
            release.AddTrack(MusicPath("02. Soap Shop Rock b) Halluzination Guillotine.mp3"), "Soap Shop Rock b) Halluzination Guillotine", "3:10");
            release.AddTrack(MusicPath("03. Soap Shop Rock c) Gulp A Sonata.mp3"), "Soap Shop Rock c) Gulp A Sonata", "0:46");
            release.AddTrack(MusicPath("04. Soap Shop Rock d) Flesh-Coloured Anti-Aircraft Alarm.mp3"), "Soap Shop Rock d) Flesh-Coloured Anti-Aircraft Alarm", "6:04");
            _source.Add(release);

            release = await ReleaseModel.Create("CAN", "Ege Bamyasi", ImagePath("Ege Bamyasi-front.jpg"), CountryCode.DEU, 1972, 1).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Psychedelic Rock");
            release.AddTrack(MusicPath("03. One More Night.mp3"), "One More Night", "5:35");
            release.AddTrack(MusicPath("04. Vitamin C.mp3"), "Vitamin C", "3:32");
            release.AddTrack(MusicPath("06. I'm So Green.mp3"), "I'm So Green", "3:05");
            _source.Add(release);

            release = await ReleaseModel.Create("Magma", "Kobaïa", ImagePath("Kobaia-front.jpg"), CountryCode.FRA, 1970, 4).ConfigureAwait(false);
            release.AddGenres("Zeuhl", "Jazz-Rock");
            release.AddTrack(MusicPath("1.01. Kobaïa.mp3"), "Kobaïa", "10:09");
            release.AddTrack(MusicPath("1.02. Aïna.mp3"), "Aïna", "6:15");
            release.AddTrack(MusicPath("1.03. Malaria.mp3"), "Malaria", "4:21");
            release.AddTrack(MusicPath("2.04. Müh.mp3"), "Müh", "11:17");
            _source.Add(release);

            release = await ReleaseModel.Create("Catapilla", "Catapilla", ImagePath("Catapilla-front.jpg"), CountryCode.GBR, 1971, 3).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Jazz-Rock", "Psychedelic Rock");
            release.AddTrack(MusicPath("01. Naked Death.mp3"), "Naked Death", "15:42");
            _source.Add(release);

            release = await ReleaseModel.Create("Magma", "Mekanïk Destruktïẁ Kommandöh", ImagePath("Mdk-front.jpg"), CountryCode.FRA, 1973, 4).ConfigureAwait(false);
            release.AddGenres("Zeuhl", "Jazz-Rock", "Post-Minimalism");
            release.AddTrack(MusicPath("01. Hortz Fur Dëhn Stekëhn West.mp3"), "Hortz Fur Dëhn Stekëhn West", "9:34");
            release.AddTrack(MusicPath("02. Ïma Sürï Dondaï.mp3"), "Ïma Sürï Dondaï", "4:28");
            release.AddTrack(MusicPath("03. Kobaïa Is De Hündïn.mp3"), "Kobaïa Is De Hündïn", "3:35");
            _source.Add(release);

            release = await ReleaseModel.Create("CAN", "Tago Mago", ImagePath("Tago Mago-front.jpg"), CountryCode.DEU, 1971, 2).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Psychedelic Rock", "Experimental Rock");
            release.AddTrack(MusicPath("01. Paperhouse.mp3"), "Paperhouse", "7:28");
            release.AddTrack(MusicPath("04. Halleluhwah.mp3"), "Halleluhwah", "18:28");
            _source.Add(release);

            release = await ReleaseModel.Create("Amon Düül II", "Phallus Dei", ImagePath("Phallus-front.jpg"), CountryCode.DEU, 1969, 4).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Psychedelic Rock", "Progressive Rock");
            release.AddTrack(MusicPath("03. Luzifers Ghilom.mp3"), "Luzifers Ghilom", "8:34");
            release.AddTrack(MusicPath("05. Phallus Dei.mp3"), "Phallus Dei", "20:46");
            _source.Add(release);

            release = await ReleaseModel.Create("Joe McPhee", "Nation Time", ImagePath("Nation Time-front.jpg"), CountryCode.USA, 1970, 4).ConfigureAwait(false);
            release.AddGenres("Avant-Garde Jazz", "Jazz-Funk", "Free Jazz");
            release.AddTrack(MusicPath("01 - Nation Time.mp3"), "Nation Time", "18:36");
            release.AddTrack(MusicPath("02 - Shakey Jake.mp3"), "Shakey Jake", "13:39");
            _source.Add(release);

            release = await ReleaseModel.Create("Area", "Arbeit macht frei", ImagePath("Arbeit-front.jpg"), CountryCode.ITA, 1973, 4).ConfigureAwait(false);
            release.AddGenres("Avant-Prog", "Jazz-Rock", "Progressive Rock", "Jazz Fusion");
            release.AddTrack(MusicPath("01. Luglio, Agosto, Settembre (Nero).mp3"), "Luglio, Agosto, Settembre (Nero)", "4:27");
            release.AddTrack(MusicPath("02. Arbeit Macht Frei.mp3"), "Arbeit Macht Frei", "8:00");
            release.AddTrack(MusicPath("04. Le Labbra Del Tempo.mp3"), "Le Labbra Del Tempo", "6:04");
            _source.Add(release);

            release = await ReleaseModel.Create("King Crimson", "In the Court of the Crimson King", ImagePath("Court-front.jpg"), CountryCode.GBR, 1969, 1).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Art Rock", "Symphonic Prog", "Jazz-Rock", "Psychedelic Rock");
            release.AddTrack(MusicPath("02 - I Talk to the Wind.mp3"), "I Talk to the Wind", "6:03");
            release.AddTrack(MusicPath("03 - Epitaph - March for No Reason - Tomorrow and Tomorrow.mp3"), "Epitaph - March for No Reason - Tomorrow and Tomorrow", "8:51");
            _source.Add(release);

            release = await ReleaseModel.Create("Miles Davis", "Ascenseur pour l'échafaud", ImagePath("Ascenseur-front.jpg"), CountryCode.USA, 1958, 1).ConfigureAwait(false);
            release.AddGenres("Cool Jazz", "Dark Jazz", "Film Score");
            release.AddTrack(MusicPath("01. Generique.mp3"), "Generique", "2:48");
            release.AddTrack(MusicPath("02. L'Assasinat De Carala.mp3"), "L'Assasinat De Carala", "2:10");
            release.AddTrack(MusicPath("03. Sur L'Autoroute.mp3"), "Sur L'Autoroute", "2:18");
            release.AddTrack(MusicPath("04. Julien Dans L'Ascenseur.mp3"), "Julien Dans L'Ascenseur", "2:10");
            release.AddTrack(MusicPath("05. Florence Sur Les Champs Elysees.mp3"), "Florence Sur Les Champs Elysees", "2:51");
            _source.Add(release);

            release = await ReleaseModel.Create("Led Zeppelin", "Houses of the Holy", ImagePath("Houses-front.jpg"), CountryCode.GBR, 1973, 1).ConfigureAwait(false);
            release.AddGenres("Hard Rock", "Folk Rock", "Blues Rock", "Progressive Rock");
            release.AddTrack(MusicPath("02. The Rain Song.mp3"), "The Rain Song", "7:39");
            release.AddTrack(MusicPath("07. No Quarter.mp3"), "No Quarter", "7:03");
            _source.Add(release);

            release = await ReleaseModel.Create("Opeth", "Damnation", ImagePath("Damnation-front.jpg"), CountryCode.SWE, 2003, 1).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Contemporary Folk");
            release.AddTrack(MusicPath("01. Windowpane.mp3"), "Windowpane", "7:44");
            release.AddTrack(MusicPath("02. In My Time Of Need.mp3"), "In My Time Of Need", "5:46");
            release.AddTrack(MusicPath("06. To Rid The Disease.mp3"), "To Rid The Disease", "6:21");
            release.AddTrack(MusicPath("08. Weakness.mp3"), "Weakness", "4:15");
            _source.Add(release);

            release = await ReleaseModel.Create("Opeth", "Blackwater Park", ImagePath("Blackwater-front.jpg"), CountryCode.SWE, 2001, 4).ConfigureAwait(false);
            release.AddGenres("Progressive Metal", "Death Metal", "Progressive Rock");
            release.AddTrack(MusicPath("02. Bleak.mp3"), "Bleak", "9:15");
            release.AddTrack(MusicPath("04. The Drapery Falls.mp3"), "The Drapery Falls", "10:53");
            _source.Add(release);

            release = await ReleaseModel.Create("Opeth", "Ghost Reveries", ImagePath("Ghost-front.jpg"), CountryCode.SWE, 2005, 4).ConfigureAwait(false);
            release.AddGenres("Progressive Metal", "Death Metal", "Progressive Rock");
            release.AddTrack(MusicPath("01. Ghost Of Perdition.mp3"), "Ghost Of Perdition", "10:29");
            release.AddTrack(MusicPath("02. The Baying Of The Hounds.mp3"), "The Baying Of The Hounds", "10:41");
            release.AddTrack(MusicPath("08. Isolation Years.mp3"), "Isolation Years", "3:51");
            _source.Add(release);

            release = await ReleaseModel.Create("Capitolo 6", "Frutti per Kagua", ImagePath("Frutti-front.jpg"), CountryCode.ITA, 1972, 3).ConfigureAwait(false);
            release.AddGenres("Progressive Rock");
            release.AddTrack(MusicPath("01.Frutti per Kagua.mp3"), "Frutti per Kagua", "18:28");
            release.AddTrack(MusicPath("04.L'ultima notte.mp3"), "L'ultima notte", "11:35");
            _source.Add(release);

            release = await ReleaseModel.Create("Deep Purple", "Who Do We Think We Are", ImagePath("Who-front.jpg"), CountryCode.GBR, 1973, 1).ConfigureAwait(false);
            release.AddGenres("Hard Rock", "Blues Rock");
            release.AddTrack(MusicPath("01. Woman From Tokyo.mp3"), "Woman From Tokyo", "5:52");
            release.AddTrack(MusicPath("05. Rat Bat Blue.mp3"), "Rat Bat Blue", "5:26");
            release.AddTrack(MusicPath("06. Place In Line.mp3"), "Place In Line", "6:33");
            _source.Add(release);

            release = await ReleaseModel.Create("Savatage", "Gutter Ballet", ImagePath("Gutter-front.jpg"), CountryCode.USA, 1989, 3).ConfigureAwait(false);
            release.AddGenres("Heavy Metal", "Progressive Metal");
            release.AddTrack(MusicPath("02 Gutter Ballet.mp3"), "Gutter Ballet", "6:20");
            release.AddTrack(MusicPath("04 When The Crowds Are Gone.mp3"), "When The Crowds Are Gone", "5:46");
            release.AddTrack(MusicPath("07 Hounds.mp3"), "Hounds", "6:28");
            release.AddTrack(MusicPath("12 All That I Bleed (Bonus track).mp3"), "All That I Bleed (Piano Version)", "4:35");
            _source.Add(release);

            release = await ReleaseModel.Create("Bubu", "Anabelas", ImagePath("Anabelas-front.jpg"), CountryCode.ARG, 1978, 3).ConfigureAwait(false);
            release.AddGenres("Progressive Rock");
            release.AddTrack(MusicPath("01. El Cortejo De Un Dia Amarillo.mp3"), "El Cortejo De Un Dia Amarillo", "19:23");
            release.AddTrack(MusicPath("02. El Viaje De Anabelas.mp3"), "El Viaje De Anabelas", "11:12");
            _source.Add(release);

            release = await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Banco Del Mutuo Soccorso", ImagePath("Banco-front.jpg"), CountryCode.ITA, 1972, 2).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock", "Progressive Folk", "Hard Rock");
            release.AddTrack(MusicPath("01  In Volo .mp3"), "In Volo", "2:14");
            release.AddTrack(MusicPath("02  R.I.P. (Requiescant In Pace) .mp3"), "R.I.P. (Requiescant In Pace)", "6:41");
            release.AddTrack(MusicPath("03  Passaggio .mp3"), "Passaggio", "1:19");
            _source.Add(release);

            release = await ReleaseModel.Create("Banco Del Mutuo Soccorso", "Io Sono Nato Libero", ImagePath("Io Sono-front.jpg"), CountryCode.ITA, 1973, 2).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock", "Progressive Folk");
            release.AddTrack(MusicPath("01  Canto nomade per un prigionero politico.mp3"), "Canto nomade per un prigionero politico", "15:47");
            release.AddTrack(MusicPath("02  Non mi rompete.mp3"), "Non mi rompete", "5:04");
            _source.Add(release);

            release = await ReleaseModel.Create("Osanna", "L'Uomo", ImagePath("Osanna-front.jpg"), CountryCode.ITA, 1971, 2).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Symphonic Prog", "Hard Rock", "Psychedelic Rock");
            release.AddTrack(MusicPath("01. Introduzione.mp3"), "Introduzione", "3:27");
            release.AddTrack(MusicPath("02. L'Uomo.mp3"), "L'Uomo", "3:33");
            release.AddTrack(MusicPath("06. In Un Vecchio Cieco.mp3"), "In Un Vecchio Cieco", "3:30");
            release.AddTrack(MusicPath("09. Lady Power.mp3"), "Lady Power", "3:55");
            _source.Add(release);

            release = await ReleaseModel.Create("Osanna", "Palepoli", ImagePath("Palepoli-front.jpg"), CountryCode.ITA, 1973, 3).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Symphonic Prog", "Hard Rock", "Jazz-Rock", "Italian Folk Music");
            release.AddTrack(MusicPath("01. Oro Caldo.mp3"), "Oro Caldo", "18:31");
            _source.Add(release);

            release = await ReleaseModel.Create("Quella Vecchia Locanda", "Il tempo della gioia", ImagePath("Tempo-front.jpg"), CountryCode.ITA, 1974, 2).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock", "Chamber Music");
            release.AddTrack(MusicPath("01.Villa Doria Pamphili.mp3"), "Villa Doria Pamphili", "5:27");
            release.AddTrack(MusicPath("04.Un giorno, un amico.mp3"), "Un giorno, un amico", "9:40");
            release.AddTrack(MusicPath("05.È accaduto una notte.mp3"), "È accaduto una notte", "8:17");
            _source.Add(release);

            release = await ReleaseModel.Create("Premiata Forneria Marconi", "Per un amico", ImagePath("Amico-front.jpg"), CountryCode.ITA, 1972, 2).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock");
            release.AddTrack(MusicPath("01  Appena Un Poco .mp3"), "Appena Un Poco", "7:46");
            release.AddTrack(MusicPath("02  Generale! .mp3"), "Generale!", "4:18");
            release.AddTrack(MusicPath("03  Per Un Amico .mp3"), "Per Un Amico", "5:24");
            _source.Add(release);

            release = await ReleaseModel.Create("Charles Mingus", "Mingus Ah Um", ImagePath("Mingus-front.jpg"), CountryCode.USA, 1959, 1).ConfigureAwait(false);
            release.AddGenres("Post-Bop", "Hard Bop", "Cool Jazz");
            release.AddTrack(MusicPath("01. Better Git it in Your Soul.mp3"), "Better Git it in Your Soul", "7:22");
            release.AddTrack(MusicPath("02. Goodbye Pork Pie Hat.mp3"), "Goodbye Pork Pie Hat", "4:47");
            release.AddTrack(MusicPath("03. Boogie Stop Shuffle.mp3"), "Boogie Stop Shuffle", "3:44");
            _source.Add(release);

            release = await ReleaseModel.Create("Mercyful Fate", "Don't Break the Oath", ImagePath("Oath-front.jpg"), CountryCode.DNK, 1984, 3).ConfigureAwait(false);
            release.AddGenres("Heavy Metal");
            release.AddTrack(MusicPath("01 - A Dangerous Meeting.mp3"), "A Dangerous Meeting", "5:09");
            release.AddTrack(MusicPath("02 - Nightmare.mp3"), "Nightmare", "6:18");
            _source.Add(release);

            release = await ReleaseModel.Create("Camel", "Mirage", ImagePath("Mirage-front.jpg"), CountryCode.GBR, 1974, 2).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock", "Canterbury Scene");
            release.AddTrack(MusicPath("03. Nimrodel-The Procession-The White Rider.mp3"), "Nimrodel-The Procession-The White Rider", "9:18");
            release.AddTrack(MusicPath("05. Lady Fantasy-Encounter, Smiles For You, Lady Fantasy.mp3"), "Lady Fantasy-Encounter, Smiles For You, Lady Fantasy", "12:45");
            _source.Add(release);

            release = await ReleaseModel.Create("Koenjihyakkei", "Angherr Shisspa", ImagePath("Angherr Shisspa-front.jpg"), CountryCode.JPN, 2005, 4).ConfigureAwait(false);
            release.AddGenres("Zeuhl", "Brutal Prog");
            release.AddTrack(MusicPath("01 - Tziidall Raszhisst.mp3"), "Tziidall Raszhisst", "7:13");
            release.AddTrack(MusicPath("02 - Rattims Friezz.mp3"), "Rattims Friezz", "7:01");
            release.AddTrack(MusicPath("07 - Angherr Shisspa.mp3"), "Angherr Shisspa", "6:34");
            _source.Add(release);

            release = await ReleaseModel.Create("Kollektiv", "Kollektiv", ImagePath("Kollektiv-front.jpg"), CountryCode.DEU, 1973, 3).ConfigureAwait(false);
            release.AddGenres("Krautrock", "Jazz-Rock", "Psychedelic Rock");
            release.AddTrack(MusicPath("01  Rambo Zambo .mp3"), "Rambo Zambo", "11:42");
            release.AddTrack(MusicPath("04  Gageg .mp3"), "Gageg", "19:58");
            _source.Add(release);

            release = await ReleaseModel.Create("Brainstorm", "Smile a While", ImagePath("Smile-front.jpg"), CountryCode.DEU, 1972, 3).ConfigureAwait(false);
            release.AddGenres("Jazz-Rock", "Krautrock", "Progressive Rock", "Canterbury Scene");
            release.AddTrack(MusicPath("01-Das Schwein Trugt.mp3"), "Das Schwein Trugt", "4:37");
            release.AddTrack(MusicPath("02-Zwick Zwick.mp3"), "Zwick Zwick", "4:37");
            release.AddTrack(MusicPath("05-Snakeskin Tango.mp3"), "Snakeskin Tango", "2:17");
            release.AddTrack(MusicPath("06-Smile A While.mp3"), "Smile A While", "15:31");
            _source.Add(release);

            release = await ReleaseModel.Create("Dün", "Eros", ImagePath("Eros-front.jpg"), CountryCode.FRA, 1981, 4).ConfigureAwait(false);
            release.AddGenres("Avant-Prog", "Zeuhl", "Jazz Fusion");
            release.AddTrack(MusicPath("04 - Eros.mp3"), "Eros", "10:29");
            release.AddTrack(MusicPath("08 - Eros (version alternative).mp3"), "Eros (version alternative)", "7:16");
            _source.Add(release);

            release = await ReleaseModel.Create("Arachnoïd", "Arachnoïd", ImagePath("Spin-front.jpg"), CountryCode.FRA, 1979, 4).ConfigureAwait(false);
            release.AddGenres("Progressive Rock", "Zeuhl", "Avant-Prog");
            release.AddTrack(MusicPath("01 - Le chamadere.mp3"), "Le chamadere", "13:51");
            release.AddTrack(MusicPath("02 - Piano caveau.mp3"), "Piano caveau", "7:18");
            _source.Add(release);

            release = await ReleaseModel.Create("Charles Mingus", "The Black Saint and the Sinner Lady", ImagePath("Saint-front.jpg"), CountryCode.USA, 1963, 3).ConfigureAwait(false);
            release.AddGenres("Avant-Garde Jazz");
            release.AddTrack(MusicPath("01. Track A - Solo Dancer.mp3"), "Track A - Solo Dancer", "6:39");
            release.AddTrack(MusicPath("02. Track B - Duet Solo Dancers.mp3"), "Track B - Duet Solo Dancers", "6:46");
            release.AddTrack(MusicPath("03. Track C - Group Dancers.mp3"), "Track C - Group Dancers", "7:23");
            release.AddTrack(MusicPath("04. Mode D - Trio and Group Dancers + Mode E - Single Solos and Group Dance + Mod.mp3"), "Mode D - Trio and Group Dancers/Mode E - Single Solos and Group Dance", "18:38");
            _source.Add(release);

            release = await ReleaseModel.Create("Änglagård", "Hybris", ImagePath("Hybris-front.jpg"), CountryCode.SWE, 1992, 3).ConfigureAwait(false);
            release.AddGenres("Symphonic Prog", "Progressive Rock", "Progressive Folk");
            release.AddTrack(MusicPath("02 - Vandringar i vilsenhet.mp3"), "Vandringar i vilsenhet", "11:57");
            release.AddTrack(MusicPath("04 - Kung Bore.mp3"), "Kung Bore", "12:57");
            _source.Add(release);

            release = await ReleaseModel.Create("Pharoah Sanders", "Karma", ImagePath("Karma-front.jpg"), CountryCode.USA, 1969, 4).ConfigureAwait(false);
            release.AddGenres("Spiritual Jazz", "Avant-Garde Jazz", "Free Jazz");
            release.AddTrack(MusicPath("01 - The Creator Has a Master Plan.mp3"), "The Creator Has a Master Plan", "32:47");
            _source.Add(release);
        }

        if (SelectedLevel?.Level > 0)
        {
            var list = _source.Where(r => r.Level == SelectedLevel.Level).OrderBy(r=>r.Artist).ThenBy(r=>r.Year).ToList();
            Releases = new ObservableCollection<ReleaseModel>(list);
        }
        else
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
