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
using NCalc;
using TLM.Core;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

namespace TLM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread solver;
        private Net net = new Net();
        private ILPlotCube plot, plotHx, plotHy, plotHz, plotEx, plotEy, tracker;
        private float maxVEz, maxVHx, maxVHy, maxVHz, maxVEx, maxVEy;

        public MainWindow()
        {
            InitializeComponent();

            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");

            Designer.TrackNode += Designer_TrackNode;

            CBMode.Items.Add("Paralelo");
            CBMode.Items.Add("Série");

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
            plotHy.Projection = Projection.Orthographic;
            plotHy.Position = new Vector3(1, 3, .5);
            plotHy.AllowRotation = false;
            plotHy.Add(surfHy);
            sceneHy.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneHy.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelHy.Scene.Add(sceneHy);

            ResultSeekerHy.ValueChanged += ResultSeekerHy_ValueChanged;

            var sceneHz = new ILScene();
            plotHz = new ILPlotCube(twoDMode: false);
            sceneHz.Add(plotHz);
            var signalHz = ILMath.ones<float>(10, 10);
            ILSurface surfHz = new ILSurface(signalHz)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,

            };
            plotHz.Projection = Projection.Orthographic;
            plotHz.Position = new Vector3(1, 3, .5);
            //plotHz.AllowRotation = false;
            plotHz.Add(surfHz);
            sceneHz.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneHz.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelHz.Scene.Add(sceneHz);

            ResultSeekerHz.ValueChanged += ResultSeekerHz_ValueChanged;

            var sceneEx = new ILScene();
            plotEx = new ILPlotCube(twoDMode: false);
            sceneEx.Add(plotEx);
            var signalEx = ILMath.ones<float>(10, 10);
            ILSurface surfEx = new ILSurface(signalEx)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotEx.Projection = Projection.Orthographic;
            plotEx.Position = new Vector3(1, 3, .5);
            //plotEx.AllowRotation = false;
            plotEx.Add(surfEx);
            sceneEx.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneEx.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelEx.Scene.Add(sceneEx);

            ResultSeekerEx.ValueChanged += ResultSeekerEx_ValueChanged;

            var sceneEy = new ILScene();
            plotEy = new ILPlotCube(twoDMode: false);
            sceneEy.Add(plotEy);
            var signalEy = ILMath.ones<float>(10, 10);
            ILSurface surfEy = new ILSurface(signalEy)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotEy.Projection = Projection.Orthographic;
            plotEy.Position = new Vector3(1, 3, .5);
            //plotEy.AllowRotation = false;
            plotEy.Add(surfEy);
            sceneHy.First<ILPlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            sceneHy.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(0.1, 0, 0), ILMath.pi / 2);
            ilPanelEy.Scene.Add(sceneEy);

            ResultSeekerEy.ValueChanged += ResultSeekerEy_ValueChanged;
        }

        void Designer_TrackNode(object sender, EventArgs e)
        {
            UpdateTrackerPlot();
        }

        #region Eventos
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

        void ResultSeekerHz_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlotHz(Convert.ToInt32(ResultSeekerHz.Value));
        }

        void ResultSeekerEx_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlotEx(Convert.ToInt32(ResultSeekerEx.Value));
        }

        void ResultSeekerEy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePlotEy(Convert.ToInt32(ResultSeekerEy.Value));
        }

        void DGMatList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");
        }
        #endregion

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
                net.N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture) + 1;
                double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
                double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
                net.boundaries = new Boundaries(bTop, bBot, bLeft, bRight);
                net.Fk = TBFk.Text;
                net.mode = CBMode.SelectedIndex;
                net.Calc(CBMode.SelectedIndex);
            }
            catch
            {
            }
        }

        public void UpdateUI()
        {
            TBSizeX.Text = net.x.ToString();
            TBSizeY.Text = net.y.ToString();
            CBMat.SelectedItem = net.material;
            TBdL.Text = net.dL.ToString();
            TBZ0.Text = net.Z0.ToString();
            TBFreq.Text = net.f0.ToString();
            TBC.Text = net.c.ToString();
            TBN.Text = net.N.ToString();
            TBBoundBot.Text = net.boundaries.Bottom.ToString();
            TBBoundLeft.Text = net.boundaries.Left.ToString();
            TBBoundRight.Text = net.boundaries.Right.ToString();
            TBBoundTop.Text = net.boundaries.Top.ToString();
            TBFk.Text = net.Fk.ToString();

            CBMat.ItemsSource = net.matList.Where(mat => mat.Name != "");
            Designer.MatList.ItemsSource = net.matList.Where(mat => mat.Name != "");

            DGMatList.ItemsSource = net.matList;

            Designer.DrawNet();
        }

        private void BTRun_Click(object sender, RoutedEventArgs e)
        {
            StatusInfo.Text = "Simulation started, please wait...";
            solver = new Thread(new ThreadStart(net.Run));
            solver.Start();
            solver.Join();
            StatusInfo.Text = "Simulation done.";
            if (this.net.mode == 0)
            {
                TBIEz.IsEnabled = true;
                TBIHx.IsEnabled = true;
                TBIHy.IsEnabled = true;
                ResultSeeker.Maximum = net.N - 1;
                ResultSeeker.Value = 0;
                ResultSeekerHx.Maximum = net.N - 1;
                ResultSeekerHx.Value = 0;
                ResultSeekerHy.Maximum = net.N - 1;
                ResultSeekerHy.Value = 0;
            }
            else
            {
                TBIHz.IsEnabled = true;
                TBIEx.IsEnabled = true;
                TBIEy.IsEnabled = true;
                ResultSeekerHz.Maximum = net.N - 1;
                ResultSeekerHz.Value = 0;
                ResultSeekerEx.Maximum = net.N - 1;
                ResultSeekerEx.Value = 0;
                ResultSeekerEy.Maximum = net.N - 1;
                ResultSeekerEy.Value = 0;
            }
            UpdateTrackerPlot();
        }

        #region Plotagens
        private void UpdateTrackerPlot()
        {
            ilPanelTracker.Scene.Children.Clear();
            var scene = new ILScene();
            // add plot cube
            tracker = scene.Add(new ILPlotCube());
            // meterialize new colormap
            ILColormap m = new ILColormap(Colormaps.Lines);
            // some data to plot 
            var datas = (from node in Designer.TrackingNodes
                         select node.GetAllEZs().ToArray()).ToList();

            //ILArray<float> data = ILMath.tosingle(ILMath.ones(1, 10));
            //// create a line plot for each entry in the colormap
            foreach (var data in datas)
            {
                ILArray<float> d = ILMath.tosingle((ILArray<double>)data);
                d = d.Reshape(new int[] { 1, d.Count() });
                tracker.Add(new ILLinePlot(d));
            }
            ilPanelTracker.Scene.Add(scene);
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
            maxVEz = (maxVEz < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVEz;
            ResultSeeker.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVEz);
            plot.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,

            };
            plot.Add(surf);
            plot.Limits.Set(new Vector3(0, 0, -maxVEz),
                                new Vector3(net.shape[1], net.shape[0], maxVEz));
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
            maxVHx = (maxVHx < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVHx;
            ResultSeekerHx.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVHx);
            plotHx.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotHx.Add(surf);
            plotHx.Limits.Set(new Vector3(0, 0, -maxVHx),
                                new Vector3(net.shape[1], net.shape[0], maxVHx));
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
            maxVHy = (maxVHy < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVHy;
            ResultSeekerHy.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVHy);
            plotHy.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotHy.Add(surf);
            plotHy.Limits.Set(new Vector3(0, 0, -maxVHy),
                                new Vector3(net.shape[1], net.shape[0], maxVHy));
            ilPanelHy.Refresh();
        }
        private void UpdatePlotHz(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select node.GetHz(iteration)).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            maxVHz = (maxVHz < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVHz;
            ResultSeekerHz.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVHz);
            plotHz.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,

            };
            plotHz.Add(surf);
            //plotHz.Limits.Set(new Vector3(0, 0, -maxVHz),
            //                    new Vector3(net.shape[1], net.shape[0], maxVHz));
            ilPanelHz.Refresh();
        }
        private void UpdatePlotEx(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select (node.GetEx(node, iteration))).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            maxVEx = (maxVEx < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVEx;
            ResultSeekerEx.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVEx);
            plotEx.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotEx.Add(surf);
            //plotEx.Limits.Set(new Vector3(0, 0, -maxVEx),
            //                    new Vector3(net.shape[1], net.shape[0], maxVEx));
            ilPanelEx.Refresh();
        }
        private void UpdatePlotEy(int iteration)
        {
            var dvalues = (from node in net.Nodes
                           orderby node.j ascending
                           orderby node.i ascending
                           select (node.GetEy(node, iteration))).ToArray();
            var values = ILMath.tosingle((ILArray<double>)dvalues);
            values = ILMath.reshape(values, new int[] { net.shape[0], net.shape[1] });
            double time = iteration * net.dT * 1E10;
            int T = Convert.ToInt32(Math.Floor(time));
            maxVEy = (maxVEy < Convert.ToSingle(Math.Round(dvalues.Max(), 2))) ? Convert.ToSingle(Math.Round(dvalues.Max(), 2)) :
                                                                                maxVEy;
            ResultSeekerEy.ToolTip = string.Format("Iteration(k):{0} - Period(T):{1} - Max. Value:{2}", iteration, T, maxVEy);
            plotEy.Children.Clear();
            var signal = ILMath.ones<float>(10, 10);
            ILSurface surf = new ILSurface(values)
            {
                Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                Colormap = Colormaps.Jet,
            };
            plotEy.Add(surf);
            //plotEy.Limits.Set(new Vector3(0, 0, -maxVEy),
            //                    new Vector3(net.shape[1], net.shape[0], maxVEy));
            ilPanelEy.Refresh();
        }
        #endregion

        private void BTRecalcNet_Click_1(object sender, RoutedEventArgs e)
        {
            CreateNet();
            Designer.WorkingNet = this.net;
            Designer.DrawNet();
        }

        private void MenuSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Arquivo de Simulação TLM|*.tlm";
            saveFileDialog1.Title = "Salvar arquivo de Simulação TLM";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Net));
                TextWriter writer = new StreamWriter(saveFileDialog1.FileName);
                serializer.Serialize(writer, this.net);
                writer.Close();
            }
        }

        private void MenuLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Arquivo de Simulação TLM|*.tlm";
            OpenDialog.Title = "Abrir arquivo de Simulação TLM";
            OpenDialog.ShowDialog();
            if (OpenDialog.FileName != "" && File.Exists(OpenDialog.FileName))
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Net));
                    fs = new FileStream(OpenDialog.FileName, FileMode.Open);
                    this.net = (Net)serializer.Deserialize(fs);
                    //Fix material bug.
                    this.net.Nodes.ForEach(n => n.material = this.net.GetMaterial(n.material.Name));
                    this.net.material = this.net.GetMaterial(this.net.material.Name);

                    Designer.WorkingNet = this.net;
                    UpdateUI();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
        
        private void CBMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBMode.SelectedIndex == 0)
            {
                TBSizeX.Text = "10E-2";
                TBSizeY.Text = "2E-2";
                TBdL.Text = "1E-3";
                TBBoundTop.Text = "1";
                TBBoundLeft.Text = "0";
                TBBoundBot.Text = "1";
                TBBoundRight.Text = "0";
                TBFk.Text = "Sin(2*[Pi]*[f0]*([k]+1)*[dT])*100";
            }
            else
            {
                TBSizeX.Text = "19.05E-3";
                TBSizeY.Text = "9.525E-3";
                TBdL.Text = "0.381E-3";
                TBBoundTop.Text = "-1";
                TBBoundLeft.Text = "-1";
                TBBoundBot.Text = "-1";
                TBBoundRight.Text = "-1";
                TBFk.Text = "Exp(-18*(Pow(((((([k]+1)*[dT])-(10*[dT])))/(10*[dT])),2)))";
            }
        }
    }
}
