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

    void PlayRelease(ReleaseModel release);

    void PlayTrack(TrackModel track);

    /// <summary>
    /// Select a Track to Play on the Playlist
    /// </summary>
    /// <param name="index">The index of the Track in the Playlist.</param>
    /// <param name="startPlay">Force start on the MediaElement.</param>
    void PlayFromPlaylist(int index, bool startPlay = false);

    void PlayNextFromPlayList();

    void PlayPreviousFromPlayList();

    void Stop();

    void OnMediaStatusChanged(MediaStateChangedEventArgs e);

    void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler);

    void SubscribeToPlaylistChanged(PlaylistChangedEventHandler handler);

    void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);

    void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler);

    void UnsubscribeFromPlaylistChanged(PlaylistChangedEventHandler handler);

    void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);
}
