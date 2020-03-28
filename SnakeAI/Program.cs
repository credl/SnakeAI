using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using NeuralNetworks;

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
            double[][] trainingset = new double[][]
                {
                    new double[]{ 1.0, 1.0, 1.0, 0.0, 0.0, 0.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 1.0, 1.0 },
                    new double[]{ 1.0, 1.0, 0.0, 0.0, 0.0, 0.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 1.0, 1.0 },
                    new double[]{ 0.0, 0.0, 0.0, 1.0, 0.0, 1.0 },
                };
            double[][] labels = new double[][]
                {
                    new double[]{ 1, 0 },
                    new double[]{ 0, 1 },
                    new double[]{ 1, 0 },
                    new double[]{ 0, 1 },
                    new double[]{ 0, 1 },
                };
            double[][] testset = new double[][]
                {
                    new double[]{ 1.0, 1.0, 0.0, 0.0, 0.0, 0.0 },
                    new double[]{ 0.0, 0.0, 0.0, 0.0, 1.0, 1.0 },
                    new double[]{ 0.0, 1.0, 1.0, 1.0, 1.0, 0.0 },
                };
            
            //            NNRestrictedBoltzmannMachine rbm = new NNRestrictedBoltzmannMachine(6, 2);
            //            rbm.train(trainingset, 20000, 0.1f);
            //            foreach (float[] t in trainingset) 
            //            {
            //                float[] recon = rbm.sample(rbm.propagateHiddenToVisible(rbm.sample(rbm.propagateVisibleToHidden(t))));
            //            }

            NNDeepBeliefNetwork dbn = new NNDeepBeliefNetwork(new int[] { 6, 4, 4, 2 });
            dbn.train(trainingset, 0, 20000, 0.1f);
            dbn.train(trainingset, 1, 20000, 0.1f);
            dbn.train(trainingset, 2, 20000, 0.1f);
            double[][] resttrainingset = new double[trainingset.Length][];
            for (int t = 0; t < trainingset.Length; t++)
            {
                double[] classification = dbn.propagateToEnd(trainingset[t]);
                resttrainingset[t] = classification;
            }

            NNFeedForwardNetwork net = new NNFeedForwardNetwork(new int[] { 2, 10, 10, 2 });
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            net.randomizeWeights();
//            FrmNetworkVisualizer vis = new FrmNetworkVisualizer(net);
//            vis.Show();
            net.bptrain(resttrainingset, labels, 30000, 1.0);
            foreach (double[] t in testset)
            {
                double[] classification = net.propagateToEnd(dbn.propagateToEnd(t));
            }
//            Application.Run(vis);

            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainMenu());
            */
        }
    }
}
