using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    public partial class frmGreedyObjectParameterSetting : Form
    {
        public double CoefficientOfTwistNum { get; set; }
        public double CoefficientOfAdjacentNum { get; set; }
        public double CoefficientOfAdjacentLength { get; set; }
        public double CoefficientOfDistanceofCentroid { get; set; }

        public frmGreedyObjectParameterSetting()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CoefficientOfTwistNum = Convert.ToDouble(tboxNumofTwist.Text);
            CoefficientOfAdjacentNum = Convert.ToDouble(tboxNumofAdjacentNum.Text);
            CoefficientOfAdjacentLength = Convert.ToDouble(tboxAdjacentLength.Text);
            CoefficientOfDistanceofCentroid = Convert.ToDouble(tboxDistanceofCentroid.Text);
        }
    }
}
