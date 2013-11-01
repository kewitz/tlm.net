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
            }

            DesignCanvas.Width = WorkingNet.shape[0] * 10;
            DesignCanvas.Height = WorkingNet.shape[1] * 10;
        }

        void BrushEvent(Objects.Node s)
        {
            //Objects.Node s = (Objects.Node)sender;
            //Seta Material
            if (ToggleMaterial.IsChecked == true)
            {
                s.node.material = s.node.material == WorkingNet.material ? (Material)MatList.SelectedValue : WorkingNet.material;
                s.node.Gs = (s.node.material.Sigma * s.node.dL) / s.node.Ylt;
                s.node.Ys = 4 * (s.node.material.Er - 1);
                s.node.Y = 4 + s.node.Ys + s.node.Gs;
            }
            //Seta Input
            if (ToggleInput.IsChecked == true)
            {
                s.node.input = !s.node.input;
            }
            s.Redraw();
        }

        void graphicNode_MouseEnter(object sender, MouseEventArgs e)
        {
            Objects.Node s = (Objects.Node)sender;
            string info = string.Format("{0}:{1}  -  Material: {2}", s.node.i, s.node.j, s.node.material.Name);
            NodeInfo.Content = info;
            if (e.LeftButton == MouseButtonState.Pressed)
                BrushEvent(s);
        }

        public void UpdateInfoLabel(string s)
        {
            NodeInfo.Content = s;
        }

    }
}
