using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class NNDeepBeliefNetwork : NNNetwork
    {
        private NNStackedRestrickedBoltzmannMachine unsupervisedNetwork;
        private NNFeedForwardNetwork supervisedNetwork;

        public NNDeepBeliefNetwork(int[] unitsPerUnsuperviesLayer, int[] unitsPerSuperviesLayer)
        {
            unsupervisedNetwork = new NNStackedRestrickedBoltzmannMachine(unitsPerUnsuperviesLayer);
            supervisedNetwork = new NNFeedForwardNetwork(unitsPerSuperviesLayer);
            supervisedNetwork.randomizeWeights();
        }

        public NNStackedRestrickedBoltzmannMachine getUnsupervisedNetwork()
        {
            return unsupervisedNetwork;
        }

        public NNFeedForwardNetwork getSupervisedNetwork()
        {
            return supervisedNetwork;
        }

        public int getLayerCount()
        {
            return getUnsupervisedLayerCount() + getSupervisedLayerCount();
        }

        public int getUnsupervisedLayerCount()
        {
            return unsupervisedNetwork.getLayerCount();
        }

        public int getSupervisedLayerCount()
        {
            return supervisedNetwork.getLayerCount();
        }

        override public double[] propagateToEnd(double[] inputVec, double[] storage = null)
        {
            return propagate(inputVec, getLayerCount(), storage);
        }

        public double[] propagateToUnsupervisedEnd(double[] inputVec)
        {
            return propagate(inputVec, getUnsupervisedLayerCount());
        }

        public double[] propagate(double[] inputVec, int propagateToOutputOfLayer, double[] storage = null)
        {
            if (propagateToOutputOfLayer < unsupervisedNetwork.getLayerCount()) return unsupervisedNetwork.propagateToLayer(inputVec, propagateToOutputOfLayer);
            else return supervisedNetwork.propagate(unsupervisedNetwork.propagateToEnd(inputVec), propagateToOutputOfLayer - getUnsupervisedLayerCount());
        }

        public double[][] propagateToUnsupervisedEnd(double[][] trainingset)
        {
            double[][] modtrainingset = new double[trainingset.Length][];
            for (int t = 0; t < trainingset.Length; t++)
            {
                modtrainingset[t] = propagateToUnsupervisedEnd(trainingset[t]);
            }
            return modtrainingset;
        }

        public void trainUnsupervised(double[][] trainingset, int layer, int epochs = 1, double learningRate = 1.0)
        {
            unsupervisedNetwork.train(trainingset, layer, epochs, learningRate);
        }

        public void trainSupervised(double[][] trainingset, double[][] labels, int epochs = 1, double learningRate = 1.0)
        {
            supervisedNetwork.train(propagateToUnsupervisedEnd(trainingset), labels, epochs, learningRate);
        }
    }
}
