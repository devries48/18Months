using System.Diagnostics;

namespace Months18.Views;

public partial class AudioPlayerView : INotifyPropertyChanged
{
    public AudioPlayerView()
    {
        InitializeComponent();
        InitializeService();

        MediaElement.Volume = Prefernces.VolumeMusic;
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

            if(_playerService != null)
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
    private bool _isSliding = false;

    public int CurrentPlaylistIndex => _playerService?.CurrentPlaylistIndex ?? -1;

    #region AudioPlayer Service Implementation
    private void InitializeService()
    {
        _playerService = MauiProgram.GetService<IAudioPlayerService>();

        if(_playerService != null)
        {
            _playerService.SubscribeToAudioPlayerAction(OnAudioPlayerAction);
            _playerService.SubscribeToPlaylistChanged(OnPlaylistChanged);
        }
    }

    private void OnAudioPlayerAction(object sender, AudioActionEventArgs e)
    {
        switch(e.Action)
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
                if(e.Track != null)
                    PlayTrack(e.Track);
                break;
            case AudioPlayerAction.LoadFromList:
                if(e.Track != null)
                    LoadTrack(e.Track);
                break;

            default:
                // All cases are handled
                break;
        }
    }

    /// <summary>
    /// Handle the AudioPlayerService PlaylistChanged event. - ListChanged, Load the playlist. - IndexChanged, Just set
    /// the CurrentPlaylistIndex and the button states.
    /// </summary>
    private void OnPlaylistChanged(object sender, PlaylistEventArgs e)  //TODO: DO we need an e.action?
    {
        if(e.Action == PlaylistAction.ListChanged) // reload the playlist 
        {
            var list = _playerService?.GetPlaylist();

            if(list != null)
            {
                CurrentPlaylist = new ObservableCollection<TrackModel>(list);
                PlaylistView.ItemsSource = CurrentPlaylist;

                if(e.PlaylistIndex.HasValue)
                    _playerService?.PlayFromPlaylist(e.PlaylistIndex.Value, true);
                else if (CurrentTrack?.Title != list.First().Title)
                    Dispatcher.DispatchIfRequired(() => LoadTrack(list.First()));
            }
        }
    }
    #endregion

    #region MediaElement, Handle events
    private void OnMediaElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == MediaElement.DurationProperty.PropertyName)
            PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
    }

    void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
    {
        _playerService?.OnMediaStatusChanged(e);

        if(e.NewState == MediaElementState.Playing)
            Dispatcher.DispatchIfRequired(FadeInCurremtImage);
        else if(e.NewState != MediaElementState.Buffering && e.NewState != MediaElementState.Opening)
            Dispatcher.DispatchIfRequired(FadeOutCurremtImage);
    }

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine(
        "Media failed. Error: {ErrorMessage}",
        e.ErrorMessage);

    void OnMediaEnded(object? sender, EventArgs e) => _playerService?.PlayNextFromPlayList();

    void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    {
        if(_isSliding)
            return;

        PositionSlider.Value = e.Position.TotalSeconds;
    }

    void OnSeekCompleted(object? sender, EventArgs e) => Debug.WriteLine("Seek completed.");
    #endregion

    private void OnPlayOrPauseClicked(object sender, EventArgs e)
    {
        if(MediaElement.CurrentState == MediaElementState.Paused)
            MediaElement.Play();
        else if(MediaElement.CurrentState == MediaElementState.Playing)
            MediaElement.Pause();
    }

    private void OnMuteClicked(object? sender, EventArgs e) { MediaElement.ShouldMute = !MediaElement.ShouldMute; }

    private void OnStopClicked(object sender, EventArgs e) { MediaElement.Stop(); }

    /// <summary>
    /// Set the postion of the slider to the track position in the MediaElement.
    /// </summary>
    private async void OnSliderDragCompleted(object? sender, EventArgs e)
    {
        _isSliding = false;

        ArgumentNullException.ThrowIfNull(sender);

        var newValue = ((Slider)sender).Value;
        await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

        MediaElement.Play();
    }

    /// <summary>
    /// Move to a position within the track started. When there is just a click on the Slider, this event is also
    /// raised, followed by the DragComplete event. The isSliding flag is set, so the synchronisation with the
    /// MediaElement will be stopped. It intefered with te track position when there was a just click on the Slider.
    /// </summary>
    private void OnSliderDragStarted(object sender, EventArgs e)
    {
        _isSliding = true;
        MediaElement.Pause();
    }

    private void OnPlaylistNextClicked(object sender, EventArgs e) { _playerService?.PlayNextFromPlayList(); }

    private void OnPlaylistPreviousClicked(object sender, EventArgs e)
    { _playerService?.PlayFromPlaylist(CurrentPlaylistIndex - 1); }

    /// <summary>
    /// The state went to "Paused' when playing a new track, now wait for the MediaOpened event. Received ComException
    /// when MediaElement.Play() was called from event handler, MainThread.BeginInvokeOnMainThread resolved this issue.
    /// </summary>
    private void PlayTrack(TrackModel track)
    {
        Dispatcher.DispatchIfRequired(() => LoadTrack(track));
        MediaElement.MediaOpened += OnMediaOpened;
    }

    private void LoadTrack(TrackModel track)
    {
        CurrentTrack = track;

        OnPropertyChanged(nameof(CurrentPlaylistIndex));

        PlaylistNextButton.IsEnabled = _playerService?.CanPlaylistMoveForward ?? false;
        PlaylistPreviousButton.IsEnabled = _playerService?.CanPlaylistMoveBack ?? false;

        MediaElement.Source = track.Source switch
        {
            MediaPlayerSource.Embed => MediaSource.FromResource(track.Uri),
            MediaPlayerSource.FileSystem => MediaSource.FromFile(track.Uri),
            MediaPlayerSource.Url => MediaSource.FromUri(track.Uri),
            _ => ""
        };
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        MediaElement.MediaOpened -= OnMediaOpened;
        Dispatcher.DispatchIfRequired(MediaElement.Play);
        //MainThread.BeginInvokeOnMainThread(MediaElement.Play);
    }


    private void FadeInCurremtImage() => ReleaseImage.FadeTo(1);

    private void FadeOutCurremtImage() => ReleaseImage.FadeTo(0.2);

    private void OnUnLoaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
       // MediaElement.Handler?.DisconnectHandler();
    }

    private void OnVolumeSliderChanged(object sender, ValueChangedEventArgs e) { Prefernces.VolumeMusic = e.NewValue; }
}
