using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace TestSkia.Extensions;

internal static class DrawableExtensions
{
    internal static void DrawBackground(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        SKPoint[] radii
    )
    {
        canvas.Save();
        var paint = new SKPaint { Color = color.ToSKColor(), };
        var path = new SKPath();
        foreach (var point in radii)
        {
            if (point.X != 0 || point.Y != 0)
            {
                paint.IsAntialias = true;
                var rect = new SKRoundRect();
                rect.SetRectRadii(bounds, radii);
                path.AddRoundRect(rect);
                canvas.DrawPath(path, paint);
                canvas.Restore();
                return;
            }
        }
        path.AddRect(bounds);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static SKPoint[] GetRadii(this CornerRadius radius)
    {
        return new SKPoint[]
        {
            new SKPoint((float)radius.TopLeft, (float)radius.TopLeft),
            new SKPoint((float)radius.TopRight, (float)radius.TopRight),
            new SKPoint((float)radius.BottomRight, (float)radius.BottomRight),
            new SKPoint((float)radius.BottomLeft, (float)radius.BottomLeft),
        };
    }

    public static SKRectI Padding(this SKRectI rect, int padding)
    {
        return new SKRectI(
            rect.Left + padding,
            rect.Top + padding,
            rect.Right - padding,
            rect.Bottom - padding);
    }
}
