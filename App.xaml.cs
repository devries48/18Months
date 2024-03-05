namespace Months18;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        if(Current != null)
            Current.UserAppTheme = Prefernces.DarkTheme ? AppTheme.Dark : AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var w = base.CreateWindow(activationState);
        w.MinimumWidth = 1096;
        w.MinimumHeight = 580;
        return w;
    }
}
