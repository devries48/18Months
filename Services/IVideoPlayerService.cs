namespace Months18.Services;

public interface IVideoPlayerService
{
    VideoModel? CurrentVideo { get; set; }

    MediaElementState? CurrentState { get; }

    void Pause();

    void Play();

    void PlayVideo(VideoModel video);

    void Stop();

    void OnMediaStatusChanged(MediaStateChangedEventArgs e);

    void SubscribeToVideoPlayerAction(VideoPlayerActionEventHandler handler);

    void UnsubscribeFromVideoPlayerAction(VideoPlayerActionEventHandler handler);

    void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);

    void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);
}
