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
        public string Name;
        public double Er, Sigma;

        public Material() { }
        public Material(double er, double sigma)
        {
            this.Er = er;
            this.Sigma = sigma;
        }
    }
}
