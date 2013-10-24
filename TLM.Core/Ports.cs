using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public struct Ports
    {
        public List<double> P1, P2, P3, P4, P5;

        public Ports(int size)
        {
            P1 = new List<double>(size);
            P2 = new List<double>(size);
            P3 = new List<double>(size);
            P4 = new List<double>(size);
            P5 = new List<double>(size);
        }

        public bool NeedToSolve(int k)
        {
            return this.P1[k] != 0 && this.P2[k] != 0 && this.P3[k] != 0 && this.P4[k] != 0 && this.P5[k] != 0;
        }
    }
}
