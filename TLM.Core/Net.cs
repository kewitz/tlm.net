using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILNumerics;

namespace TLM.Core
{
    [Serializable]
    public class Net
    {
        public double dL, Ylt, Er, c, f0, x, y, Z0, sigma, lambda0, tal0, tc, dT, Vlt, Zlt;
        public int N;
        public int[] shape;
        public List<Node> Nodes;
        public ILNumerics.ILMath ilAlg;
        public Boundaries boundaries;

        public Net() { }
        public Net(double sizeX, double sizeY, double sigma, double dL, double Z0, double Er, double f0, double c, int N, Boundaries bounds)
        {
            ILRetArray<double> vecX = ILNumerics.ILMath.vec<double>(0, dL, sizeX);
            ILRetArray<double> vecY = ILNumerics.ILMath.vec<double>(0, dL, sizeY);
            double sqrt2 = Math.Sqrt(2.0);

            this.shape = new int[2] { vecX.Count(), vecY.Count() };
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
            node.Vi.P1[k] = node.j < shape[1] ? GetNode(node.i, node.j + 1).Vr.P3[k - 1] : this.boundaries.Bottom * node.Vr.P1[k - 1];
            node.Vi.P2[k] = node.i > 0 ? GetNode(node.i - 1, node.j).Vr.P4[k - 1] : this.boundaries.Left * node.Vr.P2[k - 1];
            node.Vi.P3[k] = node.j > 0 ? GetNode(node.i, node.j - 1).Vr.P1[k - 1] : this.boundaries.Top * node.Vr.P3[k - 1];
            node.Vi.P4[k] = node.i < shape[0] ? GetNode(node.i+1, node.j).Vr.P2[k - 1] : this.boundaries.Right * node.Vr.P4[k - 1];
            node.Vi.P5[k] = node.Vr.P5[k - 1];
        }

    }
}

/*
    def run(self):
        print "Starting simulation..."
        for k in range(self.N):
            #Excitação
            for node in self.ExctNodes:
                self.Fk(k=k,node=node)
            #Logica em nivel de iteração.
            for node in filter(lambda n: n.needSolve(k) ,self.net.nodes):
                node.solveScatter(k)
            for node in self.net.nodes:
                self.net.setVi(node,k+1)
            print "%s/%s iteration done." % (k, self.N)
*/