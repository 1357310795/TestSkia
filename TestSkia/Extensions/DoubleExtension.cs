using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestSkia.Extensions;
public static class DoubleExtension
{
    public static double GetPercent(this double d, double l, double r)
    {
        if (d < l)
            return 0;
        if (d > r)
            return 1;
        return (d - l) / (r - l);
    }
}
