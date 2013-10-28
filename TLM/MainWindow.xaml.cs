using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Net net;
        private List<Material> defaultMaterials;

        public MainWindow()
        {

            InitializeComponent();
            var signal = ILMath.ones<float>(10, 10);

            defaultMaterials = new List<Material>() {
                new Material("Air", 1, 5E-15),
                new Material("Teflon", 2.1, 1E-24),
                new Material("Wood", 4, 1E-15),
                new Material("Concrete", 5, 13E-3),
                new Material("Glass", 4.7, 1E-13),
                new Material("Rubber", 7, 1E-14),
                new Material("Diamond", 7.5, 1E-13),
                new Material("Graphite", 1E-15, 2.5E5),
                new Material("Silicon", 11.68, 1.56E-3),
                new Material("Sulfur", 3.5, 1E-16),
                new Material("Water(20°C)", 80.1, 5E-3)
            };
            CBMat.ItemsSource = defaultMaterials.Where(mat => mat.Name != "");

            DGMatList.ItemsSource = defaultMaterials;
            DGMatList.RowEditEnding += DGMatList_RowEditEnding;

            var scene = new ILScene {
                new ILPlotCube(twoDMode: false) {
                    //new ILPoints {
                    //    Positions = signal,
                    //    Color = null,
                    //    Colors = signal,
                    //    Size = 2,
                    //}
                    new ILSurface(signal) {
                        Wireframe = { Color = System.Drawing.Color.FromArgb(50, 60, 60, 60) },
                        Colormap = Colormaps.Summer, 
                    }
                }
            };
            scene.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1f, 0.23f, 1), 0.7f);
            ilPanel.Scene.Add(scene);
        }

        void DGMatList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            CBMat.ItemsSource = defaultMaterials.Where(mat => mat.Name != "");
        }

        void DGMatList_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            CBMat.ItemsSource = defaultMaterials.Where(mat => mat.Name != "");
        }

        public void CreateNet()
        {
            double sizeX = Convert.ToDouble(TBSizeX.Text, System.Globalization.CultureInfo.InvariantCulture);
            double sizeY = Convert.ToDouble(TBSizeY.Text, System.Globalization.CultureInfo.InvariantCulture);
            Material mat = CBMat.SelectedItem as Material;
            double dL = Convert.ToDouble(TBdL.Text, System.Globalization.CultureInfo.InvariantCulture);
            double z0 = Convert.ToDouble(TBZ0.Text, System.Globalization.CultureInfo.InvariantCulture);            
            double f0 = Convert.ToDouble(TBFreq.Text, System.Globalization.CultureInfo.InvariantCulture);
            double C = Convert.ToDouble(TBC.Text, System.Globalization.CultureInfo.InvariantCulture);
            int N = Convert.ToInt32(TBN.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bTop = Convert.ToDouble(TBBoundTop.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bLeft = Convert.ToDouble(TBBoundLeft.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bBot = Convert.ToDouble(TBBoundBot.Text, System.Globalization.CultureInfo.InvariantCulture);
            double bRight = Convert.ToDouble(TBBoundRight.Text, System.Globalization.CultureInfo.InvariantCulture);
            
            this.net = new Net(sizeX, sizeY, mat, dL, z0, f0, C, N, new Boundaries(bTop, bBot, bLeft, bRight));
        }
        

        private void BTCreateNet_Click(object sender, RoutedEventArgs e)
        {
            CreateNet();
            Designer.WorkingNet = this.net;
            Designer.DrawNet();
        }
    }
}
