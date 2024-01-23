using System.Diagnostics;

namespace Months18.Services;

public class AudioPlayerService : IAudioPlayerService
{
    private event PlaylistChangedEventHandler? PlaylistChanged;

    private event AudioPlayerActionEventHandler? AudioPlayerAction;

    private event MediaStateChangedEventHandler? MediaStateChanged;

    private readonly List<TrackModel> playlist = [];

    public TrackModel? CurrentTrack { get; set; }

    public MediaElementState? CurrentState { get; private set; }

    public int CurrentPlaylistIndex { get; set; }

    public bool CanPlaylistMoveBack { get => playlist.Count > 0 && CurrentPlaylistIndex > 0; }

    public bool CanPlaylistMoveForward { get => CurrentPlaylistIndex < playlist.Count - 1; }

    public List<TrackModel> GetPlaylist() => playlist;

    public void AddToPlaylist(TrackModel track, AudioPlayerSource source) => OnPlaylistChanged(
        PlaylistAction.ListChanged);

    public void AddToPlaylist(ReleaseModel release, AudioPlayerSource source) => OnPlaylistChanged(
        PlaylistAction.ListChanged);

    public void Play() => OnAudioPlayerAction(Services.AudioPlayerAction.Play);

    public void PlayRelease(ReleaseModel release)
    {
        if(release.Tracks.Count > 0)
        {
            playlist.Clear();
            playlist.AddRange(release.Tracks);
            OnPlaylistChanged(PlaylistAction.ListChanged, 0);
        }
    }

    public void PlayTrack(TrackModel track)
    {
        playlist.Clear();
        playlist.Add(track);
        OnPlaylistChanged(PlaylistAction.ListChanged, 0);
    }

    public void PlayFromPlaylist(int index, bool startPlay) => OnPlaylistChanged(
        PlaylistAction.IndexChanged,
        index,
        startPlay);

    public void PlayNextFromPlayList() => OnPlaylistChanged(PlaylistAction.IndexChanged, CurrentPlaylistIndex + 1, true);

    public void PlayPreviousFromPlayList() => OnPlaylistChanged(
        PlaylistAction.IndexChanged,
        CurrentPlaylistIndex - 1,
        true);

    public void Pause() => OnAudioPlayerAction(Services.AudioPlayerAction.Pause);

    public void Stop() => OnAudioPlayerAction(Services.AudioPlayerAction.Stop);


    #region Event Subscribe & Unsubscribe
    public void SubscribeToPlaylistChanged(PlaylistChangedEventHandler handler) => PlaylistChanged += handler;

    public void UnsubscribeFromPlaylistChanged(PlaylistChangedEventHandler handler) => PlaylistChanged -= handler;

    public void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction += handler;

    public void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler) => AudioPlayerAction -= handler;

    public void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged += handler;

    public void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler) => MediaStateChanged -= handler;
    #endregion

    public  void OnPlaylistChanged(PlaylistAction action, int? playlistIndex = null, bool startPlay = false)
    {
        if(action == PlaylistAction.ListChanged)
        {
            bool isSingleArtist = IsSingleArtistPlaylist();

            for(int i = 0; i < playlist.Count; i++)
            {
                playlist[i].PlaylistPosition = i + 1;
                playlist[i].PlaylistSingleArtist = isSingleArtist;
            }
            PlaylistChanged?.Invoke(this, new PlaylistEventArgs(PlaylistAction.ListChanged, playlistIndex));
        } else if(action == PlaylistAction.IndexChanged && playlistIndex.HasValue)
        {
            var playerAction = CurrentState == MediaElementState.Playing || startPlay is true
                ? Services.AudioPlayerAction.PlayFromList
                : Services.AudioPlayerAction.LoadFromList;

            OnAudioPlayerAction(playerAction, playlistIndex.Value);
        }
    }

    public void OnAudioPlayerAction(AudioPlayerAction action, int playlistIndex = -1)
    {
        TrackModel? track = null;

        if(playlistIndex >= 0 && playlistIndex < playlist.Count)
        {
            CurrentPlaylistIndex = playlistIndex;
            track = playlist[playlistIndex];
        }

        AudioPlayerAction?.Invoke(this, new ActionEventArgs(action, track));
    }

    public void OnMediaStatusChanged(MediaStateChangedEventArgs e)
    {
        CurrentState = e.NewState;
        MediaStateChanged?.Invoke(this, e);
    }

    private bool IsSingleArtistPlaylist()
    {
        if(playlist.Count == 0)
            return false;

        var distinctArtists = playlist.Select(track => track.TrackArtist).Distinct();
        return distinctArtists.Count() == 1;
    }
}

public enum AudioPlayerAction
{
    Play,
    PlayFromList,
    LoadFromList,
    Pause,
    Stop
}

public enum PlaylistAction
{
    ListChanged,
    IndexChanged
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

public class PlaylistEventArgs(PlaylistAction action, int? playlistIndex) : EventArgs
{
    public PlaylistAction Action { get; } = action;

    public int? PlaylistIndex { get; } = playlistIndex;
}

public class ActionEventArgs(AudioPlayerAction action, TrackModel? track = null) : EventArgs
{
    public AudioPlayerAction Action { get; } = action;

    public TrackModel? Track { get; } = track;
}
