using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TLM.Core;

namespace Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Net net = new Net(10E-2, 2E-2, new Material("ar",1,5E-15) , 1E-3, 120 * Math.PI, 10E9, 300E6, 43, new Boundaries(1, 1, -0.17157, -0.17157));
            net.Fk = "Sin(2*[Pi]*[f0]*([k]+1)*[dT])";
            var inputnodes = (from node in net.Nodes where node.i == 0 select node).ToList();
            foreach (var node in inputnodes)
                node.input = true;

            net.Run();

            System.Console.ReadKey();
        }
    }
}
