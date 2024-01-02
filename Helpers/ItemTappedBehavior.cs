using System.Windows.Input;

namespace Months18.Helpers;
public class ItemTappedBehavior : Behavior<Grid>
{
    public static readonly BindableProperty TappedCommandProperty =
        BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ItemTappedBehavior), null);

    public ICommand TappedCommand
    {
        get => (ICommand)GetValue(TappedCommandProperty);
        set => SetValue(TappedCommandProperty, value);
    }

    protected override void OnAttachedTo(Grid bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnTapped()) });
    }

    private void OnTapped()
    {
        TappedCommand?.Execute(BindingContext);
    }
}
