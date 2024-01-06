namespace Months18.Services;

public class AudioPlayerService : IAudioPlayerService
{
    private event PlaylistChangedEventHandler? PlaylistChanged;

    private event AudioPlayerActionEventHandler? AudioPlayerAction;

    private event MediaStateChangedEventHandler? MediaStateChanged;

    private readonly List<TrackModel> playlist = [];

    public TrackModel? CurrentTrack { get; set; }

    public MediaElementState? CurrentState { get; private set; }

    public int CurrentPlaylistIndex { get; private set; }

    public List<TrackModel> GetPlaylist() => playlist;

    public void AddToPlaylist(TrackModel track, AudioPlayerSource source) => OnPlaylistChanged();

    public void AddToPlaylist(ReleaseModel release, AudioPlayerSource source) => OnPlaylistChanged();

    public void Play() => OnAudioPlayerAction(Services.AudioPlayerAction.Play);

    public void PlayRelease(ReleaseModel release)
    {
        if (release.Tracks.Count > 0)
        {
            playlist.Clear();
            playlist.AddRange(release.Tracks);
            OnPlaylistChanged(0);
        }
    }

    public void PlayFromPlaylist(int index) => OnAudioPlayerAction(Services.AudioPlayerAction.PlayFromList, index);

    public void Pause() => OnAudioPlayerAction(Services.AudioPlayerAction.Pause);

    public void Stop() => OnAudioPlayerAction(Services.AudioPlayerAction.Stop);

    public void SubscribeToPlaylistChanged(PlaylistChangedEventHandler handler) => PlaylistChanged += handler;

    public void UnsubscribeFromPlaylistChanged(PlaylistChangedEventHandler handler) => PlaylistChanged -= handler;

    public void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction += handler;

    public void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction -= handler;

    public void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged += handler;

    public void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged -= handler;

    protected virtual void OnPlaylistChanged(int? playTrackFromListIndex = null)
    {
        for (int i = 0; i < playlist.Count; i++)
            playlist[i].Position = i + 1;

        PlaylistChanged?.Invoke(this, new PlaylistEventArgs(playTrackFromListIndex));
    }

    protected virtual void OnAudioPlayerAction(AudioPlayerAction action, int playlistIndex = -1)
    {
        TrackModel? track = null;

        if (playlistIndex >= 0 && playlistIndex < playlist.Count)
        {
            CurrentPlaylistIndex=playlistIndex+1;
            track = playlist[playlistIndex];
        }

        AudioPlayerAction?.Invoke(this, new ActionEventArgs(action, track));
    }

    public void OnMediaStatusChanged(MediaStateChangedEventArgs e)
    {
        CurrentState = e.NewState;
        MediaStateChanged?.Invoke(this, e);
    }
}

public enum AudioPlayerAction
{
    Play,
    PlayFromList,
    Pause,
    Stop
}

public enum AudioPlayerSource
{
    Embed,
    FileSystem,
    Url
}

public delegate void PlaylistChangedEventHandler(object sender, PlaylistEventArgs e);
public delegate void AudioPlayerActionEventHandler(object sender, ActionEventArgs e);
public delegate void MediaStateChangedEventHandler(object sender, MediaStateChangedEventArgs e);

public class PlaylistEventArgs(int? playTrackFromList) : EventArgs
{
    public int? PlayTrackFromList { get; } = playTrackFromList;
}

public class ActionEventArgs(AudioPlayerAction action, TrackModel? track = null) : EventArgs
{
    public AudioPlayerAction Action { get; } = action;

    public TrackModel? Track { get; } = track;
}
