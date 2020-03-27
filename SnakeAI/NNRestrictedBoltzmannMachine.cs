﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    class NNRestrictedBoltzmannMachine
    {
        NNLayer visible, hidden;
        float[,] weights;
        float[] biasVisible;
        float[] biasHidden;
        Random rnd;

        public NNRestrictedBoltzmannMachine(int visibleUnitCnt, int hiddenUnitCnt)
        {
            rnd = new Random(System.DateTime.Now.Millisecond);
            visible = new NNLayer(visibleUnitCnt);
            hidden = new NNLayer(hiddenUnitCnt);
            biasVisible = new float[visibleUnitCnt];
            biasHidden = new float[hiddenUnitCnt];
            weights = new float[visibleUnitCnt, hiddenUnitCnt];
        }

        public float[] propagateVisibleToHidden(float[] input)
        {
            float[] res = new float[hidden.getUnitCount()];
            for (int h = 0; h < hidden.getUnitCount(); h++)
            {
                float hinput = biasHidden[h];
                for (int v = 0; v < visible.getUnitCount(); v++)
                {
                    hinput += input[v] * weights[v, h];
                }
                res[h] = hidden.getUnit(h).activation(hinput);
            }
            return res;
        }

        public float[] propagateHiddenToVisible(float[] input)
        {
            float[] res = new float[visible.getUnitCount()];
            for (int v = 0; v < visible.getUnitCount(); v++)
            {
                float vinput = biasVisible[v];
                for (int h = 0; h < hidden.getUnitCount(); h++)
                {
                    vinput += input[h] * weights[v, h];
                }
                res[v] = visible.getUnit(v).activation(vinput);
            }
            return res;
        }

        public float[] sample(float[] props)
        {
            float[] res = new float[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                res[i] = (rnd.NextDouble() < props[i] ? 1.0f : 0.0f);
            }
            return res;
        }

        public void randomizeWeights()
        {
            for (int v = 0; v < weights.GetLength(0); v++)
            {
                biasVisible[v] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                for (int h = 0; h < weights.GetLength(1); h++)
                {
                    weights[v, h] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                    if (v == 0) biasHidden[h] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1); ;
                }
            }
        }

        private float[, ] outerProd(float[] a, float[] b)
        {
            float[, ] res = new float[a.Length, b.Length];
            for (int c = 0; c < a.Length; c++) {
                for (int r = 0; r < b.Length; r++)
                {
                    res[c, r] = a[c] * b[r];
                }
            }
            return res;
        }
        private float[, ] matrixScale(float[, ] a, float factor)
        {
            float[, ] res = new float[a.GetLength(0), a.GetLength(1)];
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    res[x, y] = factor * a[x, y];
                }
            }
            return res;
        }

        private float[, ] matrixNegate(float[, ] a)
        {
            return matrixScale(a, -1);
        }

        private float[, ] matrixAdd(float[, ] a, float[, ] b)
        {
            float[, ] res = new float[a.GetLength(0), a.GetLength(1)];
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    res[x, y] = a[x, y] + b[x, y];
                }
            }
            return res;
        }

        private float[, ] wrapVectorToMatrix(float[] vector)
        {
            float[, ] ret = new float[vector.GetLength(0), 1];
            for (int i = 0; i < vector.Length; i++) ret[i, 0] = vector[i];
            return ret;
        }

        private float[] unwrapMatrixToVector(float[, ] matrix)
        {
            float[] ret = new float[matrix.GetLength(0)];
            for (int i = 0; i < ret.Length; i++) ret[i] = matrix[i, 0];
            return ret;
        }

        public void train(float[][] trainingset, int epochs = 1, float learningRate = 1.0f)
        {
            for (int e = 0; e < epochs; e++)
            {
                for (int t = 0; t < trainingset.Length; t++)
                {
                    float[] hiddenSample = sample(propagateVisibleToHidden(trainingset[t]));
                    float[, ] posGrad = outerProd(trainingset[t], hiddenSample);
                    float[] visibleSample = sample(propagateHiddenToVisible(hiddenSample));
                    float[] hiddenSample2 = sample(propagateVisibleToHidden(visibleSample));
                    float[, ] negGrad = outerProd(visibleSample, hiddenSample2);
                    float[, ] deltaW = matrixAdd(posGrad, matrixNegate(negGrad));

                    weights = matrixAdd(weights, matrixScale(deltaW, learningRate));
                    biasVisible = unwrapMatrixToVector(matrixAdd(wrapVectorToMatrix(biasVisible), matrixScale(matrixAdd(wrapVectorToMatrix(trainingset[t]), matrixNegate(wrapVectorToMatrix(visibleSample))), learningRate)));
                    biasHidden = unwrapMatrixToVector(matrixAdd(wrapVectorToMatrix(biasHidden), matrixScale(matrixAdd(wrapVectorToMatrix(hiddenSample), matrixNegate(wrapVectorToMatrix(hiddenSample2))), learningRate)));
                }
            }
        }
    }
}
