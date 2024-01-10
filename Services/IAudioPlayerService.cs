namespace Months18.Services;

public interface IAudioPlayerService
{
    TrackModel? CurrentTrack { get; set; }

    MediaElementState? CurrentState { get; }

    int CurrentPlaylistIndex { get; }

    bool CanPlaylistMoveBack { get; }

    bool CanPlaylistMoveForward { get; }

    void AddToPlaylist(TrackModel track, AudioPlayerSource source);

    void AddToPlaylist(ReleaseModel release, AudioPlayerSource source);

    List<TrackModel> GetPlaylist();

    void Pause();

    void Play();

    void PlayRelease(ReleaseModel selectedRelease);

    void PlayFromPlaylist(int index, bool startPlay = false);

    void Stop();

    void OnMediaStatusChanged(MediaStateChangedEventArgs e);

    void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler);

    void SubscribeToPlaylistChanged(PlaylistChangedEventHandler handler);

    void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);

    void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler);

    void UnsubscribeFromPlaylistChanged(PlaylistChangedEventHandler handler);

    void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);
}
