namespace Months18.Services;

public class MusicPlayerService : IMusicPlayerService
{
    private event PlayListChangedEventHandler? PlayListChanged;
    private event AudioPlayerActionEventHandler? AudioPlayerAction;

    private readonly List<TrackModel> playList = [];

    public List<TrackModel> GetPlaylist() => playList;

    public void AddToPlaylist(TrackModel track, AudioPlayerSource source) => OnPlayListChanged(track, source);
    public void Play() => OnAudioPlayerAction(Services.AudioPlayerAction.Play);

    public void PlayRelease(ReleaseModel release)
    {
        if (release.Tracks.Count > 0)
        {
            var playIndex = playList.Count;
            playList.AddRange(release.Tracks);
            PlayFromList(playIndex);
        }
    }

    public void PlayFromList(int playListIndex) => OnAudioPlayerAction(Services.AudioPlayerAction.PlayFromList, playListIndex);
    public void Pause() => OnAudioPlayerAction(Services.AudioPlayerAction.Pause);
    public void Stop() => OnAudioPlayerAction(Services.AudioPlayerAction.Stop);

    public void SubscribeToPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged += handler;
    public void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged -= handler;
    public void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction += handler;
    public void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction -= handler;

    protected virtual void OnPlayListChanged(TrackModel track, AudioPlayerSource source, bool remove = false)
    {
        if (remove)
            playList.Remove(track);
        else
            playList.Add(track);

        PlayListChanged?.Invoke(this, new TrackEventArgs(track, source, remove));
    }

    protected virtual void OnAudioPlayerAction(AudioPlayerAction action, int playListIndex = -1)
    {
        TrackModel? track = null;

        if (playListIndex >= 0 && playListIndex < playList.Count)
            track = playList[playListIndex];

        AudioPlayerAction?.Invoke(
                this,
                new ActionEventArgs(action, playListIndex, track));
    }

    public void AddToPlaylist(ReleaseModel release, AudioPlayerSource source)
    {
        throw new NotImplementedException();
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

public class TrackEventArgs(TrackModel track, AudioPlayerSource source, bool isRemove) : EventArgs
{
    public TrackModel Track { get; } = track;
    public AudioPlayerSource Source { get; } = source;
    public bool IsRemoveAction { get; } = isRemove;
}

public class ActionEventArgs(AudioPlayerAction action, int playlistIndex, TrackModel? track = null) : EventArgs
{
    public AudioPlayerAction Action { get; } = action;
    public int PlaylistIndex { get; } = playlistIndex;
    public TrackModel? Track { get; } = track;

}
