using System.Diagnostics;

namespace Months18.Views;

public partial class VideoPlayerView : INotifyPropertyChanged
{
    public VideoPlayerView()
    {
        InitializeComponent();
        InitializeService();

        MediaElement.Volume = Prefernces.VolumeVideo;
        MediaElement.PropertyChanged += OnMediaElementPropertyChanged;
        Unloaded += OnUnLoaded;
    }

    private IVideoPlayerService? _playerService;
    private VideoModel? _currentVideo;

    public VideoModel? CurrentVideo
    {
        get => _currentVideo;
        set
        {
            _currentVideo = value;

            if (_playerService != null)
                _playerService.CurrentVideo = value;

            OnPropertyChanged(nameof(CurrentDescription));
        }
    }

    public string? CurrentDescription => _currentVideo?.Description;

    private bool _isSliding = false;

    #region AudioPlayer Service Implementation
    private void InitializeService()
    {
        _playerService = MauiProgram.GetService<IVideoPlayerService>();

        if(_playerService != null)
        {
            _playerService.SubscribeToVideoPlayerAction(OnVideoPlayerAction);
        }
    }

    private void OnVideoPlayerAction(object sender, VideoActionEventArgs e)
    {
        switch(e.Action)
        {
            case VideoPlayerAction.Play:
                MediaElement.Play();
                break;
            case VideoPlayerAction.Pause:
                MediaElement.Pause();
                break;
            case VideoPlayerAction.Stop:
                MediaElement.Stop();
                break;
            case VideoPlayerAction.PlayVideo:
                if(e.Video != null)
                    PlayVideo(e.Video);
                break;
            default:
                // All cases are handled
                break;
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

        //if(e.NewState == MediaElementState.Playing)
        //    Dispatcher.DispatchIfRequired(FadeInCurremtImage);
        //else if(e.NewState != MediaElementState.Buffering && e.NewState != MediaElementState.Opening)
        //    Dispatcher.DispatchIfRequired(FadeOutCurremtImage);
    }

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine(
        "Media failed. Error: {ErrorMessage}",
        e.ErrorMessage);

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

    /// <summary>
    /// The state went to "Paused' when playing a new track, now wait for the MediaOpened event. Received ComException
    /// when MediaElement.Play() was called from event handler, MainThread.BeginInvokeOnMainThread resolved this issue.
    /// </summary>
    private void PlayVideo(VideoModel video)
    {
        Dispatcher.DispatchIfRequired(() => LoadVideo(video));
        MediaElement.MediaOpened += OnMediaOpened;
    }

    private void LoadVideo(VideoModel video)
    {
        CurrentVideo = video;

        MediaElement.Source = video.Source switch
        {
            MediaPlayerSource.Embed => MediaSource.FromResource(video.Uri),
            MediaPlayerSource.FileSystem => MediaSource.FromFile(video.Uri.Replace("/","\\")),
            MediaPlayerSource.Url => MediaSource.FromUri(video.Uri),
            _ => ""
        };
    }

    private void OnMediaOpened(object? sender, EventArgs e)
    {
        MediaElement.MediaOpened -= OnMediaOpened;
        Dispatcher.DispatchIfRequired(MediaElement.Play);
        //MainThread.BeginInvokeOnMainThread(MediaElement.Play);
    }


    private void OnUnLoaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        //MediaElement.Handler?.DisconnectHandler();
    }

    private void OnVolumeSliderChanged(object sender, ValueChangedEventArgs e) { Prefernces.VolumeVideo = e.NewValue; }
}
