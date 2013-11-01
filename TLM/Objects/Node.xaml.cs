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

namespace TLM.Objects
{
    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Node : UserControl
    {
        public TLM.Core.Node node;
        public Color color;
        public bool Tracking;

        public Node(TLM.Core.Node n)
        {
            InitializeComponent();
            this.node = n;
            this.Tracking = false;
            this.Redraw();
        }

        public void Redraw()
        {
            this.color = Color.FromArgb(node.material.color.A, node.material.color.R, node.material.color.G, node.material.color.B);
            Dot.Stroke = new SolidColorBrush(color);
            Dot.Fill = this.node.input? new SolidColorBrush(color) : Brushes.Transparent;
            IsTracked.Visibility = Tracking ? Visibility.Visible : Visibility.Hidden;
        }


    }
}
