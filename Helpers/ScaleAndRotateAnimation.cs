using CommunityToolkit.Maui.Animations;

namespace Months18.Helpers;

public class ScaleAndRotateAnimation : BaseAnimation
{
    // A composite animation is a combination of animations where two or more animations run simultaneously.
    // Composite animations can be created by combining awaited and non-awaited animations
    // see: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/animation/basic?view=net-maui-8.0
    public override async Task Animate(VisualElement view, CancellationToken token)
    {
        view.Rotation = 0;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        view.RotateTo(360, Length * 2, Easing.CubicInOut);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


        await view.ScaleTo(1.2, Length, Easing).WaitAsync(token);
        await view.ScaleTo(1, Length, Easing).WaitAsync(token);
    }
}
