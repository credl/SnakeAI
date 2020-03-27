using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    class NNDeepBeliefNetwork
    {
        NNRestrictedBoltzmannMachine[] rbms;

        public NNDeepBeliefNetwork(int[] unitsPerLayer) {
            rbms = new NNRestrictedBoltzmannMachine[unitsPerLayer.Length - 1];

            for (int l = 0; l < unitsPerLayer.Length - 1; l++)
            {
                rbms[l] = new NNRestrictedBoltzmannMachine(unitsPerLayer[l], unitsPerLayer[l + 1]);
            }
        }

        public float[] propagateToEnd(float[] input) {
            return propagateToLayer(input, rbms.Length);
        }

        public float[] propagateToLayer(float[] input, int layer) {
            float[] cur = input;
            for (int l = 0; l < layer; l++)
            {
                cur = rbms[l].sample(rbms[l].propagateVisibleToHidden(cur));
            }
            return cur;
        }

        public void train(float[][] trainingset, int layer, int epochs = 1, float learningRate = 1.0f)
        {
            float[][] trainingsetAtLayer = new float[trainingset.Length][];
            for (int t = 0; t < trainingset.Length; t++)
            {
                trainingsetAtLayer[t] = propagateToLayer(trainingset[t], layer);
            }
            rbms[layer].train(trainingsetAtLayer, epochs, learningRate);
        }
    }
}
