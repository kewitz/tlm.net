using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace TLM
{
    /// <summary>
    /// Interaction logic for NetDesigner.xaml
    /// </summary>
    public partial class NetDesigner : UserControl
    {
        public Net WorkingNet { get; set; }
        public int Spacing = 10;
        public event EventHandler TrackNode;

        public List<Node> TrackingNodes
        {
            get
            {
                var a = from gnode in DesignCanvas.Children.OfType<Objects.Node>()
                        where gnode.Tracking == true
                        select gnode.node;
                return a.ToList();
            }
        }

        public NetDesigner()
        {
            InitializeComponent();
        }

        public void DrawNet()
        {
            DesignCanvas.Children.Clear();
            foreach (Node n in WorkingNet.Nodes)
            {
                Objects.Node graphicNode = new Objects.Node(n);
                DesignCanvas.Children.Add(graphicNode);
                graphicNode.Margin = new Thickness(n.j * Spacing, n.i * Spacing, 0, 0);
                graphicNode.MouseEnter += graphicNode_MouseEnter;
                graphicNode.MouseLeftButtonDown += graphicNode_MouseLeftButtonDown;
            }

            DesignCanvas.Width = WorkingNet.shape[0] * 10;
            DesignCanvas.Height = WorkingNet.shape[1] * 10;
        }

        void graphicNode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Objects.Node s = (Objects.Node)sender;
            BrushEvent(s);
        }

        void BrushEvent(Objects.Node s)
        {
            //Objects.Node s = (Objects.Node)sender;
            //Seta Material
            if (ToggleMaterial.IsChecked == true)
            {
                s.node.material = s.node.material == WorkingNet.material ? (Material)MatList.SelectedValue : WorkingNet.material;
            }
            //Seta Input
            if (ToggleInput.IsChecked == true)
            {
                s.node.input = !s.node.input;
            }
            if (ToggleTrack.IsChecked == true)
            {
                s.Tracking = !s.Tracking;
                EvokeTrackNode(s.node);
            }
            s.Redraw();
        }

        void EvokeTrackNode(Node n)
        {
            if (TrackNode != null)
                this.TrackNode(n, null);
        }

        void graphicNode_MouseEnter(object sender, MouseEventArgs e)
        {
            Objects.Node s = (Objects.Node)sender;
            string info = string.Format("{0}:{1}  -  Material: {2}\nMax EZ: {3}", s.node.i, s.node.j, s.node.material.Name, s.node.GetAllEZs().Max());
            NodeInfo.Content = info;
            if (e.LeftButton == MouseButtonState.Pressed)
                BrushEvent(s);
        }

        public void UpdateInfoLabel(string s)
        {
            NodeInfo.Content = s;
        }

        private void ChangeTool(object sender, RoutedEventArgs e)
        {
            var tools = new List<System.Windows.Controls.Primitives.ToggleButton> { ToggleInput, ToggleMaterial, ToggleTrack };
            var thistool = ((System.Windows.Controls.Primitives.ToggleButton)sender);
            tools.Where(t => t != thistool).ToList().ForEach(t => t.IsChecked = false);
        }

    }
}
