﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NNLayer
    {
        NNUnit[] units;
        double[] propagateRet;

        public NNLayer(int cunits, ActivationFunction func)
        {
            units = new NNUnit[cunits];
            for (int i = 0; i < cunits; i++)
            {
                units[i] = new NNUnit(func);
            }
            propagateRet = new double[units.Length];
        }

        public int getUnitCount()
        {
            return units.Length;
        }

        public double[] propagate(double[] input, double[] storage = null)
        {
            double[] ret = (storage == null ? propagateRet : storage);
            for (int u = 0; u < units.Length; u++)
            {
                ret[u] = units[u].activation(input[u]);
            }
            return ret;
        }

        public NNUnit getUnit(int i)
        {
            return units[i];
        }
    }
}
