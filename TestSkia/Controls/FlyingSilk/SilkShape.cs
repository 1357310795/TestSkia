using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSkia.Controls;
public class SilkShape
{
    public SKPath Path { get; set; }
    public double Height { get; set; }
    public double StartEndShift { get; set; }
    public int SegCount { get; set; }
    public SKColor Color { get; set; }

    public long StartTime { get; set; }
    public TimeSpan Duration { get; set; }
}
