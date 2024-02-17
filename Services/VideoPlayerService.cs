namespace Months18.Services;

public class VideoPlayerService : IVideoPlayerService
{
    private event VideoPlayerActionEventHandler? VideoPlayerAction;

    private event MediaStateChangedEventHandler? MediaStateChanged;

    public MediaElementState? CurrentState { get; private set; }
    public VideoModel? CurrentVideo { get; set; }

    public void Play() => OnVideoPlayerAction(Services.VideoPlayerAction.Play);

    public void PlayVideo(VideoModel video) => OnVideoPlayerAction(Services.VideoPlayerAction.PlayVideo,video);

    public void Pause() => OnVideoPlayerAction(Services.VideoPlayerAction.Pause);

    public void Stop() => OnVideoPlayerAction(Services.VideoPlayerAction.Stop);


    #region Event Subscribe & Unsubscribe
    public void SubscribeToVideoPlayerAction(VideoPlayerActionEventHandler handler) => VideoPlayerAction += handler;

    public void UnsubscribeFromVideoPlayerAction(VideoPlayerActionEventHandler handler) => VideoPlayerAction -= handler;

    public void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged += handler;

    public void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged -= handler;
    #endregion

    public void OnVideoPlayerAction(VideoPlayerAction action, VideoModel? video = null) => VideoPlayerAction?.Invoke(this, new VideoActionEventArgs(action, video));

    public void OnMediaStatusChanged(MediaStateChangedEventArgs e)
    {
        CurrentState = e.NewState;
        MediaStateChanged?.Invoke(this, e);
    }
}

public enum VideoPlayerAction
{
    Play,
    PlayVideo,
    Pause,
    Stop
}

public delegate void VideoPlayerActionEventHandler(object sender, VideoActionEventArgs e);

public class VideoActionEventArgs(VideoPlayerAction action, VideoModel? video = null) : EventArgs
{
    public VideoPlayerAction Action { get; } = action;

    public VideoModel? Video { get; } = video;
}
