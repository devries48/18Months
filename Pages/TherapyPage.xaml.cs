using System.Diagnostics;

namespace Months18.Pages;

public partial class TherapyPage : ContentPage
{
    public TherapyPage(TherapyPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private readonly TherapyPageViewModel _viewModel;
}