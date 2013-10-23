using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    public static class Calc
    {
        public static List<double> doubles(double start, double end, double step = 1)
        {
            List<double> ret = new List<double>();
            double i = start;
            while (i < end)
            {
                ret.Add(i);
                i += step;
            }
            return ret;
        }
    }
}
