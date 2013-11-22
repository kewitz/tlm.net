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
        private Material _material;
        public double x, y, dL, Ylt, Gs, Ys, Y, Zlt, Zs, Rs, Z;
        public int i, j, mode;
        public bool input;
        public Material material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                RecalcParams(mode);
            }
        }
        public Ports Vi, Vr;

        public Node() { }
        public Node(int i, int j, Material mat, double dL, double Ylt, int N, int mode, bool input = false)
        {
            this.i = i;
            this.j = j;
            this.material = mat;            
            this.dL = dL;
            if (mode == 0) { this.Ylt = Ylt; } else { this.Zlt = 1 / Ylt; }
            this.x = j * dL;
            this.y = i * dL;           
            this.Vi = new Ports(N);
            this.Vr = new Ports(N);
            this.mode = mode;
            this.input = input;
            RecalcParams(mode);
        }       

        public void RecalcParams(int mode)
        {
            if (mode == 0)
            {
                this.Gs = (material.Sigma * dL) / Ylt;
                this.Ys = 4 * (material.Er - 1);
                this.Y = 4 + Ys + Gs;
            }
            else
            {
                this.Rs = (material.Sigma * dL) / Zlt;
                this.Zs = 4 * (material.mur - 1);
                this.Z = 4 + Zs + Rs;
            }
        }

        public void ClearSimulation()
        {
            this.Vi.ClearValues();
            this.Vr.ClearValues();
        }

        public void SetEz(int k, double Ez)
        {
            double vz = Ez * this.dL;
            double vi = ((vz - (2 * this.Vi.P5[k] * this.Ys)) * (4 * this.Ylt) + this.Ys + this.Gs) / (2 * 4 * this.Ylt);
            this.Vi.P1[k] = this.Vi.P2[k] = this.Vi.P3[k] = this.Vi.P4[k] = vi;
        }

        public void SetHz(int k, double Vy)
        {            
            this.Vi.P1[k] = this.Vi.P3[k] = Vy/2;
        }

        public void SolveScatter(int k, int mode)
        {
            //Scatter matrix.
            ILArray<double> s = (mode == 0) ? array<double>(
                                                    new double[] { 
                                                        2-this.Y, 2, 2, 2, 2,
                                                        2, 2-this.Y, 2, 2, 2,
                                                        2, 2, 2-this.Y, 2, 2,
                                                        2, 2, 2, 2-this.Y, 2,
                                                        2*this.Ys, 2*this.Ys, 2*this.Ys, 2*this.Ys, 2*this.Ys-this.Y
                                                    }, 5, 5) : 
                                              array<double>(
                                                    new double[] { 
                                                        this.Z-2, 2, 2, -2, -2 * this.Zs,
                                                        2, this.Z-2, -2, 2, 2 * this.Zs,
                                                        2, -2, this.Z-2, 2, 2 * this.Zs,
                                                        -2, 2, 2, this.Z-2, -2 * this.Zs,
                                                        -2, 2, 2, -2, this.Z - 2 * this.Zs
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
            ILArray<double> vr = ((mode == 0) ? (1 / this.Y) : (1 / this.Z)) * ILMath.multiply(s, vi);
            this.Vr.P1[k] = vr.ElementAt(0);
            this.Vr.P2[k] = vr.ElementAt(1);
            this.Vr.P3[k] = vr.ElementAt(2);
            this.Vr.P4[k] = vr.ElementAt(3);
            this.Vr.P5[k] = vr.ElementAt(4);
        }

        public double GetEz(int k)
        {
            double Ez = (2 * ((((this.Vi.P1[k]) +
                        (this.Vi.P2[k]) +
                        (this.Vi.P3[k]) +
                        (this.Vi.P4[k])) *
                        this.Ylt) +
                        (this.Vi.P5[k]) *
                        this.Ys)) / ((this.dL) *
                        ((4 * this.Ylt) +
                        this.Ys + this.Gs));
            return Ez;
        }

        public double GetHz(int k)
        {
            double Hz = (2 * ((this.Vi.P1[k]) -
                        (this.Vi.P2[k]) -
                        (this.Vi.P3[k]) +
                        (this.Vi.P4[k]) +
                        (this.Vi.P5[k]))) / ((this.dL) *
                        ((4 * this.Zlt) +
                        this.Zs + this.Rs));
            return Hz;
        }

        public double GetHy(Node node, int k)
        {
            double Hy = (node.Vi.P1[k] - node.Vi.P3[k]) / (dL * (1 / Ylt));
            return Hy;
        }

        public double GetHx(Node node, int k)
        {
            double Hx = (node.Vi.P4[k] - node.Vi.P2[k]) / (dL * (1 / Ylt));
            return Hx;
        }
        
        public double GetEy(Node node, int k)
        {
            double Ey = -(node.Vi.P1[k] + node.Vi.P3[k]) / dL;
            return Ey;
        }
        
        public double GetEx(Node node, int k)
        {
            double Ex = -(node.Vi.P2[k] + node.Vi.P4[k]) / dL;
            return Ex;
        }

        internal double CalcHz(double Ey)
        {
            double vy = -Ey * dL;
            return vy;
        }
    }
}
