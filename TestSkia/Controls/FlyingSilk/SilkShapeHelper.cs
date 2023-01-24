using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSkia.Helpers;

namespace TestSkia.Controls;
public static class SilkShapeHelper
{
    public const double MaxStartEndShift = 20d;
    public const double MaxSegShiftPercent = 0.8d;
    public const double MaxHeightOffsetPercent = 0.3d;
    public const double MinK = 3d;
    public const double MaxK = 8d;
    public static readonly double[] SegmentCountPossibility = { 0d, 1d, 1d, 0d, 0d };
    public const double MaxControlPointShift = 18d;
    public const double MinHeight = 60d;
    public const double MaxHeight = 100d;
    public const double MaxDuration = 3.0d;
    public const double MinDuration = 2.0d;

    public static SilkShape GetShape()
    {
        var shape = new SilkShape();
        var path = shape.Path = new SKPath();
        
        path.MoveTo(0, 0);
        path.LineTo(1, 0);
        var rd = new Random();
        var n = RandomHelper.GetPossResult(SegmentCountPossibility, 5);
        shape.Color = SKColor.FromHsl((float)(rd.NextDouble() * 360), 32, 84);
        shape.Duration = TimeSpan.FromSeconds(rd.NextDouble() * (MaxDuration - MinDuration) + MinDuration);
        shape.Height = rd.NextDouble() * (MaxHeight - MinHeight) + MinHeight;
        shape.SegCount = n;
        shape.StartEndShift = (rd.NextDouble() * 2 - 1) * MaxStartEndShift;
        List<double> segHeights = RandomHelper.GetDistributionList(MaxHeightOffsetPercent, shape.SegCount).Select(x => x * shape.Height).ToList();
        List<double> segShifts = RandomHelper.GetDistributionList(MaxSegShiftPercent, shape.SegCount).Select(x => x * shape.StartEndShift).ToList();
        List<Point> KeyPoints = new List<Point>();
        KeyPoints.Add(new Point(0, 0));
        for (int i = 0; i < n; i++)
        {
            KeyPoints.Add(new Point(KeyPoints.Last().X + segShifts[i], KeyPoints.Last().Y + segHeights[i]));
            if (segShifts[i]> 50)
            {
                Debug.WriteLine(">50");
            }
        }

        List<double> Ks = new List<double>();
        List<Point> Cps = new List<Point>();
        Ks.Add((rd.NextDouble() * (MaxK - MinK) + MinK) * (rd.NextDouble() > 0.5 ? -1 : 1));
        
        for (int i = 1; i <= n; i++)
        {
            double cpY = 0d;
            double cpX = 0d;
            double nextK = 0d;

            bool isValid = false;
            do 
            {
                cpY = CubicBezierHelper.GetProgress("0,1,1,0", rd.NextDouble()) * (KeyPoints[i].Y - KeyPoints[i - 1].Y) + KeyPoints[i - 1].Y;
                cpX = (cpY - KeyPoints[i - 1].Y) / Ks[i - 1] + KeyPoints[i - 1].X;
                nextK = (KeyPoints[i].Y - cpY) / (KeyPoints[i].X - cpX);
                isValid = Math.Abs(nextK) <= MaxK && Math.Abs(nextK) >= Math.Min(MinK, 0.8 * Math.Abs((KeyPoints[i].Y - KeyPoints[i - 1].Y) / (KeyPoints[i].X - KeyPoints[i - 1].X)));
            }
            while (!isValid);

            Ks.Add(nextK);
            Cps.Add(new Point(cpX, cpY));

            path.QuadTo(
                (float)(cpX + 1),
                (float)(cpY),
                (float)(KeyPoints[i].X + 1),
                (float)(KeyPoints[i].Y));
        }
        path.LineTo((float)KeyPoints[n].X, (float)KeyPoints[n].Y);

        for (int i = n; i >= 1; i--)
        {
            path.QuadTo(
                (float)(Cps[i - 1].X),
                (float)(Cps[i - 1].Y),
                (float)(KeyPoints[i - 1].X),
                (float)(KeyPoints[i - 1].Y));
        }
        path.Close();

        shape.StartTime = DateTime.Now.Ticks;
        var tmp = shape.Path.ToSvgPathData();
        return shape;
    }
}
