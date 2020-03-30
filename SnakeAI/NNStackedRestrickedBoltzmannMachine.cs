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

        public void randomizeWeights()
        {
            foreach (NNRestrictedBoltzmannMachine rbm in rbms)
            {
                rbm.randomizeWeights();
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

            /*
            double[][] f = new double[][]
                {
                    new double[]{ 1.0, 1.0, 1.0, 0.0, 0.0, 0.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 1.0, 1.0 },
                    new double[]{ 1.0, 1.0, 0.0, 0.0, 0.0, 0.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 1.0, 1.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 0.0, 1.0 },
                };
            double[][] tt = new double[][] { new double[] { 0, 0 }, new double[] { 0, 1 }, new double[] { 0, 0 }, new double[] { 0, 1 }, new double[] { 0, 1 } };
            */









            double[] cur = input;
            for (int l = 0; l < layer; l++)
            {



                /*
                if (l == 0)
                {
                    for (int i = 0; i < f.Length; i++)
                    {
                        bool eq = true;
                        if (f[i].Length != input.Length) continue;
                        for (int j = 0; j < f[i].Length; j++)
                        {
                            if (f[i][j] != input[j])
                            {
                                eq = false;
                                break;
                            }
                        }
                        if (eq)
                        {
                            cur = tt[i];
                            break;
                        }
                    }
                }else
                */





                //                if (l < layer - 1) cur = rbms[l].sample(rbms[l].propagateVisibleToHidden(cur, outputOfRBM[l]), outputOfRBM[l]);
                //                else cur = rbms[l].propagateVisibleToHidden(cur, outputOfRBM[l]);
                cur = rbms[l].sample(rbms[l].propagateVisibleToHidden(cur, outputOfRBM[l]), outputOfRBM[l]);
            }
            double[] ret = (storage == null ? new double[cur.Length] : storage);
            for (int i = 0; i < ret.Length; i++) ret[i] = cur[i];
            return ret;
        }

        public void train(double[][] trainingset, int layer, int epochs = 1, double learningRate = 1.0)
        {
            double[][] trainingsetAtLayer = new double[trainingset.Length][];
//            Parallel.For(0, trainingset.Length, t =>
//            {
//                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
//            });
            for (int t = 0; t < trainingset.Length; t++)
            {
                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
            }
            rbms[layer].train(trainingsetAtLayer, epochs, learningRate);
        }
    }
}
