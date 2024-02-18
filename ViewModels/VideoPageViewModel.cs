namespace Months18.ViewModels;

public partial class VideoPageViewModel : ObservableObject
{
    public VideoPageViewModel(IVideoPlayerService playerService)
    {
        _playerService = playerService;
        _source = [];
        _playerService.SubscribeToMediaStateChanged(OnMediaStateChanged);

        _ = GetLocalVideosAsync();
    }

    private readonly IVideoPlayerService _playerService;
    private readonly List<VideoModel> _source;

    [ObservableProperty]
    private VideoModel? _selectedVideo;

    [ObservableProperty]
    private ObservableCollection<VideoModel> _videos = [];

    [ObservableProperty]
    private bool _isPlaying;


    [RelayCommand]
    private void VideoTap(VideoModel tappedItem)
    {
        if (SelectedVideo != null && SelectedVideo != tappedItem)
            SelectedVideo.IsSelected = false;

        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            SelectedVideo = null;
        }
        else
        {
            SelectedVideo = tappedItem;
            tappedItem.IsSelected = true;
        }
    }

    [RelayCommand]
    private void PlaySelectedVideo()
    {
        if (SelectedVideo == null || SelectedVideo == null)
            return;

        if (_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if (SelectedVideo?.Title == (_playerService.CurrentVideo?.Title ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayVideo(SelectedVideo);
    }


    private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e)
    { IsPlaying = e.NewState == MediaElementState.Playing; }

    private async Task GetLocalVideosAsync()
    {
        if (_source.Count == 0)
        {
            var video = await VideoModel.Create("Warmup: Metal Screaming", "Easy and Safe Metal Screaming Tutorial (Beginner Level)", "4:20", VideoPath("Darkness.mp4"), VideoPath("Darkness-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Warmup: Macarena-audio only", "Manual and Conchita's Therapy Macarena Dance Warmup Exercise (audio only)", "2:50", VideoPath("Macarena.mp3"), VideoPath("Macarena-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Kom uit de Bedstee, m'n liefste", "Egbert Douwe - kom uit de bedstee, m'n liefste", "3:18", VideoPath("Bedstee.mp4"), VideoPath("Bedstee-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("You Can't Bring Me Down", "Suicidal Tendencies - You Can't Bring Me Down", "5:45", VideoPath("Down.mp4"), VideoPath("Down-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Alfred & Shadow: Emotions", "Alfred & Shadow - A short story about emotions", "7:03", VideoPath("Emotions.mp4"), VideoPath("Emotions-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Alfred & Shadow: Loneliness", "Alfred & Shadow - A short story about loneliness", "7:26", VideoPath("Loneliness.mp4"), VideoPath("Loneliness-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Alfred & Shadow: Self-Criticism", "Alfred & Shadow - A short story about Self-Criticism", "5:26", VideoPath("Self-Criticism.mp4"), VideoPath("Self-Criticism-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Jesus Is a Friend of Mine", "Sonseed - Jesus is a Friend of Mine", "2:48", VideoPath("Jesus.mp4"), VideoPath("Jesus-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Paloma Blanca", "Georgie Dann - Paloma Blanca", "3:22", VideoPath("Paloma.mp4"), VideoPath("Paloma-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Vitas", "Vitas - The 7th Element", "4:11", VideoPath("Vitas.mp4"), VideoPath("Vitas-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Merry Christmas", "Henrietta and Merna - Go Tell It On The Mountain", "3:07", VideoPath("Christmas.mp4"), VideoPath("Christmas-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);

            video = await VideoModel.Create("Movie: Weng Weng", "For Y'ur Height Only", "1:27:03", VideoPath("Weng.mp4"), VideoPath("Weng-thumb.jpg")).ConfigureAwait(false);
            _source.Add(video);


            //For Y'ur Height Only
        }
        Videos = new ObservableCollection<VideoModel>(_source);
    }


    private static string VideoPath(string filename) =>
 // TODO: Get from VideoMateData.json
 MusicPageViewModel.CombinePaths("C:/Users/rvrie/source/repos/18Months", "Data/Videos", filename);

}