using Microsoft.Maui.Animations;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.ComponentModel;
using System.Diagnostics;
using TestSkia.Extensions;

namespace TestSkia.Controls;

public partial class ChargingRing : SKCanvasView
{
    private const double padding = 25;
    private readonly ChargingRingDrawable drawable;
    private IAnimationManager animationManager;
    internal float AnimationPercent { get; private set; } = 0f;

    public ChargingRing()
    {
        //this.EnableTouchEvents = true;
        this.drawable = new ChargingRingDrawable(this);
    }

    private void StartAnimation()
    {
        if (this.Handler is null)
            return;
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var animation = new Microsoft.Maui.Animations.Animation(
            callback: (progress) =>
            {
                this.AnimationPercent = 0f.Lerp(1f, progress);
                this.InvalidateSurface();
            }
        );
        animation.Duration = 100;
        animation.Easing = Easing.Linear;
        animation.Repeats = true;
        animation.Finished = () =>
        {
            
        };
        this.animationManager.Add(animation);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.drawable.Draw(e.Surface, e.Info.Rect);
    }
    
    protected override void OnHandlerChanged()
    {
        this.StartAnimation();
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var conssize = Math.Min(widthConstraint, heightConstraint);
        var setsize = Math.Min(WidthRequest == -1 ? double.PositiveInfinity : WidthRequest, HeightRequest == -1 ? double.PositiveInfinity : HeightRequest);
        var finalsize = Math.Min(conssize, setsize);
        if (finalsize - 2 * padding <= 0)
            return new Size(0, 0);
        if (double.IsInfinity(finalsize))
        {
            finalsize = 200;
            if (finalsize - 2 * padding <= 0)
                return new Size(0, 0);
            DesiredSize = new Size(finalsize);
            return DesiredSize;
        }
        else
        {
            DesiredSize = new Size(finalsize);
            return DesiredSize;
        }
            
        //return base.MeasureOverride(widthConstraint, heightConstraint);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        //Debug.WriteLine(e.ActionType + " " + e.DeviceType.ToString()+ " " +e.Pressure);
        base.OnTouch(e);
    }
}
