using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eoba.Shipyard.ArrangementSimulator.MainUI;

namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    public partial class frmSetParameter : Form
    {
        public string temp1 { get; set; }
        public string temp2 { get; set; }

        public frmSetParameter()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.temp1 = textBox1.Text;
            this.temp2 = textBox2.Text;
            this.DialogResult = DialogResult.OK;

                this.Close();
        }




        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmSetParameter_Load(object sender, EventArgs e)
        {

        }
    }
}
