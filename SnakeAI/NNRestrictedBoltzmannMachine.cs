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

        public NNRestrictedBoltzmannMachine(int visibleUnitCnt, int hiddenUnitCnt)
        {
            rnd = new Random(System.DateTime.Now.Millisecond);
            visible = new NNLayer(visibleUnitCnt);
            hidden = new NNLayer(hiddenUnitCnt);
            biasVisible = new double[visibleUnitCnt];
            biasHidden = new double[hiddenUnitCnt];
            weights = new NNMatrix(hiddenUnitCnt, visibleUnitCnt);
        }

        public double[] propagateVisibleToHidden(double[] input)
        {
            double[] res = new double[hidden.getUnitCount()];
            for (int h = 0; h < hidden.getUnitCount(); h++)
            {
                double hinput = biasHidden[h];
                for (int v = 0; v < visible.getUnitCount(); v++)
                {
                    hinput += input[v] * weights[h, v];
                }
                res[h] = hidden.getUnit(h).activation(hinput);
            }
            return res;
        }

        public double[] propagateHiddenToVisible(double[] input)
        {
            double[] res = new double[visible.getUnitCount()];
            for (int v = 0; v < visible.getUnitCount(); v++)
            {
                double vinput = biasVisible[v];
                for (int h = 0; h < hidden.getUnitCount(); h++)
                {
                    vinput += input[h] * weights[h, v];
                }
                res[v] = visible.getUnit(v).activation(vinput);
            }
            return res;
        }

        public double[] sample(double[] props)
        {
            double[] res = new double[props.Length];
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

        /*
        private double[, ] outerProd(double[] a, double[] b)
        {
            double[, ] res = new double[a.Length, b.Length];
            for (int c = 0; c < a.Length; c++) {
                for (int r = 0; r < b.Length; r++)
                {
                    res[c, r] = a[c] * b[r];
                }
            }
            return res;
        }
        private double[, ] matrixScale(double[, ] a, double factor)
        {
            double[, ] res = new double[a.GetLength(0), a.GetLength(1)];
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    res[x, y] = factor * a[x, y];
                }
            }
            return res;
        }

        private double[, ] matrixNegate(double[, ] a)
        {
            return matrixScale(a, -1);
        }

        private double[, ] matrixAdd(double[, ] a, double[, ] b)
        {
            double[, ] res = new double[a.GetLength(0), a.GetLength(1)];
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    res[x, y] = a[x, y] + b[x, y];
                }
            }
            return res;
        }

        private double[, ] wrapVectorToMatrix(double[] vector)
        {
            double[, ] ret = new double[vector.GetLength(0), 1];
            for (int i = 0; i < vector.Length; i++) ret[i, 0] = vector[i];
            return ret;
        }

        private double[] unwrapMatrixToVector(double[, ] matrix)
        {
            double[] ret = new double[matrix.GetLength(0)];
            for (int i = 0; i < ret.Length; i++) ret[i] = matrix[i, 0];
            return ret;
        }
        */

        public void train(double[][] trainingset, int epochs = 1, double learningRate = 1.0)
        {
            for (int e = 0; e < epochs; e++)
            {
                for (int t = 0; t < trainingset.Length; t++)
                {
                    double[] hiddenSample = sample(propagateVisibleToHidden(trainingset[t]));
                    NNMatrix posGrad = NNMatrix.outerProduct(trainingset[t], hiddenSample);
                    double[] visibleSample = sample(propagateHiddenToVisible(hiddenSample));
                    double[] hiddenSample2 = sample(propagateVisibleToHidden(visibleSample));
                    NNMatrix negGrad = NNMatrix.outerProduct(visibleSample, hiddenSample2);
                    NNMatrix deltaW = posGrad - negGrad;

                    weights += deltaW * learningRate;
                    biasVisible = new NNMatrix(biasVisible) + (new NNMatrix(trainingset[t]) - (new NNMatrix(visibleSample)) * learningRate);
                    biasHidden = new NNMatrix(biasHidden) + ((new NNMatrix(hiddenSample) - (new NNMatrix(hiddenSample2))) * learningRate);
                }
            }
        }
    }
}
