using System.Diagnostics;

namespace Months18.Pages;

public partial class DocumentPage : ContentPage
{
    public DocumentPage(DocumentPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        viewModel.SubscribeToSelectionChanged(OnDocumentSelected);

        _viewModel = viewModel;
    }

    private readonly DocumentPageViewModel _viewModel;
    private void OnDocumentSelected(object? sender, EventArgs e)
    {
        var docPath = _viewModel.SelectedDocument?.Uri;
        pdfview.Source = docPath;
    }
}