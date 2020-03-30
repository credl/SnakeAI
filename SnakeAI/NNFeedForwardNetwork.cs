using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NNFeedForwardNetwork : NNNetwork
    {
        public delegate void NNUpdateCallback();
        NNLayer[] layers;
        NNMatrix[] weightsPerLayer;
        LinkedList<NNUpdateCallback> updateCallbacks = new LinkedList<NNUpdateCallback>();
        double[][] weightedinputOfLayer;
        double[][] outputOfLayer;
        Random rnd;

        public NNFeedForwardNetwork(int[] unitsPerLayer)
        {
            layers = new NNLayer[unitsPerLayer.Length];
            weightsPerLayer = new NNMatrix[getLayerCount()];
            weightedinputOfLayer = new double[getLayerCount()][];
            outputOfLayer = new double[getLayerCount()][];
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                layers[layer] = new NNLayer(unitsPerLayer[layer], ActivationFunctions.sigmoid);
                weightedinputOfLayer[layer] = new double[layers[layer].getUnitCount()];
                outputOfLayer[layer] = new double[layers[layer].getUnitCount()];
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
            System.Threading.Thread.Sleep(1);
            rnd = new Random(System.DateTime.Now.Millisecond);
            callUpdateCallbacks();
        }

        public NNFeedForwardNetwork(int[] unitsPerLayer, NNMatrix[] weightsPerLayer) : this(unitsPerLayer)
        {
            setWeights(weightsPerLayer);
            callUpdateCallbacks();
        }

        public void setWeights(NNMatrix[] weightsPerLayer)
        {
            if (getLayerCount() != weightsPerLayer.Length) throw new Exception("Mismatch of layer count of networks");
            for (int layer = 0; layer < getLayerCount(); layer++)
            {
                this.weightsPerLayer[layer] = new NNMatrix(weightsPerLayer[layer]);
            }
            callUpdateCallbacks();
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
            callUpdateCallbacks();
        }

        public void randomizeWeightsInc(double maxChange, double changeProp = 0.1f)
        {
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
            callUpdateCallbacks();
        }

        public void randomizeSingleWeightsInc(double maxChange)
        {
            int l = (int)(getLayerCount() * rnd.NextDouble());
            int r = (int)(weightsPerLayer[l].rowCount() * rnd.NextDouble());
            int c = (int)(weightsPerLayer[l].colCount() * rnd.NextDouble());
            weightsPerLayer[l][c, r] += maxChange * rnd.NextDouble() * (rnd.NextDouble() < 0.5 ? +1 : -1);
            callUpdateCallbacks();
        }

        override public double[] propagateToEnd(double[] inputVec, double[] storage = null)
        {
            return propagate(inputVec, layers.Length, 0, storage);
        }

        public double[] propagate(double[] inputVec, int propagateToOutputOfLayer, int propagateFromOutputOfLayer = 0, double[] storage = null)
        {
            double[] prevoutput = inputVec;
            for (int layer = propagateFromOutputOfLayer; layer < propagateToOutputOfLayer; layer++)
            {
                for (int currentLayerUnit = 0; currentLayerUnit < layers[layer].getUnitCount(); currentLayerUnit++)
                {
                    weightedinputOfLayer[layer][currentLayerUnit] = 0.0;
                    for (int prevLayerUnit = 0; prevLayerUnit < weightsPerLayer[layer].rowCount(); prevLayerUnit++)
                    {
                        weightedinputOfLayer[layer][currentLayerUnit] += prevoutput[prevLayerUnit] * weightsPerLayer[layer][currentLayerUnit, prevLayerUnit];
                    }
                }
                prevoutput = layers[layer].propagate(weightedinputOfLayer[layer], outputOfLayer[layer]);
            }
            double[] ret = (storage == null ? new double[prevoutput.Length] : storage);
            for (int i = 0; i < ret.Length; i++) ret[i] = prevoutput[i];
            return ret;
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

        public void train(double[][] trainingset, double[][] labels, int epochs = 1, double learningRate = 1.0)
        {
            double[][] deltasPerLayer = new double[getLayerCount()][];
            double[][] layeroutput = new double[getLayerCount()][];
            double[] prediction = new double[getLayer(getLayerCount() - 1).getUnitCount()];

            double qerr;
            double weightsum;
            int e, t, layer, currentLayerUnit, prevLayerUnit, nextLayerUnit;

            for (layer = 0; layer < getLayerCount(); layer++)
            {
                deltasPerLayer[layer] = new double[getLayer(layer).getUnitCount()];
                layeroutput[layer] = new double[getLayer(layer).getUnitCount()];
            }

            for (e = 0; e < epochs; e++)
            {
                for (t = 0; t < trainingset.Length; t++)
                {
                    for (layer = 0; layer < getLayerCount(); layer++)
                    {
                        if (layer == 0) propagate(trainingset[t], layer + 1, 0, layeroutput[layer]);
                        else propagate(layeroutput[layer - 1], layer + 1, layer, layeroutput[layer]);
                    }
                    //propagateToEnd(trainingset[t], prediction);
                    prediction = layeroutput[getLayerCount() - 1];
                    qerr = qerror(labels[t], prediction);

                    for (layer = getLayerCount() - 1; layer >= 1; layer--)
                    {
                        for (currentLayerUnit = 0; currentLayerUnit < getLayer(layer).getUnitCount(); currentLayerUnit++)
                        {
                            for (prevLayerUnit = 0; prevLayerUnit < getLayer(layer - 1).getUnitCount(); prevLayerUnit++)
                            {
                                if (layer == getLayerCount() - 1)
                                {
                                    deltasPerLayer[layer][currentLayerUnit] = layeroutput[layer][currentLayerUnit] * (1 - layeroutput[layer][currentLayerUnit]) * (labels[t][currentLayerUnit] - layeroutput[layer][currentLayerUnit]);
                                }
                                else
                                {
                                    weightsum = 0.0;
                                    for (nextLayerUnit = 0; nextLayerUnit < getLayer(layer + 1).getUnitCount(); nextLayerUnit++)
                                    {
                                        weightsum += deltasPerLayer[layer + 1][nextLayerUnit] * weightsPerLayer[layer + 1][nextLayerUnit, currentLayerUnit];
                                    }
                                    deltasPerLayer[layer][currentLayerUnit] = layeroutput[layer][currentLayerUnit] * (1 - layeroutput[layer][currentLayerUnit]) * weightsum;
                                }
                                weightsPerLayer[layer][currentLayerUnit, prevLayerUnit] += learningRate * deltasPerLayer[layer][currentLayerUnit] * layeroutput[layer - 1][prevLayerUnit];
                            }
                        }
                    }
                }
                callUpdateCallbacks();
            }
        }

        public void addUpdateCallback(NNUpdateCallback cb)
        {
            lock (updateCallbacks)
            {
                updateCallbacks.AddLast(cb);
            }
        }

        public void removeUpdateCallback(NNUpdateCallback cb)
        {
            lock (updateCallbacks)
            {
                updateCallbacks.Remove(cb);
            }
        }

        private void callUpdateCallbacks()
        {
            lock (updateCallbacks)
            {
                foreach (NNUpdateCallback cb in updateCallbacks)
                {
                    cb();
                }
            }
        }
    }
}
