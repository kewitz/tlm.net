using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNumerics.LinearAlgebra;

namespace TLM.Core
{
    [Serializable]
    public class Net
    {
        public float dL, Ylt, Er;
        public List<Node> Nodes;
        
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
*/