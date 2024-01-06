using System.Diagnostics;

namespace Months18.Views;

public partial class AudioPlayerView : INotifyPropertyChanged
{
    public AudioPlayerView()
    {
        InitializeComponent();
        InitializeService();

        MediaElement.PropertyChanged += OnMediaElementPropertyChanged;
        Unloaded += OnUnLoaded;
    }

    private IAudioPlayerService? _playerService;
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

    public ObservableCollection<TrackModel> CurrentPlaylist = [];
    public int CurrentListIndex => _playerService?.CurrentPlaylistIndex ?? -1;


    #region AudioPlayerService Implementation
    private void InitializeService()
    {
        _playerService = MauiProgram.GetService<IAudioPlayerService>();

        if (_playerService != null)
        {
            _playerService.SubscribeToAudioPlayerAction(OnAudioPlayerAction);
            _playerService.SubscribeToPlaylistChanged(OnPlaylistChanged);
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
                if (e.Track != null)
                    PlayTrack(e.Track);
                break;
            default:
                // All cases are handled
                break;
        }
    }

    // just get the whole playlist
    private void OnPlaylistChanged(object sender, PlaylistEventArgs e)
    {
        var list = _playerService?.GetPlaylist();

        if (list != null)
        {
            CurrentPlaylist = new ObservableCollection<TrackModel>(list);
            PlaylistView.ItemsSource = CurrentPlaylist;

            if (e.PlayTrackFromList.HasValue)
                _playerService?.PlayFromPlaylist(e.PlayTrackFromList.Value);
        }
    }
    #endregion

    #region MediaElement Events
    void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
    {
        _playerService?.OnMediaStatusChanged(e);

        if (e.NewState == MediaElementState.Playing)
            FadeCurremtImage(true);
        else if (e.NewState != MediaElementState.Buffering && e.NewState != MediaElementState.Opening)
            FadeCurremtImage(false);
    }

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine(
        "Media failed. Error: {ErrorMessage}",
        e.ErrorMessage);

    void OnMediaEnded(object? sender, EventArgs e) => Debug.WriteLine("Media ended.");

    void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    { PositionSlider.Value = e.Position.TotalSeconds; }

    void OnSeekCompleted(object? sender, EventArgs e) => Debug.WriteLine("Seek completed.");
    #endregion

    private void OnPlayOrPauseClicked(object sender, EventArgs e)
    {
        if (MediaElement.CurrentState == MediaElementState.Paused)
            MediaElement.Play();
        else if (MediaElement.CurrentState == MediaElementState.Playing)
            MediaElement.Pause();
    }

    private void OnMuteClicked(object? sender, EventArgs e) { MediaElement.ShouldMute = !MediaElement.ShouldMute; }

    private void OnStopClicked(object sender, EventArgs e) { MediaElement.Stop(); }

    private void OnMediaElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
            PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
    }

    private async void OnSliderDragCompleted(object? sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var newValue = ((Slider)sender).Value;
        await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

        MediaElement.Play();
    }

    private void OnSliderDragStarted(object sender, EventArgs e) { MediaElement.Pause(); }

    // NOTE: The state went to "Paused' when playing a new track, now wait for the MediaOpened event.
    //       Received ComException when MediaElement.Play() was called from event handler,
    //       MainThread.BeginInvokeOnMainThread resolved this issue.
    private void PlayTrack(TrackModel track)
    {
        CurrentTrack = track;

        MediaElement.Source = track.Source switch
        {
            AudioPlayerSource.Embed => MediaSource.FromResource(track.Uri),
            AudioPlayerSource.FileSystem => MediaSource.FromFile(track.Uri),
            AudioPlayerSource.Url => MediaSource.FromUri(track.Uri),
            _ => ""
        };

        MediaElement.MediaOpened += OnMediaOpened;
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        MediaElement.MediaOpened -= OnMediaOpened;
        MainThread.BeginInvokeOnMainThread(MediaElement.Play);
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
