namespace Months18.Pages;

public partial class VideoPage : ContentPage
{
    public VideoPage()
    {
        InitializeComponent();

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (testLabel.Text == "Mother Sky")
            testLabel.Text = "She Brings The Rain (From Bottom - Ein Groﬂer Graublauer Vogel)";
        else
            testLabel.Text = "Mother Sky";
    }
}