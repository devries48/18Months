using System.Diagnostics;

namespace Months18.Views;

public partial class MusicPlayerView : INotifyPropertyChanged
{
    public MusicPlayerView()
    {
        InitializeComponent();
        InitializeService();

        MediaElement.PropertyChanged += OnMediaElementPropertyChanged;
        Unloaded += OnUnLoaded;
    }

    private IMusicPlayerService? _playerService;
    private List<TrackModel> _currentPlayList = [];
    private TrackModel? _currentTrack;

    public TrackModel? CurrentTrack
    {
        get => _currentTrack;
        set
        {
            _currentTrack = value;

            if (_playerService != null)
                _playerService.CurrentTrack = value;

            OnPropertyChanged(nameof(CurrentImage));
            OnPropertyChanged(nameof(CurrentArtist));
            OnPropertyChanged(nameof(CurrentTitle));
        }
    }

    public byte[]? CurrentImage => _currentTrack?.ReleaseImage;
   
    public string? CurrentArtist => _currentTrack?.TrackArtist;

    public string? CurrentTitle => _currentTrack?.Title;

    #region MusicPlayerService Implementation
    private void InitializeService()
    {
        _playerService = MauiProgram.GetService<IMusicPlayerService>();

        if (_playerService != null)
        {
            _playerService.SubscribeToAudioPlayerAction(OnAudioPlayerAction);
            _playerService.SubscribeToPlayListChanged(OnPlayListChanged);
        }
    }

    private void OnAudioPlayerAction(object sender, ActionEventArgs e)
    {
        switch (e.Action)
        {
            case AudioPlayerAction.Play:
                MediaElement.Play();
                break;
            case AudioPlayerAction.Pause:
                MediaElement.Pause();
                break;
            case AudioPlayerAction.Stop:
                MediaElement.Stop();
                break;
            case AudioPlayerAction.PlayFromList:
                if (e.Track != null) PlayTrack(e.Track);
                break;
            default:
                // All cases are handled
                break;
        }
    }

    private void OnPlayListChanged(object sender, TrackEventArgs e)
    {
        // just get the whole playlist
        var list = _playerService?.GetPlaylist();

        if (list != null)
            _currentPlayList = list;
    }
    #endregion

    #region MediaElement Events
    void OnMediaOpened(object? sender, EventArgs e) => Debug.WriteLine("Media opened.");

    void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
    {
        _playerService?.OnMediaStatusChanged(e);

        if (e.NewState == MediaElementState.Playing)
            FadeCurremtImage(true);
        else if (e.NewState != MediaElementState.Buffering && e.NewState != MediaElementState.Opening)
            FadeCurremtImage(false);
    } // iets doen met previousstate?

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine(
        "Media failed. Error: {ErrorMessage}",
        e.ErrorMessage);

    void OnMediaEnded(object? sender, EventArgs e) => Debug.WriteLine("Media ended.");

    void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    {
        PositionSlider.Value = e.Position.TotalSeconds;
    }

    void OnSeekCompleted(object? sender, EventArgs e) => Debug.WriteLine("Seek completed.");
    #endregion

    private void OnPlayOrPauseClicked(object sender, EventArgs e)
    {
        if (MediaElement.CurrentState == MediaElementState.Paused)
            MediaElement.Play();
        else if (MediaElement.CurrentState == MediaElementState.Playing)
            MediaElement.Pause();
    }

    private void OnMuteClicked(object? sender, EventArgs e)
    {
        MediaElement.ShouldMute = !MediaElement.ShouldMute;
    }

    private void OnStopClicked(object sender, EventArgs e) { MediaElement.Stop(); }

    private void OnMediaElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
            PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
    }

    async void OnSliderDragCompleted(object? sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var newValue = ((Slider)sender).Value;
        await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

        MediaElement.Play();
    }

    void OnSliderDragStarted(object sender, EventArgs e) { MediaElement.Pause(); }

    private void PlayTrack(TrackModel track)
    {
        MediaElement.Source = track.Source switch
        {
            AudioPlayerSource.Embed => MediaSource.FromResource(track.FilePath),
            AudioPlayerSource.FileSystem => MediaSource.FromFile(track.FilePath),
            AudioPlayerSource.Url => MediaSource.FromUri(track.FilePath),
            _ => ""
        };

        MediaElement.Play();
        CurrentTrack = track;
    }

    private void FadeCurremtImage(bool fadeIn)
    {
        var opacity = fadeIn ? 1 : 0.2;
        if (ReleaseImage.Opacity == opacity)
            return;

        ReleaseImage.FadeTo(opacity);
    }
    private void OnUnLoaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        // MediaElement.Handler?.DisconnectHandler();
    }
}
