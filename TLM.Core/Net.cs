using ILNumerics;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public class Net
    {
        public event EventHandler<int> Progress;
        public event EventHandler<string> StatusUpdate;
        public event EventHandler CalcDone;

        public double dL { get; set; }
        public double Ylt { get; set; }
        public double c { get; set; }
        public double f0 { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double Z0 { get; set; }
        public double lambda0 { get; set; }
        public double tal0 { get; set; }
        public double tc { get; set; }
        public double dT { get; set; }
        public double Vlt { get; set; }
        public double Zlt { get; set; }
        public int N { get; set; }
        public int[] shape { get; set; }
        public Boundaries boundaries { get; set; }
        public string Fk { get; set; }
        public Material material { get; set; }
        public int mode { get; set; }

        public List<Node> Nodes = new List<Node>();
        public List<Material> matList = new List<Material>() {
                new Material("Air", 1, 1, 5E-15),
                new Material("Copper", 1, 1, 5.8E7),
                new Material("Teflon", 2.1, 1, 1E-24),
                new Material("Wood", 4, 1, 1E-15),
                new Material("Water(20°C)", 80.1, 1, 5E-3),
                new Material("Ferrite", 1, 320, 5E-15),
                new Material("Carbon Steel", 1, 300, 5E-15),
                new Material("Permalloy", 1, 8000, 5E-15),
                new Material("Concrete", 5, 1, 13E-3),
                new Material("Glass", 4.7, 1, 1E-13),
                new Material("Rubber", 7, 1, 1E-14),
                new Material("Diamond", 7.5, 1, 1E-13),
                new Material("Graphite", 1E-15, 1, 2.5E5),
                new Material("Silicon", 11.68, 1, 1.56E-3),
                new Material("Sulfur", 3.5, 1, 1E-16)
                
            };


        public Net() { }
        public Net(double sizeX, double sizeY, Material mat, double dL, double Z0, double f0, double c, int N, Boundaries bounds, int mode)
        {
            this.x = sizeX;
            this.y = sizeY;
            this.dL = dL;
            this.material = mat;
            this.c = c;
            this.f0 = f0;
            this.dL = dL;
            this.N = N;
            this.Z0 = Z0;
            this.boundaries = bounds;
            this.mode = mode;
            this.Calc(this.mode);
        }

        public void Calc(int mode)
        {
            Nodes.Clear();
            ILRetArray<double> vecX = ILNumerics.ILMath.vec<double>(0, this.dL, this.x).ToArray();
            ILRetArray<double> vecY = ILNumerics.ILMath.vec<double>(0, this.dL, this.y).ToArray();
            this.shape = new int[2] { vecX.Count(), vecY.Count() };

            double sqrt2 = Math.Sqrt(2.0);
            this.lambda0 = this.c / this.f0;
            this.tal0 = this.lambda0 / this.c;
            this.tc = (5 * this.tal0) / 2;
            this.dT = this.dL / (sqrt2 * this.c);
            this.Vlt = sqrt2 * this.c;
            this.Zlt = (mode == 0) ? sqrt2 * this.Z0 : this.Z0 / sqrt2;
            this.Ylt = 1 / this.Zlt;

            foreach (var x in vecX)
            {
                int j = vecX.ToList().IndexOf(x);
                foreach (var y in vecY)
                {
                    int i = vecY.ToList().IndexOf(y);
                    //bool input = j == 0;
                    bool input = false;
                    Node newNode = new Node(i, j, this.material, this.dL, this.Ylt, this.N, this.mode, input);
                    Nodes.Add(newNode);
                }
            }
        }

        public Material GetMaterial(string name)
        {
            return (from mat in this.matList where mat.Name == name select mat).FirstOrDefault();
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
            node.Vi.P1[k] = node.j < shape[0] - 1 ? GetNode(node.i, node.j + 1).Vr.P3[k - 1] : this.boundaries.Right * node.Vr.P1[k - 1];
            node.Vi.P2[k] = node.i > 0 ? GetNode(node.i - 1, node.j).Vr.P4[k - 1] : this.boundaries.Top * node.Vr.P2[k - 1];
            node.Vi.P3[k] = node.j > 0 ? GetNode(node.i, node.j - 1).Vr.P1[k - 1] : this.boundaries.Left * node.Vr.P3[k - 1];
            node.Vi.P4[k] = node.i < shape[1] - 1 ? GetNode(node.i + 1, node.j).Vr.P2[k - 1] : this.boundaries.Bottom * node.Vr.P4[k - 1];
            node.Vi.P5[k] = node.Vr.P5[k - 1];
        }

        public void Run()
        {
            Nodes.ForEach(n => n.ClearSimulation());
            for (int k = 0; k < this.N - 1; k++)
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

                var value = (double)exc.Evaluate();
                if (this.mode == 0)
                {
                    Parallel.ForEach(inputNodes, n => n.SetEz(k, value));
                }
                else 
                {
                    if (value != 0.0)
                    {
                        Parallel.ForEach(inputNodes, n => n.SetHz(k, n.CalcHz(value)));
                    }
                }

                //Solve Scatter
                Parallel.ForEach(Nodes, n => n.SolveScatter(k, mode));

                //Transmit
                Parallel.ForEach(Nodes, n => Transmit(n, k + 1));

                if (Progress != null)
                    Progress.Invoke(this, k);
            }
        }


    }
}