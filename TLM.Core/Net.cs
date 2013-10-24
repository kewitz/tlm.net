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
        public double dL, Ylt, Er, c, f0, x, y, Z0, sigma;
        public int N;
        public List<Node> Nodes;
        public ILNumerics.ILMath ilAlg;
        public Boundaries boundaries;
        

        public void net(double sizeX, double sizeY, double sigma, double dL, double Ylt, double Er, double N, Boundaries bounds)
        {
            Nodes = new List<Node>();
            ILRetArray<double> vecX = ILNumerics.ILMath.vec<double>(0, dL, sizeX);
            ILRetArray<double> vecY = ILNumerics.ILMath.vec<double>(0, dL, sizeY);
            this.dL = dL;
            this.Ylt = Ylt;
            this.Er = Er;
            this.boundaries = bounds;
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

        }

    }
}

/*
def __init__(self, sizeX, sizeY, sigma, dL, Ylt, Er, N, boundaries={'top':1, 'bottom':1, 'left':-0.17157, 'right':-0.17157}):
        vecX = np.arange(0,sizeX,dL)
        vecY = np.arange(0,sizeY,dL)
        self.dL = dL
        self.Ylt = Ylt
        self.Er = Er
        self.nodes = []
        self.boundaries = boundaries
        self.x, self.y = np.meshgrid(vecX,vecY)
        for x in vecX:
            j = vecX.tolist().index(x)
            for y in vecY:
                i = vecY.tolist().index(y)
                newNode = Node(x,y,i,j, sigma, dL, Ylt, Er, N)
                self.nodes.append(newNode)

    def getNode(self,**param):
        """
        Parameters
        ----------
        mpos:
            Get node based on it's position in the ``matrix[i,j]``.
        >>> Net.getNode(mpos=[i,j])
        """
        if param.has_key('mpos'):
            i, j = (param['mpos'][0], param['mpos'][1])
            ns = filter(lambda n: n.i == i and n.j == j, self.nodes)
            return ns[0] if len(ns)==1 else Exception('Node not found in position i={} and j={}'.format(i,j))

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

    def createNet(self):
        """
        Create matrix based on x columns, y lines and spaced by dL meters.
        """
        self.net = Net(self.x,self.y,self.sigma,self.dL,self.Ylt,self.Er,self.N)
        print "Net created..."

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