using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ILNumerics;

namespace TLM.Core
{
    [Serializable]
    public class Node : ILNumerics.ILMath
    {
        public double x, y, sigma, Er, dL, Ylt, Gs, Ys, Y;
        public int i, j;
        public bool input;
        public Ports Vi, Vr;

        public Node() { }
        public Node(int i, int j, double sigma, double dL, double Ylt, double Er, int N, bool input = false)
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
            this.Vi = new Ports(N);
            this.Vr = new Ports(N);
            this.input = input;
        }

        public void SetEz(int k, double Ez)
        {
            double vz = Ez * this.dL;
            double vi = ((4 * vz * this.Ylt) + (this.Vi.P5[k] * this.Ys) / (2 * ((4 * this.Ylt) + this.Ys + this.Gs)));
            this.Vi.P1[k] = this.Vi.P2[k] = this.Vi.P3[k] = this.Vi.P4[k] = vi;
        }

        public void SolveScatter(int k)
        {
            //Scatter matrix.
            ILArray<double> s = array<double>(
                    new double[] { 
                        2-this.Y, 2, 2, 2, 2*this.Ys,
                        2, 2-this.Y, 2, 2, 2*this.Ys,
                        2, 2, 2-this.Y, 2, 2*this.Ys,
                        2, 2, 2, 2-this.Y, 2*this.Ys,
                        2, 2, 2, 2, 2*this.Ys-this.Y
                    }, 5, 5);
            //Input Voltage array.
            ILArray<double> vi = array<double>(
                    new double[] { 
                        this.Vi.P1[k],
                        this.Vi.P2[k],
                        this.Vi.P3[k],
                        this.Vi.P4[k],
                        this.Vi.P5[k],
                    }, 5);
            //Solved reflected voltage array.
            ILArray<double> vr = (1 / this.Y) * ILMath.multiply(s,vi);
            this.Vr.P1[k] = vr.ElementAt(0);
            this.Vr.P2[k] = vr.ElementAt(1);
            this.Vr.P3[k] = vr.ElementAt(2);
            this.Vr.P4[k] = vr.ElementAt(3);
            this.Vr.P5[k] = vr.ElementAt(4);
        }
       
        public double GetEz(int k)
        {
            double Ez = (2*((((this.Vi.P1[k]) +
                        (this.Vi.P2[k]) +
                        (this.Vi.P3[k]) +
                        (this.Vi.P4[k])) *
                        this.Ylt) +
                        (this.Vi.P4[k]) *
                        this.Ys))/((this.dL) *
                        ((4*this.Ylt) +
                        this.Ys + this.Gs));
            return Ez;
        }
    }
}
