using CommunityToolkit.Maui.Animations;

namespace Months18.Helpers;

public class ScaleAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view, CancellationToken token)
    {
        await view.ScaleTo(1.2, Length, Easing).WaitAsync(token);
        await view.ScaleTo(1, Length, Easing).WaitAsync(token);
    }
}
