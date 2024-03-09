using System.Text.Json;
using System.Text.Json.Serialization;

namespace Months18.ViewModels;

public partial class VideoPageViewModel : ObservableObject
{
    public VideoPageViewModel(IVideoPlayerService playerService)
    {
        _playerService = playerService;
        _source = [];
        _playerService.SubscribeToMediaStateChanged(OnMediaStateChanged);

        _ = LoadLocalVideosAsync();
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
        if (SelectedVideo == null)
            return;

        if (_playerService.CurrentState == MediaElementState.Playing)
        {
            _playerService.Pause();
            return;
        }

        if (SelectedVideo?.Title == (_playerService.CurrentVideo?.Title ?? string.Empty))
            _playerService.Play();
        else
            _playerService.PlayVideo(SelectedVideo!);
    }

    private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e) => IsPlaying = e.NewState == MediaElementState.Playing;

    //public async Task ExportToJsonAsync(IEnumerable<VideoModel> videos)
    //{
    //    try
    //    {
    //        string filePath = "C:/Data/18Months/App/Data/videos.json";
    //        var options = new JsonSerializerOptions
    //        {
    //            WriteIndented = true,
    //            IgnoreReadOnlyProperties = true,
    //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //            PropertyNameCaseInsensitive = true,
    //            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    //            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    //            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //            IncludeFields = true,
    //            Converters =
    //                {
    //                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
    //                    new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
    //                },
    //        };

    //        string jsonString = JsonSerializer.Serialize(videos, options);

    //        await File.WriteAllTextAsync(filePath, jsonString);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error exporting to JSON: {ex.Message}");
    //    }
    //}

    private async Task LoadLocalVideosAsync()
    {
        if (_source.Count == 0)
        {
            try
            {
                var jsonString = await File.ReadAllTextAsync(Prefernces.VideoDataFilePath).ConfigureAwait(false);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
                        },
                };
                var vdeos = JsonSerializer.Deserialize<IEnumerable<VideoModel>>(jsonString, options);

                foreach (var v in vdeos ?? [])
                {
                    try
                    {
                        _source.Add(await VideoModel.Create(v).ConfigureAwait(false));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding video: {v.Title}, error " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from JSON: {ex.Message}");
            }
        }

        Videos = new ObservableCollection<VideoModel>(_source);
    }
}