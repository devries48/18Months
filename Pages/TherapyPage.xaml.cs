using System.Diagnostics;

namespace Months18.Pages;

public partial class TherapyPage : ContentPage
{
    public TherapyPage(MusicPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private MusicPageViewModel _viewModel;

    /// <summary>
    /// Calculate the number of columns of the CollectionView, 
    /// </summary>
    private void CollectionView_SizeChanged(object sender, EventArgs e)
    {
        int span = (int)(ReleaseCollectionView.Width - 20) / MusicPageViewModel.DefaultItemWidth;
        if (span == 0) span = 1;
        if (_viewModel.Span != span)
            _viewModel.Span = span;
    }
}