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
    public partial class frmArrangementRangeSetting : Form
    {
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public frmArrangementRangeSetting(DateTime earliestDate, DateTime latestDate)
        {
            InitializeComponent();
            monthCalendar1.SelectionStart = earliestDate;
            monthCalendar2.SelectionStart = latestDate;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            StartDate = monthCalendar1.SelectionStart;
            textBox1.Text = monthCalendar1.SelectionStart.ToString("yyyy-MM-dd");
        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (DateTime.Compare(StartDate, monthCalendar2.SelectionStart) > 0)
            {
                MessageBox.Show("종료일이 시작일보다 빠릅니다.");
                monthCalendar2.SelectionStart = StartDate;
            }
            else
            {
                FinishDate = monthCalendar2.SelectionStart;
                textBox2.Text = monthCalendar2.SelectionStart.ToString("yyyy-MM-dd");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartDate = monthCalendar1.SelectionStart;
            FinishDate = monthCalendar2.SelectionStart;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
