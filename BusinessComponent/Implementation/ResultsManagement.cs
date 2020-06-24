using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Implementation
{
    public class ResultsManagement: IResultsManagement
    {
        /// <summary>
        /// 각 작업장의 배치 결과를 요약하여 출력할 차트를 초기화 하는 함수
        /// </summary>
        /// <param name="_workshopInfoList">작업장 정보 리스트</param>
        /// <param name="_chartList">가시화 할 차트 리스트</param>
        /// <param name="_chart">이미 생성되어 있는 0번째 작업장 차트</param>
        /// <param name="_tabPage">가시화 할 탭 페이지</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        void IResultsManagement.InitializeIndividualWorkshopChart(List<WorkshopDTO> _workshopInfoList, List<Chart> _chartList, Chart _chart, TabControl _tabPage)
        {
            _chartList.Add(_chart);

            for (int i = 1; i < _workshopInfoList.Count; i++)
            {
                ChartArea tempChartArea = new ChartArea();
                tempChartArea.CursorX.IsUserEnabled = true;
                tempChartArea.CursorX.IsUserSelectionEnabled = true;
                tempChartArea.CursorX.SelectionColor = System.Drawing.Color.Transparent;
                tempChartArea.Name = "ChartArea" + Convert.ToInt16(i + 1);

                Chart tempChart = new Chart();
                tempChart.ChartAreas.Add(tempChartArea);
                tempChart.Dock = System.Windows.Forms.DockStyle.Fill;
                tempChart.Location = new System.Drawing.Point(3, 3);
                tempChart.Name = "ChartWorkshop1";
                tempChart.Size = new System.Drawing.Size(948, 238);
                tempChart.TabIndex = 0;
                tempChart.Text = "chart1";

                _chartList.Add(tempChart);
            }

            //tabpage 생성
            for (int i = 1; i < _workshopInfoList.Count; i++)
            {
                TabPage tempTabpages = new TabPage();
                tempTabpages.Controls.Add(_chartList[i]);
                tempTabpages.Location = new System.Drawing.Point(4, 22);
                tempTabpages.Name = "tpgWorkstage" + Convert.ToString(i + 1);
                tempTabpages.Padding = new System.Windows.Forms.Padding(3);
                tempTabpages.Size = new System.Drawing.Size(954, 244);
                tempTabpages.TabIndex = 0;
                tempTabpages.Text = "Workshop" + Convert.ToString(i + 1);
                tempTabpages.UseVisualStyleBackColor = true;

                _tabPage.TabPages.Add(tempTabpages);
            }

            // 각 차트를 차트 리스트에 입력

            //4개의 작업장 갯수 만큼 시리즈를 생성하여 각 차트에 설정
            Series IndividualLineSerise; //면적활용률
            Series IndividualBarSerise; //배치블록 갯수

            for (int i = 0; i < _chartList.Count; i++)
            {
                IndividualLineSerise = new Series();
                IndividualLineSerise.ChartType = SeriesChartType.Line;
                IndividualLineSerise.XValueType = ChartValueType.Time;
                IndividualLineSerise.Name = "면적 활용률";

                IndividualBarSerise = new Series();
                IndividualBarSerise.ChartType = SeriesChartType.Column;
                IndividualBarSerise.XValueType = ChartValueType.Time;
                IndividualBarSerise.Name = "배치블록 갯수";

                _chartList[i].Series.Add(IndividualBarSerise);
                _chartList[i].Series.Add(IndividualLineSerise);

                _chartList[i].ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                _chartList[i].ChartAreas[0].AxisX.LabelStyle.Format = "yy-MM-dd";
            }
            for (int i = 0; i < _chartList.Count; i++)
            {
                _chartList[i].Series[0].Points.Clear();
                _chartList[i].Series[1].Points.Clear();
            }
        }

        /// <summary>
        /// 전체 작업장 배치 결과를 종합하여 출력할 차트를 초기화 하는 함수
        /// </summary>
        /// <param name="_chart">전체 작업장 정보를 출력할 차트</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        void IResultsManagement.InitializeTotalWorkshopChart(Chart _chart)
        {
            Series TotalLineSeries = new Series();
            Series TotalBarSeries = new Series();
            Series TotalDelaySeries = new Series();

            TotalLineSeries.ChartType = SeriesChartType.Line;
            TotalLineSeries.XValueType = ChartValueType.Time;
            TotalLineSeries.Name = "평균 면적 활용률";

            TotalBarSeries.ChartType = SeriesChartType.Column;
            TotalBarSeries.XValueType = ChartValueType.Time;
            TotalBarSeries.Name = "평균 배치블록 갯수";

            TotalDelaySeries.ChartType = SeriesChartType.Column;
            TotalDelaySeries.XValueType = ChartValueType.Time;
            TotalDelaySeries.Name = "일별 지연블록 갯수";
            TotalDelaySeries.Color = Color.Red;

            _chart.Series.Add(TotalBarSeries);
            _chart.Series.Add(TotalLineSeries);
            _chart.Series.Add(TotalDelaySeries);

            _chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            _chart.ChartAreas[0].AxisX.LabelStyle.Format = "yy-MM-dd";

            _chart.Series[0].Points.Clear();
            _chart.Series[1].Points.Clear();
            _chart.Series[2].Points.Clear();
        }
        
        /// <summary>
        /// 블록 배치 결과 입력 받아 차트에 출력하는 함수
        /// </summary>
        /// <param name="_resultInfo">블록 배치 결과</param>
        /// <param name="_chartList">블록 정보가 출력 될 차트 리스트</param>
        /// <param name="_chart">전체 작업장 정보를 출력할 차트</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        void IResultsManagement.DrawChart(ArrangementResultWithDateDTO _resultInfo, List<Chart> _chartList, Chart _chart)
        {
            for (int i = 0; i < _resultInfo.TotalBlockImportLogList.Count; i++)
            {
                double SumofAreaUtilization = 0;
                double SumofLocatedBlock = 0;

                //작업장 별 결과
                for (int j = 0; j < _resultInfo.ResultWorkShopInfo[i].Count; j++)
                {
                    _chartList[j].Series[0].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), _resultInfo.ResultWorkShopInfo[i][j].AreaUtilization * 100);
                    _chartList[j].Series[1].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), _resultInfo.ResultWorkShopInfo[i][j].NumOfLocatedBlocks);

                    SumofAreaUtilization = SumofAreaUtilization + _resultInfo.ResultWorkShopInfo[i][j].AreaUtilization * 100;
                    SumofLocatedBlock = SumofLocatedBlock + _resultInfo.ResultWorkShopInfo[i][j].NumOfLocatedBlocks;
                }

                //종합 결과
                _chart.Series[0].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), SumofAreaUtilization);
                _chart.Series[1].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), SumofLocatedBlock / Convert.ToDouble(_resultInfo.ResultWorkShopInfo[i].Count));
                _chart.Series[2].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), _resultInfo.TotalDelayedBlockList[i].Count);
            }
        }



        /// <summary>
        /// 배치 결과를 그래프에 나타내는 함수, 주판 배치용 오버라이드를 위해 bool 변수 추가함
        /// </summary>
        /// <param name="_resultInfo">블록 배치 결과</param>
        /// <param name="_chartList">블록 정보가 출력 될 차트 리스트</param>
        /// <param name="_chart">전체 작업장 정보를 출력할 차트</param>
        /// <param name="IsPlateArrangement">주판배치를 나타내는 오버라이드용 변수, 주판일 시 true</param>
        void IResultsManagement.DrawChart(ArrangementResultWithDateDTO _resultInfo, List<Chart> _chartList, Chart _chart, bool IsPlateArrangement)
        {
            for (int i = 0; i < _resultInfo.TotalBlockImportLogList.Count; i++)
            {
                double SumofAreaUtilization = 0;
                double SumofLocatedBlock = 0;

                //작업장 별 결과
                for (int j = 0; j < _resultInfo.ResultWorkShopInfo[i].Count; j++)
                {
                    _chartList[j].Series[0].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), _resultInfo.ResultWorkShopInfo[i][j].AreaUtilization * 100);
                    _chartList[j].Series[1].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), _resultInfo.ResultWorkShopInfo[i][j].NumOfLocatedBlocks);

                    SumofAreaUtilization = SumofAreaUtilization + _resultInfo.ResultWorkShopInfo[i][j].AreaUtilization * 100;
                    SumofLocatedBlock = SumofLocatedBlock + _resultInfo.ResultWorkShopInfo[i][j].NumOfLocatedBlocks;
                }

                //종합 결과
                _chart.Series[0].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), SumofAreaUtilization / Convert.ToDouble(_resultInfo.ResultWorkShopInfo[i].Count));
                _chart.Series[1].Points.AddXY(_resultInfo.ArrangementStartDate.AddDays(i).ToShortDateString(), SumofLocatedBlock / Convert.ToDouble(_resultInfo.ResultWorkShopInfo[i].Count));
            }
        }



        
        /// <summary>
        /// 배치가 완료된 블록 정보를 바탕으로 블록 배치 정보가 입력되어 있는 UnitcellDTO 배열 목록을 생성하는 함수(배치 완료 후 결과 가시화에 사용됨)
        /// </summary>
        /// <param name="InpuBlockList">배치가 완료된 블록 리스트</param>
        /// <param name="WorkShopArrangementInfo">지번 정보가 입력되어 있는 UnitCellDTO 목록(블록 배치 정보는 입력되지 않음)</param>
        /// <returns>각 격자에 배치된 블록의 정보가 입력되어 있는 UnitCellDTO 목록</returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        List<UnitcellDTO[,]> IResultsManagement.GenterateArrangementMatrixFromBlockinfo(List<BlockDTO> InpuBlockList, List<UnitcellDTO[,]> WorkShopArrangementInfo)
        {
            for (int BlockIndex = 0; BlockIndex < InpuBlockList.Count; BlockIndex++)
            {
                for (int row = 0; row < Convert.ToInt16(Math.Ceiling(InpuBlockList[BlockIndex].RowCount)); row++)
                {
                    for (int column = 0; column < Convert.ToInt16(Math.Ceiling(InpuBlockList[BlockIndex].ColumnCount)); column++)
                    {
                        WorkShopArrangementInfo[InpuBlockList[BlockIndex].CurrentLocatedWorkshopIndex][Convert.ToInt16(InpuBlockList[BlockIndex].LocatedRow) + row, Convert.ToInt16(InpuBlockList[BlockIndex].LocatedColumn) + column].BlockIndex = InpuBlockList[BlockIndex].Index;
                        WorkShopArrangementInfo[InpuBlockList[BlockIndex].CurrentLocatedWorkshopIndex][Convert.ToInt16(InpuBlockList[BlockIndex].LocatedRow) + row, Convert.ToInt16(InpuBlockList[BlockIndex].LocatedColumn) + column].IsOccupied = true;
                    }
                }
            }
            return WorkShopArrangementInfo;
        }

        /// <summary>
        /// 배치가 완료된 주판 정보를 바탕으로 주판 배치 정보가 입력되어 있는 UnitcellDTO 배열 목록을 생성하는 함수(배치 완료 후 결과 가시화에 사용됨)
        /// </summary>
        /// <param name="InpuBlockList">배치가 완료된 주판 리스트</param>
        /// <param name="WorkShopArrangementInfo">지번 정보가 입력되어 있는 UnitCellDTO 목록(블록 배치 정보는 입력되지 않음)</param>
        /// <returns>각 격자에 배치된 블록의 정보가 입력되어 있는 UnitCellDTO 목록</returns>
        List<UnitcellDTO[,]> IResultsManagement.GenterateArrangementMatrixFromPlateinfo(List<PlateConfigDTO> InpuPlateList, List<UnitcellDTO[,]> WorkShopArrangementInfo)
        {

            for (int PlateIndex = 0; PlateIndex < InpuPlateList.Count; PlateIndex++)
            {
                for (int row = 0; row < InpuPlateList[PlateIndex].RowCount; row++)
                {
                    for (int column = 0; column < InpuPlateList[PlateIndex].ColCount; column++)
                    {
                        WorkShopArrangementInfo[InpuPlateList[PlateIndex].CurrentLocatedWorkshopIndex][InpuPlateList[PlateIndex].LocatedRow + row, InpuPlateList[PlateIndex].LocateCol + column].BlockIndex = PlateIndex;

                        //실제 주판의 형상에 따라 실제 점유표기를 다르게 함
                        if (InpuPlateList[PlateIndex].PlateConfig[row, column] == 0)
                        {
                            WorkShopArrangementInfo[InpuPlateList[PlateIndex].CurrentLocatedWorkshopIndex][InpuPlateList[PlateIndex].LocatedRow + row, InpuPlateList[PlateIndex].LocateCol + column].IsOccupied = false;
                        }
                        else
                        {
                            WorkShopArrangementInfo[InpuPlateList[PlateIndex].CurrentLocatedWorkshopIndex][InpuPlateList[PlateIndex].LocatedRow + row, InpuPlateList[PlateIndex].LocateCol + column].IsOccupied = true;
                        }
                    }
                }
            }
            return WorkShopArrangementInfo;
        }

        /// <summary>
        /// 각 작업장의 날짜별 배치 블록 현황을 출력하는 함수(csv 파일 생성)
        /// </summary>
        /// <param name="_resultInfo">블록 배치 결과</param>
        /// <param name="_fileName">생성할 파일 경로 및 이름</param>
        /// <remarks>
        /// 최초 구현 : 주수헌, 2015년 9월 16일
        /// 최종 수정 : 정용국, 2016년 01월 20일
        /// </remarks>
        void IResultsManagement.PrintArrangementResult(ArrangementResultWithDateDTO _resultInfo, string _fileName)
        {
            //작업장 갯수 만큼 반복하여 파일 생성
            for (int workshopindex = 0; workshopindex < _resultInfo.ResultWorkShopInfo[0].Count; workshopindex++)
            {
                FileStream fs = new FileStream(_fileName + "_" + _resultInfo.ResultWorkShopInfo[0][workshopindex].Name + ".csv", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                //열 이름 설정
                string FirstRow = "Date,Area Utilization,# of Blocks,Block name";
                sw.WriteLine(FirstRow);

                //날짜별 배치 정보에서 해당 작업장에 배치된 블록 검색
                List<List<BlockDTO>> tempTotalDailyBlockList = new List<List<BlockDTO>>();

                for (int i = 0; i < _resultInfo.TotalDailyArragnementedBlockList.Count; i++)
                {
                    List<BlockDTO> tempTodayBlockList = new List<BlockDTO>();

                    for (int j = 0; j < _resultInfo.TotalDailyArragnementedBlockList[i].Count; j++)
                    {
                        if (_resultInfo.ResultWorkShopInfo[0][workshopindex].Index == _resultInfo.TotalDailyArragnementedBlockList[i][j].CurrentLocatedWorkshopIndex)
                        {
                            tempTodayBlockList.Add(_resultInfo.TotalDailyArragnementedBlockList[i][j]);
                        }
                    }
                    tempTotalDailyBlockList.Add(tempTodayBlockList);
                }

                //날짜별 점유 블록 정보 입력
                DateTime CurrentDate = _resultInfo.ArrangementStartDate;

                for (int i = 0; i < _resultInfo.TotalDailyArragnementedBlockList.Count; i++)
                {
                    string tempRowstring = "";
                    tempRowstring = CurrentDate.ToShortDateString();
                    tempRowstring = tempRowstring + "," + _resultInfo.ResultWorkShopInfo[i][workshopindex].AreaUtilization;
                    tempRowstring = tempRowstring + "," + _resultInfo.ResultWorkShopInfo[i][workshopindex].NumOfLocatedBlocks;
                    string LocatedBlockName = "";

                    for (int j = 0; j < tempTotalDailyBlockList[i].Count; j++)
                    {
                        if (j == 0)
                        {
                            LocatedBlockName = "(" + tempTotalDailyBlockList[i][j].Project + ")" + tempTotalDailyBlockList[i][j].Name;
                        }
                        else
                        {
                            LocatedBlockName = LocatedBlockName + "," + "(" + tempTotalDailyBlockList[i][j].Project + ")" + tempTotalDailyBlockList[i][j].Name;
                        }
                    }

                    tempRowstring = tempRowstring + "," + LocatedBlockName;
                    sw.WriteLine(tempRowstring);
                    CurrentDate = CurrentDate.AddDays(1);
                }
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 특정 날짜의 작업장 별 배치 형상을 텍스트로 출력하는 함수(csv 파일 생성)
        /// </summary>
        /// <param name="ArrangementMatrix">작업장 별 블록 배치 현황</param>
        /// <param name="_workshopFileName">현재 선택된 작업장 정보 파일 이름</param>
        /// <param name="_blockFileName">현재 선택된 블록 정보 파일 이름</param>
        /// <param name="_resultInfo">블록 배치 결과</param>
        /// <param name="_selectedDate">확인하고자 하는 날짜의 일련번호</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        void IResultsManagement.PrintArrangementMatrixListResultWithDate(List<UnitcellDTO[,]> ArrangementMatrix, string _workshopFileName, string _blockFileName, ArrangementResultWithDateDTO _resultInfo, int _selectedDate)
        {
            if (ArrangementMatrix != null)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string selected = dialog.SelectedPath;
                    string FileName = selected + "\\" + _workshopFileName + "_" + _blockFileName + "_Matrix";

                    for (int workshopindex = 0; workshopindex < ArrangementMatrix.Count; workshopindex++)
                    {
                        FileStream fs = new FileStream(FileName + "_" + _resultInfo.ResultWorkShopInfo[_selectedDate][workshopindex].Name + ".csv", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                        for (int i = 0; i < ArrangementMatrix[workshopindex].GetLength(0); i++)
                        {
                            string row = "";

                            for (int j = 0; j < ArrangementMatrix[workshopindex].GetLength(1); j++)
                            {
                                row = row + ArrangementMatrix[workshopindex][i, j].BlockIndex + ",";
                            }
                            sw.WriteLine(row);
                        }
                        sw.Close();
                        fs.Close();
                    }
                    MessageBox.Show(selected + "에 " + _resultInfo.ResultWorkShopInfo[_selectedDate].Count.ToString() + "개의 파일이 생성되었습니다.");
                }
            }
            else MessageBox.Show("결과 정보가 없습니다.");
        }

        /// <summary>
        /// Greedy 알고리즘으로 생성된 특정 작업장의 배치 결과를 출력하는 함수(csv 파일 생성)
        /// </summary>
        /// <param name="ArrangementMatrix">특정 작업장 배치 현황</param>
        /// <param name="_workshopFileName">작업장 파일 이름</param>
        /// <param name="_blockFileName">블록 정보 파일 이름</param>
        void IResultsManagement.PrintArrangementMatrixResult(UnitcellDTO[,] ArrangementMatrix, string _workshopFileName, string _blockFileName)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string FilePath = dialog.SelectedPath;
                string FileName = FilePath + "\\" + _workshopFileName + "_" + _blockFileName;

                FileStream fs = new FileStream(FileName + Environment.TickCount.ToString() + ".csv", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                //각 격자에 배치된 블록의 Index 출력
                int row = ArrangementMatrix.GetLength(0);
                int col = ArrangementMatrix.GetLength(1);

                for (int i = 0; i < row; i++)
                {
                    string temp = ArrangementMatrix[i, 0].BlockIndex.ToString();
                    for (int j = 1; j < col; j++)
                    {
                        temp = temp + "," + ArrangementMatrix[i, j].BlockIndex.ToString();
                    }
                    sw.WriteLine(temp);
                }

                sw.Close();
                fs.Close();

            }
            MessageBox.Show("결과 출력 파일이 생성되었습니다.");
        }

        /// <summary>
        /// Greedy 알고리즘으로 생성된 특정 작업장의 누적 점유 정보 결과를 출력하는 함수(csv 파일 생성)
        /// </summary>
        /// <param name="ArrangementMatrix">특정 작업장 배치 현황</param>
        /// <param name="_workshopFileName">작업장 파일 이름</param>
        /// <param name="_blockFileName">블록 정보 파일 이름</param>
        void IResultsManagement.PrintArrangementMatrixResult(double[,] ArrangementMatrix, string _workshopFileName, string _blockFileName, int workshopNum)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string FilePath = dialog.SelectedPath;
                string FileName = FilePath + "\\" + _workshopFileName + "_" + _blockFileName;

                FileStream fs = new FileStream(FileName + "_" +workshopNum.ToString() + Environment.TickCount.ToString() + ".csv", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                //각 격자에 배치된 블록의 Index 출력
                int row = ArrangementMatrix.GetLength(0);
                int col = ArrangementMatrix.GetLength(1);

                for (int i = 0; i < row; i++)
                {
                    string temp = ArrangementMatrix[i, 0].ToString();
                    for (int j = 1; j < col; j++)
                    {
                        temp = temp + "," + ArrangementMatrix[i, j].ToString();
                    }
                    sw.WriteLine(temp);
                }

                sw.Close();
                fs.Close();

            }
            MessageBox.Show("결과 출력 파일이 생성되었습니다.");
        }

        /// <summary>
        /// BLF 알고리즘 결과 요약 출력
        /// </summary>
        /// <param name="_resultInfo">배치결과</param>
        /// <param name="_sw">초시계</param>
        void IResultsManagement.PrintBLFAlgorithmResultsSummary(ArrangementResultWithDateDTO _resultInfo, Stopwatch _sw)
        {
            int LocatedBlockNo = 0;
            int FinishedBlockNo = 0;
            int DelayedBlockNo = 0;
            int UnarrangementedBlockNo = 0;
            int TotalDelayedTime = 0;
            for (int i = 0; i < _resultInfo.BlockResultList.Count; i++)
            {

                if (_resultInfo.BlockResultList[i].IsLocated == true)
                    LocatedBlockNo++;

                if (_resultInfo.BlockResultList[i].IsFinished == true)
                    FinishedBlockNo++;

                if (_resultInfo.BlockResultList[i].IsDelayed == true && _resultInfo.BlockResultList[i].IsFinished == true)
                {
                    DelayedBlockNo++;
                    TotalDelayedTime += _resultInfo.BlockResultList[i].DelayedTime;
                }
                if (_resultInfo.BlockResultList[i].IsFinished == false && _resultInfo.BlockResultList[i].IsLocated == false)
                {
                    UnarrangementedBlockNo++;
                }
            }

            double AverageDelayedTime = Math.Round((double)TotalDelayedTime / (double)DelayedBlockNo, 2);


            //배치결과 요약 출력
            if (_sw.ElapsedMilliseconds < 5000)
            {
                string strTS = _sw.ElapsedMilliseconds.ToString();
                MessageBox.Show("블록 배치가 완료되었습니다.\r처리시간 : " + strTS + "ms \r전체 대상블록 개수 :" + _resultInfo.BlockResultList.Count.ToString() + "개\r완료된 블록 개수(배치중 블록 개수) : " + FinishedBlockNo.ToString() + "(" + LocatedBlockNo.ToString() + ")" + "개\r미배치 블록 개수 : " + UnarrangementedBlockNo.ToString() + "개 \r지연된 블록 개수 : " + DelayedBlockNo.ToString() + "개\n평균 지연 일수 : " + AverageDelayedTime.ToString() + "일");
                //MessageBox.Show("블록 배치가 완료되었습니다.\r처리시간 : " + strTS + "ms \r전체 대상블록 개수 :" + mResultInfo.BlockResultList.Count.ToString() + "개\r완료된 블록 개수(배치중 블록 개수) : " + FinishedBlockNo.ToString() + "(" + LocatedBlockNo.ToString() + ")" + "개\r미배치 블록 개수 : " + UnarrangementedBlockNo.ToString() + "개 \r지연된 블록 개수 : " + DelayedBlockNo.ToString() + "개 \r총 지연 일수 : " + DelayedDays.ToString() + "일");
            }
            if (_sw.ElapsedMilliseconds > 5000)
            {
                string strTS = (_sw.ElapsedMilliseconds / 1000).ToString();
                MessageBox.Show("블록 배치가 완료되었습니다.\r처리시간 : " + strTS + "s \r전체 대상블록 개수 :" + _resultInfo.BlockResultList.Count.ToString() + "개\r완료된 블록 개수(배치중 블록 개수) : " + FinishedBlockNo.ToString() + "(" + LocatedBlockNo.ToString() + ")" + "개\r미배치 블록 개수 : " + UnarrangementedBlockNo.ToString() + "개 \r지연된 블록 개수 : " + DelayedBlockNo.ToString() +"개\n평균 일수 : " + AverageDelayedTime.ToString() + "일");
                //MessageBox.Show("블록 배치가 완료되었습니다.\r처리시간 : " + strTS + "s \r전체 대상블록 개수 :" + mResultInfo.BlockResultList.Count.ToString() + "개\r완료된 블록 개수(배치중 블록 개수) : " + FinishedBlockNo.ToString() + "(" + LocatedBlockNo.ToString() + ")" + "개\r미배치 블록 개수 : " + UnarrangementedBlockNo.ToString() + "개 \r지연된 블록 개수 : " + DelayedBlockNo.ToString() + "개 \r총 지연 일수 : " + DelayedDays.ToString() + "일");
            }
        }

        

    }
}
