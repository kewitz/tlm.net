using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLM.Core
{
    [Serializable]
    public class Material
    {
        public string Name { get; set; }
        public double Er { get; set; }
        public double Sigma { get; set; }

        public Material() { }
        public Material(string name, double er, double sigma)
        {
            this.Name = name;
            this.Er = er;
            this.Sigma = sigma;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
