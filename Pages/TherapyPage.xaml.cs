using System.Diagnostics;

namespace Months18.Pages;

public partial class TherapyPage : ContentPage
{
    public TherapyPage()
    {
        InitializeComponent();

        _triviaGame = Path.Combine(Prefernces.TriviaPath, "Therapy Trivia.exe");
    }

    private readonly string _triviaGame;

    public void OnLaunchGameClick(object sender, EventArgs e)
    {
        if (File.Exists(_triviaGame))
            Process.Start(_triviaGame);
        else
            Console.WriteLine("Executable file not found: " + _triviaGame);
    }

}