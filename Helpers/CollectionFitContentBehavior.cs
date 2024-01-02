using System.Diagnostics;

namespace Months18.Helpers;

public class CollectionFitContentBehavior : Behavior<CollectionView>
{
    List<View>? _itemsView;
    CollectionView? _control;
    protected override void OnAttachedTo(CollectionView bindable)
    {
        base.OnAttachedTo(bindable);
        _control = bindable;
        _control.ChildAdded += ChildsAdded;
        _itemsView = [];
    }

    protected override void OnDetachingFrom(CollectionView bindable)
    {
        base.OnDetachingFrom(bindable);
        _control.ChildAdded -= ChildsAdded;

        foreach (var item in _itemsView)
            item.SizeChanged -= ChildSize;
    }

    private void ChildsAdded(object? sender, ElementEventArgs e)
    {
        var cell = (e.Element as View);
        cell.SizeChanged += ChildSize;
        _itemsView.Add(cell);
    }

    private void ChildSize(object? sender, EventArgs e)
    {
        var cell = (sender as View);
        // _control.HeightRequest = _control.HeightRequest + cell.Height;
        Debug.Write("cell: " + cell.Height);
        Debug.WriteLine(", control: " + _control.GetType().ToString());
    }
}
