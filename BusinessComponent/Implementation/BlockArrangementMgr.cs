using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;
using System.Drawing;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Implementation
{
    public class BlockArrangementMgr : IBlockArrangement
    {
        #region 지번 자동할당 

        /// <summary>
        /// 지번 자동 할당 (지번 갯수 만큼 작업장을 열방향으로 등분함)
        /// </summary>
        /// <param name="WorkShopList">작업장 정보 리스트</param>
        /// <returns>지번 정보가 입력되어 있는 UnitcellDTO 배열 목록</returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        List<UnitcellDTO[,]> IBlockArrangement.InitializeArrangementMatrixWithAddress(List<WorkshopDTO> WorkShopList)
        {
            List<UnitcellDTO[,]> ResultWorkShopList = new List<UnitcellDTO[,]>();

            for (int WorkShopCount = 0; WorkShopCount < WorkShopList.Count; WorkShopCount++)
            {
                UnitcellDTO[,] tempWorkShop = new UnitcellDTO[Convert.ToInt16(Math.Ceiling(WorkShopList[WorkShopCount].RowCount)), Convert.ToInt16(Math.Ceiling(WorkShopList[WorkShopCount].ColumnCount))];

                int NumOfAddress = WorkShopList[WorkShopCount].NumOfAddress;
                int addressStartColumn = 0;

                for (int CurrentAddress = 0; CurrentAddress < NumOfAddress; CurrentAddress++)
                {
                    int rowOfAddress = 0;
                    if (CurrentAddress != (NumOfAddress - 1)) rowOfAddress = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(WorkShopList[WorkShopCount].ColumnCount) / Convert.ToDouble(NumOfAddress)));
                    else rowOfAddress = Convert.ToInt16(Math.Ceiling(WorkShopList[WorkShopCount].ColumnCount)) - Convert.ToInt16(Math.Ceiling(WorkShopList[WorkShopCount].ColumnCount / NumOfAddress)) * (NumOfAddress - 1);

                    for (int row = 0; row < WorkShopList[WorkShopCount].RowCount; row++)
                    {
                        for (int column = addressStartColumn; column < addressStartColumn + rowOfAddress; column++)
                        {
                            tempWorkShop[row, column] = new UnitcellDTO(-1, false, CurrentAddress);
                        }
                    }
                    addressStartColumn = addressStartColumn + rowOfAddress;
                }
                ResultWorkShopList.Add(tempWorkShop);
            }

            return ResultWorkShopList;
        }


        /// <summary>
        /// 배치 매트릭스 생성, 배치 불가구역 설정 
        /// </summary>
        /// <param name="WorkShopList">작업장 정보 리스트</param>
        /// <returns>배치 불가구역이 설정되어 있는 배치 매트릭스 리스트</returns>
        /// <remarks>
        /// 최초 작성 : 유상현, 2020년 05월 12일
        /// </remarks>
        List<Int32[,]> IBlockArrangement.InitializeArrangementMatrix(List<WorkshopDTO> WorkShopList)
        {
            List<Int32[,]> ArrangementMatrixList = new List<int[,]>();

            for (int WorkShopCount = 0; WorkShopCount < WorkShopList.Count; WorkShopCount++)
            {
                // 모든 칸이 0으로 초기화된 작업장 크기만큼의 배치 매트릭스 생성 
                // 작업장 타입이 -1인 경우는 모두 1로 초기화
                int tempWorkshopRowCount = Convert.ToInt32(Math.Ceiling(WorkShopList[WorkShopCount].RowCount));
                int tempWorkshopColumnCount = Convert.ToInt32(Math.Ceiling(WorkShopList[WorkShopCount].ColumnCount));
                Int32[,] tempArrangementMatrix = new Int32[tempWorkshopRowCount, tempWorkshopColumnCount];

                if (WorkShopList[WorkShopCount].Type != -1)
                {
                    for (int i = 0; i < tempWorkshopRowCount; i++)
                    {
                        for (int j = 0; j < tempWorkshopColumnCount; j++)
                        {
                            tempArrangementMatrix[i, j] = 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tempWorkshopRowCount; i++)
                    {
                        for (int j = 0; j < tempWorkshopColumnCount; j++)
                        {
                            tempArrangementMatrix[i, j] = 1;
                        }
                    }
                }

                // 해당 배치 매트릭스에 배치 불가구역이 있는 경우 배치

                foreach (ArrangementMatrixInfoDTO arrangementMatrixInfo in WorkShopList[WorkShopCount].ArrangementMatrixInfoList)
                {
                    int tempRowLocation = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.RowLocation));
                    int tempColumnLocation = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.ColumnLocation));
                    int tempRowCount = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.RowCount));
                    int tempColumnCount = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.ColumnCount));

                    tempArrangementMatrix = PutBlockOnMatrix(tempArrangementMatrix, tempRowLocation, tempColumnLocation, tempRowCount, tempColumnCount, 0);
                }

                ArrangementMatrixList.Add(tempArrangementMatrix);
            }

            return ArrangementMatrixList;
        }

        /// <summary>
        /// 배치 매트릭스 생성, 배치 불가구역 설정 
        /// </summary>
        /// <param name="WorkShopList">작업장 정보 리스트</param>
        /// <returns>배치 불가구역이 설정되어 있는 배치 매트릭스 리스트</returns>
        /// <remarks>
        /// 최초 작성 : 유상현, 2020년 05월 12일
        /// </remarks>
        public List<Int32[,]> InitializeArrangementMatrixWithSlack(List<WorkshopDTO> WorkShopList, int Slack)
        {
            List<Int32[,]> ArrangementMatrixList = new List<int[,]>();

            for (int WorkShopCount = 0; WorkShopCount < WorkShopList.Count; WorkShopCount++)
            {
                // 모든 칸이 0으로 초기화된 작업장 크기만큼의 배치 매트릭스 생성 
                int tempWorkshopRowCount = Convert.ToInt32(Math.Ceiling(WorkShopList[WorkShopCount].RowCount));
                int tempWorkshopColumnCount = Convert.ToInt32(Math.Ceiling(WorkShopList[WorkShopCount].ColumnCount));

                // 양측 여유공간 더해줌
                tempWorkshopRowCount += Slack * 2;
                tempWorkshopColumnCount += Slack * 2;

                Int32[,] tempArrangementMatrix = new Int32[tempWorkshopRowCount, tempWorkshopColumnCount];

                if (WorkShopList[WorkShopCount].Type != -1)
                {
                    for (int i = 0; i < tempWorkshopRowCount; i++)
                    {
                        for (int j = 0; j < tempWorkshopColumnCount; j++)
                        {
                            tempArrangementMatrix[i, j] = 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tempWorkshopRowCount; i++)
                    {
                        for (int j = 0; j < tempWorkshopColumnCount; j++)
                        {
                            tempArrangementMatrix[i, j] = 1;
                        }
                    }
                }

                // 해당 배치 매트릭스에 배치 불가구역이 있는 경우 배치

                foreach (ArrangementMatrixInfoDTO arrangementMatrixInfo in WorkShopList[WorkShopCount].ArrangementMatrixInfoList)
                {
                    int tempRowLocation = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.RowLocation));
                    int tempColumnLocation = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.ColumnLocation));
                    int tempRowCount = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.RowCount));
                    int tempColumnCount = Convert.ToInt32(Math.Ceiling(arrangementMatrixInfo.ColumnCount));

                    // 여유공간만큼 이격해서 배치
                    tempRowLocation += Slack;
                    tempColumnLocation += Slack;

                    tempArrangementMatrix = PutBlockOnMatrix(tempArrangementMatrix, tempRowLocation, tempColumnLocation, tempRowCount, tempColumnCount, 0);
                }

                ArrangementMatrixList.Add(tempArrangementMatrix);
            }

            return ArrangementMatrixList;
        }

        #endregion 지번 자동할당

        #region BLF, Greedy 알고리즘

        /// <summary>
        /// 투입일, 반출일, 지번을 고려한 BLF 알고리즘
        /// </summary>
        /// <param name="inputBlockList">입력 블록 정보</param>
        /// <param name="ArrangementMatrix">배치할 작업장에 대한 정보(그리드)</param>
        /// <param name="WorkShopInfo">배치할 작업장에 대한 정보</param>
        /// <param name="ArrangementStartDate">배치 시작일</param>
        /// <param name="ArrangementFinishDate">배치 종료일</param>
        /// <returns>블록 배치 결과</returns>
        /// 최초 작성 : 주수헌, 2015년 9월 20일
        /// 수정 일자 : 유상현, 2020년 5월 15일
        ArrangementResultWithDateDTO IBlockArrangement.RunBLFAlgorithm(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArrangementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel)
        {
            ArrangementResultWithDateDTO ResultInfo;

            List<List<WorkshopDTO>> TotalWorkshopResultList = new List<List<WorkshopDTO>>();
            List<Int32[,]> CurrentArrangementMatrix = ArrangementMatrixList;
            List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
            List<BlockDTO> BlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalDailyDelayedBlockList = new List<List<BlockDTO>>();

            DateTime CurrentArrangementDate = new DateTime();

            // 배치 시작일을 현재 배치일로 지정
            CurrentArrangementDate = ArrangementStartDate;

            // 입력 블록 리스트를 함수 내부에서 사용할 리스트로 복사
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                BlockList.Add(inputBlockList[i].Clone());
            }

            List<BlockDTO> TodayCandidateBlockList;
            List<BlockDTO> TodayImportBlock;
            List<BlockDTO> TodayExportBlock;
            List<BlockDTO> TodayDelayedBlockList;

            TimeSpan ts = ArrangementFinishDate - ArrangementStartDate;
            int differenceInDays = ts.Days;
            ProgressBar.Maximum = differenceInDays;
            int count = 0;

            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                        Block.IsLocated = false;
                        Block.IsFinished = true;
                        Block.ActualExportDate = CurrentArrangementDate;
                        //당일 반출 블록 목록에 추가
                        TodayExportBlock.Add(Block);
                        // 배치 매트릭스에서 반출 블록 점유 정보 제거
                        CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, 0);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가
                if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                
                // 원래 배치 일이 오늘인 것 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate) TodayCandidateBlockList.Add(Block);


                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];


                        // 지번 신경 안쓰고 그냥 배치
                        // 배치 가능 위치 탐색
                        int[] BlockLocation = new int[3];
                        // 도로 주변에 배치해야하는 경우

                        BlockLocation = DetermineBlockLocation(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0);


                        // 배치 가능한 경우 배치
                        if (BlockLocation[0] != -1)
                        {
                            // 배치 매트릭스 점유정보 업데이트
                            CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            // 블록 정보 업데이트
                            Block.LocatedRow = BlockLocation[0];
                            Block.LocatedColumn = BlockLocation[1];
                            Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.IsLocated = true;
                            Block.ActualImportDate = CurrentArrangementDate;
                            // 기타 블록 리스트에 블록 추가
                            CurrentArrangementedBlockList.Add(Block);
                            TodayImportBlock.Add(Block);
                            IsLocated = true;
                            break; // 작업장 순회 for loop 탈출
                        }

                    } // 작업장 순회 for loop 종료
                    if (!IsLocated)
                    {
                        TodayDelayedBlockList.Add(Block);
                        Block.IsDelayed = true;
                        Block.DelayedTime += 1;
                        Block.ImportDate = Block.ImportDate.AddDays(1);
                        Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료

                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                List<List<int>> tempBlockIndexList = new List<List<int>>();
                double[] SumBlockArea = new double[WorkShopInfo.Count];
                for (int n = 0; n < WorkShopInfo.Count; n++)
                {
                    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                    tempBlockIndexList.Add(new List<int>());
                    SumBlockArea[n] = 0;
                }
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                }
                foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                {
                    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                }

                //날짜별 변수를 리스트에 저장
                TotalBlockImportLogList.Add(TodayImportBlock);
                TotalBlockExportLogList.Add(TodayExportBlock);
                TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                TotalWorkshopResultList.Add(tempWorkshopInfo);

                List<BlockDTO> tempBlockList = new List<BlockDTO>();

                for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                {
                    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow;
                    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn;
                    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                    tempBlockList.Add(temp);
                }

                TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                count++;

            }//while 종료

            ProgressLabel.Text = "Completed!";
            ProgressBar.GetCurrentParent().Refresh();

            //결과 전달을 위한 DTO 생성
            ResultInfo = new ArrangementResultWithDateDTO(TotalWorkshopResultList, BlockList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDailyDelayedBlockList);
            return ResultInfo;
        }

        /// <summary>
        /// 투입일, 반출일, 지번을 고려한 BLF 알고리즘
        /// </summary>
        /// <param name="inputBlockList">입력 블록 정보</param>
        /// <param name="ArrangementMatrix">배치할 작업장에 대한 정보(그리드)</param>
        /// <param name="WorkShopInfo">배치할 작업장에 대한 정보</param>
        /// <param name="ArrangementStartDate">배치 시작일</param>
        /// <param name="ArrangementFinishDate">배치 종료일</param>
        /// <returns>블록 배치 결과</returns>
        /// 최초 작성 : 주수헌, 2015년 9월 20일
        /// 수정 일자 : 유상현, 2020년 5월 15일
        ArrangementResultWithDateDTO IBlockArrangement.RunBLFAlgorithmWithAddress(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArrangementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel)
        {
            ArrangementResultWithDateDTO ResultInfo;

            List<List<WorkshopDTO>> TotalWorkshopResultList = new List<List<WorkshopDTO>>();
            List<Int32[,]> CurrentArrangementMatrix = ArrangementMatrixList;
            List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
            List<BlockDTO> BlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalDailyDelayedBlockList = new List<List<BlockDTO>>();

            DateTime CurrentArrangementDate = new DateTime();

            // 배치 시작일을 현재 배치일로 지정
            CurrentArrangementDate = ArrangementStartDate;

            // 입력 블록 리스트를 함수 내부에서 사용할 리스트로 복사
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                BlockList.Add(inputBlockList[i].Clone());
            }

            List<BlockDTO> TodayCandidateBlockList;
            List<BlockDTO> TodayImportBlock;
            List<BlockDTO> TodayExportBlock;
            List<BlockDTO> TodayDelayedBlockList;

            TimeSpan ts = ArrangementFinishDate - ArrangementStartDate;
            int differenceInDays = ts.Days;
            ProgressBar.Maximum = differenceInDays;
            int count = 0;

            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                        Block.IsLocated = false;
                        Block.IsFinished = true;
                        Block.ActualExportDate = CurrentArrangementDate;
                        //당일 반출 블록 목록에 추가
                        TodayExportBlock.Add(Block);
                        // 배치 매트릭스에서 반출 블록 점유 정보 제거
                        CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가
                if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 원래 배치 일이 오늘인 것 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate) TodayCandidateBlockList.Add(Block);


                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                        #region 첫 작업장에서 선호 지번 고려한 배치
                        // PreferWorkShopCount = 0이면 선호 지번 고려한 배치. if (PreferWorkshopCount == 0) for (int PreferAddressCount = 0; PreferAddressCount < Block.PreferAddressIndexList.Count; PreferAddressCount++) 선호지번 고려한 배치가능구역 탐색 함수
                        if (PreferWorkShopCount == 0)
                        {
                            for (int PreferAddressCount = 0; PreferAddressCount < Block.PreferAddressIndexList.Count; PreferAddressCount++)
                            {
                                // 선호지번의 좌우측 경계 열 번호 가져오기
                                double AddressLeftPoint = WorkShopInfo[CurrentWorkShopNumber].AddressColumnLocation[Block.PreferAddressIndexList[PreferAddressCount]];
                                double AddressRightPoint;
                                try
                                {
                                    AddressRightPoint = WorkShopInfo[CurrentWorkShopNumber].AddressColumnLocation[Block.PreferAddressIndexList[PreferAddressCount] + 1];
                                }
                                catch (Exception)
                                {
                                    AddressRightPoint = WorkShopInfo[CurrentWorkShopNumber].ColumnCount;
                                }

                                // 해당 선호지번 구역 내에서 배치 가능 유무 파악
                                int[] tempBlockLocation = new int[3];
                                tempBlockLocation = DetermineBlockLocationWithAddress(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, AddressLeftPoint, AddressRightPoint, 1);
                                if (tempBlockLocation[0] == -1) tempBlockLocation = DetermineBlockLocationWithAddress(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, AddressLeftPoint, AddressRightPoint, 0);

                                // 배치 가능한 경우 배치
                                if (tempBlockLocation[0] != -1)
                                {
                                    // 배치 매트릭스 점유정보 업데이트
                                    CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], tempBlockLocation[0], tempBlockLocation[1], Block.RowCount, Block.ColumnCount, tempBlockLocation[2]);
                                    // 블록 정보 업데이트
                                    Block.LocatedRow = tempBlockLocation[0];
                                    Block.LocatedColumn = tempBlockLocation[1];
                                    Block.Orientation = tempBlockLocation[2];
                                    Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.IsLocated = true;
                                    Block.IsConditionSatisfied = true;
                                    Block.ActualImportDate = CurrentArrangementDate;
                                    // 기타 블록 리스트에 블록 추가
                                    CurrentArrangementedBlockList.Add(Block);
                                    TodayImportBlock.Add(Block);
                                    IsLocated = true;
                                    break;
                                }
                            }
                        }
                        if (IsLocated) break; // 작업장 순회 for loop 탈출
                        #endregion 첫 작업장에서 선호 지번 고려한 배치

                        // 지번 신경 안쓰고 그냥 배치
                        // 배치 가능 위치 탐색
                        int[] BlockLocation = new int[3];
                        // 도로 주변에 배치해야하는 경우
                        if (Block.IsRoadSide == true)
                        {
                            BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                            if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                        }
                        else
                        {
                            BlockLocation = DetermineBlockLocation(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1);
                            if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocation(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0);
                        }

                        // 배치 가능한 경우 배치
                        if (BlockLocation[0] != -1)
                        {
                            // 배치 매트릭스 점유정보 업데이트
                            CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            // 블록 정보 업데이트
                            Block.LocatedRow = BlockLocation[0];
                            Block.LocatedColumn = BlockLocation[1];
                            Block.Orientation = BlockLocation[2];
                            Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.IsLocated = true;
                            Block.ActualImportDate = CurrentArrangementDate;
                            // 기타 블록 리스트에 블록 추가
                            CurrentArrangementedBlockList.Add(Block);
                            TodayImportBlock.Add(Block);
                            IsLocated = true;
                            break; // 작업장 순회 for loop 탈출
                        }

                    } // 작업장 순회 for loop 종료
                    if (!IsLocated)
                    {
                        TodayDelayedBlockList.Add(Block);
                        Block.IsDelayed = true;
                        Block.DelayedTime += 1;
                        Block.ImportDate = Block.ImportDate.AddDays(1);
                        Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료

                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                List<List<int>> tempBlockIndexList = new List<List<int>>();
                double[] SumBlockArea = new double[WorkShopInfo.Count];
                for (int n = 0; n < WorkShopInfo.Count; n++)
                {
                    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                    tempBlockIndexList.Add(new List<int>());
                    SumBlockArea[n] = 0;
                }
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                }
                foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                {
                    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                }

                //날짜별 변수를 리스트에 저장
                TotalBlockImportLogList.Add(TodayImportBlock);
                TotalBlockExportLogList.Add(TodayExportBlock);
                TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                TotalWorkshopResultList.Add(tempWorkshopInfo);

                List<BlockDTO> tempBlockList = new List<BlockDTO>();

                for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                {
                    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow;
                    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn;
                    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;
                    tempBlockList.Add(temp);
                }

                TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                count++;

            }//while 종료

            ProgressLabel.Text = "Completed!";
            ProgressBar.GetCurrentParent().Refresh();

            //결과 전달을 위한 DTO 생성
            ResultInfo = new ArrangementResultWithDateDTO(TotalWorkshopResultList, BlockList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDailyDelayedBlockList);
            return ResultInfo;
        }

        /// <summary>
        /// 투입일, 반출일을 고려한 BLF 알고리즘 + 여유공간
        /// </summary>
        /// <param name="inputBlockList">입력 블록 정보</param>
        /// <param name="ArrangementMatrix">배치할 작업장에 대한 정보(그리드)</param>
        /// <param name="WorkShopInfo">배치할 작업장에 대한 정보</param>
        /// <param name="ArrangementStartDate">배치 시작일</param>
        /// <param name="ArrangementFinishDate">배치 종료일</param>
        /// <returns>블록 배치 결과</returns>
        /// 최초 작성 : 주수헌, 2015년 9월 20일
        /// 수정 일자 : 유상현, 2020년 5월 15일
        ArrangementResultWithDateDTO IBlockArrangement.RunBLFAlgorithmWithSlack(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArrangementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack)
        {
            ArrangementResultWithDateDTO ResultInfo;

            List<List<WorkshopDTO>> TotalWorkshopResultList = new List<List<WorkshopDTO>>();
            List<Int32[,]> CurrentArrangementMatrix = ArrangementMatrixList;
            List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
            List<BlockDTO> BlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalDailyDelayedBlockList = new List<List<BlockDTO>>();

            DateTime CurrentArrangementDate = new DateTime();

            // 배치 시작일을 현재 배치일로 지정
            CurrentArrangementDate = ArrangementStartDate;

            // 입력 블록 리스트를 함수 내부에서 사용할 리스트로 복사 + 양측 여유공간 더해줌
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                BlockList.Add(inputBlockList[i].Clone());
                BlockList[i].RowCount += Slack * 2 + BlockList[i].UpperSideCount + BlockList[i].BottomSideCount;
                BlockList[i].ColumnCount += Slack * 2 + BlockList[i].LeftSideCount + BlockList[i].RightSideCount;
            }

            List<BlockDTO> TodayCandidateBlockList;
            List<BlockDTO> TodayImportBlock;
            List<BlockDTO> TodayExportBlock;
            List<BlockDTO> TodayDelayedBlockList;

            TimeSpan ts = ArrangementFinishDate - ArrangementStartDate;
            int differenceInDays = ts.Days;
            ProgressBar.Maximum = differenceInDays;
            int count = 0;

            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                        Block.IsLocated = false;
                        Block.IsFinished = true;
                        Block.ActualExportDate = CurrentArrangementDate;
                        //당일 반출 블록 목록에 추가
                        TodayExportBlock.Add(Block);
                        // 배치 매트릭스에서 반출 블록 점유 정보 제거
                        CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가
                if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 원래 배치 일이 오늘인 것 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate) TodayCandidateBlockList.Add(Block);


                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                        // 지번 신경 안쓰고 그냥 배치
                        // 배치 가능 위치 탐색
                        int[] BlockLocation = new int[3];
                        // 도로 주변에 배치해야하는 경우
                        if (Block.IsRoadSide == true)
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                            }
                            else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                        }
                        else
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                            }
                            else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                        }

                        // 배치 가능한 경우 배치
                        if (BlockLocation[0] != -1)
                        {
                            // 배치 매트릭스 점유정보 업데이트
                            CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            // 블록 정보 업데이트
                            Block.LocatedRow = BlockLocation[0];
                            Block.LocatedColumn = BlockLocation[1];
                            Block.Orientation = BlockLocation[2];
                            Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.IsLocated = true;
                            Block.ActualImportDate = CurrentArrangementDate;
                            // 기타 블록 리스트에 블록 추가
                            CurrentArrangementedBlockList.Add(Block);
                            TodayImportBlock.Add(Block);
                            IsLocated = true;
                            break; // 작업장 순회 for loop 탈출
                        }

                    } // 작업장 순회 for loop 종료
                    if (!IsLocated)
                    {
                        TodayDelayedBlockList.Add(Block);
                        Block.IsDelayed = true;
                        Block.DelayedTime += 1;
                        Block.ImportDate = Block.ImportDate.AddDays(1);
                        Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료

                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                List<List<int>> tempBlockIndexList = new List<List<int>>();
                double[] SumBlockArea = new double[WorkShopInfo.Count];
                for (int n = 0; n < WorkShopInfo.Count; n++)
                {
                    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                    tempBlockIndexList.Add(new List<int>());
                    SumBlockArea[n] = 0;
                }
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                }
                foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                {
                    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                }

                //날짜별 변수를 리스트에 저장
                TotalBlockImportLogList.Add(TodayImportBlock);
                TotalBlockExportLogList.Add(TodayExportBlock);
                TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                TotalWorkshopResultList.Add(tempWorkshopInfo);

                List<BlockDTO> tempBlockList = new List<BlockDTO>();

                for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                {
                    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow + temp.UpperSideCount;
                    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn + temp.LeftSideCount;
                    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;

                    //여유공간 다시 빼줌
                    temp.RowCount -= Slack * 2 + temp.UpperSideCount + temp.BottomSideCount;
                    temp.ColumnCount -= Slack * 2 + temp.LeftSideCount + temp.RightSideCount;

                    tempBlockList.Add(temp);
                }

                TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                count++;

            }//while 종료

            ProgressLabel.Text = "Completed!";
            ProgressBar.GetCurrentParent().Refresh();

            //결과 전달을 위한 DTO 생성
            ResultInfo = new ArrangementResultWithDateDTO(TotalWorkshopResultList, BlockList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDailyDelayedBlockList);
            return ResultInfo;
        }

        /// <summary>
        /// 투입일, 반출일, 지번을 고려한 BLF 알고리즘 + 여유공간 + 우선배치 
        /// </summary>
        /// <param name="inputBlockList">입력 블록 정보</param>
        /// <param name="ArrangementMatrix">배치할 작업장에 대한 정보(그리드)</param>
        /// <param name="WorkShopInfo">배치할 작업장에 대한 정보</param>
        /// <param name="ArrangementStartDate">배치 시작일</param>
        /// <param name="ArrangementFinishDate">배치 종료일</param>
        /// <returns>블록 배치 결과</returns>
        /// 최초 작성 : 주수헌, 2015년 9월 20일
        /// 수정 일자 : 유상현, 2020년 6월 27일
        ArrangementResultWithDateDTO IBlockArrangement.RunBLFAlgorithmWithSlackWithPriority(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArrangementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack)
        {
            ArrangementResultWithDateDTO ResultInfo;

            List<List<WorkshopDTO>> TotalWorkshopResultList = new List<List<WorkshopDTO>>();
            List<Int32[,]> CurrentArrangementMatrix = ArrangementMatrixList;
            List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
            List<BlockDTO> BlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalDailyDelayedBlockList = new List<List<BlockDTO>>();

            List<List<Int32[,]>> PriorityMatrixList = new List<List<int[,]>>();

            DateTime CurrentArrangementDate = new DateTime();

            // 배치 시작일을 현재 배치일로 지정
            CurrentArrangementDate = ArrangementStartDate;

            // 입력 블록 리스트를 함수 내부에서 사용할 리스트로 복사 + 양측 여유공간 더해줌
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                BlockList.Add(inputBlockList[i].Clone());
                BlockList[i].RowCount += Slack * 2 + BlockList[i].UpperSideCount + BlockList[i].BottomSideCount;
                BlockList[i].ColumnCount += Slack * 2 + BlockList[i].LeftSideCount + BlockList[i].RightSideCount;
            }

            List<BlockDTO> TodayCandidateBlockList;
            List<BlockDTO> TodayImportBlock;
            List<BlockDTO> TodayExportBlock;
            List<BlockDTO> TodayDelayedBlockList;

            TimeSpan ts = ArrangementFinishDate - ArrangementStartDate;
            int differenceInDays = ts.Days;
            ProgressBar.Maximum = differenceInDays;
            int count = 0;

            //우선순위 블록 먼저 배치
            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = "우선 배치 블록 : " + CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                        Block.IsLocated = false;
                        Block.IsFinished = true;
                        Block.ActualExportDate = CurrentArrangementDate;
                        //당일 반출 블록 목록에 추가
                        TodayExportBlock.Add(Block);
                        // 배치 매트릭스에서 반출 블록 점유 정보 제거
                        CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가 - 우선배치 블록은 지연 없음
                // if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 원래 배치 일이 오늘인 것 추가
                // 우선순위 블록만 배치 리스트에 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate & Block.IsPrior == true) TodayCandidateBlockList.Add(Block);


                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                        // 지번 신경 안쓰고 그냥 배치
                        // 배치 가능 위치 탐색
                        int[] BlockLocation = new int[3];
                        // 도로 주변에 배치해야하는 경우
                        if (Block.IsRoadSide == true)
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                            }
                            else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                        }
                        else
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                            }
                            else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                        }

                        // 배치 가능한 경우 배치
                        if (BlockLocation[0] != -1)
                        {
                            // 배치 매트릭스 점유정보 업데이트
                            CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            // 블록 정보 업데이트
                            Block.LocatedRow = BlockLocation[0];
                            Block.LocatedColumn = BlockLocation[1];
                            Block.Orientation = BlockLocation[2];
                            Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.IsLocated = true;
                            Block.ActualImportDate = CurrentArrangementDate;
                            // 기타 블록 리스트에 블록 추가 -> 일반 블록 배치하고 나서 같이 하기
                            CurrentArrangementedBlockList.Add(Block);
                            //TodayImportBlock.Add(Block);
                            IsLocated = true;
                            break; // 작업장 순회 for loop 탈출
                        }

                    } // 작업장 순회 for loop 종료
                    //if (!IsLocated)
                    //{
                    //    TodayDelayedBlockList.Add(Block);
                    //    Block.IsDelayed = true;
                    //    Block.DelayedTime += 1;
                    //    Block.ImportDate = Block.ImportDate.AddDays(1);
                    //    Block.ExportDate = Block.ExportDate.AddDays(1);
                    //}
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료

                //이건 일반 배치 블록 다 끝나고 할 수 있도록. 그러려면 CurrentArrangementedBlockList가 보존되어야 함. 문제 -> 우선순위 블록 배치하는 과정에서, 제거하게 되는 블록은 CurrentArrangementedBlockList에서도 제거하게 됨. -> TotalDailyArrangementedBlockList를 활용해야 할 듯 싶은데??
                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                //List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                //List<List<int>> tempBlockIndexList = new List<List<int>>();
                //double[] SumBlockArea = new double[WorkShopInfo.Count];
                //for (int n = 0; n < WorkShopInfo.Count; n++)
                //{
                //    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                //    tempBlockIndexList.Add(new List<int>());
                //    SumBlockArea[n] = 0;
                //}
                //foreach (BlockDTO Block in CurrentArrangementedBlockList)
                //{
                //    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                //    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                //}
                //foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                //{
                //    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                //    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                //    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                //}

                // 일반배치 블록에서는 여기서 리스트를 더하는게 아니라. TotalBlockImportLogList[count].Add(Block) 이렇게 블록단위로해야 할 듯. 아니면 tempList에 저장해놨다가 여기서 한번에 하던가.
                // 아니면 우선순위 블록배치할 때는 진짜 위치, 날짜 등만 정하고 끝나고, 각종 리스트에 더하는건 일반 블록 배치할 때 우선순위 블록 예외처리해서 위치 결정과정만 건너뛰고 리스트에는 그 때 추가하는 걸로 하던가. 내일 뭐가 더 좋을지 생각해보자!
                // 일단 후자가 더 좋아보임 ㅎ
                //날짜별 변수를 리스트에 저장
                //TotalBlockImportLogList.Add(TodayImportBlock);
                //TotalBlockExportLogList.Add(TodayExportBlock);
                //TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                //TotalWorkshopResultList.Add(tempWorkshopInfo);

                //List<BlockDTO> tempBlockList = new List<BlockDTO>();

                //for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                //{
                //    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                //    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow + temp.UpperSideCount;
                //    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn + temp.LeftSideCount;
                //    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                //    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                //    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                //    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                //    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                //    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                //    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                //    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;

                //    //여유공간 다시 빼줌
                //    temp.RowCount -= Slack * 2 + temp.UpperSideCount + temp.BottomSideCount;
                //    temp.ColumnCount -= Slack * 2 + temp.LeftSideCount + temp.RightSideCount;

                //    tempBlockList.Add(temp);
                //}

                //TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                List<Int32[,]> tempMatrixList = new List<Int32[,]>();
                foreach(int[,] Matrix in CurrentArrangementMatrix)
                {
                    int[,] tempMatrix = CloneMatrix(Matrix);
                    tempMatrixList.Add(tempMatrix);
                }

                PriorityMatrixList.Add(tempMatrixList);

                count++;

            }//while 종료


            // 현재 배치일 초기화
            CurrentArrangementDate = ArrangementStartDate;
            count = 0;

            //일반 배치 블록에 대해서 배치 진행
            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = "일반 배치 블록 : " + CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        // 일반 블록인 경우 그대로 진행
                        if (Block.IsPrior == false)
                        {
                            //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                            Block.IsLocated = false;
                            Block.IsFinished = true;
                            Block.ActualExportDate = CurrentArrangementDate;
                            //당일 반출 블록 목록에 추가
                            TodayExportBlock.Add(Block);
                            // 배치 매트릭스에서 반출 블록 점유 정보 제거
                            CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                        }
                        // 우선 배치 블록인 경우 리스트에만 추가
                        else TodayExportBlock.Add(Block);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가
                if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 일반 블록 중 원래 배치 일이 오늘인 것 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate & Block.IsPrior == false) TodayCandidateBlockList.Add(Block);

                // 우선순위 블록 중 오늘 배치해야할 블록은 결과 리스트에만 추가
                foreach (BlockDTO Block in BlockList)
                {
                    if (Block.ImportDate == CurrentArrangementDate & Block.IsPrior == true)
                    {
                        CurrentArrangementedBlockList.Add(Block);
                        TodayImportBlock.Add(Block);
                    }
                }

                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                        // 지번 신경 안쓰고 그냥 배치
                        // 배치 가능 위치 탐색
                        int[] BlockLocation = new int[3];
                        // 도로 주변에 배치해야하는 경우
                        if (Block.IsRoadSide == true)
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                            }
                            else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                        }
                        else
                        {
                            if (Block.ArrangementDirection == -1)
                            {
                                BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                                if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                            }
                            else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                        }

                        // 우선 배치 블록과 충돌하는지 체크
                        if (BlockLocation[0] != -1)
                        {
                            for (int dayCount = 0; dayCount < Block.Leadtime; dayCount++)
                            {
                                if (CheckInterference(PriorityMatrixList[count + dayCount][CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]) == false)
                                {
                                    BlockLocation[0] = -1;
                                    break;
                                }
                            }
                        }

                        // 배치 가능한 경우 배치
                        if (BlockLocation[0] != -1)
                        {
                            // 배치 매트릭스 점유정보 업데이트
                            CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            // 블록 정보 업데이트
                            Block.LocatedRow = BlockLocation[0];
                            Block.LocatedColumn = BlockLocation[1];
                            Block.Orientation = BlockLocation[2];
                            Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                            Block.IsLocated = true;
                            Block.ActualImportDate = CurrentArrangementDate;
                            // 기타 블록 리스트에 블록 추가
                            CurrentArrangementedBlockList.Add(Block);
                            TodayImportBlock.Add(Block);
                            IsLocated = true;
                            break; // 작업장 순회 for loop 탈출
                        }

                    } // 작업장 순회 for loop 종료
                    if (!IsLocated)
                    {
                        TodayDelayedBlockList.Add(Block);
                        Block.IsDelayed = true;
                        Block.DelayedTime += 1;
                        Block.ImportDate = Block.ImportDate.AddDays(1);
                        Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료


                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                List<List<int>> tempBlockIndexList = new List<List<int>>();
                double[] SumBlockArea = new double[WorkShopInfo.Count];
                for (int n = 0; n < WorkShopInfo.Count; n++)
                {
                    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                    tempBlockIndexList.Add(new List<int>());
                    SumBlockArea[n] = 0;
                }
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                }
                foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                {
                    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                }

                //날짜별 변수를 리스트에 저장
                TotalBlockImportLogList.Add(TodayImportBlock);
                TotalBlockExportLogList.Add(TodayExportBlock);
                TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                TotalWorkshopResultList.Add(tempWorkshopInfo);

                List<BlockDTO> tempBlockList = new List<BlockDTO>();

                for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                {
                    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow + temp.UpperSideCount;
                    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn + temp.LeftSideCount;
                    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;

                    //여유공간 다시 빼줌
                    temp.RowCount -= Slack * 2 + temp.UpperSideCount + temp.BottomSideCount;
                    temp.ColumnCount -= Slack * 2 + temp.LeftSideCount + temp.RightSideCount;

                    tempBlockList.Add(temp);
                }

                TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                count++;

            }//while 종료

            ProgressLabel.Text = "Completed!";
            ProgressBar.GetCurrentParent().Refresh();

            //결과 전달을 위한 DTO 생성
            ResultInfo = new ArrangementResultWithDateDTO(TotalWorkshopResultList, BlockList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDailyDelayedBlockList);
            return ResultInfo;
        }

        /// <summary>
        /// 투입일, 반출일, 지번을 고려한 BLF 알고리즘 + 여유공간 + 우선배치 + 해상크레인 출고장 개념 도입
        /// </summary>
        /// <param name="inputBlockList">입력 블록 정보</param>
        /// <param name="ArrangementMatrix">배치할 작업장에 대한 정보(그리드)</param>
        /// <param name="WorkShopInfo">배치할 작업장에 대한 정보</param>
        /// <param name="ArrangementStartDate">배치 시작일</param>
        /// <param name="ArrangementFinishDate">배치 종료일</param>
        /// <returns>블록 배치 결과</returns>
        /// 최초 작성 : 주수헌, 2015년 9월 20일
        /// 수정 일자 : 유상현, 2020년 6월 27일
        ArrangementResultWithDateDTO IBlockArrangement.RunBLFAlgorithmWithFloatingCrane(List<BlockDTO> inputBlockList, List<Int32[,]> ArrangementMatrixList, List<WorkshopDTO> WorkShopInfo, DateTime ArrangementStartDate, DateTime ArrangementFinishDate, ToolStripProgressBar ProgressBar, ToolStripStatusLabel ProgressLabel, int Slack, int ExportWorkshopIndex)
        {
            ArrangementResultWithDateDTO ResultInfo;

            List<List<WorkshopDTO>> TotalWorkshopResultList = new List<List<WorkshopDTO>>();
            List<Int32[,]> CurrentArrangementMatrix = ArrangementMatrixList;
            List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
            List<BlockDTO> BlockList = new List<BlockDTO>();
            List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
            List<List<BlockDTO>> TotalDailyDelayedBlockList = new List<List<BlockDTO>>();

            List<List<Int32[,]>> PriorityMatrixList = new List<List<int[,]>>();

            DateTime CurrentArrangementDate = new DateTime();

            // 배치 시작일을 현재 배치일로 지정
            CurrentArrangementDate = ArrangementStartDate;

            // 입력 블록 리스트를 함수 내부에서 사용할 리스트로 복사 + 양측 여유공간 더해줌
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                BlockList.Add(inputBlockList[i].Clone());
                BlockList[i].RowCount += Slack * 2 + BlockList[i].UpperSideCount + BlockList[i].BottomSideCount;
                BlockList[i].ColumnCount += Slack * 2 + BlockList[i].LeftSideCount + BlockList[i].RightSideCount;
            }

            List<BlockDTO> TodayCandidateBlockList;
            List<BlockDTO> TodayImportBlock;
            List<BlockDTO> TodayExportBlock;
            List<BlockDTO> TodayDelayedBlockList;
            

            TimeSpan ts = ArrangementFinishDate - ArrangementStartDate;
            int differenceInDays = ts.Days;
            ProgressBar.Maximum = differenceInDays;
            int count = 0;


            //출고장 스케줄 먼저 확정
            // 우선순위 블록의 출고일에 해당하는 BlockDTO List 생성
            // 해당 리스트의 블록들 가시화 할 수 있게 하기
            List<BlockDTO> FloatingCraneExportBlockList = new List<BlockDTO>();
            foreach (BlockDTO Block in BlockList)
            {
                if (Block.IsPrior == true)
                {
                    BlockDTO tempBlock = Block.Clone();
                    tempBlock.ImportDate = Block.ExportDate;
                    tempBlock.ExportDate = Block.ExportDate;
                    tempBlock.ArrangementDirection = Block.ArrangementDirection;
                    tempBlock.IsFloatingCraneExportBlock = true;
                    FloatingCraneExportBlockList.Add(tempBlock);
                    Block.ExportDate = Block.ExportDate.AddDays(-1);
                }
            }
            // 해당 리스트의 블록들 매트릭스에 배치
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {
                ProgressBar.Value = count;
                ProgressLabel.Text = "주기조립장 출고 스케줄 : " + CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                List<Int32[,]> tempMatrixList = new List<Int32[,]>();
                foreach (int[,] Matrix in CurrentArrangementMatrix)
                {
                    int[,] tempMatrix = CloneMatrix(Matrix);
                    tempMatrixList.Add(tempMatrix);
                }

                foreach (BlockDTO Block in FloatingCraneExportBlockList)
                {
                    if (Block.ImportDate == CurrentArrangementDate)
                    {
                        if (CurrentArrangementDate == new DateTime(2020,3,5))
                        {
                            int testtest = 1;
                        }
                        if (Block.Index == 271)
                        {
                            int testttt = 1;
                        }


                        int[] BlockLocation = new int[3];
                        if (Block.ArrangementDirection == -1)
                        {
                            BlockLocation = DetermineBlockLocationWithSearchDirection(tempMatrixList[ExportWorkshopIndex], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                            if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(tempMatrixList[ExportWorkshopIndex], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                        }
                        else BlockLocation = DetermineBlockLocationWithSearchDirection(tempMatrixList[ExportWorkshopIndex], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                        // 배치 매트릭스 점유정보 업데이트
                        if (BlockLocation[0] != -1)
                        {
                            tempMatrixList[ExportWorkshopIndex] = PutBlockOnMatrix(tempMatrixList[ExportWorkshopIndex], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                            Block.LocatedRow = BlockLocation[0] + Block.UpperSideCount;
                            Block.LocatedColumn = BlockLocation[1] + Block.LeftSideCount;
                            Block.Orientation = BlockLocation[2];
                            Block.CurrentLocatedWorkshopIndex = ExportWorkshopIndex;
                            //여유공간 다시 빼줌
                            Block.RowCount -= Slack * 2 + Block.UpperSideCount + Block.BottomSideCount;
                            Block.ColumnCount -= Slack * 2 + Block.LeftSideCount + Block.RightSideCount;
                        }
                        // 배치 불가능할 경우 다음날로 미룸
                        else Block.ImportDate = Block.ImportDate.AddDays(1); Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                }


                PriorityMatrixList.Add(tempMatrixList);

                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);
                count++;

            }// while 종료
            // 현재 배치일 초기화
            CurrentArrangementDate = ArrangementStartDate;
            count = 0;


            //우선순위 블록 먼저 배치
            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = "우선 배치 블록 : " + CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                        Block.IsLocated = false;
                        Block.IsFinished = true;
                        Block.ActualExportDate = CurrentArrangementDate;
                        //당일 반출 블록 목록에 추가
                        TodayExportBlock.Add(Block);
                        // 배치 매트릭스에서 반출 블록 점유 정보 제거
                        CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가 - 우선배치 블록은 지연 없음
                // if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 원래 배치 일이 오늘인 것 추가
                // 우선순위 블록만 배치 리스트에 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate & Block.IsPrior == true) TodayCandidateBlockList.Add(Block);

                if (CurrentArrangementDate == new DateTime(2020, 7,10))
                {
                    foreach (BlockDTO Block in BlockList)
                    {
                        if (Block.Index == 332)
                        {
                            int aaaaaa = 1;
                        }
                    }
                }

                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    if (Block.Index == 332)
                    {
                        int fff = 1;
                    }

                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;

                    // 출고장 배치 가능 여부 파악
                    #region 출고장 배치 가능 여부 파악
                    CurrentWorkShopNumber = ExportWorkshopIndex;

                    // 지번 신경 안쓰고 그냥 배치
                    // 배치 가능 위치 탐색
                    int[] BlockLocation = new int[3];
                    // 도로 주변에 배치해야하는 경우
                    if (Block.IsRoadSide == true)
                    {
                        if (Block.ArrangementDirection == -1)
                        {
                            BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                            if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                        }
                        else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                    }
                    else
                    {
                        if (Block.ArrangementDirection == -1)
                        {
                            BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                            if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                        }
                        else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                    }

                    // 출고장 스케줄과 충돌하는지 체크
                    if (BlockLocation[0] != -1)
                    {
                        for (int dayCount = 0; dayCount < Block.Leadtime; dayCount++)
                        {
                            if (CheckInterference(PriorityMatrixList[count + dayCount][CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]) == false)
                            {
                                BlockLocation[0] = -1;
                                break;
                            }
                        }
                    }

                    // 배치 가능한 경우 배치
                    if (BlockLocation[0] != -1)
                    {
                        // 배치 매트릭스 점유정보 업데이트
                        CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                        // 블록 정보 업데이트
                        Block.LocatedRow = BlockLocation[0];
                        Block.LocatedColumn = BlockLocation[1];
                        Block.Orientation = BlockLocation[2];
                        Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                        Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                        Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                        Block.IsLocated = true;
                        Block.ActualImportDate = CurrentArrangementDate;
                        // 기타 블록 리스트에 블록 추가 -> 일반 블록 배치하고 나서 같이 하기
                        CurrentArrangementedBlockList.Add(Block);
                        //TodayImportBlock.Add(Block);
                        IsLocated = true;
                        //break; // 작업장 순회 for loop 탈출 -> 작업장 순회 for loop 따위 없으니까 필요 없음
                    }
                    #endregion


                    // 작업장 순회 for loop, 출고장에 배치되지 않았을 때만, 출고장은 빼고 탐색
                    if (Block.IsLocated == false)
                    {
                        for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                        {
                            CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                            if (CurrentWorkShopNumber != ExportWorkshopIndex)
                            {

                                // 지번 신경 안쓰고 그냥 배치
                                // 배치 가능 위치 탐색
                                //int[] BlockLocation = new int[3];
                                // 도로 주변에 배치해야하는 경우
                                if (Block.IsRoadSide == true)
                                {
                                    if (Block.ArrangementDirection == -1)
                                    {
                                        BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                                        if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                                    }
                                    else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                                }
                                else
                                {
                                    if (Block.ArrangementDirection == -1)
                                    {
                                        BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                                        if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                                    }
                                    else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                                }

                                // 배치 가능한 경우 배치
                                if (BlockLocation[0] != -1)
                                {
                                    // 배치 매트릭스 점유정보 업데이트
                                    CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                                    // 블록 정보 업데이트
                                    Block.LocatedRow = BlockLocation[0];
                                    Block.LocatedColumn = BlockLocation[1];
                                    Block.Orientation = BlockLocation[2];
                                    Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                                    Block.IsLocated = true;
                                    Block.ActualImportDate = CurrentArrangementDate;
                                    // 기타 블록 리스트에 블록 추가 -> 일반 블록 배치하고 나서 같이 하기
                                    CurrentArrangementedBlockList.Add(Block);
                                    //TodayImportBlock.Add(Block);
                                    IsLocated = true;
                                    break; // 작업장 순회 for loop 탈출
                                }
                            }

                        } // 작업장 순회 for loop 종료
                    } // 작업장 순회 for loop를 돌지 결정하는 if문 종료
                    //if (!IsLocated)
                    //{
                    //    TodayDelayedBlockList.Add(Block);
                    //    Block.IsDelayed = true;
                    //    Block.DelayedTime += 1;
                    //    Block.ImportDate = Block.ImportDate.AddDays(1);
                    //    Block.ExportDate = Block.ExportDate.AddDays(1);
                    //}
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료

                //이건 일반 배치 블록 다 끝나고 할 수 있도록. 그러려면 CurrentArrangementedBlockList가 보존되어야 함. 문제 -> 우선순위 블록 배치하는 과정에서, 제거하게 되는 블록은 CurrentArrangementedBlockList에서도 제거하게 됨. -> TotalDailyArrangementedBlockList를 활용해야 할 듯 싶은데??
                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                //List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                //List<List<int>> tempBlockIndexList = new List<List<int>>();
                //double[] SumBlockArea = new double[WorkShopInfo.Count];
                //for (int n = 0; n < WorkShopInfo.Count; n++)
                //{
                //    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                //    tempBlockIndexList.Add(new List<int>());
                //    SumBlockArea[n] = 0;
                //}
                //foreach (BlockDTO Block in CurrentArrangementedBlockList)
                //{
                //    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                //    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                //}
                //foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                //{
                //    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                //    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                //    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                //}

                // 일반배치 블록에서는 여기서 리스트를 더하는게 아니라. TotalBlockImportLogList[count].Add(Block) 이렇게 블록단위로해야 할 듯. 아니면 tempList에 저장해놨다가 여기서 한번에 하던가.
                // 아니면 우선순위 블록배치할 때는 진짜 위치, 날짜 등만 정하고 끝나고, 각종 리스트에 더하는건 일반 블록 배치할 때 우선순위 블록 예외처리해서 위치 결정과정만 건너뛰고 리스트에는 그 때 추가하는 걸로 하던가. 내일 뭐가 더 좋을지 생각해보자!
                // 일단 후자가 더 좋아보임 ㅎ
                //날짜별 변수를 리스트에 저장
                //TotalBlockImportLogList.Add(TodayImportBlock);
                //TotalBlockExportLogList.Add(TodayExportBlock);
                //TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                //TotalWorkshopResultList.Add(tempWorkshopInfo);

                //List<BlockDTO> tempBlockList = new List<BlockDTO>();

                //for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                //{
                //    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                //    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow + temp.UpperSideCount;
                //    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn + temp.LeftSideCount;
                //    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                //    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                //    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                //    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                //    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                //    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                //    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                //    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;

                //    //여유공간 다시 빼줌
                //    temp.RowCount -= Slack * 2 + temp.UpperSideCount + temp.BottomSideCount;
                //    temp.ColumnCount -= Slack * 2 + temp.LeftSideCount + temp.RightSideCount;

                //    tempBlockList.Add(temp);
                //}

                //TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                List<Int32[,]> tempMatrixList = new List<Int32[,]>();
                foreach (int[,] Matrix in CurrentArrangementMatrix)
                {
                    int[,] tempMatrix = CloneMatrix(Matrix);
                    tempMatrixList.Add(tempMatrix);
                }

                // 우선배치 매트릭스의 현재 날짜에 해당하는 매트릭스 업데이트
                PriorityMatrixList[count] = tempMatrixList;

                count++;

            }//while 종료


            // 현재 배치일 초기화
            CurrentArrangementDate = ArrangementStartDate;
            count = 0;

            //일반 배치 블록에 대해서 배치 진행
            //시작일과 종료일 사이에서 하루씩 시간을 진행하면서 배치를 진행
            while (DateTime.Compare(CurrentArrangementDate, ArrangementFinishDate) < 0)
            {

                ProgressBar.Value = count;
                ProgressLabel.Text = "일반 배치 블록 : " + CurrentArrangementDate.ToString("yyyy-MM-dd") + " (" + (Math.Round(((double)count / (double)differenceInDays) * 100.0, 2)).ToString() + "%)";
                ProgressBar.GetCurrentParent().Refresh();

                TodayCandidateBlockList = new List<BlockDTO>();
                TodayImportBlock = new List<BlockDTO>();
                TodayExportBlock = new List<BlockDTO>();
                TodayDelayedBlockList = new List<BlockDTO>();

                //현재의 작업장 배치 정보 중, 반출일에 해당하는 블록을 제거
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.ExportDate.AddDays(1) == CurrentArrangementDate)
                    {
                        // 일반 블록인 경우 그대로 진행
                        if (Block.IsPrior == false)
                        {
                            //블록의 배치 상태 및 완료 상태, 실제 반출 날짜 기록
                            Block.IsLocated = false;
                            Block.IsFinished = true;
                            Block.ActualExportDate = CurrentArrangementDate;
                            //당일 반출 블록 목록에 추가
                            TodayExportBlock.Add(Block);
                            // 배치 매트릭스에서 반출 블록 점유 정보 제거
                            CurrentArrangementMatrix[Block.LocatedWorkshopIndex] = RemoveBlockFromMatrix(CurrentArrangementMatrix[Block.LocatedWorkshopIndex], Block.LocatedRow, Block.LocatedColumn, Block.RowCount, Block.ColumnCount, Block.Orientation);
                        }
                        // 우선 배치 블록인 경우 리스트에만 추가
                        else TodayExportBlock.Add(Block);
                    }
                }
                // 현재 작업장에 배치중인 블록 리스트에서 반출 블록을 제거
                foreach (BlockDTO Block in TodayExportBlock) CurrentArrangementedBlockList.Remove(Block);

                // 오늘 배치해야 할 BlockList 생성
                // Delay 된 것 먼저 추가
                if (TotalDailyDelayedBlockList.Count != 0) foreach (BlockDTO Block in TotalDailyDelayedBlockList[TotalDailyDelayedBlockList.Count - 1]) TodayCandidateBlockList.Add(Block);
                // 일반 블록 중 원래 배치 일이 오늘인 것 추가
                foreach (BlockDTO Block in BlockList) if (Block.IsDelayed == false & Block.IsFinished == false & Block.IsLocated == false & Block.ImportDate == CurrentArrangementDate & Block.IsPrior == false) TodayCandidateBlockList.Add(Block);

                // 우선순위 블록 중 오늘 배치해야할 블록은 결과 리스트에만 추가
                foreach (BlockDTO Block in BlockList)
                {
                    if (Block.ImportDate == CurrentArrangementDate & Block.IsPrior == true)
                    {
                        CurrentArrangementedBlockList.Add(Block);
                        TodayImportBlock.Add(Block);
                    }
                }

                // 오늘 배치해야 할 BlockList에서 차례로 배치
                foreach (BlockDTO Block in TodayCandidateBlockList)
                {
                    bool IsLocated = false;
                    int CurrentWorkShopNumber = 0;
                    // 작업장 순회 for loop
                    for (int PreferWorkShopCount = 0; PreferWorkShopCount < Block.PreferWorkShopIndexList.Count; PreferWorkShopCount++)
                    {
                        CurrentWorkShopNumber = Block.PreferWorkShopIndexList[PreferWorkShopCount];

                        // 출고장은 빼고 배치
                        if (CurrentWorkShopNumber != ExportWorkshopIndex)
                        {

                            // 지번 신경 안쓰고 그냥 배치
                            // 배치 가능 위치 탐색
                            int[] BlockLocation = new int[3];
                            // 도로 주변에 배치해야하는 경우
                            if (Block.IsRoadSide == true)
                            {
                                if (Block.ArrangementDirection == -1)
                                {
                                    BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, WorkShopInfo[CurrentWorkShopNumber]);
                                    if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, WorkShopInfo[CurrentWorkShopNumber]);
                                }
                                else BlockLocation = DetermineBlockLocationOnRoadSide(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, WorkShopInfo[CurrentWorkShopNumber]);

                            }
                            else
                            {
                                if (Block.ArrangementDirection == -1)
                                {
                                    BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 0, Block.SearchDirection);
                                    if (BlockLocation[0] == -1) BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, 1, Block.SearchDirection);
                                }
                                else BlockLocation = DetermineBlockLocationWithSearchDirection(CurrentArrangementMatrix[CurrentWorkShopNumber], Block.RowCount, Block.ColumnCount, Block.ArrangementDirection, Block.SearchDirection);
                            }

                            // 우선 배치 블록과 충돌하는지 체크
                            if (BlockLocation[0] != -1)
                            {
                                for (int dayCount = 0; dayCount < Block.Leadtime; dayCount++)
                                {
                                    if (CheckInterference(PriorityMatrixList[count + dayCount][CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]) == false)
                                    {
                                        BlockLocation[0] = -1;
                                        break;
                                    }
                                }
                            }

                            // 배치 가능한 경우 배치
                            if (BlockLocation[0] != -1)
                            {
                                // 배치 매트릭스 점유정보 업데이트
                                CurrentArrangementMatrix[CurrentWorkShopNumber] = PutBlockOnMatrix(CurrentArrangementMatrix[CurrentWorkShopNumber], BlockLocation[0], BlockLocation[1], Block.RowCount, Block.ColumnCount, BlockLocation[2]);
                                // 블록 정보 업데이트
                                Block.LocatedRow = BlockLocation[0];
                                Block.LocatedColumn = BlockLocation[1];
                                Block.Orientation = BlockLocation[2];
                                Block.LocatedWorkshopIndex = CurrentWorkShopNumber;
                                Block.CurrentLocatedWorkshopIndex = CurrentWorkShopNumber;
                                Block.ActualLocatedWorkshopIndex = CurrentWorkShopNumber;
                                Block.IsLocated = true;
                                Block.ActualImportDate = CurrentArrangementDate;
                                // 기타 블록 리스트에 블록 추가
                                CurrentArrangementedBlockList.Add(Block);
                                TodayImportBlock.Add(Block);
                                IsLocated = true;
                                break; // 작업장 순회 for loop 탈출
                            }
                        } // if
                    } // 작업장 순회 for loop 종료
                    if (!IsLocated)
                    {
                        TodayDelayedBlockList.Add(Block);
                        Block.IsDelayed = true;
                        Block.DelayedTime += 1;
                        Block.ImportDate = Block.ImportDate.AddDays(1);
                        Block.ExportDate = Block.ExportDate.AddDays(1);
                    }
                    //if 종료
                } // 오늘 배치해야 할 블록들 배치 완료


                //날짜별 배치 결과를 바탕으로 날짜별 작업장의 블록 배치 개수와 공간 점유율 계산
                List<WorkshopDTO> tempWorkshopInfo = new List<WorkshopDTO>();
                List<List<int>> tempBlockIndexList = new List<List<int>>();
                double[] SumBlockArea = new double[WorkShopInfo.Count];
                for (int n = 0; n < WorkShopInfo.Count; n++)
                {
                    tempWorkshopInfo.Add(WorkShopInfo[n].Clone());
                    tempBlockIndexList.Add(new List<int>());
                    SumBlockArea[n] = 0;
                }
                foreach (BlockDTO Block in CurrentArrangementedBlockList)
                {
                    if (Block.CurrentLocatedWorkshopIndex == -1)
                    {
                        int testtestttt = 1;
                    }
                    tempBlockIndexList[Block.CurrentLocatedWorkshopIndex].Add(Block.Index);
                    SumBlockArea[Block.CurrentLocatedWorkshopIndex] += Block.RowCount * Block.ColumnCount;
                }
                foreach (WorkshopDTO Workshop in tempWorkshopInfo)
                {
                    Workshop.LocatedBlockIndexList = tempBlockIndexList[Workshop.Index];
                    Workshop.NumOfLocatedBlocks = Workshop.LocatedBlockIndexList.Count;
                    Workshop.AreaUtilization = SumBlockArea[Workshop.Index] / (Workshop.RowCount * Workshop.ColumnCount);
                }

                //날짜별 변수를 리스트에 저장
                TotalBlockImportLogList.Add(TodayImportBlock);
                TotalBlockExportLogList.Add(TodayExportBlock);
                TotalDailyDelayedBlockList.Add(TodayDelayedBlockList);
                TotalWorkshopResultList.Add(tempWorkshopInfo);

                List<BlockDTO> tempBlockList = new List<BlockDTO>();

                for (int i = 0; i < CurrentArrangementedBlockList.Count; i++)
                {
                    BlockDTO temp = CurrentArrangementedBlockList[i].Clone();
                    temp.LocatedRow = CurrentArrangementedBlockList[i].LocatedRow + temp.UpperSideCount;
                    temp.LocatedColumn = CurrentArrangementedBlockList[i].LocatedColumn + temp.LeftSideCount;
                    temp.Orientation = CurrentArrangementedBlockList[i].Orientation;
                    temp.CurrentLocatedWorkshopIndex = CurrentArrangementedBlockList[i].CurrentLocatedWorkshopIndex;
                    temp.CurrentLocatedAddressIndex = CurrentArrangementedBlockList[i].CurrentLocatedAddressIndex;
                    temp.IsLocated = CurrentArrangementedBlockList[i].IsLocated;
                    temp.IsFinished = CurrentArrangementedBlockList[i].IsFinished;
                    temp.IsRoadSide = CurrentArrangementedBlockList[i].IsRoadSide;
                    temp.IsConditionSatisfied = CurrentArrangementedBlockList[i].IsConditionSatisfied;
                    temp.IsDelayed = CurrentArrangementedBlockList[i].IsDelayed;

                    //여유공간 다시 빼줌
                    temp.RowCount -= Slack * 2 + temp.UpperSideCount + temp.BottomSideCount;
                    temp.ColumnCount -= Slack * 2 + temp.LeftSideCount + temp.RightSideCount;

                    tempBlockList.Add(temp);
                }

                TotalDailyArragnementedBlockList.Add(tempBlockList);
                CurrentArrangementDate = CurrentArrangementDate.AddDays(1);

                count++;

            }//while 종료

            // 해상크레인 출고장 블록 가시화를 위해 TotalDailyArrangementedBlockList에 추가
            foreach (BlockDTO Block in FloatingCraneExportBlockList)
            {
                int tempDayCount = (Block.ImportDate - ArrangementStartDate).Days;
                TotalDailyArragnementedBlockList[tempDayCount].Add(Block);
            }

            ProgressLabel.Text = "Completed!";
            ProgressBar.GetCurrentParent().Refresh();

            //결과 전달을 위한 DTO 생성
            ResultInfo = new ArrangementResultWithDateDTO(TotalWorkshopResultList, BlockList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDailyDelayedBlockList);
            return ResultInfo;
        }




        #endregion


        #region 기타 메서드(최소,최대 입출고일 계산, 면적활용률 계산, 리스트 중복제거 등등)

        /// <summary>
        /// 현재 입력되어 있는 블록 정보 중 가장 빠른 입고일과 가장 늦은 출고일을 계산하는 함수
        /// </summary>
        /// <param name="_blockInfoList">블록 정보</param>
        /// <returns>
        /// [0]: 가장 빠른 입고일
        /// [1]: 가장 늦은 출고일
        /// </returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// 수정 작성 : 이동건, 2016년 03월 30일
        /// </remarks>
        DateTime[] IBlockArrangement.CalcEarliestAndLatestDates(List<BlockDTO> _blockInfoList)
        {
            DateTime[] returnDates = new DateTime[2];

            List<DateTime> importDateList = new List<DateTime>();
            List<DateTime> exportDateList = new List<DateTime>();
            for (int i = 0; i < _blockInfoList.Count; i++)
            {
                importDateList.Add(_blockInfoList[i].ImportDate);
                exportDateList.Add(_blockInfoList[i].ExportDate);
            }

            //가장 빠른 입고일, 가장 늦은 입고일 찾기
            DateTime earliestDate = FindEarliestDate(importDateList);
            DateTime latestDate = FindLatestDate(exportDateList);
            latestDate = latestDate.AddMonths(2);

            returnDates[0] = earliestDate;
            returnDates[1] = latestDate;

            return returnDates;
        }




        /// <summary>
        /// 입력된 날짜 리스트 중 가장 빠른 날짜를 반환하는 함수
        /// </summary>
        /// <param name="inputList">날짜 리스트</param>
        /// <returns>가장 빠른 날짜</returns>
        /// <remarks>
        /// 최초 구현: 정용국, 2015년 10월 23일
        /// 최종 수정: 정용국, 2015년 10월 23일
        /// </remarks>
        DateTime FindEarliestDate(List<DateTime> inputList)
        {
            DateTime returnDate = new DateTime();

            returnDate = inputList[0];

            for (int i = 1; i < inputList.Count; i++)
            {
                if (DateTime.Compare(inputList[i], returnDate) < 0) returnDate = inputList[i];
            }

            return returnDate;
        }

        /// <summary>
        /// 입력된 날짜 리스트 중 가장 느린 날짜를 반환하는 함수
        /// </summary>
        /// <param name="inputList">날짜 리스트</param>
        /// <returns>가장 느린 날짜</returns>
        /// <remarks>
        /// 최초 구현: 정용국, 2015년 10월 23일
        /// 최종 수정: 정용국, 2015년 10월 23일
        /// </remarks>
        DateTime FindLatestDate(List<DateTime> inputList)
        {
            DateTime returnDate = new DateTime();

            returnDate = inputList[0];

            for (int i = 1; i < inputList.Count; i++)
            {
                if (DateTime.Compare(inputList[i], returnDate) > 0) returnDate = inputList[i];
            }

            return returnDate;
        }

        /// <summary>
        /// 현재 작업장에 배치된 블록의 index를 검색하는 함수
        /// </summary>
        /// <param name="CurrentArrangementMatrix">검색하고자 하는 작업장의 배치정보</param>
        /// <param name="WorkShopInfo">작업장 정보</param>
        /// <param name="workshopIndex">작업장의 번호</param>
        /// <returns>현재 작업장에 배치된 블록의 일련번호 목록</returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        List<int> FindMyLocatedBlockIndex(List<UnitcellDTO[,]> CurrentArrangementMatrix, List<WorkshopDTO> WorkShopInfo, int workshopIndex)
        {

            List<int> returnBlockIndexList = new List<int>();
            List<int> _locatedWorkshopIndex = new List<int>();

            for (int i = 0; i < CurrentArrangementMatrix[workshopIndex].GetLength(0); i++)
            {
                for (int j = 0; j < CurrentArrangementMatrix[workshopIndex].GetLength(1); j++)
                {
                    if (CurrentArrangementMatrix[workshopIndex][i, j].BlockIndex != -1)
                    {
                        bool isduplicated = false;
                        for (int k = 0; k < returnBlockIndexList.Count; k++)
                        {
                            if (CurrentArrangementMatrix[workshopIndex][i, j].BlockIndex == returnBlockIndexList[k])
                            {
                                isduplicated = true;
                            }
                        }
                        if (isduplicated == false)
                        {
                            returnBlockIndexList.Add(CurrentArrangementMatrix[workshopIndex][i, j].BlockIndex);
                        }
                    }
                }
            }

            return returnBlockIndexList;
        }

        /// <summary>
        /// 선택된 작업장의 면적활용률을 계산하는 함수
        /// </summary>
        /// <param name="CurrentArrangementMatrix">현재 배치상황을 저장하고 있는 목록</param>
        /// <param name="WorkShopInfo">작업장 정보</param>
        /// <param name="workshopIndex">선택된 작업장 일련번호</param>
        /// <returns>면적활용률</returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        double CalcMyAreaUtilization(List<UnitcellDTO[,]> CurrentArrangementMatrix, List<WorkshopDTO> WorkShopInfo, int workshopIndex)
        {
            int occupiedAreaCount = 0;
            int totalAreaCount = 0;
            for (int row = 0; row < CurrentArrangementMatrix[workshopIndex].GetLength(0); row++)
            {
                for (int column = 0; column < CurrentArrangementMatrix[workshopIndex].GetLength(1); column++)
                {
                    totalAreaCount++;
                    if (CurrentArrangementMatrix[workshopIndex][row, column].IsOccupied == true) occupiedAreaCount++;
                }
            }
            return (double)(occupiedAreaCount) / (double)(totalAreaCount);
        }

        /// <summary>
        /// 정수 리스트에서 최소값을 반환하는 함수
        /// </summary>
        /// <param name="inputList">정수 리스트</param>
        /// <returns>최소값</returns>
        /// <remarks>
        /// 최초 작성: 주수헌
        /// 최종 수정: 정용국, 2016년 05월 08일
        /// </remarks>
        int CalcLowestValue(List<int> inputList)
        {
            int tempValue = 100000;
            int returnIndex = 0;

            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i] < tempValue)
                {
                    tempValue = inputList[i];
                    returnIndex = i;
                }
            }
            return tempValue;
        }

        /// <summary>
        /// 후보점 리스트에서 중복된 점을 확인하여 제거하는 함수
        /// </summary>
        /// <param name="ListofCandidateLocations">후보점 리스트</param>
        /// <returns>중복된 점이 제거된 리스트</returns>
        List<Point> RemoveDuplicatedLocations(List<Point> ListofCandidateLocations)
        {
            List<Point> candidateLocations = new List<Point>();
            Dictionary<string, Point> DictionaryForCheck = new Dictionary<string, Point>();
            for (int i = 0; i < ListofCandidateLocations.Count; i++)
            {
                string sKeyforCheck = "X" + Convert.ToString(ListofCandidateLocations[i].X) + "Y" + Convert.ToString(ListofCandidateLocations[i].Y);
                if (!DictionaryForCheck.ContainsKey(sKeyforCheck))
                {
                    DictionaryForCheck.Add(sKeyforCheck, ListofCandidateLocations[i]);
                    candidateLocations.Add(ListofCandidateLocations[i]);
                }
            }
            return candidateLocations;
        }

        /// <summary>
        /// 사각형 블록의 꼭지점을 반환하는 함수
        /// </summary>
        /// <param name="currentLocatedBlock">블록 정보</param>
        /// <returns>꼭지점 4개</returns>
        Point[] FindMyVertices(BlockDTO currentLocatedBlock)
        {
            int tempLocatedX = System.Convert.ToInt32(currentLocatedBlock.LocatedColumn);
            int tempLocatedY = System.Convert.ToInt32(currentLocatedBlock.LocatedRow);
            int tempRowCount = System.Convert.ToInt32(Math.Ceiling(currentLocatedBlock.RowCount));
            int tempColumnCount = System.Convert.ToInt32(Math.Ceiling(currentLocatedBlock.ColumnCount));

            Point temp1 = new Point(tempLocatedX, tempLocatedY);
            Point temp2 = new Point(tempLocatedX + tempColumnCount - 1, tempLocatedY);
            Point temp3 = new Point(tempLocatedX, tempLocatedY + tempRowCount - 1);
            Point temp4 = new Point(tempLocatedX + tempColumnCount - 1, tempLocatedY + tempRowCount - 1);

            Point[] ArrayofVertex = new Point[4] { temp1, temp2, temp3, temp4 };
            return ArrayofVertex;
        }

        /// <summary>
        /// 대상 날짜에 대해 Block,workshop 정보를 바탕으로 해당 RepresentingBlockInfoAtGrid날짜의 배치 형상을 만드는 함수
        /// </summary>
        /// <param name="inputBlockList"></param>
        /// <param name="ArrangementMatrix"></param>
        /// <param name="WorkShopInfo"></param>
        /// <param name="TargetDate"></param>
        /// <returns></returns>
        List<UnitcellDTO[,]> RepresentingBlockInfoAtGrid(List<BlockDTO> inputBlockList, List<WorkshopDTO> WorkShopInfo, DateTime TargetDate)
        {
            List<UnitcellDTO[,]> ResultArrangementedWorkShop = new List<UnitcellDTO[,]>();

            //지번자동할당(배치 Grid 정보 생성)
            ResultArrangementedWorkShop = ((IBlockArrangement)this).InitializeArrangementMatrixWithAddress(WorkShopInfo);

            List<BlockDTO> LocatedBlockList = new List<BlockDTO>();

            //배치된 블록 선정
            for (int i = 0; i < inputBlockList.Count; i++)
            {
                if (DateTime.Compare(TargetDate, inputBlockList[i].ActualImportDate) >= 0 & DateTime.Compare(inputBlockList[i].ActualExportDate, TargetDate) >= 0)
                {
                    BlockDTO tempBlockDTO = inputBlockList[i].Clone();
                    tempBlockDTO.ActualLocatedWorkshopIndex = inputBlockList[i].ActualLocatedWorkshopIndex;
                    tempBlockDTO.IsFinished = inputBlockList[i].IsFinished;
                    tempBlockDTO.IsLocated = inputBlockList[i].IsLocated;
                    tempBlockDTO.LocatedRow = inputBlockList[i].LocatedRow;
                    tempBlockDTO.LocatedColumn = inputBlockList[i].LocatedColumn;
                    LocatedBlockList.Add(tempBlockDTO);

                }
            }

            //배치된 블록에 대해 작업장 grid로 배치현황을 표현
            for (int i = 0; i < LocatedBlockList.Count; i++)
            {
                if (LocatedBlockList[i].IsLocated == true || LocatedBlockList[i].IsFinished == true)
                {
                    int Locatedworkshop = LocatedBlockList[i].ActualLocatedWorkshopIndex;
                    for (int j = 0; j < Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].RowCount)); j++)
                    {
                        for (int k = 0; k < Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].ColumnCount)); k++)
                        {
                            ResultArrangementedWorkShop[Locatedworkshop][Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].LocatedRow)) + j, Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].LocatedColumn)) + k].BlockIndex = i;
                            ResultArrangementedWorkShop[Locatedworkshop][Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].LocatedRow)) + j, Convert.ToInt16(Math.Ceiling(LocatedBlockList[i].LocatedColumn)) + k].IsOccupied = true;
                        }
                    }
                }
            }

            return ResultArrangementedWorkShop;
        }

        /// <summary>
        /// 특정 2차원 배열을 오른쪽으로 90도 회전시키는 함수
        /// </summary>
        /// <param name="inputMartix"></param>
        /// <returns></returns>
        double[,] MatrixRightRotate(double[,] inputMartix)
        {
            int LengthX = inputMartix.GetLength(1);
            int LengthY = inputMartix.GetLength(0);

            double[,] ResultMaritx = new double[LengthX, LengthY];

            for (int y = 0; y < LengthY; y++)
            {
                for (int x = 0; x < LengthX; x++)
                {
                    ResultMaritx[x, y] = inputMartix[LengthY - 1 - y, x];
                }
            }

            return ResultMaritx;
        }

        /// <summary>
        /// 특정 2차원 배열을 오른쪽으로 N회 회전시키는 함수
        /// </summary>
        /// <param name="inputMatrix"></param>
        /// <param name="RotateCount"></param>
        /// <returns></returns>
        double[,] MatrixRightRotate(double[,] inputMatrix, int RotateCount)
        {
            double[,] ResultMatrix = new double[inputMatrix.GetLength(0), inputMatrix.GetLength(1)];

            ResultMatrix = inputMatrix;

            for (int i = 0; i < RotateCount; i++)
            {
                ResultMatrix = this.MatrixRightRotate(ResultMatrix);
            }

            return ResultMatrix;
        }





        /// <summary>
        /// 현재 작업장 배치 현황에서 주판의 형상을 고려한 BLF 배치 위치를 찾는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <param name="TargetPlateConfig"></param>
        /// <returns>배치될 주판의 바운딩박스 시작 위치 (행,렬), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] DeterminePlateLocationWithConfig(UnitcellDTO[,] CurrentWorkShopMatrix, double[,] TargetPlateConfig)
        {
            int[] ResultRowCol = new int[2];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;

            int WorkshopRow = CurrentWorkShopMatrix.GetLength(0);
            int WorkshopCol = CurrentWorkShopMatrix.GetLength(1);

            //배치 대상 주판의 가로 세로 바운딩 박스의 길이/
            int rowCount = TargetPlateConfig.GetLength(0);
            int colCount = TargetPlateConfig.GetLength(1);

            bool IsLocated = false; //주판의 배치 유무

            for (int column = 0; column < WorkshopCol; column++)
            {
                for (int row = 0; row < WorkshopRow; row++)
                {
                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    for (int PlateColumn = 0; PlateColumn < colCount; PlateColumn++)
                    {
                        for (int PlateRow = 0; PlateRow < rowCount; PlateRow++)
                        {
                            //config 형상이 존재하는 위치에 대해서만, 점유 정보를 확인함
                            if (TargetPlateConfig[PlateRow, PlateColumn] == 1) // 사각형 안에 포함되지만 형상이 존재하는 부분은 점유함
                            {
                                if (row + PlateRow < WorkshopRow && column + PlateColumn < WorkshopCol)
                                {
                                    IsOccupied = IsOccupied || CurrentWorkShopMatrix[row + PlateRow, column + PlateColumn].IsOccupied;
                                    if (IsOccupied) break;
                                }
                                else
                                {
                                    IsOccupied = true;
                                    if (IsOccupied) break;
                                }
                            }
                        }
                        if (IsOccupied) break;
                    }
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) break;
                }
                if (IsLocated) break;
            }

            return ResultRowCol;
        }

        /// <summary>
        /// "Roller 용"  현재 작업장 배치 현황에서 주판의 형상을 고려한 BLF 배치 위치를 찾는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <param name="TargetPlateConfig"></param>
        /// <returns>배치될 주판의 바운딩박스 시작 위치 (행,렬), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] Roller_DeterminePlateLocationWithConfig(UnitcellDTO[,] CurrentWorkShopMatrix, double[,] TargetPlateConfig, int WidthOfAddress)
        {
            int[] ResultRowCol = new int[2];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;

            int WorkshopRow = CurrentWorkShopMatrix.GetLength(0);
            int WorkshopCol = WidthOfAddress;

            //배치 대상 주판의 가로 세로 바운딩 박스의 길이/
            int rowCount = TargetPlateConfig.GetLength(0);
            int colCount = TargetPlateConfig.GetLength(1);

            bool IsLocated = false; //주판의 배치 유무

            for (int column = 0; column < WorkshopCol; column++)
            {
                for (int row = 0; row < WorkshopRow; row++)
                {
                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    for (int PlateColumn = 0; PlateColumn < colCount; PlateColumn++)
                    {
                        for (int PlateRow = 0; PlateRow < rowCount; PlateRow++)
                        {
                            //config 형상이 존재하는 위치에 대해서만, 점유 정보를 확인함
                            if (TargetPlateConfig[PlateRow, PlateColumn] == 1) // 사각형 안에 포함되지만 형상이 존재하는 부분은 점유함
                            {
                                if (row + PlateRow < WorkshopRow && column + PlateColumn < WorkshopCol)
                                {
                                    IsOccupied = IsOccupied || CurrentWorkShopMatrix[row + PlateRow, column + PlateColumn].IsOccupied;
                                    if (IsOccupied) break;
                                }
                                else
                                {
                                    IsOccupied = true;
                                    if (IsOccupied) break;
                                }
                            }
                        }
                        if (IsOccupied) break;
                    }
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) break;
                }
                if (IsLocated) break;
            }

            return ResultRowCol;
        }

        #endregion

        #region 탐색, 배치 실행 기능 함수

        /// <summary>
        /// 현재 작업장 배치 현황에서 BLF 배치 위치를 찾는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <returns>배치될 블록의 바운딩박스 시작 위치 (행,열), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] DetermineBlockLocation(Int32[,] CurrentArrangementMatrix, double _BlockRowCount, double _BlockColumnCount, int Orientation)
        {
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }
            // Orientation == -1 일 때, 회전시켜서 비교 후 좋은 점 반환하는 코드 작성해야함

            int[] ResultRowCol = new int[3];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;
            ResultRowCol[2] = Orientation;

            int WorkshopRow = CurrentArrangementMatrix.GetLength(0);
            int WorkshopCol = CurrentArrangementMatrix.GetLength(1);

            bool IsLocated = false; //블록의 배치 유무

            for (int column = 0; column < WorkshopCol - BlockColumnCount; column++)
            {
                for (int row = 0; row < WorkshopRow - BlockRowCount; row++)
                {
                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    // 네 꼭짓점 먼저 탐색
                    if (CurrentArrangementMatrix[row, column] == 0 & CurrentArrangementMatrix[row + BlockRowCount - 1, column] == 0 & CurrentArrangementMatrix[row, column + BlockColumnCount - 1] == 0 & CurrentArrangementMatrix[row + BlockRowCount, column + BlockColumnCount] == 0)
                    {
                        // 네 꼭짓점 모두 비어있다면, 내부 탐색
                        for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                        {
                            for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                            {
                                if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                                {
                                    IsOccupied = true;
                                }
                                if (IsOccupied) break;
                            }
                            if (IsOccupied) break;
                        }
                    } // 네 꼭짓점 중 비어있지 않은 곳이 있다면
                    else IsOccupied = true;
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) break;
                }
                if (IsLocated) break;
            }

            return ResultRowCol;
        }

        /// <summary>
        /// 현재 작업장 배치 현황 중 선호지번 구역에서 BLF 배치 위치를 찾는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <returns>배치될 블록의 바운딩박스 시작 위치 (행,열), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] DetermineBlockLocationWithAddress(Int32[,] CurrentArrangementMatrix, double _BlockRowCount, double _BlockColumnCount, double _AddressLeftPoint, double _AddressRightPoint, int Orientation)
        {
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }
            // Orientation == -1 일 때, 회전시켜서 비교 후 좋은 점 반환하는 코드 작성해야함


            int AddressLeftPoint = Convert.ToInt32(Math.Ceiling(_AddressLeftPoint));
            int AddressRightPoint = Convert.ToInt32(Math.Ceiling(_AddressRightPoint));

            int[] ResultRowCol = new int[3];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;
            ResultRowCol[2] = Orientation;

            int WorkshopRow = CurrentArrangementMatrix.GetLength(0);
            int WorkshopCol = CurrentArrangementMatrix.GetLength(1);

            bool IsLocated = false; //블록의 배치 유무

            for (int column = AddressLeftPoint; column < AddressRightPoint - BlockColumnCount; column++)
            {
                for (int row = 0; row < WorkshopRow - BlockRowCount; row++)
                {
                    bool IsOccupied = false; // 작업장 셀의 점유유무
                    // 네 꼭짓점 먼저 탐색
                    if (CurrentArrangementMatrix[row, column] == 0 & CurrentArrangementMatrix[row + BlockRowCount - 1, column] == 0 & CurrentArrangementMatrix[row, column + BlockColumnCount - 1] == 0 & CurrentArrangementMatrix[row + BlockRowCount, column + BlockColumnCount] == 0)
                    {
                        // 네 꼭짓점 모두 비어있다면, 내부 탐색
                        for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                        {
                            for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                            {
                                if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                                {
                                    IsOccupied = true;
                                }
                                if (IsOccupied) break;
                            }
                            if (IsOccupied) break;
                        }
                    } // 네 꼭짓점 중 비어있지 않은 곳이 있다면
                    else IsOccupied = true;
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) break;
                }
                if (IsLocated) break;
            }

            return ResultRowCol;
        }

        /// <summary>
        /// 현재 작업장 배치 현황에서 도로변에 BLF 배치 위치를 찾는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <returns>배치될 블록의 바운딩박스 시작 위치 (행,열), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] DetermineBlockLocationOnRoadSide(Int32[,] CurrentArrangementMatrix, double _BlockRowCount, double _BlockColumnCount, int Orientation, WorkshopDTO _WorkshopInfo)
        {
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }
            // Orientation == -1 일 때, 회전시켜서 비교 후 좋은 점 반환하는 코드 작성해야함

            int[] ResultRowCol = new int[3];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;
            ResultRowCol[2] = Orientation;

            int WorkshopRow = CurrentArrangementMatrix.GetLength(0);
            int WorkshopCol = CurrentArrangementMatrix.GetLength(1);
            bool IsLocated = false; //블록의 배치 유무

            // 모서리를 돌아가면서 탐색, 방향은 좌측 -> 하단 -> 우측 -> 상단
            // Left side 탐색
            if (_WorkshopInfo.LeftRoadside != null)
            {
                for (int row = Convert.ToInt32(Math.Ceiling(_WorkshopInfo.LeftRoadside[0])); row < Convert.ToInt32(Math.Ceiling(_WorkshopInfo.LeftRoadside[1])) - BlockRowCount ; row++)
                {
                    int column = 0;
                    if (column < 0) break;

                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                    {
                        for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                        {
                            if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                            {
                                IsOccupied = true;
                            }
                            if (IsOccupied) break;
                        }
                        if (IsOccupied) break;
                    }
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) return ResultRowCol;
                }
            }

            // Bottom side 탐색
            if (_WorkshopInfo.BottomRoadside != null)
            {
                for (int column = Convert.ToInt32(Math.Ceiling(_WorkshopInfo.BottomRoadside[0])); column < Convert.ToInt32(Math.Ceiling(_WorkshopInfo.BottomRoadside[1])) - BlockColumnCount; column++)
                {
                    int row = WorkshopRow - BlockRowCount;
                    if (row < 0) break;

                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                    {
                        for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                        {
                            if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                            {
                                IsOccupied = true;
                            }
                            if (IsOccupied) break;
                        }
                        if (IsOccupied) break;
                    }
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) return ResultRowCol;
                }
            }

            // Right side 탐색
            if (_WorkshopInfo.RightRoadside != null)
            {
                for (int row = Convert.ToInt32(Math.Ceiling(_WorkshopInfo.RightRoadside[0])); row < Convert.ToInt32(Math.Ceiling(_WorkshopInfo.RightRoadside[1])) - BlockRowCount; row++)
                {
                    int column = WorkshopCol - BlockColumnCount;
                    if (column < 0) break;

                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                    {
                        for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                        {
                            if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                            {
                                IsOccupied = true;
                            }
                            if (IsOccupied) break;
                        }
                        if (IsOccupied) break;
                    }
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) return ResultRowCol;
                }
            }

            // Upper side 탐색
            if (_WorkshopInfo.UpperRoadside != null)
            {
                for (int column = Convert.ToInt32(Math.Ceiling(_WorkshopInfo.UpperRoadside[0])); column < Convert.ToInt32(Math.Ceiling(_WorkshopInfo.UpperRoadside[1])) - BlockColumnCount; column++)
                {
                    int row = 0;

                    bool IsOccupied = false; // 작업장 셀의 점유유무

                    // 네 꼭짓점 먼저 탐색
                    if (CurrentArrangementMatrix[row, column] == 0 & CurrentArrangementMatrix[row + BlockRowCount - 1, column] == 0 & CurrentArrangementMatrix[row, column + BlockColumnCount - 1] == 0 & CurrentArrangementMatrix[row + BlockRowCount, column + BlockColumnCount] == 0)
                    {
                        // 네 꼭짓점 모두 비어있다면, 내부 탐색
                        for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                        {
                            for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                            {
                                if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                                {
                                    IsOccupied = true;
                                }
                                if (IsOccupied) break;
                            }
                            if (IsOccupied) break;
                        }
                    } // 네 꼭짓점 중 비어있지 않은 곳이 있다면
                    else IsOccupied = true;
                    //만약에 IsOccupied가 false라서 배치가 가능하다면,
                    if (!IsOccupied)
                    {
                        IsLocated = true;
                        ResultRowCol[0] = row;
                        ResultRowCol[1] = column;
                    }
                    if (IsLocated) return ResultRowCol;
                }
            }
            
            // 어느 곳도 배치를 못했다면
            return ResultRowCol;
        }

        /// <summary>
        /// 현재 작업장 배치 현황에서 BLF 배치 위치를 찾는 함수, 탐색 방향 명시 기능 추가
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <returns>배치될 블록의 바운딩박스 시작 위치 (행,열), 둘다 -1, -1 이면 배치 불가능</returns>
        int[] DetermineBlockLocationWithSearchDirection(Int32[,] CurrentArrangementMatrix, double _BlockRowCount, double _BlockColumnCount, int Orientation, int SearchDirection)
        {
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }
            // Orientation == -1 일 때, 회전시켜서 비교 후 좋은 점 반환하는 코드 작성해야함

            int[] ResultRowCol = new int[3];
            ResultRowCol[0] = -1;
            ResultRowCol[1] = -1;
            ResultRowCol[2] = Orientation;

            int WorkshopRow = CurrentArrangementMatrix.GetLength(0);
            int WorkshopCol = CurrentArrangementMatrix.GetLength(1);

            bool IsLocated = false; //블록의 배치 유무

            //좌->우 탐색
            if (SearchDirection == 1)
            {
                for (int column = 0; column < WorkshopCol - BlockColumnCount; column++)
                {
                    for (int row = 0; row < WorkshopRow - BlockRowCount; row++)
                    {
                        bool IsOccupied = false; // 작업장 셀의 점유유무

                        // 네 꼭짓점 먼저 탐색
                        if (CurrentArrangementMatrix[row, column] == 0 & CurrentArrangementMatrix[row + BlockRowCount - 1, column] == 0 & CurrentArrangementMatrix[row, column + BlockColumnCount - 1] == 0 & CurrentArrangementMatrix[row + BlockRowCount, column + BlockColumnCount] == 0)
                        {
                            // 네 꼭짓점 모두 비어있다면, 내부 탐색
                            for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                            {
                                for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                                {
                                    if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                                    {
                                        IsOccupied = true;
                                    }
                                    if (IsOccupied) break;
                                }
                                if (IsOccupied) break;
                            }
                        } // 네 꼭짓점 중 비어있지 않은 곳이 있다면
                        else IsOccupied = true;
                        //만약에 IsOccupied가 false라서 배치가 가능하다면,
                        if (!IsOccupied)
                        {
                            IsLocated = true;
                            ResultRowCol[0] = row;
                            ResultRowCol[1] = column;
                        }
                        if (IsLocated) break;
                    }
                    if (IsLocated) break;
                }
            }
            //우->좌 탐색
            else
            {
                for (int column = WorkshopCol - BlockColumnCount-1; column >=0 ; column--)
                {
                    for (int row = 0; row < WorkshopRow - BlockRowCount; row++)
                    {
                        bool IsOccupied = false; // 작업장 셀의 점유유무

                        // 네 꼭짓점 먼저 탐색
                        if (CurrentArrangementMatrix[row, column] == 0 & CurrentArrangementMatrix[row + BlockRowCount - 1, column] == 0 & CurrentArrangementMatrix[row, column + BlockColumnCount - 1] == 0 & CurrentArrangementMatrix[row + BlockRowCount, column + BlockColumnCount] == 0)
                        {
                            // 네 꼭짓점 모두 비어있다면, 내부 탐색
                            for (int BlockColumn = 0; BlockColumn < BlockColumnCount; BlockColumn++)
                            {
                                for (int BlockRow = 0; BlockRow < BlockRowCount; BlockRow++)
                                {
                                    if (CurrentArrangementMatrix[row + BlockRow, column + BlockColumn] != 0)
                                    {
                                        IsOccupied = true;
                                    }
                                    if (IsOccupied) break;
                                }
                                if (IsOccupied) break;
                            }
                        } // 네 꼭짓점 중 비어있지 않은 곳이 있다면
                        else IsOccupied = true;
                        //만약에 IsOccupied가 false라서 배치가 가능하다면,
                        if (!IsOccupied)
                        {
                            IsLocated = true;
                            ResultRowCol[0] = row;
                            ResultRowCol[1] = column;
                        }
                        if (IsLocated) break;
                    }
                    if (IsLocated) break;
                }
            }

            return ResultRowCol;
        }

        /// <summary>
        /// 배치 매트릭스, 블록 위치, 가로 및 세로 길이, 방향을 입력받고 배치 매트릭스에 배치 정보를 반영하는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <param name="TargetPlateConfig"></param>
        /// <returns>입력 블록이 배치된 배치 매트릭스 - 0이면 빈 그리드, 1이면 배치된 그리드</returns>
        Int32[,] PutBlockOnMatrix(Int32[,] CurrentArrangementMatrix, double _RowLocation, double _ColumnLocation, double _BlockRowCount, double _BlockColumnCount, int Orientation)
        {
            int RowLocation = Convert.ToInt32(Math.Ceiling(_RowLocation));
            int ColumnLocation = Convert.ToInt32(Math.Ceiling(_ColumnLocation));
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }

            for (int i = 0; i < BlockRowCount; i++)
            {
                for (int j = 0; j < BlockColumnCount; j++)
                {
                    CurrentArrangementMatrix[RowLocation + i, ColumnLocation + j] = 1;
                }
            }

            return CurrentArrangementMatrix;
        }

        /// <summary>
        /// 제거 대상 매트릭스, 블록 위치, 가로 및 세로 길이, 방향을 입력받고 배치 매트릭스에서 블록 배치 정보를 제거하는 함수
        /// </summary>
        /// <param name="CurrentWorkShopMatrix"></param>
        /// <param name="TargetPlateConfig"></param>
        /// <returns>입력 블록이 배치된 배치 매트릭스 - 0이면 빈 그리드, 1이면 배치된 그리드</returns>
        Int32[,] RemoveBlockFromMatrix(Int32[,] CurrentArrangementMatrix, double _RowLocation, double _ColumnLocation, double _BlockRowCount, double _BlockColumnCount, int Orientation)
        {
            int RowLocation = Convert.ToInt32(Math.Ceiling(_RowLocation));
            int ColumnLocation = Convert.ToInt32(Math.Ceiling(_ColumnLocation));
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }

            for (int i = 0; i < BlockRowCount; i++)
            {
                for (int j = 0; j < BlockColumnCount; j++)
                {
                    CurrentArrangementMatrix[RowLocation + i, ColumnLocation + j] = 0;
                }
            }

            return CurrentArrangementMatrix;
        }


        // 매트릭스 복사
        Int32[,] CloneMatrix(Int32[,] ArrangementMatrix)
        {
            Int32[,] tempMatrix = new Int32[ArrangementMatrix.GetLength(0), ArrangementMatrix.GetLength(1)];

            for (int i=0; i < ArrangementMatrix.GetLength(0); i++)
            {
                for (int j=0; j < ArrangementMatrix.GetLength(1); j++)
                {
                    tempMatrix[i, j] = ArrangementMatrix[i, j];
                }
            }

            return tempMatrix;
        }

        // 매트릭스와 블록 위치, 크기, 방향을 입력받았을 때 간섭하는지 체크하는 함수
        // 간섭 안하면 true, 간섭하면 false 반환
        bool CheckInterference(Int32[,] CurrentArrangementMatrix, double _RowLocation, double _ColumnLocation, double _BlockRowCount, double _BlockColumnCount, int Orientation)
        {
            int RowLocation = Convert.ToInt32(Math.Ceiling(_RowLocation));
            int ColumnLocation = Convert.ToInt32(Math.Ceiling(_ColumnLocation));
            int BlockRowCount = 0;
            int BlockColumnCount = 0;
            if (Orientation == 0)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
            }
            else if (Orientation == 1)
            {
                BlockRowCount = Convert.ToInt32(Math.Ceiling(_BlockColumnCount));
                BlockColumnCount = Convert.ToInt32(Math.Ceiling(_BlockRowCount));
            }

            for (int i = 0; i < BlockRowCount; i++)
            {
                for (int j = 0; j < BlockColumnCount; j++)
                {
                    if(CurrentArrangementMatrix[RowLocation + i, ColumnLocation + j] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        #endregion

    }
}

