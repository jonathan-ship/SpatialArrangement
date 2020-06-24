using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;

namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    public partial class frmResultsViewer : Form
    {
        public frmResultsViewer()
        {
            InitializeComponent();
        }

        public frmResultsViewer(ArrangementResultWithDateDTO _resultsInfo, List<WorkshopDTO> _workshopInfoList)
        {
            InitializeComponent(_resultsInfo, _workshopInfoList);
        }

        public frmResultsViewer(List<BlockDTO> _blockInfoList, List<WorkshopDTO> _workshopInfoList, List<UnitcellDTO[,]> _arrangementMatrixList)
        {
            InitializeComponent(_blockInfoList, _workshopInfoList, _arrangementMatrixList);
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void elementHost1_ChildChanged_1(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void elementHost1_ChildChanged_2(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
