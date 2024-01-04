
namespace Months18.Services
{
    public interface IMusicPlayerService
    {
        TrackModel? CurrentTrack { get; set; }
        MediaElementState? CurrentState { get; }

        void AddToPlaylist(TrackModel track, AudioPlayerSource source);
        void AddToPlaylist(ReleaseModel release, AudioPlayerSource source);
        List<TrackModel> GetPlaylist();
        void Pause();
        void Play();
        void PlayRelease(ReleaseModel selectedRelease);
        void PlayFromList(int playListIndex);
        void Stop();

        void OnMediaStatusChanged(MediaStateChangedEventArgs e);

        void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler);
        void SubscribeToPlayListChanged(PlayListChangedEventHandler handler);
        void SubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);

        void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler);
        void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler);
        void UnsubscribeToMediaStateChanged(MediaStateChangedEventHandler handler);
    }
}
