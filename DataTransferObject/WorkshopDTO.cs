using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    public class WorkshopDTO
    {
        List<int> mLocatedBlockIndexList = new List<int>();
        List<int[]> mAddressInfoList = new List<int[]>();  //지번의 시작 위치와 행,열의 갯수를 나타내기 위한 리스트 int[4] = [지번 시작행, 지번 시작열,지번 행길이,지번 열길이]
        double mAreaUtilization = 0.0;
        int mNumOfLocatedBlocks = 0;

        public int Index { get; set; }
        public string Name { get; set; }
        public double RowCount { get; set; }
        public double ColumnCount { get; set; }
        public int NumOfAddress { get; set; }

        public double[] AddressColumnLocation { get; set; }
        public string RoadSide { get; set; }

        public List<int> LocatedBlockIndexList 
        {
            get
            {
                List<int> _getter = null;
                if (mLocatedBlockIndexList != null)
                {
                    _getter = new List<int>();
                    for (int i = 0; i < mLocatedBlockIndexList.Count; i++) _getter.Add(mLocatedBlockIndexList[i]);
                }
                return _getter;
            }
            set
            {
                if (value != null)
                {
                    mLocatedBlockIndexList = new List<int>();
                    for (int i = 0; i < value.Count; i++) mLocatedBlockIndexList.Add(value[i]);
                }
            }
        }
        public List<int[]> AddressInfoList
        {
            get
            {
                List<int[]> _getter = null;
                if (mAddressInfoList != null)
                {
                    _getter = new List<int[]>();
                    for (int i = 0; i < mAddressInfoList.Count; i++) _getter.Add(mAddressInfoList[i]);
                }
                return _getter;
            }
            set
            {
                if (value != null)
                {
                    mAddressInfoList = new List<int[]>();
                    for (int i = 0; i < value.Count; i++) mAddressInfoList.Add(value[i]);
                }
            }
        }
        public List<ArrangementMatrixInfoDTO> ArrangementMatrixInfoList = new List<ArrangementMatrixInfoDTO>();

        public double AreaUtilization
        {
            get { return mAreaUtilization; }
            set { mAreaUtilization = value ; }
        }
        public int NumOfLocatedBlocks
        {
            get { return mNumOfLocatedBlocks; }
            set { mNumOfLocatedBlocks = value; }
        }


        public WorkshopDTO(int _index, string _name, double _rowCount, double _colCount, int _numOfAddress)
        {
            Index = _index;
            Name = _name;
            RowCount = _rowCount;
            ColumnCount = _colCount;
            NumOfAddress = _numOfAddress;
            
            AddressColumnLocation = new double[NumOfAddress];
            //지번 자동 할당
            for (int i = 0; i < NumOfAddress; i++)
            {
                AddressColumnLocation[i] = (ColumnCount / NumOfAddress) * Convert.ToDouble(i);
            }
        }

        public WorkshopDTO()
        {
        }


        public WorkshopDTO Clone()
        {
            WorkshopDTO tempWorkshopDTO =  new WorkshopDTO(Index, Name, RowCount, ColumnCount, NumOfAddress);
            if (ArrangementMatrixInfoList.Count != 0) tempWorkshopDTO.ArrangementMatrixInfoList = ArrangementMatrixInfoList;
            return tempWorkshopDTO;
        }
        


    }

    public class ArrangementMatrixInfoDTO
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int WorkshopIndex { get; set; }
        public string WorkshopName { get; set; }
        public double RowLocation { get; set; }
        public double ColumnLocation { get; set; }
        public double RowCount { get; set; }
        public double ColumnCount { get; set; }
        public int type { get; set; }

        public ArrangementMatrixInfoDTO(int _Index, string _Name, int _WorkshopIndex, string _WorkshopName, double _RowLocation, double _ColumnLocation, double _RowCount, double _ColumnCount, int _type)
        {
            Index = _Index;
            Name = _Name;
            WorkshopIndex = _WorkshopIndex;
            WorkshopName = _WorkshopName;
            RowLocation = _RowLocation;
            ColumnLocation = _ColumnLocation;
            RowCount = _RowCount;
            ColumnCount = _ColumnCount;
            type = _type;
        }
    }
}
