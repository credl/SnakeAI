using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    class NNUnit
    {
        public float activation(float input)
        {
//            return (input >= 0 ? 1 : 0);
            return (float)(1.0 / (1.0 + Math.Pow(Math.E, -input)));
        }
    }
}
