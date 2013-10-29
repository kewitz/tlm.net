﻿using System;
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

        void graphicNode_MouseEnter(object sender, MouseEventArgs e)
        {
            Objects.Node s = (Objects.Node)sender;
            string info = string.Format("{0}:{1}  -  Material: {2}", s.node.i, s.node.j, s.node.material.Name);
            NodeInfo.Content = info;
        }

        public void UpdateInfoLabel(string s)
        {
            NodeInfo.Content = s;
        }
                
    }
}
