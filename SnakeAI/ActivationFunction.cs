using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public delegate double ActivationFunction(double input);

    public class ActivationFunctions
    {
        public static ActivationFunction sigmoid = new ActivationFunction(
            delegate (double input) {
                return (1.0 / (1.0 + Math.Pow(Math.E, -input))); ;
            });

        public static ActivationFunction identity = new ActivationFunction(
            delegate (double input)
            {
                return input;
            });

        public static ActivationFunction sign = new ActivationFunction(
            delegate (double input)
            {
                return input >= 0 ? 1.0 : 0.0;
            });
    };
}
