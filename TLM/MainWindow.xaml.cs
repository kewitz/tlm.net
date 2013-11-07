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
        private ILPlotCube plot, plotHx, plotHy;

        public MainWindow()
        {
            InitializeComponent();

            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");

            DGMatList.ItemsSource = net.matList;
            DGMatList.RowEditEnding += DGMatList_RowEditEnding;

            var scene = new ILScene();
            plot = new ILPlotCube(twoDMode: false);            
            scene.Add(plot);
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(signal)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,                
            };
            plot.AllowPan = false;
            plot.AllowZoom = false;                       
            plot.Projection = Projection.Orthographic;
            plot.Position = new Vector3(1, 3, .5);
            plot.AllowRotation = false;            
            plot.Add(surf);
            scene.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            scene.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanel.Scene.Add(scene);

            ResultSeeker.ValueChanged += ResultSeeker_ValueChanged;

            var sceneHx = new ILScene();
            plotHx = new ILPlotCube(twoDMode: false);
            sceneHx.Add(plotHx);
            var signalHx = ILMath.ones<float>(10, 10);
            ILSurface surfHx = new ILSurface(signalHx)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotHx.AllowPan = false;
            plotHx.AllowZoom = false;
            plotHx.Projection = Projection.Orthographic;
            plotHx.Position = new Vector3(1, 3, .5);
            plotHx.AllowRotation = false;
            plotHx.Add(surfHx);
            sceneHx.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneHx.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelHx.Scene.Add(sceneHx);

            ResultSeekerHx.ValueChanged += ResultSeekerHx_ValueChanged;

            var sceneHy = new ILScene();
            plotHy = new ILPlotCube(twoDMode: false);
            sceneHy.Add(plotHy);
            var signalHy = ILMath.ones<float>(10, 10);
            ILSurface surfHy = new ILSurface(signalHy)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotHy.AllowPan = false;
            plotHy.AllowZoom = false;
            plotHy.Projection = Projection.Orthographic;
            plotHy.Position = new Vector3(1, 3, .5);
            plotHy.AllowRotation = false;
            plotHy.Add(surfHy);
            sceneHy.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneHy.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelHy.Scene.Add(sceneHy);

            ResultSeekerHy.ValueChanged += ResultSeekerHy_ValueChanged;
        }

        void ResultSeeker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlot(Convert.ToInt32(ResultSeeker.Value));
        }

        void ResultSeekerHx_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlotHx(Convert.ToInt32(ResultSeekerHx.Value));
        }

        void ResultSeekerHy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlotHy(Convert.ToInt32(ResultSeekerHy.Value));
        }

        void DGMatList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");
        }


        public void CreateNet()
        {
            try
            {
                net.x = Convert.ToDouble(TBSizeX.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.y = Convert.ToDouble(TBSizeY.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.material = CBMat.SelectedItem as Material;
                net.dL = Convert.ToDouble(TBdL.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.Z0 = Convert.ToDouble(TBZ0.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.f0 = Convert.ToDouble(TBFreq.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.c = Convert.ToDouble(TBC.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture)+1;
                double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.boundaries = new Boundaries(bTop, bBot, bLeft, bRight);
                net.Fk = TBFk.Text;
                net.Calc();
            }
            catch
            {
            }
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
            ResultSeekerHx.Maximum = net.N - 1;
            ResultSeekerHx.Value = 0;
            ResultSeekerHy.Maximum = net.N - 1;
            ResultSeekerHy.Value = 0;
        }

        private void UpdatePlot(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select node.GetEz(iteration)).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            float maxV = Convert.ToSingle(Math.Round(dvalues.Max(),2));
            ResultSeeker.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxV);
            plot.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };                        
            plot.Add(surf);
            ilPanel.Refresh();
        }

        private void UpdatePlotHx(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select (node.GetHx(node, iteration))).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            float maxV = Convert.ToSingle(Math.Round(dvalues.Max(), 2));
            ResultSeekerHx.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxV);
            plotHx.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };            
            plotHx.Add(surf);
            ilPanelHx.Refresh();
        }

        private void UpdatePlotHy(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select (node.GetHy(node, iteration))).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            float maxV = Convert.ToSingle(Math.Round(dvalues.Max(), 2));
            ResultSeekerHy.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxV);            
            plotHy.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };                      
            plotHy.Add(surf);
            ilPanelHy.Refresh();
        }

        private void BTRecalcNet_Click_1(object sender, RoutedEventArgs e)
        {
            CreateNet();
            Designer.WorkingNet = this.net;
            Designer.DrawNet();
        }

    }
}
