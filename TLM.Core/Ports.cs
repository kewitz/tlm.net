using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ILNumerics;

namespace TLM.Core
{
    [Serializable]
    public struct Ports
    {
        public List<double> P1, P2, P3, P4, P5;

        public Ports(int size)
        {
            P1 = ILMath.zeros<double>(size).ToList();
            P2 = ILMath.zeros<double>(size).ToList();
            P3 = ILMath.zeros<double>(size).ToList();
            P4 = ILMath.zeros<double>(size).ToList();
            P5 = ILMath.zeros<double>(size).ToList();
        }

        public bool NeedToSolve(int k)
        {
            return this.P1[k] != 0 && this.P2[k] != 0 && this.P3[k] != 0 && this.P4[k] != 0;
        }

        public void ClearValues()
        {
            int size = P1.Count;
            P1 = ILMath.zeros<double>(size).ToList();
            P2 = ILMath.zeros<double>(size).ToList();
            P3 = ILMath.zeros<double>(size).ToList();
            P4 = ILMath.zeros<double>(size).ToList();
            P5 = ILMath.zeros<double>(size).ToList();
        }
    }
}
