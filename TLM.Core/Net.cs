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
    def setVi(self,node,k):
        """
        Return the incident voltages for a `node` in the `k` iteration for each
        port. Boundary conditions are considered.

            :math:`_kV_1^i(i,j) = _{k-1}V_3^r(i,j+1)`
        """
        maxi, maxj = max(map(lambda x: (x.i ,x.j), self.nodes))
        node.Vi[1][k] = self.getNode(mpos=[node.i,node.j+1]).Vr[3][k-1] if node.j < maxj else self.boundaries["bottom"]*node.Vr[1][k-1]
        node.Vi[2][k] = self.getNode(mpos=[node.i-1,node.j]).Vr[4][k-1] if node.i > 0 else self.boundaries["left"]*node.Vr[2][k-1]
        node.Vi[3][k] = self.getNode(mpos=[node.i,node.j-1]).Vr[1][k-1] if node.j > 0 else self.boundaries["top"]*node.Vr[3][k-1]
        node.Vi[4][k] = self.getNode(mpos=[node.i+1,node.j]).Vr[2][k-1] if node.i < maxi else self.boundaries["right"]*node.Vr[4][k-1]
        node.Vi[5][k] = node.Vr[5][k-1]
        #Vis = dict(zip([1,2,3,4,5],[V1,V2,V3,V4,V5]))

 * def __init__(self,c,f0,dL,x,y,N,Z0,Er,sigma):
        self.c = c
        self.f0 = f0
        self.dL = dL
        self.x = x
        self.y = y
        self.N = N
        self.Z0 = Z0
        self.Er = Er
        self.sigma = sigma
        self.Fk = None
        self.ExctNodes = []
        print "Simulation created..."
        self.calcParams()
        
    def calcParams(self):
        """
        Determinate parameters values for simulation.
        """
        self.lambda0 = self.c/self.f0
        self.tal0 = self.lambda0/self.c
        self.tc = (5*self.tal0)/2
        self.dT = self.dL/(np.sqrt(2)*self.c)
        self.Vlt = np.sqrt(2)*self.c
        self.Zlt = np.sqrt(2)*self.Z0
        self.Ylt = 1/self.Zlt
        print "Simulation parameters defined..."


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