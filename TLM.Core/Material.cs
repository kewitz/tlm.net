using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TLM.Core
{
    [Serializable]
    public class Material
    {
        public string Name { get; set; }
        public double Er { get; set; }
        public double mur { get; set; }
        public double Sigma { get; set; }
        public System.Drawing.Color color { get; set; }

        public Material() { }
        public Material(string name, double er, double mur, double sigma) : this(name, er, mur, sigma, Color.White) { }
        public Material(string name, double er, double mur, double sigma, Color color)
        {
            this.Name = name;
            this.Er = er;
            this.mur = mur;
            this.Sigma = sigma;
            this.color = color;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
