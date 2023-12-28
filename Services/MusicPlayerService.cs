namespace Months18.Services;

public class MusicPlayerService
{
    private event PlayListChangedEventHandler? PlayListChanged;
    private event AudioPlayerActionEventHandler? AudioPlayerAction;

    private readonly List<string> playList = [];
    public void SubscribeToPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged += handler;
    public void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged -= handler;
    public void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction += handler;
    public void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction -= handler;

    public List<string> GetPlaylist() => playList;
    public void AddToPlaylist(string track, AudioPlayerSource source) => OnPlayListChanged(track, source);
    public void Play() => OnAudioPlayerAction(Services.AudioPlayerAction.Play);
    public void Pause() => OnAudioPlayerAction(Services.AudioPlayerAction.Pause);
    public void Stop() => OnAudioPlayerAction(Services.AudioPlayerAction.Stop);

    protected virtual void OnPlayListChanged(string track, AudioPlayerSource source, bool remove = false)
    {
        if (remove)
            playList.Remove(track);
        else
            playList.Add(track);

        PlayListChanged?.Invoke(this, new TrackEventArgs(track, source, remove));
    }

    protected virtual void OnAudioPlayerAction(AudioPlayerAction action)
        => AudioPlayerAction?.Invoke(this, new ActionEventArgs(action));
}

public enum AudioPlayerAction { Play, Pause, Stop }
public enum AudioPlayerSource { Embed, FileSystem, Url }

public delegate void PlayListChangedEventHandler(Object sender, TrackEventArgs e);
public delegate void AudioPlayerActionEventHandler(Object sender, ActionEventArgs e);

public class TrackEventArgs(string track, AudioPlayerSource source, bool isRemove) : EventArgs
{
    public string Track { get; } = track;
    public AudioPlayerSource Source { get; } = source;
    public bool IsRemoveAction { get; } = isRemove;
}

public class ActionEventArgs(AudioPlayerAction action) : EventArgs
{
    public AudioPlayerAction Action { get; } = action;
}
