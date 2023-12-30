using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Months18.Services;

namespace Months18.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel(ISettingsService settingsService) => _settingsService = settingsService;

    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private string? connection;

    [ObservableProperty]
    private string? musicMateFile;

    [ObservableProperty]
    private string? musicMateUrl;

    [RelayCommand]
    public async Task LoadSettings()
    {
        Connection = await _settingsService.GetValue(nameof(Connection), "LOCAL");
        MusicMateFile = await _settingsService.GetValue(nameof(MusicMateFile), "");
        MusicMateUrl = await _settingsService.GetValue(nameof(MusicMateUrl), "");
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        await _settingsService.SetValue(nameof(Connection), Connection);
        await _settingsService.SetValue(nameof(MusicMateFile), MusicMateFile);
        await _settingsService.SetValue(nameof(MusicMateUrl), MusicMateUrl);

        await Shell.Current.DisplayAlert(Shell.Current.Title, "Settings saved!", "OK");
    }
}
