using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILNumerics;
using NCalc;

namespace TLM.Core
{
    [Serializable]
    public class Net
    {
        public double dL, Ylt, Er, c, f0, x, y, Z0, sigma, lambda0, tal0, tc, dT, Vlt, Zlt;
        public int N;
        public int[] shape;
        public List<Node> Nodes;
        public Boundaries boundaries;
        public string Fk;

        public Net() { }
        public Net(double sizeX, double sizeY, double sigma, double dL, double Z0, double Er, double f0, double c, int N, Boundaries bounds)
        {
            ILRetArray<double> vecX = ILNumerics.ILMath.vec<double>(0, dL, sizeX);
            ILRetArray<double> vecY = ILNumerics.ILMath.vec<double>(0, dL, sizeY);
            double sqrt2 = Math.Sqrt(2.0);

            this.shape = new int[2] { vecX.Count() - 1, vecY.Count() - 1 };
            this.dL = dL;
            this.Er = Er;
            this.c = c;
            this.f0 = f0;
            this.dL = dL;
            this.N = N;
            this.Z0 = Z0;
            this.Er = Er;
            this.sigma = sigma;
            this.lambda0 = this.c / this.f0;
            this.tal0 = this.lambda0 / this.c;
            this.tc = (5 * this.tal0) / 2;
            this.dT = this.dL / (sqrt2 * this.c);
            this.Vlt = sqrt2 * this.c;
            this.Zlt = sqrt2 * this.Z0;
            this.Ylt = 1 / this.Zlt;
            this.boundaries = bounds;

            Nodes = new List<Node>();
            foreach (var x in vecX)
            {
                int j = vecX.ToList().IndexOf(x);
                foreach (var y in vecY)
                {
                    int i = vecY.ToList().IndexOf(y);
                    Node newNode = new Node(i, j, this.sigma, this.dL, this.Ylt, this.Er, this.N);
                    Nodes.Add(newNode);
                }
            }
        }

        public Node GetNode(int i, int j)
        {
            Node n = (from node in this.Nodes
                      where node.i == i && node.j == j
                      select node).FirstOrDefault();
            return n;
        }

        public void Transmit(Node node, int k)
        {
            node.Vi.P1[k] = node.j < shape[0] ? GetNode(node.i, node.j + 1).Vr.P3[k - 1] : this.boundaries.Bottom * node.Vr.P1[k - 1];
            node.Vi.P2[k] = node.i > 0 ? GetNode(node.i - 1, node.j).Vr.P4[k - 1] : this.boundaries.Left * node.Vr.P2[k - 1];
            node.Vi.P3[k] = node.j > 0 ? GetNode(node.i, node.j - 1).Vr.P1[k - 1] : this.boundaries.Top * node.Vr.P3[k - 1];
            node.Vi.P4[k] = node.i < shape[1] ? GetNode(node.i + 1, node.j).Vr.P2[k - 1] : this.boundaries.Right * node.Vr.P4[k - 1];
            node.Vi.P5[k] = node.Vr.P5[k - 1];
        }

        public void Run()
        {
            for (int k = 0; k < this.N -1; k++)
            {
                //Excitação
                Expression exc = new Expression(Fk);
                exc.Parameters["k"] = k;
                exc.Parameters["dT"] = dT;
                exc.Parameters["f0"] = f0;
                exc.Parameters["Pi"] = Math.PI;
                var inputNodes = (from node in Nodes
                                  where node.input == true
                                  select node).ToList();
                foreach (Node node in inputNodes)
                {
                    var value = (double)exc.Evaluate();
                    node.SetEz(k, value);
                }
                //Solve Scatter
                var needSolve = (from node in Nodes
                                 where node.Vi.NeedToSolve(k)
                                 select node).ToList();
                foreach (Node node in needSolve)
                    node.SolveScatter(k);
                //Transmit
                foreach (Node node in Nodes)
                    Transmit(node, k + 1);
            }
        }

    }
}