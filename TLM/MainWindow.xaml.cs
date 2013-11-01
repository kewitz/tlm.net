using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TLM.Core;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;

namespace TLM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread solver;
        private Net net = new Net();
        private ILScene scene;
        private ILPlotCube plot;

        public MainWindow()
        {
            InitializeComponent();


            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");

            DGMatList.ItemsSource = net.matList;
            DGMatList.RowEditEnding += DGMatList_RowEditEnding;

            scene = new ILScene();
            plot = new ILPlotCube(twoDMode: false);
            scene.Add(plot);
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(signal)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Summer,
            };
            plot.Add(surf);
            scene.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);
            ilPanel.Scene.Add(scene);

            ResultSeeker.ValueChanged += ResultSeeker_ValueChanged;
        }

        void ResultSeeker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlot(Convert.ToInt32(ResultSeeker.Value));
        }

        void DGMatList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");
        }


        public void CreateNet()
        {
            net.x = Convert.ToDouble(TBSizeX.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.y = Convert.ToDouble(TBSizeY.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.material = CBMat.SelectedItem as Material;
            net.dL = Convert.ToDouble(TBdL.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.Z0 = Convert.ToDouble(TBZ0.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.f0 = Convert.ToDouble(TBFreq.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.c = Convert.ToDouble(TBC.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.E0Init = Convert.ToDouble(TBE0.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.boundaries = new Boundaries(bTop, bBot, bLeft, bRight);           
            net.Fk = TBFk.Text;
            net.Calc();
            TBdL.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBZ0.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBFreq.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBC.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBN.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBE0.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBBoundTop.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBBoundLeft.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBBoundBot.LostFocus += new RoutedEventHandler(TB_TextChanged);
            TBBoundRight.LostFocus += new RoutedEventHandler(TB_TextChanged);
        }

        private void BTCreateNet_Click(object sender, RoutedEventArgs e)
        {
            CreateNet();
            Designer.WorkingNet = this.net;
            Designer.DrawNet();
        }

        private void BTRun_Click(object sender, RoutedEventArgs e)
        {
            StatusInfo.Text = "Simulation started, please wait...";
            solver = new Thread(new ThreadStart(net.Run));
            solver.Start();
            solver.Join();
            StatusInfo.Text = "Simulation done.";
            ResultSeeker.Maximum = net.N - 1;
            ResultSeeker.Value = 0;
        }

        private void UpdatePlot(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select node.GetEz(iteration)).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            //double p = net.f0*net.dT*iteration;
            //int period = Convert.ToInt32(Math.Round(p));            
            //ResultSeeker.ToolTip = string.Format("Iteration (N):{0}  -  Periods (T): {1}", iteration, period);

            plot.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Summer,
            };
            plot.Add(surf);
            //scene.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);
            ilPanel.Refresh();
        }



        private void UpdateRunParameters()
        {
            net.dL = Convert.ToDouble(TBdL.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.Z0 = Convert.ToDouble(TBZ0.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.f0 = Convert.ToDouble(TBFreq.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.c = Convert.ToDouble(TBC.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.E0Init = Convert.ToDouble(TBE0.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
            net.boundaries = new Boundaries(bTop, bBot, bLeft, bRight);
            net.Fk = TBFk.Text;
            net.Calc();
        }

        private void TB_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                UpdateRunParameters();
            }
        }
       
    }
}
