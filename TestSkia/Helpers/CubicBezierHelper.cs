using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSkia.Helpers;
public static class CubicBezierHelper
{
    public static double GetProgress(string bezier, double linearProgress)
    {
        var points = bezier.Split(',').Select(double.Parse).ToArray();
        var p1 = new Point(points[0], points[1]);
        var p2 = new Point(points[2], points[3]);

        var key = new KeySpline(p1, p2);

        return key.GetSplineProgress(linearProgress);
    }

}

public class KeySpline
{
    /// <summary>初始化 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 类的新实例。</summary>
    // Token: 0x06002F9A RID: 12186 RVA: 0x0012B01C File Offset: 0x0012981C
    public KeySpline()
    {
        this._controlPoint1 = new Point(0.0, 0.0);
        this._controlPoint2 = new Point(1.0, 1.0);
    }

    /// <summary>使用指定的控制点坐标初始化 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 类的新实例。</summary>
    /// <param name="x1">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint1" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的 X 坐标。</param>
    /// <param name="y1">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint1" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的 Y 坐标。</param>
    /// <param name="x2">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint2" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的 X 坐标。</param>
    /// <param name="y2">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint2" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的 Y 坐标。</param>
    // Token: 0x06002F9B RID: 12187 RVA: 0x0012B069 File Offset: 0x00129869
    public KeySpline(double x1, double y1, double x2, double y2)
        : this(new Point(x1, y1), new Point(x2, y2))
    {
    }

    /// <summary>使用指定的控制点初始化 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 类的新实例。</summary>
    /// <param name="controlPoint1">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint1" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的控制点。</param>
    /// <param name="controlPoint2">
    ///   <see cref="P:System.Windows.Media.Animation.KeySpline.ControlPoint2" /> 的 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的控制点。</param>
    // Token: 0x06002F9C RID: 12188 RVA: 0x0012B080 File Offset: 0x00129880
    public KeySpline(Point controlPoint1, Point controlPoint2)
    {
        if (!this.IsValidControlPoint(controlPoint1))
            throw new ArgumentException("Animation_KeySpline_InvalidValue");
        if (!this.IsValidControlPoint(controlPoint2))
            throw new ArgumentException("Animation_KeySpline_InvalidValue");
        this._controlPoint1 = controlPoint1;
        this._controlPoint2 = controlPoint2;
        this._isDirty = true;
    }

    /// <summary>用于定义描述 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的贝塞尔曲线的第一个控制点。</summary>
    /// <returns>贝塞尔曲线的第一个控制点。 该点的 <see cref="P:System.Windows.Point.X" /> 和 <see cref="P:System.Windows.Point.Y" /> 值都必须介于 0 和 1 之间（包括 0 和 1）。 默认值为 (0,0)。</returns>
    // Token: 0x17000908 RID: 2312
    // (get) Token: 0x06002FA3 RID: 12195 RVA: 0x0012B1AD File Offset: 0x001299AD
    // (set) Token: 0x06002FA4 RID: 12196 RVA: 0x0012B1BC File Offset: 0x001299BC
    public Point ControlPoint1
    {
        get
        {
            return this._controlPoint1;
        }
        set
        {
            if (value != this._controlPoint1)
            {
                if (!this.IsValidControlPoint(value))
                    throw new ArgumentException("Animation_KeySpline_InvalidValue");
                this._controlPoint1 = value;
            }
        }
    }

    /// <summary>用于定义描述 <see cref="T:System.Windows.Media.Animation.KeySpline" /> 的贝塞尔曲线的第二个控制点。</summary>
    /// <returns>贝塞尔曲线的第二个控制点。 该点的 <see cref="P:System.Windows.Point.X" /> 和 <see cref="P:System.Windows.Point.Y" /> 值都必须介于 0 和 1 之间（包括 0 和 1）。 默认值为 (1,1)。</returns>
    // Token: 0x17000909 RID: 2313
    // (get) Token: 0x06002FA5 RID: 12197 RVA: 0x0012B21A File Offset: 0x00129A1A
    // (set) Token: 0x06002FA6 RID: 12198 RVA: 0x0012B228 File Offset: 0x00129A28
    public Point ControlPoint2
    {
        get
        {
            return this._controlPoint2;
        }
        set
        {
            if (value != this._controlPoint2)
            {
                if (!this.IsValidControlPoint(value))
                    throw new ArgumentException("Animation_KeySpline_InvalidValue");
                this._controlPoint2 = value;
            }
        }
    }

    /// <summary>从所提供的线性进度计算样条进度。</summary>
    /// <param name="linearProgress">要计算的线性进度。</param>
    /// <returns>计算的样条进度。</returns>
    // Token: 0x06002FA7 RID: 12199 RVA: 0x0012B286 File Offset: 0x00129A86
    public double GetSplineProgress(double linearProgress)
    {
        if (this._isDirty)
            this.Build();
        if (!this._isSpecified)
            return linearProgress;
        this.SetParameterFromX(linearProgress);
        return GetBezierValue(this._By, this._Cy, this._parameter);
    }

    // Token: 0x06002FA8 RID: 12200 RVA: 0x0012B2C4 File Offset: 0x00129AC4
    private bool IsValidControlPoint(Point point)
    {
        return point.X >= 0.0 && point.X <= 1.0;
    }

    // Token: 0x06002FA9 RID: 12201 RVA: 0x0012B2F0 File Offset: 0x00129AF0
    private void Build()
    {
        if (this._controlPoint1 == new Point(0.0, 0.0) && this._controlPoint2 == new Point(1.0, 1.0))
            this._isSpecified = false;
        else
        {
            this._isSpecified = true;
            this._parameter = 0.0;
            this._Bx = 3.0 * this._controlPoint1.X;
            this._Cx = 3.0 * this._controlPoint2.X;
            this._Cx_Bx = 2.0 * (this._Cx - this._Bx);
            this._three_Cx = 3.0 - this._Cx;
            this._By = 3.0 * this._controlPoint1.Y;
            this._Cy = 3.0 * this._controlPoint2.Y;
        }
        this._isDirty = false;
    }

    // Token: 0x06002FAA RID: 12202 RVA: 0x0012B410 File Offset: 0x00129C10
    public static double GetBezierValue(double b, double c, double t)
    {
        var num = 1.0 - t;
        var num2 = t * t;
        return b * t * num * num + c * num2 * num + num2 * t;
    }

    // Token: 0x06002FAB RID: 12203 RVA: 0x0012B440 File Offset: 0x00129C40
    private void GetXAndDx(double t, out double x, out double dx)
    {
        var num = 1.0 - t;
        var num2 = t * t;
        var num3 = num * num;
        x = this._Bx * t * num3 + this._Cx * num2 * num + num2 * t;
        dx = this._Bx * num3 + this._Cx_Bx * num * t + this._three_Cx * num2;
    }

    // Token: 0x06002FAC RID: 12204 RVA: 0x0012B49C File Offset: 0x00129C9C
    private void SetParameterFromX(double time)
    {
        var num = 0.0;
        var num2 = 1.0;
        if (time == 0.0)
        {
            this._parameter = 0.0;
            return;
        }
        if (time == 1.0)
        {
            this._parameter = 1.0;
            return;
        }
        while (num2 - num > 1E-06)
        {
            double num3;
            double num4;
            this.GetXAndDx(this._parameter, out num3, out num4);
            var num5 = Math.Abs(num4);
            if (num3 > time)
                num2 = this._parameter;
            else
                num = this._parameter;
            if (Math.Abs(num3 - time) < 0.001 * num5)
                break;
            if (num5 > 1E-06)
            {
                var num6 = this._parameter - (num3 - time) / num4;
                if (num6 >= num2)
                    this._parameter = (this._parameter + num2) / 2.0;
                else if (num6 <= num)
                    this._parameter = (this._parameter + num) / 2.0;
                else
                    this._parameter = num6;
            }
            else
                this._parameter = (num + num2) / 2.0;
        }
    }

    // Token: 0x04001F1D RID: 7965
    private Point _controlPoint1;

    // Token: 0x04001F1E RID: 7966
    private Point _controlPoint2;

    // Token: 0x04001F1F RID: 7967
    private bool _isSpecified;

    // Token: 0x04001F20 RID: 7968
    private bool _isDirty;

    // Token: 0x04001F21 RID: 7969
    private double _parameter;

    // Token: 0x04001F22 RID: 7970
    private double _Bx;

    // Token: 0x04001F23 RID: 7971
    private double _Cx;

    // Token: 0x04001F24 RID: 7972
    private double _Cx_Bx;

    // Token: 0x04001F25 RID: 7973
    private double _three_Cx;

    // Token: 0x04001F26 RID: 7974
    private double _By;

    // Token: 0x04001F27 RID: 7975
    private double _Cy;

    // Token: 0x04001F28 RID: 7976
    private const double accuracy = 0.001;

    // Token: 0x04001F29 RID: 7977
    private const double fuzz = 1E-06;
}