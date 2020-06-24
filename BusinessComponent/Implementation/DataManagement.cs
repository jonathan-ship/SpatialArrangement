using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface;
using System.Windows.Forms;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Implementation
{
    public class DataManagement : IDataManagement
    {
        /// <summary>
        /// 작업장 정보를 불러와 그리드에 출력하는 함수
        /// </summary>
        /// <param name="_datagrid">작업장 정보를 출력할 대상 그리드</param>
        /// <param name="_workshopList">작업장 정보</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// </remarks>
        /// 
        void IDataManagement.PrintWorkshopDataOnGrid(DataGridView _datagrid, List<WorkshopDTO> _workshopList)
        {

            _datagrid.Rows.Clear();
            for (int row = 0; row < _workshopList.Count; row++)
            {
                string[] temprow = { Convert.ToString(_workshopList[row].Index), 
                                       _workshopList[row].Name, 
                                           Convert.ToString(_workshopList[row].RowCount), 
                                           Convert.ToString(_workshopList[row].ColumnCount), 
                                           Convert.ToString(_workshopList[row].NumOfAddress) };
                _datagrid.Rows.Add(temprow);
            }
        }

        /// <summary>
        /// 작업장 그리드 정보가 갱신되면 이를 불러와 작업장 정보에 저장하는 함수
        /// </summary>
        /// <param name="_datagrid">작업장 정보가 변경된 그리드</param>
        /// <param name="_workshopList">갱신할 작업장 정보</param>
        /// <remarks>
        /// 최초 작성 : 이동건, 2016년 02월 25일
        /// </remarks>
        void IDataManagement.UpdateWorkshopDataOfGrid(DataGridView _datagrid, List<WorkshopDTO> _workshopList)
        {
            WorkshopDTO tempWorkshopDTO;
            _workshopList.Clear();
            foreach (DataGridViewRow row in _datagrid.Rows)
            {
                //for(int i=0; i < row.Cells.Count; i++)
                //{
                //    if (row.IsNewRow || row.Cells[i].Value == null)
                //        break;
                //                  }
                if (row.Cells[0].Value != null)
                {
                    string value1 = row.Cells[0].Value.ToString();
                    string value2 = row.Cells[1].Value.ToString();
                    string value3 = row.Cells[2].Value.ToString();
                    string value4 = row.Cells[3].Value.ToString();
                    string value5 = row.Cells[4].Value.ToString();

                    tempWorkshopDTO = new WorkshopDTO(Convert.ToInt16(value1), value2, Convert.ToDouble(value3), Convert.ToDouble(value4), Convert.ToInt32(value5));

                    _workshopList.Add(tempWorkshopDTO);
                }
            }

        }

        /// <summary>
        /// 블록 정보를 불러와 그리드에 출력하는 함수
        /// </summary>
        /// <param name="_datagrid">블록 정보를 출력할 대상 그리드</param>
        /// <param name="_blockList">블록 정보</param>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// 수정 작성 : 이동건, 2016년 02월 19일
        /// </remarks>
        void IDataManagement.PrintBlockDataOnGrid(DataGridView _datagrid, List<BlockDTO> _blockList)
        {
            _datagrid.Rows.Clear();
            _datagrid.Columns.Clear();

            _datagrid.ColumnCount = 14;
            _datagrid.Columns[0].Name = "Index";
            _datagrid.Columns[1].Name = "프로젝트번호";
            _datagrid.Columns[2].Name = "블록번호";
            _datagrid.Columns[3].Name = "세로";
            _datagrid.Columns[4].Name = "가로";
            _datagrid.Columns[5].Name = "상향작업공간";
            _datagrid.Columns[6].Name = "하향작업공간";
            _datagrid.Columns[7].Name = "좌측작업공간";
            _datagrid.Columns[8].Name = "우측작업공간";
            _datagrid.Columns[9].Name = "리드타임";
            _datagrid.Columns[10].Name = "투입날짜";
            _datagrid.Columns[11].Name = "반출날짜";
            _datagrid.Columns[12].Name = "선호 작업장";
            _datagrid.Columns[13].Name = "선호 지번";

            for (int row = 0; row < _blockList.Count; row++)
            {
                string strPreferedWorkShop = "";
                string strPreferedAddress = "";

                for (int i = 0; i < _blockList[row].PreferWorkShopIndexList.Count; i++)
                {
                    if (i != _blockList[row].PreferWorkShopIndexList.Count - 1)
                        strPreferedWorkShop = strPreferedWorkShop + Convert.ToString(_blockList[row].PreferWorkShopIndexList[i]) + ",";
                    else
                        strPreferedWorkShop = strPreferedWorkShop + Convert.ToString(_blockList[row].PreferWorkShopIndexList[i]);
                }

                for (int i = 0; i < _blockList[row].PreferAddressIndexList.Count; i++)
                {
                    if (i != _blockList[row].PreferAddressIndexList.Count - 1)
                        strPreferedAddress = strPreferedAddress + Convert.ToString(_blockList[row].PreferAddressIndexList[i]) + ",";
                    else
                        strPreferedAddress = strPreferedAddress + Convert.ToString(_blockList[row].PreferAddressIndexList[i]);
                }

                string[] temprow = { Convert.ToString(_blockList[row].Index), 
                                       _blockList[row].Project, 
                                       _blockList[row].Name, 
                                       Convert.ToString(_blockList[row].RowCount), 
                                       Convert.ToString(_blockList[row].ColumnCount), 
                                       Convert.ToString(_blockList[row].UpperSideCount),
                                       Convert.ToString(_blockList[row].BottomSideCount),
                                       Convert.ToString(_blockList[row].LeftSideCount),
                                       Convert.ToString(_blockList[row].RightSideCount),
                                       Convert.ToString(_blockList[row].Leadtime), 
                                       _blockList[row].ImportDate.ToShortDateString(), 
                                       _blockList[row].ExportDate.ToShortDateString(), 
                                       strPreferedWorkShop, 
                                       strPreferedAddress 
                                   };
                _datagrid.Rows.Add(temprow);
            }
        }

        /// <summary>
        /// 주판 정보를 불러와 그리드에 출력하는 함수
        /// </summary>
        /// <param name="_datagrid">주판 정보를 출력할 대상 그리드</param>
        /// <param name="_blockList">주판 정보</param>
        /// 최종수정 : 2019-09-28 (주수헌)
        void IDataManagement.PrintPlateDataOnFrid(DataGridView _datagrid, List<PlateConfigDTO> _plateList)
        {
            _datagrid.Rows.Clear();

            // 주판의 정보 입력을 위한 그리드 컬럼 변경
            _datagrid.Columns.Clear();

            _datagrid.ColumnCount = 8;
            _datagrid.Columns[0].Name = "주판번호";
            _datagrid.Columns[1].Name = "세로";
            _datagrid.Columns[2].Name = "가로";
            _datagrid.Columns[3].Name = "리드타임";
            _datagrid.Columns[4].Name = "투입날짜";
            _datagrid.Columns[5].Name = "반출날짜";
            _datagrid.Columns[6].Name = "선호 작업장";
            _datagrid.Columns[7].Name = "선호 지번";
 
            for (int row = 0; row < _plateList.Count; row++)
            {
                string strPreferedWorkShop = "";
                string strPreferedAddress = "";

                for (int i = 0; i < _plateList[row].PreferWorkShopIndexList.Count; i++)
                {
                    if (i != _plateList[row].PreferWorkShopIndexList.Count - 1)
                        strPreferedWorkShop = strPreferedWorkShop + Convert.ToString(_plateList[row].PreferWorkShopIndexList[i]) + ",";
                    else
                        strPreferedWorkShop = strPreferedWorkShop + Convert.ToString(_plateList[row].PreferWorkShopIndexList[i]);
                }

                for (int i = 0; i < _plateList[row].PreferAddressIndexList.Count; i++)
                {
                    if (i != _plateList[row].PreferAddressIndexList.Count - 1)
                        strPreferedAddress = strPreferedAddress + Convert.ToString(_plateList[row].PreferAddressIndexList[i]) + ",";
                    else
                        strPreferedAddress = strPreferedAddress + Convert.ToString(_plateList[row].PreferAddressIndexList[i]);
                }

                string[] temprow = {   _plateList[row].Name, 
                                       Convert.ToString(_plateList[row].RowCount), 
                                       Convert.ToString(_plateList[row].ColCount), 
                                       Convert.ToString(_plateList[row].LeadTime), 
                                       _plateList[row].PlanImportDate.ToShortDateString(), 
                                       _plateList[row].PlanExportDate.ToShortDateString(), 
                                       strPreferedWorkShop, 
                                       strPreferedAddress 
                                   };
                _datagrid.Rows.Add(temprow);
            }
        }



        /// <summary>
        /// 블록 그리드 정보가 갱신되면 이를 불러와 블록 정보에 저장하는 함수
        /// </summary>
        /// <param name="_datagrid">블록 정보가 변경된 그리드</param>
        /// <param name="_workshopList">갱신할 블록 정보</param>
        /// <remarks>
        /// 최초 작성 : 이동건, 2016년 02월 25일
        /// </remarks>
        void IDataManagement.UpdateBlockDataOfGrid(DataGridView _datagrid, List<BlockDTO> _blockList)
        {
            BlockDTO tempBlockDTO;
            _blockList.Clear();


            foreach (DataGridViewRow row in _datagrid.Rows)
            {
                //for (int i = 0; i < row.Cells.Count; i++)
                //{
                //    if (row.IsNewRow || row.Cells[i].Value == null)
                //        continue;
                //}

                if (row.Cells[0].Value != null)
                {

                    string value1 = row.Cells[0].Value.ToString();
                    string value2 = row.Cells[1].Value.ToString();
                    string value3 = row.Cells[2].Value.ToString();
                    string value4 = row.Cells[3].Value.ToString();
                    string value5 = row.Cells[4].Value.ToString();
                    string value6 = row.Cells[5].Value.ToString();
                    string value7 = row.Cells[6].Value.ToString();
                    string value8 = row.Cells[7].Value.ToString();
                    string value9 = row.Cells[8].Value.ToString();
                    string value10 = row.Cells[9].Value.ToString();
                    string value11 = row.Cells[10].Value.ToString();
                    string value12 = row.Cells[11].Value.ToString();
                    string value13 = row.Cells[12].Value.ToString();
                    string value14 = row.Cells[13].Value.ToString();

                    List<int> tempPreferWorkShop = new List<int>();
                    List<int> tempPreferAddress = new List<int>();

                    string[] PreferWorkshoptemp = value13.Split(',');
                    for (int j = 0; j < PreferWorkshoptemp.Length; j++) tempPreferWorkShop.Add(Convert.ToInt32(PreferWorkshoptemp[j]));

                    if (value14 != "")
                    {
                        string[] PreferAddresstemp = value14.Split(',');
                        for (int j = 0; j < PreferAddresstemp.Length; j++) tempPreferAddress.Add(Convert.ToInt32(PreferAddresstemp[j]));
                    }

                    tempBlockDTO = new BlockDTO(Convert.ToInt16(value1),
                            value2,
                            value3,
                            Convert.ToDouble(value4),
                            Convert.ToDouble(value5),
                            Convert.ToDouble(value6),
                            Convert.ToDouble(value7),
                            Convert.ToDouble(value8),
                            Convert.ToDouble(value9),
                            tempPreferWorkShop,
                            tempPreferAddress,
                            Convert.ToDouble(value10),
                            DateTime.Parse(value11),
                            DateTime.Parse(value12),
                            DateTime.MinValue,
                            DateTime.MinValue);

                    _blockList.Add(tempBlockDTO);
                }
            }

        }

        /// <summary>
        /// 입력한 블록 정보를 정제하는 함수
        /// </summary>
        /// <param name="_inputStringList">csv 파일로부터 읽어온 블록 정보 raw 데이터</param>
        /// <param name="_workshopList">작업장 정보</param>
        /// <returns>정제된 블록 정보</returns>
        /// <remarks>
        /// 최초 작성 : 정용국, 2016년 01월 20일
        /// 수정 작성 : 유상현, 2020년 05월 13일
        /// </remarks>
        List<BlockDTO> IDataManagement.RefineInputBlockData(List<string[]> _inputStringList, List<WorkshopDTO> _workshopList, double _UnitGridLength)
        {
            List<BlockDTO> returnList = new List<BlockDTO>();

            //작업영역 고려 여부 변수 추가
            //bool IsWorkAreaInfoConsidered = false;


            for (int i = 1; i < _inputStringList.Count; i++)
            {
                //double tempUpperCount;
                //double tempBotCount;
                //double tempLeftCount;
                //double tempRightCount;
                double tempLeadTime;
                double tempLength;
                double tempBreadth;
                DateTime tempImportDate;
                DateTime tempExportDate;

                List<int> tempPreferWorkShop = new List<int>();
                List<int> tempPreferAddress = new List<int>();

                bool PreferWorkShop = true;
                bool PreferAddress = true;
                bool IsRoadSide = false;

                BlockDTO tempBlockDTO;

                if (Convert.ToDouble(_inputStringList[i][3]) >= Convert.ToDouble(_inputStringList[i][4])) tempLength = Convert.ToDouble(_inputStringList[i][4]);
                else tempLength = Convert.ToDouble(_inputStringList[i][3]);

                if (Convert.ToDouble(_inputStringList[i][4]) < Convert.ToDouble(_inputStringList[i][3])) tempBreadth = Convert.ToDouble(_inputStringList[i][3]);
                else tempBreadth = Convert.ToDouble(_inputStringList[i][4]);

                tempLength /= _UnitGridLength;
                tempBreadth /= _UnitGridLength;

                //if (IsWorkAreaInfoConsidered)
                //{
                //    if (_inputStringList[i][5] == "") tempUpperCount = 1.0;
                //    else tempUpperCount = Convert.ToDouble(_inputStringList[i][5]);

                //    if (_inputStringList[i][6] == "") tempBotCount = 1.0;
                //    else tempBotCount = Convert.ToDouble(_inputStringList[i][6]);

                //    if (_inputStringList[i][7] == "") tempLeftCount = 1.0;
                //    else tempLeftCount = Convert.ToDouble(_inputStringList[i][7]);

                //    if (_inputStringList[i][8] == "") tempRightCount = 1.0;
                //    else tempRightCount = Convert.ToDouble(_inputStringList[i][8]);
                //}
                //else
                //{
                //    tempUpperCount = 0.0;
                //    tempBotCount = 0.0;
                //    tempLeftCount = 0.0;
                //    tempRightCount = 0.0;
                //}

                tempLeadTime = 0.0;
                //if (_inputStringList[i][9] == "") tempLeadTime = 0.0;
                //else tempLeadTime = Convert.ToDouble(_inputStringList[i][9]);

                if (_inputStringList[i][6] == "") tempImportDate = DateTime.Parse("0001-01-01");
                else tempImportDate = DateTime.Parse(_inputStringList[i][6]);

                if (_inputStringList[i][7] == "") tempExportDate = DateTime.Parse("0001-01-01");
                else tempExportDate = DateTime.Parse(_inputStringList[i][7]);

                //입고일과 출고일이 같은 데이터가 입력된 경우에는 출고일 +1
                //TimeSpan ts = tempExportDate - tempImportDate;
                //int tempTS = ts.Days;

                //if (tempTS == 0) tempExportDate = tempExportDate.AddDays(1);

                if (_inputStringList[i][8] == "")
                {
                    PreferWorkShop = false;
                    PreferAddress = false;
                }
                else if (_inputStringList[i][8] == "-1")
                {
                    PreferWorkShop = false;
                    IsRoadSide = true;

                }
                if (_inputStringList[i][9] == "") PreferAddress = false;

                if (PreferWorkShop == false) //선호작업장 값이 없는 경우
                {
                    //모든 작업장을 선호작업장으로 가짐
                    int totalWorkShop = _workshopList.Count;
                    for (int j = 0; j < totalWorkShop; j++) tempPreferWorkShop.Add(j);
                }
                else//선호작업장 값이 있는 경우
                {
                    string[] temp = _inputStringList[i][8].Split('-');
                    for (int j = 0; j < temp.Length; j++) tempPreferWorkShop.Add(Convert.ToInt32(temp[j]));
                    int totalWorkShop = _workshopList.Count;
                    for (int j = 0; j < totalWorkShop; j++) if(!tempPreferWorkShop.Contains(j)) tempPreferWorkShop.Add(j);
                }

                if (PreferAddress == false) //선호지번 값이 없는 경우
                {
                    //선호지번은 빈 리스트
                }
                else//선호지번 값이 있는 경우
                {
                    string[] temp = _inputStringList[i][9].Split('-');
                    for (int j = 0; j < temp.Length; j++) tempPreferAddress.Add(Convert.ToInt32(temp[j]));
                }


                tempBlockDTO = new BlockDTO(
                        Convert.ToInt16(_inputStringList[i][0]),
                        _inputStringList[i][1],
                        _inputStringList[i][2],
                        tempLength,
                        tempBreadth,
                        //tempUpperCount,
                        //tempBotCount,
                        //tempLeftCount,
                        //tempRightCount,

                        tempPreferWorkShop,
                        tempPreferAddress,
                        tempLeadTime,
                        tempImportDate,
                        tempExportDate,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        IsRoadSide);

                returnList.Add(tempBlockDTO);
                
            }

            return returnList;
        }

    }
}
