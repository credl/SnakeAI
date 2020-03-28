using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NNUnit
    {
        public double activation(double input)
        {
//            return (input >= 0 ? 1 : 0);
            return (double)(1.0 / (1.0 + Math.Pow(Math.E, -input)));
        }
    }
}
