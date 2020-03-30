using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord.Statistics;
using Accord.Neuro;
using Accord.Neuro.Networks;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;
using Accord.Statistics.Analysis;

namespace NeuralNetworks
{
    public class NNAccordInterface : NNNetwork
    {
        DeepBeliefNetwork classifier;

        public NNAccordInterface(int[] unitsPerLayer)
        {
            int[] hiddenUnits = new int[unitsPerLayer.Length - 1];
            for (int i = 1; i < unitsPerLayer.Length; i++) hiddenUnits[i - 1] = unitsPerLayer[i];
            classifier = new DeepBeliefNetwork(new BernoulliFunction(), unitsPerLayer[0], hiddenUnits);
            ((DeepBeliefNetwork)classifier).UpdateVisibleWeights();
            new GaussianWeights((DeepBeliefNetwork)classifier).Randomize();
        }

        public override double[] propagateToEnd(double[] inputVec, double[] storage = null)
        {
            return classifier.Compute(inputVec);
        }

        public void train(double[][] trainingset, double[][] labels, int epochs = 1, double learningRate = 1.0)
        {
            var teacher = new BackPropagationLearning((DeepBeliefNetwork)classifier)
            {
                LearningRate = 0.1,
                Momentum = 0.001
            };
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double error = teacher.RunEpoch(trainingset, labels);
            }
            ((DeepBeliefNetwork)classifier).UpdateVisibleWeights();
        }
    }
}
