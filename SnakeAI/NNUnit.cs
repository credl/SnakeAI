using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NNUnit
    {
        ActivationFunction func;

        public NNUnit(ActivationFunction func)
        {
            this.func = func;
        }

        public double activation(double input)
        {
            return func(input);
        }
    }
}
