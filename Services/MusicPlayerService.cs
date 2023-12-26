namespace Months18.Services
{
    public class MusicPlayerService
    {
        private event PlayListChangedEventHandler? PlayListChanged;
        private event MediaPlayerActionEventHandler? MediaPlayerAction;

        public void SubscribeToPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged += handler;
        public void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler) => PlayListChanged -= handler;
        public void SubscribeToMediaPlayerAction(MediaPlayerActionEventHandler handler) => MediaPlayerAction += handler;
        public void UnsubscribeFromMediaPlayerAction(MediaPlayerActionEventHandler handler) => MediaPlayerAction -= handler;

        public void AddToPlaylist(string track) => OnPlayListChanged(track);
        public void Play() => OnMediaPlayerAction(Services.MediaPlayerAction.Play);
        public void Pause() => OnMediaPlayerAction(Services.MediaPlayerAction.Pause);
        public void Stop() => OnMediaPlayerAction(Services.MediaPlayerAction.Stop);

        protected virtual void OnPlayListChanged(string track, bool remove = false) 
            => PlayListChanged?.Invoke(this, new TrackEventArgs(track, remove));

        protected virtual void OnMediaPlayerAction(MediaPlayerAction action) 
            => MediaPlayerAction?.Invoke(this, new ActionEventArgs(action));
    }

    public enum MediaPlayerAction
    {
        Play, Pause, Stop
    }

    public delegate void PlayListChangedEventHandler(Object sender, TrackEventArgs e);
    public delegate void MediaPlayerActionEventHandler(Object sender, ActionEventArgs e);

    public class TrackEventArgs(string track, bool isRemove) : EventArgs
    {
        public string? Track { get; } = track;
        public bool IsRemoveAction {  get; } = isRemove;
    }

    public class ActionEventArgs(MediaPlayerAction action) : EventArgs
    {
        public MediaPlayerAction Action { get; } = action;
    }
}
