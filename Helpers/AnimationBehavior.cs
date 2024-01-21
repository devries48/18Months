using CommunityToolkit.Maui.Behaviors;

namespace Months18.Helpers;

public class AnimationBehavior : BaseBehavior<VisualElement>
{
    public static readonly BindableProperty ShouldAnimateProperty =
        BindableProperty.Create(
        nameof(ShouldAnimate),
        typeof(bool),
        typeof(AnimationBehavior),
        false,
        propertyChanged: OnShouldAnimateChanged);

    public static readonly BindableProperty AnimationTypeProperty =
    BindableProperty.Create(
        nameof(AnimationType),
        typeof(AnimationType),
        typeof(AnimationBehavior),
        AnimationType.Fading);

    public static readonly BindableProperty MinimumOpacityProperty =
      BindableProperty.Create(nameof(MinimumOpacity), typeof(double), typeof(AnimationBehavior), 0.0);

    public static readonly BindableProperty AnimationLengthProperty =
    BindableProperty.Create(nameof(AnimationLength), typeof(uint), typeof(AnimationBehavior), (uint)250);

    public bool ShouldAnimate
    {
        get => (bool)GetValue(ShouldAnimateProperty);
        set => SetValue(ShouldAnimateProperty, value);
    }

    public AnimationType AnimationType
    {
        get => (AnimationType)GetValue(AnimationTypeProperty);
        set => SetValue(AnimationTypeProperty, value);
    }

    public double MinimumOpacity
    {
        get => (double)GetValue(MinimumOpacityProperty);
        set => SetValue(MinimumOpacityProperty, value);
    }

    public uint AnimationLength
    {
        get => (uint)GetValue(AnimationLengthProperty);
        set => SetValue(AnimationLengthProperty, value);
    }

    private static async void OnShouldAnimateChanged(BindableObject bindable, object oldValue, object newValue) => await Animate((AnimationBehavior)bindable);

    private static async Task Animate(AnimationBehavior behavior)
    {
        var element = behavior.View;

        if (element == null)
            return;

        switch (GetAnimationAction(behavior, element))
        {
            case AnimationAction.FadeIn:
                element.Opacity = behavior.MinimumOpacity;

                if (behavior.MinimumOpacity.Equals(0))
                    element.IsVisible = true;
 
                await element.FadeTo(1, behavior.AnimationLength, Easing.SinIn);
                break;

            case AnimationAction.FadeOut:
                await Task.Run(
                    async () =>
                    {
                        await element.FadeTo(behavior.MinimumOpacity, behavior.AnimationLength, Easing.SinIn);
                    })
                    .ContinueWith(
                        _ => HideElement(element, behavior.MinimumOpacity),
                        TaskScheduler.FromCurrentSynchronizationContext());
                break;

            case AnimationAction.ScaleIn:
                element.Scale = 0;
                element.IsVisible = true;

                await element.ScaleTo(1, behavior.AnimationLength, Easing.CubicInOut);
                break;

            case AnimationAction.ScaleOut:
                await Task.Run(
                    async () =>
                    {
                        await element.ScaleTo(0, behavior.AnimationLength, Easing.SpringIn);
                    })
                    .ContinueWith(
                        _ => HideElement(element, 0),
                        TaskScheduler.FromCurrentSynchronizationContext());
                break;
        }
    }

    private static AnimationAction GetAnimationAction(AnimationBehavior behavior, VisualElement element)
    {
        switch (behavior.AnimationType)
        {
            case AnimationType.Fading:
                return element.Opacity.IsApproximatelyEqual(1) && element.IsVisible ? AnimationAction.FadeOut : AnimationAction.FadeIn;

            case AnimationType.Scaling:
                return element.Scale.IsApproximatelyEqual(1) && element.IsVisible ? AnimationAction.ScaleOut : AnimationAction.ScaleIn;
        }
        return AnimationAction.None;
    }

    private static void HideElement(VisualElement element, double minOpacity)
    {
        if (minOpacity.Equals(0))
            element.IsVisible = false;
    }
}