using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSkia.Helpers;
public static class RandomHelper
{
    public static List<double> GetDistributionList(double maxOffset, int n)
    {
        Random rd = new Random();
        List<double> list= new List<double>();
        for (int i = 0; i < n; i++)
            list.Add(1d);
        for (int i = 0; i < n; i++)
            list[i] = list[i] + (rd.NextDouble() * 2 - 1) * maxOffset * list[i];
        double sum = 0;
        for (int i = 0; i < n; i++)
            sum += list[i];
        for (int i = 0; i < n; i++)
            list[i] = list[i] / sum;
        return list;
    }

    public static int GetPossResult(double[] list, int n)
    {
        var sum = 0d;
        for (int i = 0; i < n; i++)
            sum += list[i];
        var rd = new Random();
        var r = rd.NextDouble() * sum;
        for (int i = 0; i < n; i++)
        {
            if (r < list[i])
                return i;
            r -= list[i];
        }
        return n - 1;
    }
}
