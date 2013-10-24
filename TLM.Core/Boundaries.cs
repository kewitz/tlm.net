using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public struct Boundaries
    {
        public double Top, Left, Bottom, Right;

        public Boundaries(double top, double bottom, double left, double right)
        {
            this.Top = top;
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
        }
    }
}
