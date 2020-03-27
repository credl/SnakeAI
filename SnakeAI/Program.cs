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

            float[][] trainingset = new float[][]
                {
                    new float[]{ 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f },
                    new float[]{ 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f },
                };
            float[][] labels = new float[][]
                {
                    new float[]{ 1, 0 },
                    new float[]{ 0, 1 },
                    new float[]{ 1, 0 },
                    new float[]{ 0, 1 },
                    new float[]{ 0, 1 },
                };
            float[][] testset = new float[][]
                {
                    new float[]{ 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[]{ 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f },
                    new float[]{ 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f },
                };
            /*
                        NNRestrictedBoltzmannMachine rbm = new NNRestrictedBoltzmannMachine(6, 2);
                        rbm.train(trainingset, 20000, 0.1f);
                        foreach (float[] t in trainingset) 
                        {
                            float[] recon = rbm.sample(rbm.propagateHiddenToVisible(rbm.sample(rbm.propagateVisibleToHidden(t))));
                        }
            //            float[] invent = rbm.sample(rbm.propagateHiddenToVisible(rbm.sample(rbm.propagateVisibleToHidden(new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f }))));
            */

            NNDeepBeliefNetwork dbn = new NNDeepBeliefNetwork(new int[] { 6, 4, 4, 2 });
            dbn.train(trainingset, 0, 20000, 0.1f);
            dbn.train(trainingset, 1, 20000, 0.1f);
            dbn.train(trainingset, 2, 20000, 0.1f);
            float[][] resttrainingset = new float[trainingset.Length][];
            for (int t = 0; t < trainingset.Length; t++)
            {
                float[] classification = dbn.propagateToEnd(trainingset[t]);
                resttrainingset[t] = classification;
            }

            NNNetwork net = new NNNetwork(new int[] { 2, 3, 3, 2 });
            net.randomizeWeights();
            net.bptrain(resttrainingset, labels, 30000, 1.0f);
            foreach (float[] t in testset)
            {
                float[] classification = net.propagateToEnd(dbn.propagateToEnd(t));
            }

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainMenu());
        }
    }
}
