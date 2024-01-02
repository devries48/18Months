namespace Months18.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "Class instantiated by Dependency Injection.")]
    public SettingsPageViewModel(ISettingsService settingsService) => _settingsService = settingsService;

    private readonly ISettingsService _settingsService;

    public const string LocalConnection = "LOCAL";
    public const string RemoteConnection = "REMOTE";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string? connection;

    [ObservableProperty]
    private string? musicMateFile;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string? musicMateUrl;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string? userName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string? password;

    public async Task LoadSettings()
    {
        Connection = await _settingsService.GetValue(nameof(Connection), "LOCAL");
        MusicMateFile = await _settingsService.GetValue(nameof(MusicMateFile), "");
        MusicMateUrl = await _settingsService.GetValue(nameof(MusicMateUrl), "");
    }

    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {

    }

    private bool CanSignIn()
    {
        return (Connection ?? "") == RemoteConnection
            && !string.IsNullOrWhiteSpace(MusicMateUrl)
            && !string.IsNullOrWhiteSpace(UserName)
            && !string.IsNullOrWhiteSpace(Password);
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
