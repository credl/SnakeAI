using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public abstract class NNNetwork
    {
        public abstract double[] propagateToEnd(double[] inputVec);
    }
}
