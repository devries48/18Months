﻿using System.Diagnostics;

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

    private void CollectionView_SizeChanged(object sender, EventArgs e)
    {
        int span = (int)(ReleaseCollectionView.Width - 50) / MusicPageViewModel.DefaultItemWidth;
        if (span == 0) span = 1;
        _viewModel.Span = span;
    }

    private void View_Focused(object sender, FocusEventArgs e)
    {
        if (Toolbar.IsExpanded)
            Toolbar.IsExpanded = false;
    }
}
