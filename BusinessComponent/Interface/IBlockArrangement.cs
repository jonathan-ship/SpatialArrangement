using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Diagnostics;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface
{
    public interface IBlockArrangement
    {
        //지번 자동 할당
        List<UnitcellDTO[,]> InitializeArrangementMatrixWithAddress(List<WorkshopDTO> WorkShopList);
        List<Int32[,]> InitializeArrangementMatrix(List<WorkshopDTO> WorkShopList);
        List<Int32[,]> InitializeArrangementMatrixWithSlack(List<WorkshopDTO> WorkShopList, int Slack);


        //BLF 알고리즘 수행
        DateTime[] CalcEarliestAndLatestDates(List<BlockDTO> _blockInfoList);
        ArrangementResultWithDateDTO RunBLFAlgorithm(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArranegementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel);
        ArrangementResultWithDateDTO RunBLFAlgorithmWithAddress(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArranegementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel);
        ArrangementResultWithDateDTO RunBLFAlgorithmWithSlack(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArranegementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack);
        ArrangementResultWithDateDTO RunBLFAlgorithmWithSlackWithPriority(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArranegementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack);
        ArrangementResultWithDateDTO RunBLFAlgorithmWithFloatingCrane(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArranegementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack, int ExportWorkshopIndex);

    }
}
