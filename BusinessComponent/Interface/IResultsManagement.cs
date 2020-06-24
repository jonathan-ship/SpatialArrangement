using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Diagnostics;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface
{
    public interface IResultsManagement
    {
        //차트 초기화
        void InitializeIndividualWorkshopChart(List<WorkshopDTO> _workshopInfoList, List<Chart> _chartList, Chart _chart, TabControl _tabPage);
        void InitializeTotalWorkshopChart(Chart _chart);

        //배치 결과 차트에 출력
        void DrawChart(ArrangementResultWithDateDTO _resultInfo, List<Chart> _chartList, Chart _chart);
        void DrawChart(ArrangementResultWithDateDTO _resultInfo, List<Chart> _chartList, Chart _chart, bool IsPlateArrangement);


        //배치 결과 출력
        List<UnitcellDTO[,]> GenterateArrangementMatrixFromBlockinfo(List<BlockDTO> InpuBlockList, List<UnitcellDTO[,]> WorkShopArrangementInfo);
        List<UnitcellDTO[,]> GenterateArrangementMatrixFromPlateinfo(List<PlateConfigDTO> InpuBlockList, List<UnitcellDTO[,]> WorkShopArrangementInfo);



        void PrintArrangementResult(ArrangementResultWithDateDTO _resultInfo, string _fileName);
        void PrintArrangementMatrixListResultWithDate(List<UnitcellDTO[,]> ArrangementMatrix, string _workshopFileName, string _blockFileName, ArrangementResultWithDateDTO _resultInfo, int _selectedDate);
        void PrintArrangementMatrixResult(UnitcellDTO[,] ArrangementMatrix, string _workshopFileName, string _blockFileName);
        void PrintArrangementMatrixResult(double[,] ArrangementMatrix, string _workshopFileName, string _blockFileName, int workshopNum);
        void PrintBLFAlgorithmResultsSummary(ArrangementResultWithDateDTO _resultInfo, Stopwatch _sw);
        
        

        
    }
}
