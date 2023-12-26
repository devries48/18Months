using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Months18.Services;
using Months18.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace Months18.Views;

public partial class MusicPlayerView
{
    public MusicPlayerView()
    {
        InitializeComponent();
        MediaElement.PropertyChanged += OnMediaElementPropertyChanged;
        
        Loaded += OnLoaded;
        Unloaded += OnUnLoaded;
    }

    private MusicPlayerService musicPlayerService;

    // MediaElement.Source = MediaSource.FromUri(CustomSourceEntry.Text);
    // MediaSource.FromResource() and MediaSource.FromFile()

    private void OnMediaElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
        {
            PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
        }
    }

    void OnMediaOpened(object? sender, EventArgs e) => Debug.WriteLine("Media opened.");

    void OnStateChanged(object? sender, MediaStateChangedEventArgs e) =>
        Debug.WriteLine("Media State Changed. Old State: {PreviousState}, New State: {NewState}", e.PreviousState, e.NewState);

    void OnMediaFailed(object? sender, MediaFailedEventArgs e) => Debug.WriteLine("Media failed. Error: {ErrorMessage}", e.ErrorMessage);

    void OnMediaEnded(object? sender, EventArgs e) => Debug.WriteLine("Media ended.");

    void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
    {
        Debug.WriteLine("Position changed to {position}", e.Position.ToString());
        PositionSlider.Value = e.Position.TotalSeconds;
    }

    void OnSeekCompleted(object? sender, EventArgs e) => Debug.WriteLine("Seek completed.");

    void OnPlayClicked(object? sender, EventArgs e)
    {
        MediaElement.Play();
    }

    void OnPauseClicked(object? sender, EventArgs e)
    {
        MediaElement.Pause();
    }

    void OnStopClicked(object? sender, EventArgs e)
    {
        MediaElement.Stop();
    }

    void OnMuteClicked(object? sender, EventArgs e)
    {
        MediaElement.ShouldMute = !MediaElement.ShouldMute;
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

    private void OnLoaded(object? sender, EventArgs e)
    {
        var viewModel = MauiProgram.GetService<MusicPlayerViewModel>();
      
        if (viewModel != null)
        {
            var musicPlayerService = MauiProgram.GetService<MusicPlayerService>();
            viewModel.MediaElement = MediaElement;
        }
        Loaded -= OnLoaded;
    }
    private void OnUnLoaded(object? sender, EventArgs e)
    {
        // Stop and cleanup MediaElement when we navigate away
        MediaElement.Handler?.DisconnectHandler();
    }
}
