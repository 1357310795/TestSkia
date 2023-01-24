using Microsoft.Maui.Animations;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using TestSkia.Extensions;
using TestSkia.Helpers;

namespace TestSkia.Controls;

public class ChargingRingDrawable
{
    private readonly ChargingRing view;
    private SKImage tmp;

    public ChargingRingDrawable(ChargingRing view)
    {
        this.view = view;
    }
    
    public void Draw(SKSurface surface, SKRect bounds)
    {
        surface.Canvas.Clear();
        //surface.Snapshot();

        //this.DrawDebugBack(surface.Canvas, bounds);
        this.DrawBackShape(surface.Canvas, bounds);
        this.DrawRipple(surface.Canvas, bounds);
        this.DrawCircle(surface.Canvas, bounds);
        this.DrawTopCircle(surface.Canvas, bounds);
        this.DrawText(surface.Canvas, bounds);
        //this.DrawCircularBackgroundShadow(surface.Canvas,surface, bounds);
        //this.DrawCircularBackground(surface.Canvas, bounds);
        //this.DrawCircularIndeterminateAnimation(surface.Canvas, bounds);
        
    }

    private void DrawDebugBack(
        SKCanvas canvas,
        SKRect bounds
    )
    {
        canvas.DrawRect(0, 0, bounds.Width, bounds.Height, new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = Colors.DodgerBlue.ToSKColor().WithAlpha(50),
            BlendMode = SKBlendMode.SrcOver,
            IsAntialias = false,
        });
    }

    private void DrawBackShape(SKCanvas canvas, SKRect bounds)
    {
        var canvassize = bounds.Height;
        var scale = 280f / canvassize;
        var shapes = ChargingRingShape.GetShapes();
        
        foreach (var shape in shapes)
        {
            SKPath shapepath = SKPath.ParseSvgPathData(shape.Path);
            shapepath.Offset(shape.OffsetX, shape.OffsetY);
            var paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = shape.Color.WithAlpha(36),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = false,
            };
            float degree = this.view.AnimationPercent * shape.N * 360f;
            float size = 1f + 0.05f * (float)Math.Sin(this.view.AnimationPercent * 200 + shape.Phrase);
            canvas.Scale(1 / scale, 1 / scale, 0, 0);
            canvas.Scale(1 / size, 1 / size, 280f / 2, 280f / 2);
            canvas.RotateDegrees(degree, 280f / 2, 280f / 2);
            
            canvas.DrawPath(shapepath, paint);
            canvas.ResetMatrix();
        }
    }

    private void DrawRipple(SKCanvas canvas, SKRect bounds)
    {
        const int rippleCount = 25;
        double ripplePercent = this.view.AnimationPercent * rippleCount - (int)(this.view.AnimationPercent * rippleCount);
        double first = ripplePercent.GetPercent(0.1, 0.7);
        double second = ripplePercent.GetPercent(0.3, 0.9);
        
        var canvassize = bounds.Height;
        var circlesize = canvassize / 280f * 200f;

        if (first > 0)
        {
            var first_1 = CubicBezierHelper.GetProgress(".25,0,.35,.83", first);
            var first_2 = CubicBezierHelper.GetProgress(".37,.01,.53,.77", first);
            canvas.DrawCircle(new SKPoint(canvassize / 2, canvassize / 2), circlesize / 2 * (1f).Lerp(1.6f, first_1), new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White.WithAlpha((byte)(150d).Lerp(0d, first_2)),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = false,
            });
        }

        if (second > 0)
        {
            var second_1 = CubicBezierHelper.GetProgress(".25,0,.35,.83", second);
            var second_2 = CubicBezierHelper.GetProgress(".37,.01,.53,.77", second);
            canvas.DrawCircle(new SKPoint(canvassize / 2, canvassize / 2), circlesize / 2 * (1f).Lerp(1.6f, second_1), new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White.WithAlpha((byte)(150d).Lerp(0d, second_2)),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = false,
            });
        }
    }

    private void DrawCircle(SKCanvas canvas, SKRect bounds)
    {
        var canvassize = bounds.Height;
        var circlesize = canvassize / 280f * 200f;
        canvas.DrawCircle(new SKPoint(canvassize / 2, canvassize / 2), circlesize / 2, new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White,
            BlendMode = SKBlendMode.SrcOver,
            IsAntialias = true,
        });
    }

    private void DrawTopCircle(SKCanvas canvas, SKRect bounds)
    {
        var FillShadowColor = SKColor.Parse("#0d113f").WithAlpha(60);
        var FillShadow = SKImageFilter.CreateDropShadow(1f, 6f, 6f, 6f, FillShadowColor, null, null);
        var FillColor = new SKColor(255, 255, 255, 255);
        var FillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = FillColor,
            BlendMode = SKBlendMode.SrcOver,
            IsAntialias = true,
            ImageFilter = FillShadow
        };
        
        var canvassize = bounds.Height;
        var circlesize = canvassize / 280f * 140f;
        canvas.DrawCircle(new SKPoint(canvassize / 2, canvassize / 2), circlesize / 2, FillPaint);
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        var canvassize = bounds.Height;
        var text = $"{(this.view.AnimationPercent*100).ToString("0.")}%";
        //var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("Gordita");
        //var font = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
        var paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse("#7a2b8a"),
            BlendMode = SKBlendMode.SrcOver,
            IsAntialias = true,
            TextSize = 60,
            //Typeface = font
        };
        var size = paint.MeasureText(text);
        canvas.DrawText(text, canvassize / 2 - size / 2, canvassize / 2 + 20, paint);
    }

    private void DrawCircularBackgroundShadow(
        SKCanvas canvas,
        SKSurface surface, 
        SKRect bounds
    )
    {
        if (tmp != null)
        {
            canvas.DrawImage(tmp, new SKPoint(0, 0));
            return;
        }
        var FillShadowColor = SKColors.DodgerBlue.WithAlpha(120);
        var FillShadow = SKImageFilter.CreateDropShadow(0f, 0f, 20f, 20f, FillShadowColor, null, null);
        var FillColor = new SKColor(255, 255, 255, 255);
        var FillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = FillColor,
            BlendMode = SKBlendMode.SrcOver,
            IsAntialias = true,
            ImageFilter = FillShadow
        };
        var radii = new CornerRadius(bounds.Width * 0.5f).GetRadii();
        
        var path = new SKPath();
        foreach (var point in radii)
        {
            if (point.X != 0 || point.Y != 0)
            {
                var rect = new SKRoundRect();
                rect.SetRectRadii(bounds, radii);
                path.AddRoundRect(rect);
                canvas.DrawPath(path, FillPaint);
                canvas.Restore();
                return;
            }
        }
        path.AddRect(bounds);
        canvas.DrawPath(path, FillPaint);

        if (tmp == null)
        {
            tmp = surface.Snapshot();
        }
    }

    private void DrawCircularBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = Colors.LightBlue.MultiplyAlpha(0.2f);
        var radii = new CornerRadius(bounds.Width * 0.5f).GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawCircularIndeterminateAnimation(SKCanvas canvas, SKRect bounds)
    {
        //canvas.Save();
        bounds.Inflate(-10f, -10f);
        var paint = new SKPaint
        {
            Color = Colors.DodgerBlue.ToSKColor(),
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 4,
        };
        var percent = this.view.AnimationPercent;
        canvas.DrawArc(bounds, 360 * percent, 320 * percent, false, paint);
        //canvas.Restore();
    }
}
