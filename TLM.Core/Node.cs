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
        public Ports Vi, Vr;

        public Node() { }
        public Node(int i, int j, double sigma, double dL, double Ylt, double Er, int N)
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
            ILArray<double> vr = (1 / this.Y) * s * vi;

            this.Vr.P1[k] = vr.ElementAt(0);
            this.Vr.P2[k] = vr.ElementAt(1);
            this.Vr.P3[k] = vr.ElementAt(2);
            this.Vr.P4[k] = vr.ElementAt(3);
            this.Vr.P5[k] = vr.ElementAt(4);
        }

    }
}

/*
 def setEz(self, k, Ez):
        """
        Parameters
        ----------
        k: int
            Iteration number
        Ez: double
            Electrical field at z axis
        Result
        ------
        Function translate the electrical field into voltages (Vi) for the
        system
        """
        Vz = Ez*self.netParams["dL"]
        Vi = ((4*Vz * self.netParams["Ylt"]) +
                (self.Vi[5][k] * self.Ys)/(2*((4 * self.netParams["Ylt"]) +
                self.Ys + self.Gs)))
        for n in [1,2,3,4]:
            self.Vi[n][k] = Vi

    def getEz(self, k):
        """
        Parameters
        ----------
        k: int
            Iteration number
        Result
        ------
        Return the Ez at k iteration
        """
        Ez = (2*((((self.Vi[1][k]) +
                        (self.Vi[2][k]) +
                        (self.Vi[3][k]) +
                        (self.Vi[4][k])) *
                        self.netParams["Ylt"]) +
                        (self.Vi[5][k]) *
                        self.Ys))/((self.netParams["dL"]) *
                        ((4*self.netParams["Ylt"]) +
                        self.Ys + self.Gs))
        return Ez
*/