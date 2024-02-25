using System.Text.Json;
using System.Text.Json.Serialization;

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
    private CancellationTokenSource? _cancelGetReleases;

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
            if(!EqualityComparer<double>.Default.Equals(_levelValue, value))
            {
                _levelValue = value;
                OnPropertyChanged(nameof(LevelValue));

                var lvl = (int)Math.Round(_levelValue);
                if(SelectedLevel == null || lvl != SelectedLevel.Level)
                {
                    SelectedLevel = LevelModel.Create(lvl);
                    Prefernces.Level = lvl;

                    _cancelGetReleases?.Cancel();
                    _cancelGetReleases = new CancellationTokenSource();
                    _ = DelayGetLocalReleasesAsync(_cancelGetReleases.Token);
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
        if(Span + byAmount <= 0)   //Prevent span from being <= 0.
        {
            Span = 1;
            return;
        }

        Span += byAmount;
    }

    [RelayCommand]
    private void ReleaseTap(ReleaseModel tappedItem)
    {
        if(SelectedRelease != null && SelectedRelease != tappedItem)
            SelectedRelease.IsSelected = false;

        if(tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            SelectedRelease = null;
        } else
        {
            SelectedRelease = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void TrackTap(TrackModel tappedItem)
    {
        if(_selectedTrack != null && _selectedTrack != tappedItem)
            _selectedTrack.IsSelected = false;

        if(tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            _selectedTrack = null;
        } else
        {
            _selectedTrack = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void AddSelectedReleaseToPlaylist()
    {
        if(SelectedRelease == null || _playerService == null)
            return;

        _playerService.AddToPlaylist(SelectedRelease);
    }

    [RelayCommand]
    private void AddSelectedTrackToPlaylist()
    {
        if(_selectedTrack == null || _playerService == null)
            return;

        _playerService.AddToPlaylist(_selectedTrack);
    }

    [RelayCommand]
    private void PlaySelectedRelease()
    {
        if(SelectedRelease == null || _playerService == null)
            return;

        if(_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if(SelectedRelease.Title == (_playerService.CurrentTrack?.ReleaseTitle ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayRelease(SelectedRelease);
    }

    [RelayCommand]
    private void PlaySelectedTrack()
    {
        if(_selectedTrack == null || _playerService == null)
            return;

        if(_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if(_selectedTrack.Title == (_playerService.CurrentTrack?.Title ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayTrack(_selectedTrack);
    }

    [RelayCommand]
    private void PlayRandomTrack()
    {
        if(Releases == null || Releases.Count == 0)
            return;

        var allTracks = Releases.SelectMany(release => release.Tracks).ToList();

        if(allTracks.Count == 0)
            return;

        var randomTrack = allTracks[new Random().Next(allTracks.Count)];
        _playerService.PlayTrack(randomTrack);
    }

    [RelayCommand]
    private void ShowSelectedRelease()
    {
        if(SelectedRelease == null || _playerService == null)
            return;

        IsDetailViewVisible = true;
    }

    [RelayCommand] private void HideSelectedRelease() => IsDetailViewVisible = false;

    private async Task DelayGetLocalReleasesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(500, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            await LoadLocalReleasesAsync().ConfigureAwait(false);
        } catch(OperationCanceledException)
        {
            // Task was cancelled, do nothing
        }
    }

    private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e) => IsPlaying = e.NewState == MediaElementState.Playing;

    private async Task LoadLocalReleasesAsync()
    {
        if (_source.Count == 0)
        {
            try
            {
                var jsonString = await File.ReadAllTextAsync(Prefernces.MusicDataFilePath).ConfigureAwait(false);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
                        },
                };
                var releases = JsonSerializer.Deserialize<IEnumerable<ReleaseModel>>(jsonString, options);

                foreach (var r in releases ?? [])
                {
                    try
                    {
                        _source.Add(await ReleaseModel.Create(r).ConfigureAwait(false));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding release: {r.Artist} - {r.Title}, error " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from JSON: {ex.Message}");
            }
        }

        if (SelectedLevel?.Level > 0)
        {
            var list = _source.Where(r => r.Level == SelectedLevel.Level)
                .OrderBy(r => r.Artist)
                .ThenBy(r => r.Year)
                .ToList();
            Releases = new ObservableCollection<ReleaseModel>(list);
        }
        else
            Releases = new ObservableCollection<ReleaseModel>(_source);
    }

    //TODO: Check if file exists
    public static string CombinePaths(params string[] paths)
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
