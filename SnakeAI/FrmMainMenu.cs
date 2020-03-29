using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
    public partial class FrmMainMenu : Form
    {
        public FrmMainMenu()
        {
            InitializeComponent();
        }

        private void btnPlayManual_Click(object sender, EventArgs e)
        {
            new FrmPlay().Show();
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            new FrmTrain().Show();
        }

        private void btnTrainByPlay_Click(object sender, EventArgs e)
        {
            new FrmPlay(true).Show();
        }
    }
}
