using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    /// 수정 일자 : 이동건, 2016년 2월 19일
    public class BlockDTO
    {
        private List<int> mPreferAddressIndexList = new List<int>();
        private List<int> mPreferWorkShopIndexList = new List<int>();

        public int Index { get; set; }
        public string Project { get; set; }
        public string Name { get; set; }
        public double RowCount { get; set; }
        public double ColumnCount { get; set; }
        public double UpperSideCount { get; set; }
        public double BottomSideCount { get; set; }
        public double LeftSideCount { get; set; }
        public double RightSideCount { get; set; }
        public int Orientation { get; set; }
        

        public double Leadtime { get; set; }
        public DateTime InitialImportDate { get; set; }
        public DateTime InitialExportDate { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime ExportDate { get; set; }
        public DateTime ActualImportDate { get; set; }
        public DateTime ActualExportDate { get; set; }
        public List<int> PreferWorkShopIndexList
        {
            get
            {
                List<int> _getter = null;
                if (mPreferWorkShopIndexList != null)
                {
                    _getter = new List<int>();
                    for (int i = 0; i < mPreferWorkShopIndexList.Count; i++) _getter.Add(mPreferWorkShopIndexList[i]);
                }
                return _getter;
            }
            set
            {
                if (value != null)
                {
                    mPreferWorkShopIndexList = new List<int>();
                    for (int i = 0; i < value.Count; i++) mPreferWorkShopIndexList.Add(value[i]);
                }
            }
        }
        public List<int> PreferAddressIndexList
        {
            get
            {
                List<int> _getter = null;
                if (mPreferAddressIndexList != null)
                {
                    _getter = new List<int>();
                    for (int i = 0; i < mPreferAddressIndexList.Count; i++) _getter.Add(mPreferAddressIndexList[i]);
                }
                return _getter;
            }
            set
            {
                if (value != null)
                {
                    mPreferAddressIndexList = new List<int>();
                    for (int i = 0; i < value.Count; i++) mPreferAddressIndexList.Add(value[i]);
                }
            }
        }

        public double LocatedRow { get; set; }
        public double LocatedColumn { get; set; }
        public int LocatedWorkshopIndex { get; set; }
        public int CurrentLocatedWorkshopIndex { get; set; }
        public int CurrentLocatedAddressIndex { get; set; }
        public int ActualLocatedWorkshopIndex { get; set; }
        public int ActualLocatedAddressIndex { get; set; }

        public bool IsLocated { get; set; }
        public bool IsDelayed { get; set; }
        public int DelayedTime { get; set; }
        public bool IsRoadSide { get; set; }
        public bool IsConditionSatisfied { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPrior { get; set; }
        
        public int SearchDirection { get; set; }
        public int ArrangementDirection { get; set; }
        

        public BlockDTO(int _index, string _project, string _name, double _rowCount, double _colCount, double _uppCount, double _botCount, double _lefCount, double _rigCount, List<int> _preferWorkShopIndexList, List<int> _preferAddressIndexList, double _leadtime, DateTime _ImportDate, DateTime _ExportDate, DateTime _actualImporteDate, DateTime _actualExportDate)
        {
            Index = _index;
            Project = _project;
            Name = _name;
            RowCount = _rowCount;
            ColumnCount = _colCount;
            UpperSideCount = _uppCount;
            BottomSideCount = _botCount;
            LeftSideCount = _lefCount;
            RightSideCount = _rigCount;
            Leadtime = _leadtime;
            InitialImportDate = _ImportDate;
            InitialExportDate = _ExportDate;
            ImportDate = _ImportDate;
            ExportDate = _ExportDate;
            LocatedRow = -1;
            LocatedColumn = -1;
            CurrentLocatedWorkshopIndex = -1;
            CurrentLocatedAddressIndex = -1;
            ActualLocatedWorkshopIndex = -1;
            ActualLocatedAddressIndex = -1;
            ActualImportDate = _actualImporteDate;
            ActualExportDate = _actualExportDate;
           
            for (int i = 0; i < _preferWorkShopIndexList.Count; i++) mPreferWorkShopIndexList.Add(_preferWorkShopIndexList[i]);
            for (int i = 0; i < _preferAddressIndexList.Count; i++) mPreferAddressIndexList.Add(_preferAddressIndexList[i]);
            IsLocated = false;
            IsFinished = false;
            IsDelayed = false;
            DelayedTime = 0;
            IsRoadSide = false;
            IsConditionSatisfied = false;
        }
        
        //최종
        public BlockDTO(int _index, string _project, string _name, double _rowCount, double _colCount, double _uppCount, double _botCount, double _lefCount, double _rigCount, List<int> _preferWorkShopIndexList, List<int> _preferAddressIndexList, double _leadtime, DateTime _ImportDate, DateTime _ExportDate, DateTime _actualImporteDate, DateTime _actualExportDate, bool _IsRoadSide, bool _IsPrior, int _SearchDirection, int _ArrangementDirection)
        {
            Index = _index;
            Project = _project;
            Name = _name;
            RowCount = _rowCount;
            ColumnCount = _colCount;
            UpperSideCount = _uppCount;
            BottomSideCount = _botCount;
            LeftSideCount = _lefCount;
            RightSideCount = _rigCount;
            Leadtime = _leadtime;
            InitialImportDate = _ImportDate;
            InitialExportDate = _ExportDate;
            ImportDate = _ImportDate;
            ExportDate = _ExportDate;
            LocatedRow = -1;
            LocatedColumn = -1;
            CurrentLocatedWorkshopIndex = -1;
            CurrentLocatedAddressIndex = -1;
            ActualLocatedWorkshopIndex = -1;
            ActualLocatedAddressIndex = -1;
            ActualImportDate = _actualImporteDate;
            ActualExportDate = _actualExportDate;

            for (int i = 0; i < _preferWorkShopIndexList.Count; i++) mPreferWorkShopIndexList.Add(_preferWorkShopIndexList[i]);
            for (int i = 0; i < _preferAddressIndexList.Count; i++) mPreferAddressIndexList.Add(_preferAddressIndexList[i]);
            IsLocated = false;
            IsFinished = false;
            IsDelayed = false;
            DelayedTime = 0;
            IsRoadSide = _IsRoadSide;
            IsConditionSatisfied = _IsRoadSide;
            IsPrior = _IsPrior;
            SearchDirection = _SearchDirection;
            ArrangementDirection = _ArrangementDirection;
        }
        //public BlockDTO(int _index, string _project, string _name, double _rowCount, double _colCount, double _uppCount, double _botCount, double _lefCount, double _rigCount, List<int> _preferWorkShopIndexList, List<int> _preferAddressIndexList, double _leadtime, DateTime _ImportDate, DateTime _ExportDate, DateTime _actualImporteDate, DateTime _actualExportDate, double _LocatedRow, double _LocatedColumn, bool _IsLocated)
        //{
        //    Index = _index;
        //    Project = _project;
        //    Name = _name;
        //    RowCount = _rowCount;
        //    ColumnCount = _colCount;
        //    UpperSideCount = _uppCount;
        //    BottomSideCount = _botCount;
        //    LeftSideCount = _lefCount;
        //    RightSideCount = _rigCount;
        //    Leadtime = _leadtime;
        //    InitialImportDate = _ImportDate;
        //    InitialExportDate = _ExportDate;
        //    ImportDate = _ImportDate;
        //    ExportDate = _ExportDate;
        //    LocatedRow = _LocatedRow;
        //    LocatedColumn = _LocatedColumn;
        //    CurrentLocatedWorkshopIndex = -1;
        //    CurrentLocatedAddressIndex = -1;
        //    ActualLocatedWorkshopIndex = -1;
        //    ActualLocatedAddressIndex = -1;
        //    ActualImportDate = _actualImporteDate;
        //    ActualExportDate = _actualExportDate;

        //    for (int i = 0; i < _preferWorkShopIndexList.Count; i++) mPreferWorkShopIndexList.Add(_preferWorkShopIndexList[i]);
        //    for (int i = 0; i < _preferAddressIndexList.Count; i++) mPreferAddressIndexList.Add(_preferAddressIndexList[i]);
        //    IsLocated = _IsLocated;
        //    IsFinished = false;
        //    IsDelayed = false;
        //    DelayedTime = 0;
        //    IsRoadSide = false;
        //    IsConditionSatisfied = false;
        //}
        public BlockDTO(int _index, string _project, string _name, double _rowCount, double _colCount, List<int> _preferWorkShopIndexList, List<int> _preferAddressIndexList, double _leadtime, DateTime _ImportDate, DateTime _ExportDate, DateTime _actualImporteDate, DateTime _actualExportDate)
        {
            Index = _index;
            Project = _project;
            Name = _name;
            RowCount = _rowCount;
            ColumnCount = _colCount;
            Leadtime = _leadtime;
            InitialImportDate = _ImportDate;
            InitialExportDate = _ExportDate;
            ImportDate = _ImportDate;
            ExportDate = _ExportDate;
            LocatedRow = -1;
            LocatedColumn = -1;
            CurrentLocatedWorkshopIndex = -1;
            CurrentLocatedAddressIndex = -1;
            ActualLocatedWorkshopIndex = -1;
            ActualLocatedAddressIndex = -1;
            ActualImportDate = _actualImporteDate;
            ActualExportDate = _actualExportDate;

            for (int i = 0; i < _preferWorkShopIndexList.Count; i++) mPreferWorkShopIndexList.Add(_preferWorkShopIndexList[i]);
            for (int i = 0; i < _preferAddressIndexList.Count; i++) mPreferAddressIndexList.Add(_preferAddressIndexList[i]);
            IsLocated = false;
            IsFinished = false;
            IsDelayed = false;
            DelayedTime = 0;
            IsRoadSide = false;
            IsConditionSatisfied = false;
        }
        // IsRoadSide 추가
        public BlockDTO(int _index, string _project, string _name, double _rowCount, double _colCount, List<int> _preferWorkShopIndexList, List<int> _preferAddressIndexList, double _leadtime, DateTime _ImportDate, DateTime _ExportDate, DateTime _actualImporteDate, DateTime _actualExportDate, bool _IsRoadSide)
        {
            Index = _index;
            Project = _project;
            Name = _name;
            RowCount = _rowCount;
            ColumnCount = _colCount;
            Leadtime = _leadtime;
            InitialImportDate = _ImportDate;
            InitialExportDate = _ExportDate;
            ImportDate = _ImportDate;
            ExportDate = _ExportDate;
            LocatedRow = -1;
            LocatedColumn = -1;
            CurrentLocatedWorkshopIndex = -1;
            CurrentLocatedAddressIndex = -1;
            ActualLocatedWorkshopIndex = -1;
            ActualLocatedAddressIndex = -1;
            ActualImportDate = _actualImporteDate;
            ActualExportDate = _actualExportDate;


            for (int i = 0; i < _preferWorkShopIndexList.Count; i++) mPreferWorkShopIndexList.Add(_preferWorkShopIndexList[i]);
            for (int i = 0; i < _preferAddressIndexList.Count; i++) mPreferAddressIndexList.Add(_preferAddressIndexList[i]);
            IsLocated = false;
            IsFinished = false;
            IsDelayed = false;
            DelayedTime = 0;
            IsRoadSide = _IsRoadSide;
            IsConditionSatisfied = _IsRoadSide;
        }


        public BlockDTO Clone()
        {
            return new BlockDTO(Index, Project, Name, RowCount, ColumnCount, UpperSideCount, BottomSideCount, LeftSideCount, RightSideCount, PreferWorkShopIndexList, PreferAddressIndexList, Leadtime, ImportDate, ExportDate, ActualImportDate, ActualExportDate, IsRoadSide, IsPrior, SearchDirection, ArrangementDirection);
        }

        //public BlockDTO Clone(int isLocated)
        //{
        //    return new BlockDTO(Index, Project, Name, RowCount, ColumnCount, UpperSideCount, BottomSideCount, LeftSideCount, RightSideCount, PreferWorkShopIndexList, PreferAddressIndexList, Leadtime, ImportDate, ExportDate, ActualImportDate, ActualExportDate, LocatedRow, LocatedColumn, IsLocated);
        //}
    }
}
