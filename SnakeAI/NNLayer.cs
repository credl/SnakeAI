using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    public class NNLayer
    {
        NNUnit[] units;

        public NNLayer(int cunits)
        {
            units = new NNUnit[cunits];
            for (int i = 0; i < cunits; i++)
            {
                units[i] = new NNUnit();
            }
        }

        public int getUnitCount()
        {
            return units.Length;
        }

        public double[] propagate(double[] input)
        {
            double[] ret = new double[units.Length];
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
