using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Implementation;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent;
using Eoba.Shipyard.ArrangementSimulator.ResultsViewer;
using System.Diagnostics;

//using Excel = Microsoft.Office.Interop.Excel;

namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    public partial class frmMain : Form
    {
        List<BlockDTO> mBlockInfoList;
        List<WorkshopDTO> mWorkshopInfoList;
        ArrangementResultWithDateDTO mResultInfo;
        List<Int32[,]> mArrangementMatrixList;
        List<ArrangementMatrixInfoDTO> mArrangementMatrixInfoList;
        List<PlateConfigDTO> mPlateConfigList = new List<PlateConfigDTO>();

        List<ArrangementResultDTO> mArrangementResultList = new List<ArrangementResultDTO>();
        List<Chart> mChartList = new List<Chart>();

        string OpenedWorkshopFileName;
        string OpenedArrangementMatrixFileName;
        string OpenedBlockFileName;

        IBlockArrangement mBlockArrangement;
        IDataManagement mDataManagement;
        IResultsManagement mResultsManagement;

        bool IsWorkshopInfoReady;
        bool IsBlockInfoReady;
        bool IsPlateInfoReady = false;
        int ArrangementAlgorithmMode = 0;//0:BLF, 1:Greedy
        double UnitGridLength = 1;
        double InputSlack = 2;
        int ExportWorkshopIndex = -1;

        OpenFileDialog myOpenFileDialog = new OpenFileDialog();
        SaveFileDialog mySaveFileDialog = new SaveFileDialog();

        public frmMain()
        {
            InitializeComponent();
            IsWorkshopInfoReady = false;
            IsBlockInfoReady = false;
            mBlockArrangement = new BlockArrangementMgr();
            mDataManagement = new DataManagement();
            mResultsManagement = new ResultsManagement();
            
            grdWorkshopInfo.ColumnCount = 5;
            grdWorkshopInfo.Columns[0].Name = "Index";
            grdWorkshopInfo.Columns[1].Name = "작업장이름";
            grdWorkshopInfo.Columns[2].Name = "세로";
            grdWorkshopInfo.Columns[3].Name = "가로";
            grdWorkshopInfo.Columns[4].Name = "지번 갯수";

            grdBlockInfo.ColumnCount = 14;
            grdBlockInfo.Columns[0].Name = "Index";
            grdBlockInfo.Columns[1].Name = "프로젝트번호";
            grdBlockInfo.Columns[2].Name = "블록번호";
            grdBlockInfo.Columns[3].Name = "세로";
            grdBlockInfo.Columns[4].Name = "가로";
            grdBlockInfo.Columns[5].Name = "상향작업공간";
            grdBlockInfo.Columns[6].Name = "하향작업공간";
            grdBlockInfo.Columns[7].Name = "좌측작업공간";
            grdBlockInfo.Columns[8].Name = "우측작업공간";
            grdBlockInfo.Columns[9].Name = "리드타임";
            grdBlockInfo.Columns[10].Name = "투입날짜";
            grdBlockInfo.Columns[11].Name = "반출날짜";
            grdBlockInfo.Columns[12].Name = "선호 작업장";
            grdBlockInfo.Columns[13].Name = "선호 지번";
        }

        /// <summary>
        /// Workshop 정보 읽기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 최초 구현 : 주수헌, 2015년 9월 16일
        /// 최종 수정 : 정용국, 2016년 02월 25일
        /// </remarks>
        private void openWorkshopInfomationWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAllData();

            try
            {
                string path = Environment.CurrentDirectory.ToString();

                myOpenFileDialog.Filter = "CSV Files|*.csv";
                myOpenFileDialog.InitialDirectory = path;

                List<string[]> input = new List<string[]>();

                if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFileName = myOpenFileDialog.FileName;
                    string tempFileName = selectedFileName.Substring(selectedFileName.LastIndexOf("\\") + 1);
                    tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                    OpenedWorkshopFileName = tempFileName;

                    StreamReader sr = new StreamReader(selectedFileName, Encoding.GetEncoding("euc-kr"));
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        string[] temp = s.Split(',');
                        input.Add(temp);
                    }
                    IsWorkshopInfoReady = true;
                }

                //data 읽기 완료
                mWorkshopInfoList = new List<WorkshopDTO>();
                for (int i = 1; i < input.Count; i++)
                {
                    mWorkshopInfoList.Add(new WorkshopDTO(Convert.ToInt16(input[i][0]), input[i][1], Convert.ToDouble(input[i][2]) / UnitGridLength, Convert.ToDouble(input[i][3]) / UnitGridLength, Convert.ToInt32(input[i][4])));
                    mWorkshopInfoList[mWorkshopInfoList.Count - 1].RowLocation = Convert.ToDouble(input[i][5]) / UnitGridLength;
                    mWorkshopInfoList[mWorkshopInfoList.Count - 1].ColumnLocation = Convert.ToDouble(input[i][6]) / UnitGridLength;

                    if (input[i][7] != "")
                    {
                        string[] temp = input[i][7].Split('-');
                        double[] tempDouble = new double[2];
                        for (int j = 0; j < temp.Length; j++) tempDouble[j] = Convert.ToDouble(temp[j]) / UnitGridLength;
                        mWorkshopInfoList[mWorkshopInfoList.Count - 1].UpperRoadside = tempDouble;
                    }
                    if (input[i][8] != "")
                    {
                        string[] temp = input[i][8].Split('-');
                        double[] tempDouble = new double[2];
                        for (int j = 0; j < temp.Length; j++) tempDouble[j] = Convert.ToDouble(temp[j]) / UnitGridLength;
                        mWorkshopInfoList[mWorkshopInfoList.Count - 1].BottomRoadside = tempDouble;
                    }
                    if (input[i][9] != "")
                    {
                        string[] temp = input[i][9].Split('-');
                        double[] tempDouble = new double[2];
                        for (int j = 0; j < temp.Length; j++) tempDouble[j] = Convert.ToDouble(temp[j]) / UnitGridLength;
                        mWorkshopInfoList[mWorkshopInfoList.Count - 1].LeftRoadside = tempDouble;
                    }
                    if (input[i][10] != "")
                    {
                        string[] temp = input[i][10].Split('-');
                        double[] tempDouble = new double[2];
                        for (int j = 0; j < temp.Length; j++) tempDouble[j] = Convert.ToDouble(temp[j]) / UnitGridLength;
                        mWorkshopInfoList[mWorkshopInfoList.Count - 1].RightRoadside = tempDouble;
                    }
                    if (input[i][11] == "") mWorkshopInfoList[mWorkshopInfoList.Count - 1].Type = 0;
                    else mWorkshopInfoList[mWorkshopInfoList.Count - 1].Type = Convert.ToInt32(input[i][11]);

                }

                foreach (WorkshopDTO workshop in mWorkshopInfoList)
                {
                    if (workshop.Type == 1) ExportWorkshopIndex = workshop.Index;
                }

                //그리드에 입력 결과 출력
                mDataManagement.PrintWorkshopDataOnGrid(grdWorkshopInfo, mWorkshopInfoList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Workshop Matrix 정보 읽기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 최초 구현 : 유상현, 2020년 5월 12일
        /// 최종 수정 : 유상현, 2020년 5월 12일
        /// </remarks>
        private void openWorkshopMatrixInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsWorkshopInfoReady) MessageBox.Show("작업장 정보를 먼저 불러와야 합니다.");
            else
            {
                try
                {
                    string path = Environment.CurrentDirectory.ToString();

                    myOpenFileDialog.Filter = "CSV Files|*.csv";
                    myOpenFileDialog.InitialDirectory = path;

                    List<string[]> input = new List<string[]>();

                    if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFileName = myOpenFileDialog.FileName;
                        string tempFileName = selectedFileName.Substring(selectedFileName.LastIndexOf("\\") + 1);
                        tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                        OpenedArrangementMatrixFileName = tempFileName;

                        StreamReader sr = new StreamReader(selectedFileName, Encoding.GetEncoding("euc-kr"));
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            string[] temp = s.Split(',');
                            input.Add(temp);
                        }
                    }

                    mArrangementMatrixInfoList = new List<ArrangementMatrixInfoDTO>();
                    for (int i = 1; i < input.Count; i++)
                    {
                        mArrangementMatrixInfoList.Add(new ArrangementMatrixInfoDTO(Convert.ToInt32(input[i][0]), input[i][1], Convert.ToInt32(input[i][2]), input[i][3], Convert.ToDouble(input[i][4]) / UnitGridLength, Convert.ToDouble(input[i][5]) / UnitGridLength, Convert.ToDouble(input[i][6]) / UnitGridLength, Convert.ToDouble(input[i][7]) / UnitGridLength, Convert.ToInt32(input[i][8])));
                    }
                    foreach (ArrangementMatrixInfoDTO matrixInfoDTO in mArrangementMatrixInfoList) mWorkshopInfoList[matrixInfoDTO.WorkshopIndex].ArrangementMatrixInfoList.Add(matrixInfoDTO);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }


        /// <summary>
        /// Block 정보 읽기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 최초 구현 : 주수헌, 2015년 9월 16일
        /// 최종 수정 : 정용국, 2016년 02월 25일
        /// </remarks>
        private void openBlockInfomationBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsWorkshopInfoReady) MessageBox.Show("작업장 정보를 먼저 불러와야 합니다.");
            else
            {
                try
                {
                    string path = Environment.CurrentDirectory.ToString();

                    myOpenFileDialog.Filter = "CSV Files|*.csv";
                    myOpenFileDialog.InitialDirectory = path;

                    List<string[]> input = new List<string[]>();

                    if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFileName = myOpenFileDialog.FileName;
                        string tempFileName = selectedFileName.Substring(selectedFileName.LastIndexOf("\\") + 1);
                        tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                        OpenedBlockFileName = tempFileName;

                        StreamReader sr = new StreamReader(selectedFileName, Encoding.GetEncoding("euc-kr"));
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            string[] temp = s.Split(',');
                            input.Add(temp);
                        }
                        IsBlockInfoReady = true;
                    }

                    //블록 입력 정보 정제
                    mBlockInfoList = new List<BlockDTO>();

                    mBlockInfoList = mDataManagement.RefineInputBlockData(input, mWorkshopInfoList, UnitGridLength);
                    

                    //그리드에 입력 결과 출력  
                    mDataManagement.PrintBlockDataOnGrid(grdBlockInfo, mBlockInfoList);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 닫기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 블록 배치 정보를 3차원으로 가시화하는 창 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        private void dViewerVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmResultsViewer myfrmResultsViewer;

            myfrmResultsViewer = new frmResultsViewer(mResultInfo, mWorkshopInfoList);

            // 2016년 11월 28일 주수헌 수정, BLF, Greedy 모두 날짜 개념이 들어가므로 아래의 구분은 무효

            ////BLF 알고리즘
            //if (ArrangementAlgorithmMode == 0) myfrmResultsViewer = new frmResultsViewer(mResultInfo, mWorkshopInfoList);
            ////Greedy 알고리즘
            //else myfrmResultsViewer = new frmResultsViewer(mBlockInfoList, mWorkshopInfoList, mUnitcellInfoList);
            
            myfrmResultsViewer.Show();
        }

        /// <summary>
        /// 블록배치 정보 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 최초 작성 : 주수헌, 2015년 9월 19일
        /// </remarks>
        private void reportRToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (mResultInfo != null)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = dialog.SelectedPath;
                    string FileName = FilePath + "\\" + OpenedWorkshopFileName + "_" + OpenedBlockFileName;
                    mResultsManagement.PrintArrangementResult(mResultInfo, FileName);
                    
                }
                else MessageBox.Show("결과 정보가 없습니다.");
            }
        }

        /// <summary>
        /// BLF 알고리즘 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// 수정 작성 : 유상현, 2020년 05월 13일
        private void bLFAlgorithmBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mDataManagement.UpdateWorkshopDataOfGrid(grdWorkshopInfo, mWorkshopInfoList);
            //mDataManagement.UpdateBlockDataOfGrid(grdBlockInfo, mBlockInfoList);

            if (mBlockInfoList != null)
            {
                //모든 블록의 입고/출고 날짜 저장 + 가장 빠른 날짜, 가장 늦은 날짜 계산
                DateTime[] simDates = new DateTime[2];
                simDates = mBlockArrangement.CalcEarliestAndLatestDates(mBlockInfoList);

                DateTime startDate = new DateTime();
                DateTime finishDate = new DateTime();

                //추천된 시뮬레이션 시작일, 종료일을 기준으로 사용자가 UI에서 시작일, 종료일 선택
                using (var form = new frmArrangementRangeSetting(simDates[0], simDates[1]))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        startDate = form.StartDate;
                        finishDate = form.FinishDate;
                    }
                }

                //Chart 초기화(작업장 별 차트 + Tab page)
                mResultsManagement.InitializeIndividualWorkshopChart(mWorkshopInfoList, mChartList, chtWorkshop1, tabChart);


                //Chart 초기화(전체 요약 차트)
                mResultsManagement.InitializeTotalWorkshopChart(chtTotal);

                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    //입력한 날짜를 기준으로 BLF 알고리즘 시작
                    mArrangementResultList.Clear();
                    if (IsWorkshopInfoReady && IsBlockInfoReady)
                    {

                        // WorkshopMatrix 초기설정 세팅
                        // Input : 작업장 정보 리스트, 작업장 배치 불가 구역 리스트
                        // Output : 작업장 매트릭스 리스트  List<Int32[,]> 
                        List<Int32[,]> ArrangementMatrixList = mBlockArrangement.InitializeArrangementMatrix(mWorkshopInfoList);

                        // RunBLFAlgorithm
                        // Input : 작업장 정보리스트, 블록정보리스트, 작업장 매트릭스리스트, 시작날짜, 끝 날짜
                        // Output : Result
                        mResultInfo = mBlockArrangement.RunBLFAlgorithmWithAddress(mBlockInfoList, ArrangementMatrixList, mWorkshopInfoList, startDate, finishDate, toolStripProgressBar1, toolStripStatusLabel1);


                        reportRToolStripMenuItem1.Enabled = true;
                        dViewerVToolStripMenuItem.Enabled = true;
                        dCumlatedOccupyingVToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("입력 정보가 부족합니다.");
                    }
                    sw.Stop();

                    //배치 결과 Chart에 출력
                    mResultsManagement.DrawChart(mResultInfo, mChartList, chtTotal);

                    //배치 결과 요약 메시지 박스 출력
                    mResultsManagement.PrintBLFAlgorithmResultsSummary(mResultInfo, sw);

                    //알고리즘 모드 = BLF 알고리즘
                    ArrangementAlgorithmMode = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 여유 작업 공간을 고려한 BLF 알고리즘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 최초 작성 : 유상현, 2020년 05월 17일
        /// 수정 작성 : 
        private void bLFWithSlackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mDataManagement.UpdateWorkshopDataOfGrid(grdWorkshopInfo, mWorkshopInfoList);
            //mDataManagement.UpdateBlockDataOfGrid(grdBlockInfo, mBlockInfoList);

            int Slack = Convert.ToInt32(Math.Ceiling(InputSlack / UnitGridLength));

            if (mBlockInfoList != null)
            {
                //모든 블록의 입고/출고 날짜 저장 + 가장 빠른 날짜, 가장 늦은 날짜 계산
                DateTime[] simDates = new DateTime[2];
                simDates = mBlockArrangement.CalcEarliestAndLatestDates(mBlockInfoList);

                DateTime startDate = new DateTime();
                DateTime finishDate = new DateTime();

                //추천된 시뮬레이션 시작일, 종료일을 기준으로 사용자가 UI에서 시작일, 종료일 선택
                using (var form = new frmArrangementRangeSetting(simDates[0], simDates[1]))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        startDate = form.StartDate;
                        finishDate = form.FinishDate;
                    }
                }

                //Chart 초기화(작업장 별 차트 + Tab page)
                mResultsManagement.InitializeIndividualWorkshopChart(mWorkshopInfoList, mChartList, chtWorkshop1, tabChart);


                //Chart 초기화(전체 요약 차트)
                mResultsManagement.InitializeTotalWorkshopChart(chtTotal);

                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    //입력한 날짜를 기준으로 BLF 알고리즘 시작
                    mArrangementResultList.Clear();
                    if (IsWorkshopInfoReady && IsBlockInfoReady)
                    {

                        // WorkshopMatrix 초기설정 세팅
                        // Input : 작업장 정보 리스트, 작업장 배치 불가 구역 리스트
                        // Output : 작업장 매트릭스 리스트  List<Int32[,]> 
                        List<Int32[,]> ArrangementMatrixList = mBlockArrangement.InitializeArrangementMatrixWithSlack(mWorkshopInfoList, Slack);

                        // RunBLFAlgorithm
                        // Input : 작업장 정보리스트, 블록정보리스트, 작업장 매트릭스리스트, 시작날짜, 끝 날짜
                        // Output : Result
                        mResultInfo = mBlockArrangement.RunBLFAlgorithmWithSlack(mBlockInfoList, ArrangementMatrixList, mWorkshopInfoList, startDate, finishDate, toolStripProgressBar1, toolStripStatusLabel1, Slack);


                        reportRToolStripMenuItem1.Enabled = true;
                        dViewerVToolStripMenuItem.Enabled = true;
                        dCumlatedOccupyingVToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("입력 정보가 부족합니다.");
                    }
                    sw.Stop();

                    //배치 결과 Chart에 출력
                    mResultsManagement.DrawChart(mResultInfo, mChartList, chtTotal);

                    //배치 결과 요약 메시지 박스 출력
                    mResultsManagement.PrintBLFAlgorithmResultsSummary(mResultInfo, sw);

                    //알고리즘 모드 = BLF 알고리즘
                    ArrangementAlgorithmMode = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 여유 작업 공간을 고려한 BLF 알고리즘 + 우선 배치기능 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 최초 작성 : 유상현, 2020년 05월 17일
        /// 수정 작성 : 
        private void bLFWithPriorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mDataManagement.UpdateWorkshopDataOfGrid(grdWorkshopInfo, mWorkshopInfoList);
            //mDataManagement.UpdateBlockDataOfGrid(grdBlockInfo, mBlockInfoList);

            int Slack = Convert.ToInt32(Math.Ceiling(InputSlack / UnitGridLength));

            if (mBlockInfoList != null)
            {
                //모든 블록의 입고/출고 날짜 저장 + 가장 빠른 날짜, 가장 늦은 날짜 계산
                DateTime[] simDates = new DateTime[2];
                simDates = mBlockArrangement.CalcEarliestAndLatestDates(mBlockInfoList);

                DateTime startDate = new DateTime();
                DateTime finishDate = new DateTime();

                //추천된 시뮬레이션 시작일, 종료일을 기준으로 사용자가 UI에서 시작일, 종료일 선택
                using (var form = new frmArrangementRangeSetting(simDates[0], simDates[1]))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        startDate = form.StartDate;
                        finishDate = form.FinishDate;
                    }
                }

                //Chart 초기화(작업장 별 차트 + Tab page)
                mResultsManagement.InitializeIndividualWorkshopChart(mWorkshopInfoList, mChartList, chtWorkshop1, tabChart);


                //Chart 초기화(전체 요약 차트)
                mResultsManagement.InitializeTotalWorkshopChart(chtTotal);

                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    //입력한 날짜를 기준으로 BLF 알고리즘 시작
                    mArrangementResultList.Clear();
                    if (IsWorkshopInfoReady && IsBlockInfoReady)
                    {

                        // WorkshopMatrix 초기설정 세팅
                        // Input : 작업장 정보 리스트, 작업장 배치 불가 구역 리스트
                        // Output : 작업장 매트릭스 리스트  List<Int32[,]> 
                        List<Int32[,]> ArrangementMatrixList = mBlockArrangement.InitializeArrangementMatrixWithSlack(mWorkshopInfoList, Slack);

                        // RunBLFAlgorithm
                        // Input : 작업장 정보리스트, 블록정보리스트, 작업장 매트릭스리스트, 시작날짜, 끝 날짜
                        // Output : Result
                        mResultInfo = mBlockArrangement.RunBLFAlgorithmWithSlackWithPriority(mBlockInfoList, ArrangementMatrixList, mWorkshopInfoList, startDate, finishDate, toolStripProgressBar1, toolStripStatusLabel1, Slack);


                        reportRToolStripMenuItem1.Enabled = true;
                        dViewerVToolStripMenuItem.Enabled = true;
                        dCumlatedOccupyingVToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("입력 정보가 부족합니다.");
                    }
                    sw.Stop();

                    //배치 결과 Chart에 출력
                    mResultsManagement.DrawChart(mResultInfo, mChartList, chtTotal);

                    //배치 결과 요약 메시지 박스 출력
                    mResultsManagement.PrintBLFAlgorithmResultsSummary(mResultInfo, sw);

                    //알고리즘 모드 = BLF 알고리즘
                    ArrangementAlgorithmMode = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 여유 작업 공간을 고려한 BLF 알고리즘 + 우선 배치기능 추가 + 해상크레인 출고장 기능 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 최초 작성 : 유상현, 2020년 06월 29일
        /// 수정 작성 : 
        private void bLFWithFlotidsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mDataManagement.UpdateWorkshopDataOfGrid(grdWorkshopInfo, mWorkshopInfoList);
            //mDataManagement.UpdateBlockDataOfGrid(grdBlockInfo, mBlockInfoList);

            int Slack = Convert.ToInt32(Math.Ceiling(InputSlack / UnitGridLength));

            if (mBlockInfoList != null)
            {
                //모든 블록의 입고/출고 날짜 저장 + 가장 빠른 날짜, 가장 늦은 날짜 계산
                DateTime[] simDates = new DateTime[2];
                simDates = mBlockArrangement.CalcEarliestAndLatestDates(mBlockInfoList);

                DateTime startDate = new DateTime();
                DateTime finishDate = new DateTime();

                //추천된 시뮬레이션 시작일, 종료일을 기준으로 사용자가 UI에서 시작일, 종료일 선택
                using (var form = new frmArrangementRangeSetting(simDates[0], simDates[1]))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        startDate = form.StartDate;
                        finishDate = form.FinishDate;
                    }
                }

                //Chart 초기화(작업장 별 차트 + Tab page)
                mResultsManagement.InitializeIndividualWorkshopChart(mWorkshopInfoList, mChartList, chtWorkshop1, tabChart);


                //Chart 초기화(전체 요약 차트)
                mResultsManagement.InitializeTotalWorkshopChart(chtTotal);

                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    //입력한 날짜를 기준으로 BLF 알고리즘 시작
                    mArrangementResultList.Clear();
                    if (IsWorkshopInfoReady && IsBlockInfoReady)
                    {

                        // WorkshopMatrix 초기설정 세팅
                        // Input : 작업장 정보 리스트, 작업장 배치 불가 구역 리스트
                        // Output : 작업장 매트릭스 리스트  List<Int32[,]> 
                        List<Int32[,]> ArrangementMatrixList = mBlockArrangement.InitializeArrangementMatrixWithSlack(mWorkshopInfoList, Slack);

                        // RunBLFAlgorithm
                        // Input : 작업장 정보리스트, 블록정보리스트, 작업장 매트릭스리스트, 시작날짜, 끝 날짜
                        // Output : Result
                        mResultInfo = mBlockArrangement.RunBLFAlgorithmWithFloatingCrane(mBlockInfoList, ArrangementMatrixList, mWorkshopInfoList, startDate, finishDate, toolStripProgressBar1, toolStripStatusLabel1, Slack, ExportWorkshopIndex);


                        reportRToolStripMenuItem1.Enabled = true;
                        dViewerVToolStripMenuItem.Enabled = true;
                        dCumlatedOccupyingVToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("입력 정보가 부족합니다.");
                    }
                    sw.Stop();

                    //배치 결과 Chart에 출력
                    mResultsManagement.DrawChart(mResultInfo, mChartList, chtTotal);

                    //배치 결과 요약 메시지 박스 출력
                    mResultsManagement.PrintBLFAlgorithmResultsSummary(mResultInfo, sw);

                    //알고리즘 모드 = BLF 알고리즘
                    ArrangementAlgorithmMode = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }



        //private void dCumlatedOccupyingVToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    List<double[,]> CumulatedGridOccupationMatrix = new List<double[,]>();

        //    CumulatedGridOccupationMatrix = mBlockArrangement.CalculateOccupiedDaysAteachGrid(mWorkshopInfoList, mResultInfo.BlockResultList, mResultInfo.ArrangementStartDate, mResultInfo.ArrangementFinishDate);

        //    if (CumulatedGridOccupationMatrix != null)
        //    {
        //        for (int i = 0; i < CumulatedGridOccupationMatrix.Count; i++)
        //        {
        //            mResultsManagement.PrintArrangementMatrixResult(CumulatedGridOccupationMatrix[i], OpenedWorkshopFileName, OpenedBlockFileName, i);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("파일을 출력할 수 없습니다.");
        //    }
        //}


        private void clearAllAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //모든 데이터 삭제
            ClearAllData();
        }


        //모든 데이터를 전부 초기화함 (입력 데이터도 포함)
        void ClearAllData()
        {
            IsWorkshopInfoReady = false;
            IsBlockInfoReady = false;
            mBlockArrangement = new BlockArrangementMgr();

            mBlockInfoList = new List<BlockDTO>();
            mWorkshopInfoList = new List<WorkshopDTO>();
            mResultInfo = new ArrangementResultWithDateDTO();

            mArrangementResultList = new List<ArrangementResultDTO>();
            mChartList = new List<Chart>();

            grdBlockInfo.Rows.Clear();
            grdWorkshopInfo.Rows.Clear();

            tabChart.TabPages.Clear();

            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.tabPage1);
            this.tabChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChart.Location = new System.Drawing.Point(3, 23);
            this.tabChart.Name = "tabChart";
            this.tabChart.SelectedIndex = 0;
            this.tabChart.Size = new System.Drawing.Size(571, 285);
            this.tabChart.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chtWorkshop1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(563, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Workshop 1";
            this.tabPage1.UseVisualStyleBackColor = true;

            chtTotal.Series.Clear();
            chtWorkshop1.Series.Clear();

        }

        //입력 데이터는 그대로 + 결과만 초기화함
        void ClearResultsData()
        {
            mBlockArrangement = new BlockArrangementMgr();

            mResultInfo = new ArrangementResultWithDateDTO();

            mArrangementResultList = new List<ArrangementResultDTO>();
            mChartList = new List<Chart>();

            tabChart.TabPages.Clear();

            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.tabPage1);
            this.tabChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChart.Location = new System.Drawing.Point(3, 23);
            this.tabChart.Name = "tabChart";
            this.tabChart.SelectedIndex = 0;
            this.tabChart.Size = new System.Drawing.Size(571, 285);
            this.tabChart.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chtWorkshop1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(563, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Workshop 1";
            this.tabPage1.UseVisualStyleBackColor = true;

            chtTotal.Series.Clear();
            chtWorkshop1.Series.Clear();
        }

        private void clearResultsRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearResultsData();
        }

        private void openPlateConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!IsWorkshopInfoReady) MessageBox.Show("작업장 정보를 먼저 불러와야 합니다.");
            else
            {
                try
                {
                    mPlateConfigList.Clear();

                    string path = Environment.CurrentDirectory.ToString();
                    myOpenFileDialog.InitialDirectory = path;

                    List<string> TargetCSVFiles = new List<string>();


                    //대상 폴더를 선택하고, 해당 폴더 하위의 모든 CSV 파일의 이름을 가져옴
                    if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //주수헌 임시로 박아둔 경로, 나중에 바꿔야 함
                        string Fullpath = myOpenFileDialog.FileName;
                        string FileName = myOpenFileDialog.SafeFileName;

                        string selectedDirectory = Fullpath.Replace(FileName, "");


                        if (Directory.Exists(selectedDirectory))
                        {
                            DirectoryInfo direc = new DirectoryInfo(selectedDirectory);

                            foreach (FileInfo f in direc.GetFiles())
                            {
                                if (f.Extension.ToLower().CompareTo(".csv") == 0)
                                {
                                    string strCsvFileName = direc.FullName + f.Name;
                                    TargetCSVFiles.Add(strCsvFileName);
                                }
                            }
                        }


                        //CSV의 주판 정보를 하나씩 읽어서 MainPlateDTO에 저장함    
                        for (int i = 0; i < TargetCSVFiles.Count; i++)
                        {
                            string strTargetCSVFile = TargetCSVFiles[i];
                            string PlateName = strTargetCSVFile.Replace(selectedDirectory, "");

                            PlateName = PlateName.Substring(0, PlateName.Length - 4);

                            List<string[]> Plateinput = new List<string[]>();

                            StreamReader sr = new StreamReader(strTargetCSVFile, Encoding.GetEncoding("euc-kr"));
                            while (!sr.EndOfStream)
                            {
                                string s = sr.ReadLine();
                                string[] temp = s.Split(',');
                                Plateinput.Add(temp);
                            }

                            int RowCount = Plateinput.Count;
                            int ColCount = Plateinput[0].GetLength(0);

                            double[,] tempPlateConfig = new double[RowCount, ColCount];
                            //주판 형상 채우기
                            for (int j = 0; j < RowCount; j++)
                            {
                                for (int k = 0; k < ColCount; k++)
                                {
                                    tempPlateConfig[j, k] = Convert.ToDouble(Plateinput[j][k]);
                                }
                            }

                            PlateConfigDTO tempConfig = new PlateConfigDTO(PlateName, RowCount, ColCount);
                            tempConfig.PlateConfig = tempPlateConfig;

                            mPlateConfigList.Add(tempConfig);
                        }

                        //팝업창으로 결과를 확인
                        MessageBox.Show(TargetCSVFiles.Count.ToString() + "개의 주판 형상 정보가 정상적으로 로드 되었습니다.");

                        mDataManagement.PrintPlateDataOnFrid(grdBlockInfo, mPlateConfigList);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void openPlateDateInfomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mPlateConfigList.Count==0) MessageBox.Show("주판 정보를 먼저 불러와야 합니다.");
            else
            {
                try
                {
                    string path = Environment.CurrentDirectory.ToString();

                    myOpenFileDialog.Filter = "CSV Files|*.csv";
                    myOpenFileDialog.InitialDirectory = path;

                    List<string[]> input = new List<string[]>();

                    if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFileName = myOpenFileDialog.FileName;
                        string tempFileName = selectedFileName.Substring(selectedFileName.LastIndexOf("\\") + 1);
                        tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                        OpenedBlockFileName = tempFileName;

                        StreamReader sr = new StreamReader(selectedFileName, Encoding.GetEncoding("euc-kr"));
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            string[] temp = s.Split(',');
                            input.Add(temp);
                        }
                    }

                    // 주판의 정보에 입출고 날짜 정보 입력하기
                    for (int i = 1; i < input.Count; i++)
                    {
                        if (mPlateConfigList[i-1].Name == input[i][1])
                        {
                            mPlateConfigList[i - 1].InitialImportDate = DateTime.Parse(input[i][2]);
                            mPlateConfigList[i - 1].PlanImportDate = DateTime.Parse(input[i][2]);
                            mPlateConfigList[i - 1].InitialExportDate = DateTime.Parse(input[i][3]);
                            mPlateConfigList[i - 1].PlanExportDate = DateTime.Parse(input[i][3]);

                            TimeSpan Leadtime = mPlateConfigList[i - 1].PlanExportDate - mPlateConfigList[i - 1].PlanImportDate;
                            mPlateConfigList[i - 1].LeadTime = Leadtime.Days;
                        }

                        //선호작업장, 선호 지번 입력
                        List<int> tempPreferWorkShop = new List<int>();

                        bool PreferWorkShop = true;

                        if (input[i][4] == "") PreferWorkShop = false;

                        if (PreferWorkShop == false) //선호지번 값이 없는 경우
                        {
                            //모든 지번을 선호지번으로 가짐
                            int totalWorkShop = mWorkshopInfoList.Count;
                            for (int j = 0; j < totalWorkShop; j++) tempPreferWorkShop.Add(j);

                            mPlateConfigList[i - 1].PreferWorkShopIndexList = tempPreferWorkShop;
                        }
                        else//선호지번 값이 있는 경우
                        {
                            string[] temp = input[i][4].Split('/');
                            for (int j = 0; j < temp.Length; j++) tempPreferWorkShop.Add(Convert.ToInt32(temp[j]));
                            mPlateConfigList[i - 1].PreferWorkShopIndexList = tempPreferWorkShop;
                        }
                    }

                    IsPlateInfoReady = true;

                    //팝업창으로 결과를 확인
                    MessageBox.Show((input.Count-1).ToString() + "개의 주판정보가 정상적으로 로드 되었습니다.");

                    //그리드에 결과 표시
                    mDataManagement.PrintPlateDataOnFrid(grdBlockInfo, mPlateConfigList);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }




        // 행렬 회전을 위한 데이터 입출력 및 회전 함수 짜기 - 1월 29일까지
        // BLF 알고리즘 내용 정리해서 PPT로 순서도로 정리하기
        // (추가) 시간을 고려한 배치 중, BLF외 다른 논문 찾고 PPT로 정리 
        private void rotateTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //행렬 A를 불러와서, 90 , 180, 270도 씩 회전하는 코드를 짜보자


            //csv 파일로 테스트용 행렬 파일 읽어오는 코드를 짜야함
            double[,] inputmatrix = null;
            double[,] outputmatrix = null;
            try
            {
                string path = Environment.CurrentDirectory.ToString();

                myOpenFileDialog.Filter = "CSV Files|*.csv";
                myOpenFileDialog.InitialDirectory = path;

                List<double[]> input = new List<double[]>();

                if (myOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFileName = myOpenFileDialog.FileName;
                    string tempFileName = selectedFileName.Substring(selectedFileName.LastIndexOf("\\") + 1);
                    tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                    OpenedWorkshopFileName = tempFileName;

                    StreamReader sr = new StreamReader(selectedFileName, Encoding.GetEncoding("euc-kr"));
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        string[] temp = s.Split(',');
                        double[] tempda = new double[temp.Length];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            double tempd = Convert.ToDouble(temp[i]);
                            tempda[i] = tempd;
                        }
                        input.Add(tempda);
                    }
                    double[,] input_2d = new double[input.Count, input[0].Length];
                    for (int i=0; i < input_2d.GetLength(0); i++)
                    {
                        for (int j = 0; j < input_2d.GetLength(1); j++)
                        {
                            input_2d[i,j] = input[i][j];
                        }
                    }
                    inputmatrix = input_2d;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            // 행렬 회전하는 함수(RotateMatrix)를 짜고, 불러와서 사용한다.


            outputmatrix = RotateMatrix(inputmatrix, 90);

            // 출력
            string outpath = Environment.CurrentDirectory.ToString();
            var filepath = outpath + "/RotatedMatrix.csv";
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
            {
                for (int i = 0; i < outputmatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < outputmatrix.GetLength(1); j++)
                    {
                        writer.Write(outputmatrix[i, j]);
                        writer.Write(',');
                    }
                    writer.WriteLine();
                }
            }


        }



            // 회전된 행렬을 csv 파일로 출력해주는코드

        

        //90, 180, 270도 회전만 하면됨
        public double[,] RotateMatrix(double[,] inputmatrix, int degree)
        {
            int new_row = inputmatrix.GetLength(1);
            int new_column = inputmatrix.GetLength(0);
            
            double[,] Resultmatrix = new double[new_row, new_column];

            //행렬을 회전하는 함수 짤 것

            if (degree == 90)
            {
                for (int i = 0; i < new_row; i++)
                {
                    for (int j = 0; j < new_column; j++)
                    {
                        Resultmatrix[i, j] = inputmatrix[ new_column - j -1 , i ];
                    }
                }
            }
            else if (degree == 180)
            { }
            else if (degree == 270)
            { }
            else
            {
                string error = "올바른 각도를 선택하십시오";
;               MessageBox.Show(error);
            }

            return Resultmatrix;
        }
        

        


        private void reportRToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chtTotal_Click(object sender, EventArgs e)
        {

        }

        private void grdWorkshopInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}
