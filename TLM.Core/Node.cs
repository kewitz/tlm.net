using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public class Node
    {
        public float x, y, sigma, Er, dL, Ylt, Gs, Ys, Y;
        public int i, j;

        public Node(int i, int j, float sigma, float Er, float dL, float Ylt)
        {
            this.i = i;
            this.j = j;
            this.sigma = sigma;
            this.Er = Er;
            this.dL = dL;
            this.Ylt = Ylt;
            this.x = j * dL;
            this.y = i * dL;
            this.Gs = (sigma * dL) / Ylt;
            this.Ys = 4 * (Er - 1);
            this.Y = 4 + Ys + Gs;
        }
    }
}
