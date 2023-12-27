using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Months18.Services;
using System.ComponentModel;
using System.Diagnostics;

namespace Months18.Views;

public partial class MusicPlayerView
{
    public MusicPlayerView()
    {
        InitializeComponent();

        MediaElement.PropertyChanged += OnMediaElementPropertyChanged;
        Unloaded += OnUnLoaded;

        InitializeService();
    }

    private MusicPlayerService? musicPlayerService;
    private List<string> _currentPlayList = [];

    public static readonly BindableProperty CurrentPositionProperty =
        BindableProperty.Create(nameof(CurrentPositionLabel),
            typeof(string),
            typeof(MusicPlayerView),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: OnStringChanged);

    private static void OnStringChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var a = bindable as MusicPlayerView;
        //a.currentPosLabel.Text = newValue.ToString();
    }

    public string CurrentPositionLabel
    {
        get => (string)GetValue(CurrentPositionProperty);
        set => SetValue(CurrentPositionProperty, value);
    }

    #region MusicPlayerService Implementation
    void InitializeService()
    {
        musicPlayerService = MauiProgram.GetService<MusicPlayerService>();

        if (musicPlayerService != null)
        {
            musicPlayerService.SubscribeToAudioPlayerAction(OnAudioPlayerAction);
            musicPlayerService.SubscribeToPlayListChanged(OnPlayListChanged);
        }
    }

    private void OnAudioPlayerAction(object sender, ActionEventArgs e)
    {
        switch (e.Action)
        {
            case AudioPlayerAction.Play:
                MediaElement.Play();
                break;
            case AudioPlayerAction.Pause:
                MediaElement.Pause();
                break;
            case AudioPlayerAction.Stop:
                MediaElement.Stop();
                break;
            default:
                break;
        }
    }

    private void OnPlayListChanged(object sender, TrackEventArgs e)
    {
        // just get the whole playlist
        var list = musicPlayerService?.GetPlaylist();

        if (list != null)
        {
            _currentPlayList = list;
            if (!e.IsRemoveAction && list.Count == 1)
                PlayTrack(e.Track, e.Source);
        }
    }
    #endregion

    #region MediaElement Events
    void OnMediaOpened(object? sender, EventArgs e) => Debug.WriteLine("Media opened.");

    void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
    {
        //Debug.WriteLine("Media State Changed. New State: {NewState}", e.NewState);
    } // iets doen met previousstate?

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine("Media failed. Error: {ErrorMessage}", e.ErrorMessage);

    void OnMediaEnded(object? sender, EventArgs e) => Debug.WriteLine("Media ended.");

    void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    {
        Debug.WriteLine("Position changed to {position}", e.Position.ToString());
        PositionSlider.Value = e.Position.TotalSeconds;
    }


    void OnSeekCompleted(object? sender, EventArgs e) => Debug.WriteLine("Seek completed.");

    #endregion

    private void OnPlayOrPauseClicked(object sender, EventArgs e)
    {
        MediaElement.Play();
    }
    private void OnStopClicked(object sender, EventArgs e)
    {
        MediaElement.Stop();
    }

    private void OnMediaElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
            PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
    }

    async void OnSliderDragCompleted(object? sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var newValue = ((Slider)sender).Value;
        await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

        MediaElement.Play();
    }

    void OnSliderDragStarted(object sender, EventArgs e)
    {
        MediaElement.Pause();
    }


    private void PlayTrack(string uri, AudioPlayerSource source)
    {
        MediaElement.Source = source switch
        {
            AudioPlayerSource.Embed => MediaSource.FromResource(uri),
            AudioPlayerSource.FileSystem => MediaSource.FromFile(uri),
            AudioPlayerSource.Url => MediaSource.FromUri(uri),
            _ => ""
        };

        MediaElement.Play();
    }

    private void OnUnLoaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        MediaElement.Handler?.DisconnectHandler();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }
}
