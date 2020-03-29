using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class NNRestrictedBoltzmannMachine
    {
        NNLayer visible, hidden;
        NNMatrix weights;
        double[] biasVisible;
        double[] biasHidden;
        Random rnd;
        double[] propVisibleToHiddenResult;
        double[] propHiddenToVisibleResult;

        public NNRestrictedBoltzmannMachine(int visibleUnitCnt, int hiddenUnitCnt)
        {
            rnd = new Random(System.DateTime.Now.Millisecond);
            visible = new NNLayer(visibleUnitCnt, ActivationFunctions.sigmoid);
            hidden = new NNLayer(hiddenUnitCnt, ActivationFunctions.sigmoid);
            biasVisible = new double[visibleUnitCnt];
            biasHidden = new double[hiddenUnitCnt];
            weights = new NNMatrix(hiddenUnitCnt, visibleUnitCnt);

            propVisibleToHiddenResult = new double[hidden.getUnitCount()];
            propHiddenToVisibleResult = new double[visible.getUnitCount()];
        }

        public double[] propagateVisibleToHidden(double[] input)
        {
            for (int h = 0; h < hidden.getUnitCount(); h++)
            {
                double hinput = biasHidden[h];
                for (int v = 0; v < visible.getUnitCount(); v++)
                {
                    hinput += input[v] * weights[h, v];
                }
                propVisibleToHiddenResult[h] = hidden.getUnit(h).activation(hinput);
            }
            return propVisibleToHiddenResult;
        }

        public double[] propagateHiddenToVisible(double[] input)
        {
            for (int v = 0; v < visible.getUnitCount(); v++)
            {
                double vinput = biasVisible[v];
                for (int h = 0; h < hidden.getUnitCount(); h++)
                {
                    vinput += input[h] * weights[h, v];
                }
                propHiddenToVisibleResult[v] = visible.getUnit(v).activation(vinput);
            }
            return propHiddenToVisibleResult;
        }

        public double[] sample(double[] props)
        {
            double[] res;
            if (props.Length == propHiddenToVisibleResult.Length) res = propHiddenToVisibleResult;
            else if (props.Length == propVisibleToHiddenResult.Length) res = propVisibleToHiddenResult;
            else res = new double[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                res[i] = (rnd.NextDouble() < props[i] ? 1.0f : 0.0f);
            }
            return res;
        }

        public void randomizeWeights()
        {
            for (int v = 0; v < weights.rowCount(); v++)
            {
                biasVisible[v] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                for (int h = 0; h < weights.colCount(); h++)
                {
                    weights[h, v] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                    if (v == 0) biasHidden[h] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                }
            }
        }

        public void train(double[][] trainingset, int epochs = 1, double learningRate = 1.0)
        {
            NNMatrix mat = new NNMatrix(hidden.getUnitCount(), visible.getUnitCount());
            for (int e = 0; e < epochs; e++)
            {
                for (int t = 0; t < trainingset.Length; t++)
                {
                    double[] hiddenSample = sample(propagateVisibleToHidden(trainingset[t]));
                    NNMatrix posGrad = NNMatrix.outerProduct(trainingset[t], hiddenSample, mat);
                    double[] visibleSample = sample(propagateHiddenToVisible(hiddenSample));
                    double[] hiddenSample2 = sample(propagateVisibleToHidden(visibleSample));
                    NNMatrix negGrad = NNMatrix.outerProduct(visibleSample, hiddenSample2, mat);
                    // NNMatrix deltaW = posGrad - negGrad;
                    NNMatrix deltaW = posGrad;
                    deltaW.applyOperatorToThis(negGrad, (x, y) => (x - y));

                    //weights += deltaW * learningRate;
                    deltaW.applyOperatorToThis(deltaW, (x, y) => (x * learningRate));
                    //biasVisible = new NNMatrix(biasVisible) + (new NNMatrix(trainingset[t]) - (new NNMatrix(visibleSample)) * learningRate);
                    for (int i = 0; i < biasVisible.Length; i++) biasVisible[i] += (trainingset[t][i] - visibleSample[i]) * learningRate;
                    //biasHidden = new NNMatrix(biasHidden) + ((new NNMatrix(hiddenSample) - (new NNMatrix(hiddenSample2))) * learningRate);
                    for (int i = 0; i < biasHidden.Length; i++) biasHidden[i] += (hiddenSample[i] - hiddenSample2[i]) * learningRate;
                }
            }
        }
    }
}
