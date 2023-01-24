using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSkia.Controls;
public class ChargingRingShape
{
    public static List<ChargingRingShape> Shapes;
    public static List<ChargingRingShape> GetShapes()
    {
        if (Shapes != null)
            return Shapes;

        Shapes = new List<ChargingRingShape>();
        Shapes.Add(new ChargingRingShape()
        {
            Path = "M5.32428 151.954C-8.54064 111.86 6.25547 49.9889 31.4551 12.6319C54.7372 -21.8823 95.1388 24.3274 129.108 33.063C181.856 46.628 188.454 113.429 204.982 130.228C220.767 146.272 241.24 168.78 226.673 196.469C197.37 252.168 77.448 274.015 29.4801 243.386C-6.00368 220.729 16.8051 185.154 5.32428 151.954Z",
            OffsetX = 25f,
            OffsetY = 10f,
            Color = SKColor.Parse("#FF6B6B")
        });
        Shapes.Add(new ChargingRingShape()
        {
            Path = "M234.477 250.313C212.964 281.285 180.746 305.972 136.021 300.479C94.6986 295.404 60.784 228.229 53.1459 193.997C41.2854 140.84 70.9782 86.0165 85.925 67.7961C101.387 48.9477 123.228 30.8396 154.448 32.8817C186.129 34.9538 206.879 38.8973 235.532 76.3754C261.102 109.821 254.517 221.461 234.477 250.313Z",
            OffsetX = -8f,
            OffsetY = -30f,
            Color = SKColor.Parse("#FFB56B")
        });
        Shapes.Add(new ChargingRingShape()
        {
            Path = "M2.93562 139.594C-5.41165 102.819 3.5227 76.6844 36.4356 29.0943C55.4814 1.55537 101.467 -5.14131 135.436 3.59434C188.184 17.1592 241.86 66.5348 242.437 90.0944C243.044 114.928 215.43 125.204 199.437 152.095C183.208 179.381 193.421 214.079 157.436 230.095C118.973 247.213 9.96153 170.548 2.93562 139.594Z",
            OffsetX = 30f,
            OffsetY = 35f,
            Color = SKColor.Parse("#B36BFF")
        });
        Shapes.Add(new ChargingRingShape()
        {
            Path = "M3.45735 145.978C-4.88988 109.203 -1.41791 35.8396 51.4574 12.3395C104.332 -11.1606 205.445 1.8265 223.457 26.8395C239.065 48.5124 213.514 101.312 203.575 130.978C193.597 160.764 138.858 244.491 97.4574 238.84C55.7439 233.146 10.4832 176.932 3.45735 145.978Z",
            OffsetX = 30f,
            OffsetY = 35f,
            Color = SKColor.Parse("#6BFF89")
        });
        Shapes.Add(new ChargingRingShape()
        {
            Path = "M1.11683 157.138C-7.23039 120.363 32.242 29.1376 85.1171 5.63754C137.992 -17.8625 194.605 38.1248 212.618 63.1377C228.225 84.8106 238.056 112.472 228.118 142.138C218.139 171.924 184.868 225.888 128.117 229.638C71.3672 233.388 8.14268 188.091 1.11683 157.138Z",
            OffsetX = 10f,
            OffsetY = 15f,
            Color = SKColor.Parse("#6B7CFF")
        });
        Random rd = new Random();
        foreach (var shape in Shapes)
        {
            shape.N = rd.Next(8, 12);
            if (rd.NextDouble() > 0.5)
                shape.N = -shape.N;
            shape.Phrase = (float)(rd.NextDouble() * 360);
        }
        return Shapes;
    }

    public string Path { get; set; }
    public int N { get; set; }
    public float Phrase { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public SKColor Color { get; set; }
}
