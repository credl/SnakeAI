using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetworks;

namespace NeuralNetworks
{
    class NNStackedRestrickedBoltzmannMachine : NNNetwork
    {
        NNRestrictedBoltzmannMachine[] rbms;
        double[][] outputOfRBM;

        public NNStackedRestrickedBoltzmannMachine(int[] unitsPerLayer) {
            rbms = new NNRestrictedBoltzmannMachine[unitsPerLayer.Length - 1];

            outputOfRBM = new double[unitsPerLayer.Length][];
            for (int layer = 0; layer < unitsPerLayer.Length - 1; layer++)
            {
                rbms[layer] = new NNRestrictedBoltzmannMachine(unitsPerLayer[layer], unitsPerLayer[layer + 1]);
                outputOfRBM[layer] = new double[unitsPerLayer[layer + 1]];
            }
        }

        public int getLayerCount()
        {
            return rbms.Length;
        }

        override public double[] propagateToEnd(double[] input, double[] storage = null) {
            return propagateToLayer(input, rbms.Length, storage);
        }

        public double[] propagateToLayer(double[] input, int layer, double[] storage = null) {
            double[] cur = input;
            for (int l = 0; l < layer; l++)
            {
                cur = rbms[l].sample(rbms[l].propagateVisibleToHidden(cur, outputOfRBM[l]), outputOfRBM[l]);
            }
            double[] ret = (storage == null ? new double[cur.Length] : storage);
            for (int i = 0; i < ret.Length; i++) ret[i] = cur[i];
            return ret;
        }

        public void train(double[][] trainingset, int layer, int epochs = 1, double learningRate = 1.0)
        {
            double[][] trainingsetAtLayer = new double[trainingset.Length][];
            Parallel.For(0, trainingset.Length, t =>
            {
                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
            });
//            for (int t = 0; t < trainingset.Length; t++)
//            {
//                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
//            }
            rbms[layer].train(trainingsetAtLayer, epochs, learningRate);
        }
    }
}
