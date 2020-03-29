﻿using System;
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

        public NNStackedRestrickedBoltzmannMachine(int[] unitsPerLayer) {
            rbms = new NNRestrictedBoltzmannMachine[unitsPerLayer.Length - 1];

            for (int l = 0; l < unitsPerLayer.Length - 1; l++)
            {
                rbms[l] = new NNRestrictedBoltzmannMachine(unitsPerLayer[l], unitsPerLayer[l + 1]);
            }
        }

        public int getLayerCount()
        {
            return rbms.Length;
        }

        override public double[] propagateToEnd(double[] input) {
            return propagateToLayer(input, rbms.Length);
        }

        public double[] propagateToLayer(double[] input, int layer) {
            double[] cur = input;
            for (int l = 0; l < layer; l++)
            {
                cur = rbms[l].sample(rbms[l].propagateVisibleToHidden(cur));
            }
            return cur;
        }

        public void train(double[][] trainingset, int layer, int epochs = 1, double learningRate = 1.0)
        {
            double[][] trainingsetAtLayer = new double[trainingset.Length][];
            for (int t = 0; t < trainingset.Length; t++)
            {
                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
            }
            rbms[layer].train(trainingsetAtLayer, epochs, learningRate);
        }
    }
}