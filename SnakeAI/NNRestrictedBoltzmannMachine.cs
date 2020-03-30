using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class NNRestrictedBoltzmannMachine
    {
        private NNLayer visible, hidden;
        private NNMatrix weights;
        private double[] biasVisible;
        private double[] biasHidden;
        private Random rnd;

        public NNRestrictedBoltzmannMachine(int visibleUnitCnt, int hiddenUnitCnt)
        {
            rnd = new Random(System.DateTime.Now.Millisecond);
            visible = new NNLayer(visibleUnitCnt, ActivationFunctions.sigmoid);
            hidden = new NNLayer(hiddenUnitCnt, ActivationFunctions.sigmoid);
            biasVisible = new double[visibleUnitCnt];
            biasHidden = new double[hiddenUnitCnt];
            weights = new NNMatrix(hiddenUnitCnt, visibleUnitCnt);
        }

        public double[] propagateVisibleToHidden(double[] input, double[] storage = null)
        {
            double[] ret = (storage == null ? new double[hidden.getUnitCount()] : storage);
            double hinput;
            for (int h = 0; h < hidden.getUnitCount(); h++)
            {
                hinput = biasHidden[h];
                for (int v = 0; v < visible.getUnitCount(); v++)
                {
                    hinput += input[v] * weights[h, v];
                }
                ret[h] = hidden.getUnit(h).activation(hinput);
            }
            return ret;
        }

        public double[] propagateHiddenToVisible(double[] input, double[] storage = null)
        {
            double[] ret = (storage == null ? new double[visible.getUnitCount()] : storage);
            double vinput;
            for (int v = 0; v < visible.getUnitCount(); v++)
            {
                vinput = biasVisible[v];
                for (int h = 0; h < hidden.getUnitCount(); h++)
                {
                    vinput += input[h] * weights[h, v];
                }
                ret[v] = visible.getUnit(v).activation(vinput);
            }
            return ret;
        }

        public double[] sample(double[] props, double[] storage = null)
        {
            double[] ret = (storage == null ? new double[props.Length] : storage);
            for (int i = 0; i < props.Length; i++)
            {
                ret[i] = (rnd.NextDouble() < props[i] ? 1.0f : 0.0f);
            }
            return ret;
        }

        public void randomizeWeights()
        {
            for (int v = 0; v < weights.rowCount(); v++)
            {
                biasVisible[v] = rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                for (int h = 0; h < weights.colCount(); h++)
                {
                    weights[h, v] = rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                    if (v == 0) biasHidden[h] = rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                }
            }
        }

        public void train(double[][] trainingset, int epochs = 1, double learningRate = 1.0)
        {
            double[] visibleSample = new double[visible.getUnitCount()];
            double[] hiddenSample = new double[hidden.getUnitCount()];
            double[] hiddenSample2 = new double[hidden.getUnitCount()];
            NNMatrix posGrad = new NNMatrix(hidden.getUnitCount(), visible.getUnitCount());
            NNMatrix negGrad = new NNMatrix(hidden.getUnitCount(), visible.getUnitCount());

            int e, t, b;
            for (e = 0; e < epochs; e++)
            {
                for (t = 0; t < trainingset.Length; t++)
                {
                    sample(propagateVisibleToHidden(trainingset[t], hiddenSample), hiddenSample);
                    NNMatrix.outerProduct(trainingset[t], hiddenSample, posGrad);
                    sample(propagateHiddenToVisible(hiddenSample, visibleSample), visibleSample);
                    sample(propagateVisibleToHidden(visibleSample, hiddenSample2), hiddenSample2);
                    NNMatrix.outerProduct(visibleSample, hiddenSample2, negGrad);

                    // weights += (posGrad - negGrad) * learningRate
                    weights.applyOperatorToThis(posGrad, negGrad, (weight, posG, negG) => (weight + (posG - negG) * learningRate));

                    // biasVisible = new NNMatrix(biasVisible) + (new NNMatrix(trainingset[t]) - (new NNMatrix(visibleSample)) * learningRate);
                    for (b = 0; b < biasVisible.Length; b++) biasVisible[b] += ((trainingset[t][b] - visibleSample[b]) * learningRate);
                    // biasHidden = new NNMatrix(biasHidden) + ((new NNMatrix(hiddenSample) - (new NNMatrix(hiddenSample2))) * learningRate);
                    for (b = 0; b < biasHidden.Length; b++) biasHidden[b] += ((hiddenSample[b] - hiddenSample2[b]) * learningRate);
                }
            }
        }
    }
}
