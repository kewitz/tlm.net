using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public class Node
    {
        public double x, y, sigma, Er, dL, Ylt, Gs, Ys, Y;
        public int i, j;
        public Ports Vi;

        public Node(int i, int j, double sigma, double Er, double dL, double Ylt)
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
            this.Vi = new Ports();
        }

        public void SetEz(int k, double Ez)
        {
            double vz = Ez * this.dL;
            double vi = ((4 * vz * this.Ylt) + (this.Vi.P5[k] * this.Ys) / (2 * ((4 * this.Ylt) + this.Ys + this.Gs)));
            this.Vi.P1[k] = this.Vi.P2[k] = this.Vi.P3[k] = this.Vi.P4[k] = vi;
        }
    }

    [Serializable]
    public struct Ports
    {
        public List<double> P1, P2, P3, P4, P5;

        public Ports(int size)
        {
            P1 = new List<double>(size);
            P2 = new List<double>(size);
            P3 = new List<double>(size);
            P4 = new List<double>(size);
            P5 = new List<double>(size);
        }

        public bool NeedToSolve(int k)
        {
            return this.P1[k] != 0 && this.P2[k] != 0 && this.P3[k] != 0 && this.P4[k] != 0 && this.P5[k] != 0;
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

    def needSolve(self,k):
        """
        Check if any of the Vi ports are different than 0.
        """
        return not (self.Vi[1][k] == self.Vi[2][k] == self.Vi[3][k] == self.Vi[4][k] == self.Vi[5][k] == 0)
    
    def solveScatter(self,k):
        """
        Parameters
        ----------
        k:int
            Iteration number
        Result
        ------
        Solve the Scattering Matrix and sets the reflected voltage based on
        reference node incident voltage
        """
        #Assemble the scattering matrix.
        s = np.matrix([
            [2-self.Y,2,2,2,2*self.Ys],
            [2,2-self.Y,2,2,2*self.Ys],
            [2,2,2-self.Y,2,2*self.Ys],
            [2,2,2,2-self.Y,2*self.Ys],
            [2,2,2,2,2*self.Ys-self.Y],
            ])
        #Assemble the input voltage matrix.
        vi = np.matrix([
            [self.Vi[1][k]],
            [self.Vi[2][k]],
            [self.Vi[3][k]],
            [self.Vi[4][k]],
            [self.Vi[5][k]],
        ])
        #Calculate the reflex voltage matrix.
        vr = (1/self.Y)*s*vi
        self.Vr[1][k] = vr.tolist()[0][0]
        self.Vr[2][k] = vr.tolist()[1][0]
        self.Vr[3][k] = vr.tolist()[2][0]
        self.Vr[4][k] = vr.tolist()[3][0]
        self.Vr[5][k] = vr.tolist()[4][0]
        return vr
*/