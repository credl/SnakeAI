using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            NNRestrictedBoltzmannMachine rbm = new NNRestrictedBoltzmannMachine(3, 2);
            float[][] trainingset = new float[][]
                {
                    new float[]{ 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f },
                    new float[]{ 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f },
                };
//            rbm.train(trainingset, 20000, 0.1f);
//            foreach (float[] t in trainingset) 
//            {
//                float[] recon = rbm.sample(rbm.propagateHiddenToVisible(rbm.sample(rbm.propagateVisibleToHidden(t))));
//            }
            NNDeepBeliefNetwork dbn = new NNDeepBeliefNetwork(new int[] { 6, 4, 4, 1 });
            dbn.train(trainingset, 0, 20000, 0.1f);
            dbn.train(trainingset, 1, 20000, 0.1f);
            dbn.train(trainingset, 2, 20000, 0.1f);
            foreach (float[] t in trainingset)
            {
                float[] classification = dbn.propagateToEnd(t);
            }
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainMenu());
        }
    }
}
