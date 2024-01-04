namespace Months18.Services;

public class MusicPlayerService : IMusicPlayerService
{
    private event PlayListChangedEventHandler? PlayListChanged;

    private event AudioPlayerActionEventHandler? AudioPlayerAction;

    private event MediaStateChangedEventHandler? MediaStateChanged;

    private readonly List<TrackModel> playList = [];

    public TrackModel? CurrentTrack { get; set; }

    public MediaElementState? CurrentState { get; private set; }

    public List<TrackModel> GetPlaylist() => playList;

    public void AddToPlaylist(TrackModel track, AudioPlayerSource source) => OnPlayListChanged(track, source);

    public void AddToPlaylist(ReleaseModel release, AudioPlayerSource source) => OnPlayListChanged(
        release.Tracks,
        source);

    public void Play() => OnAudioPlayerAction(Services.AudioPlayerAction.Play);

    public void PlayRelease(ReleaseModel release)
    {
        if(release.Tracks.Count > 0)
        {
            playList.Clear();
            playList.AddRange(release.Tracks);
            PlayFromList(0);
        }
    }

    public void PlayFromList(int playListIndex) => OnAudioPlayerAction(
        Services.AudioPlayerAction.PlayFromList,
        playListIndex);

    public void Pause() => OnAudioPlayerAction(Services.AudioPlayerAction.Pause);

    public void Stop() => OnAudioPlayerAction(Services.AudioPlayerAction.Stop);

    public void SubscribeToPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged += handler;

    public void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged -= handler;

    public void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction += handler;

    public void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction -= handler;

    public void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged += handler;

    public void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged -= handler;

    protected virtual void OnPlayListChanged(TrackModel track, AudioPlayerSource source, bool remove = false)
    {
        if(remove)
            playList.Remove(track);
        else
            playList.Add(track);

        PlayListChanged?.Invoke(this, new TrackEventArgs(track, source, remove));
    }

    protected virtual void OnPlayListChanged(
        ICollection<TrackModel> tracks,
        AudioPlayerSource source,
        bool remove = false)
    {
        if(remove)
        {
            foreach(var track in tracks)
                playList.Remove(track);
        } else
            playList.AddRange(tracks);

        PlayListChanged?.Invoke(this, new TrackEventArgs(tracks, source, remove));
    }

    protected virtual void OnAudioPlayerAction(AudioPlayerAction action, int playListIndex = -1)
    {
        TrackModel? track = null;

        if(playListIndex >= 0 && playListIndex < playList.Count)
            track = playList[playListIndex];

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

public delegate void PlayListChangedEventHandler(object sender, TrackEventArgs e);
public delegate void AudioPlayerActionEventHandler(object sender, ActionEventArgs e);
public delegate void MediaStateChangedEventHandler(object sender, MediaStateChangedEventArgs e);

public class TrackEventArgs : EventArgs
{
    public TrackEventArgs(TrackModel track, AudioPlayerSource source, bool isRemove)
    {
        Source = source;
        IsRemoveAction = isRemove;
        Tracks.Add(track);
    }

    public TrackEventArgs(ICollection<TrackModel> tracks, AudioPlayerSource source, bool isRemove)
    {
        Source = source;
        IsRemoveAction = isRemove;
        Tracks.AddRange(tracks);
    }

    public List<TrackModel> Tracks { get; } = [];

    public AudioPlayerSource Source { get; }

    public bool IsRemoveAction { get; }
}

public class ActionEventArgs(AudioPlayerAction action, TrackModel? track = null) : EventArgs
{
    public AudioPlayerAction Action { get; } = action;

    public TrackModel? Track { get; } = track;
}
