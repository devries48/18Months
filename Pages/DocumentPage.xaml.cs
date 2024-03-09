using System.Diagnostics;

namespace Months18.Pages;

public partial class DocumentPage : ContentPage
{
    public DocumentPage()
    {
        InitializeComponent();
        var trackPath = Path.Combine(Prefernces.DocumentDataPath, "Je bent met zijn tweeën.pdf");

        pdfview.Source = trackPath;

    }
}