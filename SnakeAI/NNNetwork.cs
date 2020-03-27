﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    public class NNNetwork
    {
        NNLayer[] layers;
        float[][] weights;

        public NNNetwork(int[] unitsPerLayer)
        {
            layers = new NNLayer[unitsPerLayer.Length];
            weights = new float[unitsPerLayer.Length][];
            for (int i = 0; i < unitsPerLayer.Length; i++)
            {
                layers[i] = new NNLayer(unitsPerLayer[i]);
                if (i == 0)
                {
                    weights[i] = new float[unitsPerLayer[i]];
                    for (int u = 0; u < weights[i].Length; u++)
                    {
                        weights[i][u] = 1.0f;
                    }
                }
                else
                {
                    weights[i] = new float[unitsPerLayer[i] * unitsPerLayer[i - 1]];
                }
            }
        }


        public NNNetwork(int[] unitsPerLayer, float[][] weights) : this(unitsPerLayer)
        {
            for (int l = 0; l < weights.Length; l++)
                for (int u = 0; u < weights[l].Length; u++)
                    this.weights[l][u] = weights[l][u];
        }

        public float[][] getWeights() {
            return weights;
        }

        public NNLayer getLayer(int i)
        {
            return layers[i];
        }

        public int getLayerCount()
        {
            return layers.Length;
        }

        public void randomizeWeights()
        {
            System.Threading.Thread.Sleep(1);
            Random rnd = new Random(System.DateTime.Now.Millisecond);
            for (int l = 1; l < weights.Length; l++)
            {
                for (int u = 0; u < weights[l].Length; u++)
                {
                    weights[l][u] = (float)rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                }
            }
        }

        public void randomizeWeightsInc(float maxChange, float changeProp = 0.1f)
        {
            System.Threading.Thread.Sleep(1);
            Random rnd = new Random(System.DateTime.Now.Millisecond);
            for (int l = 1; l < weights.Length; l++)
            {
                for (int u = 0; u < weights[l].Length; u++)
                {
                    if (rnd.NextDouble() < changeProp)
                    {
                        weights[l][u] += (float)(maxChange * rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1));
                    }
                }
            }
        }
        public void randomizeSingleWeightsInc(float maxChange)
        {
            System.Threading.Thread.Sleep(1);
            Random rnd = new Random(System.DateTime.Now.Millisecond);
            int l = (int)(weights.Length * rnd.NextDouble());
            int u = (int)(layers[l].getUnitCount() * rnd.NextDouble());
            weights[l][u] += (float)(maxChange * rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1));
        }

        public float[] propagateToEnd(float[] input)
        {
            return propagate(input, layers.Length);
        }

        public float[] propagate(float[] input, int layer)
        {
            float[] prevoutput = input;
            for (int l = 0; l < layer; l++)
            {
                float[] weightedinput = new float[layers[l].getUnitCount()];
                int w = 0;
                for (int u = 0; u < layers[l].getUnitCount(); u++)
                {
                    if (l == 0)
                    {
                        weightedinput[u] = input[u];
                    }
                    else
                    {
                        for (int po = 0; po < prevoutput.Length; po++)
                        {
                            weightedinput[u] += prevoutput[po] * weights[l][w++];
                        }
                    }
                }
                prevoutput = layers[l].propagate(weightedinput);
            }
            return prevoutput;
        }

        private float qerror(float[] target, float[] prediction)
        {
            float err = 0.0f;
            for (int i = 0; i < target.Length; i++)
            {
                err += (float)Math.Pow(target[i] - prediction[i], 2.0);
            }
            err /= 2.0f;
            return err;
        }

        public void bptrain(float[][] trainingset, float[][] labels, int epochs = 1, float learningRate = 1.0f)
        {
            float[][] delta = new float[getLayerCount()][];
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                delta[layer] = new float[getLayer(layer).getUnitCount()];
            }

            for (int e = 0; e < epochs; e++)
            {
                for (int t = 0; t < trainingset.Length; t++)
                {
                    float[] prediction = propagateToEnd(trainingset[t]);
                    float qerr = qerror(labels[t], prediction);

                    float[][] layeroutput = new float[getLayerCount()][];
                    for (int layer = 0; layer < getLayerCount(); layer++)
                    {
                        layeroutput[layer] = propagate(trainingset[t], layer + 1);
                    }

                    for (int layer = getLayerCount() - 1; layer >= 1; layer--)
                    {
                        int we = 0;
                        for (int u = 0; u < getLayer(layer).getUnitCount(); u++)
                        {
                            for (int prevu = 0; prevu < getLayer(layer - 1).getUnitCount(); prevu++)
                            {
                                if (layer == getLayerCount() - 1)
                                {
                                    delta[layer][u] = layeroutput[layer][u] * (1 - layeroutput[layer][u]) * (labels[t][u] - layeroutput[layer][u]);
                                }
                                else
                                {
                                    float weightsum = 0.0f;
                                    int wen = u;
                                    for (int unext = 0; unext < getLayer(layer + 1).getUnitCount(); unext++)
                                    {
                                        weightsum += delta[layer + 1][unext] * weights[layer + 1][wen];
                                        wen += getLayer(layer).getUnitCount();
                                    }
                                    delta[layer][u] = layeroutput[layer][u] * (1 - layeroutput[layer][u]) * weightsum;
                                }

                                weights[layer][we] += learningRate * delta[layer][u] * layeroutput[layer - 1][prevu];
                                we++;
                            }
                        }
                    }
                }
            }
        }
    }
}
