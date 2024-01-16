namespace Months18.Pages;

public partial class MusicPage : ContentPage
{
    public MusicPage(MusicPageViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private readonly MusicPageViewModel _viewModel;

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        _ =  _viewModel.GetLocalReleasesAsync().ConfigureAwait(false);
    }

    private void CollectionView_SizeChanged(object sender, EventArgs e)
    {
        int span = (int)(ReleaseCollectionView.Width - 40) / MusicPageViewModel.DefaultItemWidth;
        if (span == 0) span = 1;
        _viewModel.Span = span;
    }
}
