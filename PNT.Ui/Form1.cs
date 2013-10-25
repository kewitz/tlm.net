using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using TLM.Core;

namespace TLM.Ui
{
    public partial class Form1 : Form
    {
        private Thread genious;
        private Net net;

        public Form1()
        {
            InitializeComponent();
            
            // have some data
            ILArray<float> A = new float[] { 5, 6, 7, 3, 5, 1, 7, 9 };
            // create new scene, add plot cube
            var scene = new ILScene {
                new ILPlotCube {
                    // add line plot, provide data as rows
                    new ILLinePlot(A.T, lineColor: Color.Blue)
                }
            };
            ilPanel.Scene.Add(scene);
        }

        private void netStatus(object sender, string e)
        {
            StatusLabel.Text = e;
        }

        private void BTCreateNet_Click(object sender, EventArgs e)
        {
            double sizeX = Convert.ToDouble(TBSizeX.Text, System.Globalization.CultureInfo.InvariantCulture);
            double sizeY = Convert.ToDouble(TBSizeY.Text, System.Globalization.CultureInfo.InvariantCulture);
            double sigma = Convert.ToDouble(TBSigma.Text, System.Globalization.CultureInfo.InvariantCulture);
            double dL = Convert.ToDouble(TBDl.Text, System.Globalization.CultureInfo.InvariantCulture);
            double z0 = Convert.ToDouble(TBZ0.Text, System.Globalization.CultureInfo.InvariantCulture);
            double Er = Convert.ToDouble(TBEr.Text, System.Globalization.CultureInfo.InvariantCulture);
            double f0 = Convert.ToDouble(TBFreq.Text, System.Globalization.CultureInfo.InvariantCulture);
            double C = Convert.ToDouble(TBC.Text, System.Globalization.CultureInfo.InvariantCulture);
            int N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
            this.net = new Net(sizeX, sizeY, sigma, dL, z0, Er, f0, C, N, new Boundaries(bTop, bBot, bLeft, bRight));

        }

        private void BTRun_Click(object sender, EventArgs e)
        {
            net.Fk = TBInputFunc.Text;
            genious = new Thread(new ThreadStart(net.Run));
            net.Progress += netStatus;
            genious.Start();
            genious.Join();
        }

        private void netStatus(object sender, int e)
        {
            StatusLabel.Text = string.Format("Iteration {0}", e);
        }


    }
}
