using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public class Simulation
    {
        public float c, f0, dL, x, y, Z0, Er, sigma;
        public int N;


    }
}


/*
def __init__(self,c,f0,dL,x,y,N,Z0,Er,sigma):
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