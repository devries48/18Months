
namespace Months18.Services
{
    public interface IMusicPlayerService
    {
        void AddToPlaylist(TrackModel track, AudioPlayerSource source);
        void AddToPlaylist(ReleaseModel release, AudioPlayerSource source);
        List<TrackModel> GetPlaylist();
        void Pause();
        void Play();
        void PlayRelease(ReleaseModel selectedRelease);
        void Stop();

        void SubscribeToAudioPlayerAction(AudioPlayerActionEventHandler handler);
        void SubscribeToPlayListChanged(PlayListChangedEventHandler handler);
        void UnsubscribeFromAudioPlayerAction(AudioPlayerActionEventHandler handler);
        void UnsubscribeFromPlayListChanged(PlayListChangedEventHandler handler);
    }
}
