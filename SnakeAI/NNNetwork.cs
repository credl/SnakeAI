using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    public class NNNetwork
    {
        NNLayer[] layers;
        NNMatrix[] weightsPerLayer;

        public NNNetwork(int[] unitsPerLayer)
        {
            layers = new NNLayer[unitsPerLayer.Length];
            weightsPerLayer = new NNMatrix[getLayerCount()];
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                layers[layer] = new NNLayer(unitsPerLayer[layer]);
                if (layer == 0)
                {
                    weightsPerLayer[layer] = new NNMatrix(getLayer(layer).getUnitCount(), getLayer(layer).getUnitCount());
                    for (int unit = 0; unit < getLayer(layer).getUnitCount(); unit++)
                    {
                        weightsPerLayer[layer][unit, unit] = 1.0;
                    }
                }
                else
                {
                    weightsPerLayer[layer] = new NNMatrix(getLayer(layer).getUnitCount(), getLayer(layer - 1).getUnitCount());
                }
            }
        }


        public NNNetwork(int[] unitsPerLayer, NNMatrix[] weightsPerLayer) : this(unitsPerLayer)
        {
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                this.weightsPerLayer[layer] = new NNMatrix(weightsPerLayer[layer]);
            }
        }

        public NNMatrix[] getWeights() {
            return weightsPerLayer;
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
            for (int layer = 1; layer < getLayerCount(); layer++)
            {
                for (int r = 0; r < weightsPerLayer[layer].rowCount(); r++)
                {
                    for (int c = 0; c < weightsPerLayer[layer].colCount(); c++)
                    {
                        weightsPerLayer[layer][c, r] = rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                    }
                }
            }
        }

        public void randomizeWeightsInc(double maxChange, double changeProp = 0.1f)
        {
            System.Threading.Thread.Sleep(1);
            Random rnd = new Random(System.DateTime.Now.Millisecond);
            for (int layer = 1; layer < getLayerCount(); layer++)
            {
                for (int r = 0; r < weightsPerLayer[layer].rowCount(); r++)
                {
                    for (int c = 0; c < weightsPerLayer[layer].colCount(); c++)
                    {
                        if (rnd.NextDouble() < changeProp)
                        {
                            weightsPerLayer[layer][c, r] += maxChange * rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
                        }
                    }
                }
            }
        }

        public void randomizeSingleWeightsInc(double maxChange)
        {
            System.Threading.Thread.Sleep(1);
            Random rnd = new Random(System.DateTime.Now.Millisecond);
            int l = (int)(getLayerCount() * rnd.NextDouble());
            int r = (int)(weightsPerLayer[l].rowCount() * rnd.NextDouble());
            int c = (int)(weightsPerLayer[l].colCount() * rnd.NextDouble());
            weightsPerLayer[l][c, r] += maxChange * rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
        }

        public double[] propagateToEnd(double[] inputVec)
        {
            return propagate(inputVec, layers.Length);
        }

        public double[] propagate(double[] inputVec, int propagateToOutputOfLayer)
        {
            double[] prevoutput = inputVec;
            for (int layer = 0; layer < propagateToOutputOfLayer; layer++)
            {
                double[] weightedinputOfCurrentLayer = new double[layers[layer].getUnitCount()];
                for (int currentLayerUnit = 0; currentLayerUnit < layers[layer].getUnitCount(); currentLayerUnit++)
                {
                    for (int prevLayerUnit = 0; prevLayerUnit < weightsPerLayer[layer].rowCount(); prevLayerUnit++)
                    {
                        weightedinputOfCurrentLayer[currentLayerUnit] += prevoutput[prevLayerUnit] * weightsPerLayer[layer][currentLayerUnit, prevLayerUnit];
                    }
                }
                prevoutput = layers[layer].propagate(weightedinputOfCurrentLayer);
            }
            return prevoutput;
        }

        private double qerror(double[] target, double[] prediction)
        {
            double err = 0.0;
            for (int i = 0; i < target.Length; i++)
            {
                err += Math.Pow(target[i] - prediction[i], 2.0);
            }
            err /= 2.0;
            return err;
        }

        public void bptrain(double[][] trainingset, double[][] labels, int epochs = 1, double learningRate = 1.0)
        {
            double[][] deltasPerLayer = new double[getLayerCount()][];
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                deltasPerLayer[layer] = new double[getLayer(layer).getUnitCount()];
            }

            for (int e = 0; e < epochs; e++)
            {
                for (int t = 0; t < trainingset.Length; t++)
                {
                    double[] prediction = propagateToEnd(trainingset[t]);
                    double qerr = qerror(labels[t], prediction);

                    double[][] layeroutput = new double[getLayerCount()][];
                    for (int layer = 0; layer < getLayerCount(); layer++)
                    {
                        layeroutput[layer] = propagate(trainingset[t], layer + 1);
                    }

                    for (int layer = getLayerCount() - 1; layer >= 1; layer--)
                    {
                        for (int currentLayerUnit = 0; currentLayerUnit < getLayer(layer).getUnitCount(); currentLayerUnit++)
                        {
                            for (int prevLayerUnit = 0; prevLayerUnit < getLayer(layer - 1).getUnitCount(); prevLayerUnit++)
                            {
                                if (layer == getLayerCount() - 1)
                                {
                                    deltasPerLayer[layer][currentLayerUnit] = layeroutput[layer][currentLayerUnit] * (1 - layeroutput[layer][currentLayerUnit]) * (labels[t][currentLayerUnit] - layeroutput[layer][currentLayerUnit]);
                                }
                                else
                                {
                                    double weightsum = 0.0;
                                    int wen = currentLayerUnit;
                                    for (int nextLayerUnit = 0; nextLayerUnit < getLayer(layer + 1).getUnitCount(); nextLayerUnit++)
                                    {
                                        weightsum += deltasPerLayer[layer + 1][nextLayerUnit] * weightsPerLayer[layer + 1][nextLayerUnit, currentLayerUnit];
                                        wen += getLayer(layer).getUnitCount();
                                    }
                                    deltasPerLayer[layer][currentLayerUnit] = layeroutput[layer][currentLayerUnit] * (1 - layeroutput[layer][currentLayerUnit]) * weightsum;
                                }
                                weightsPerLayer[layer][currentLayerUnit, prevLayerUnit] += learningRate * deltasPerLayer[layer][currentLayerUnit] * layeroutput[layer - 1][prevLayerUnit];
                            }
                        }
                    }
                }
            }
        }
    }
}
