using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    public class PlateConfigDTO
    {
        private double[,] mPlateConfig = null;
        private List<int> mPreferAddressIndexList = new List<int>();
        private List<int> mPreferWorkShopIndexList = new List<int>();
        

        public string Name { get; set; }
        public DateTime InitialImportDate { get; set; } // 최초 투입 예정일
        public DateTime InitialExportDate { get; set; } // 최초 종료 예정일
        public DateTime ActualImportDate { get; set; } // 실제 투입일
        public DateTime ActualExportDate { get; set; } // 실제 종료일
        public DateTime PlanImportDate { get; set; } //(알고리즘 내에서) 현재 예정된 투입일
        public DateTime PlanExportDate { get; set; } //(알고리즘 내에서) 현재 예정된 종료일
        public bool IsDelayed { get; set; }
        public bool IsLocated { get; set; }
        public bool IsFinished { get; set; }
        public double DelayedDays { get; set; }
        public double LeadTime { get; set; }
        //Period 추가
        public int Period { get; set; }

        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public int CurrentLocatedWorkshopIndex { get; set; }
        public int ActualLocatedWorkshopIndex { get; set; }
        public int CurrentLocatedAddressIndex { get; set; }

        public int LocatedRow { get; set; }
        public int LocateCol { get; set; }

        public double[,] PlateConfig //비어있는곳은 0, 차있으면 1
        {
            get
            {
                double[,] _getter = null;
                if (mPlateConfig != null)
                {
                    _getter = (double[,])mPlateConfig.Clone();
                }
                return _getter;
            }
            set
            {
                if (value != null)
                {
                    double[,] _setter = (double[,])value.Clone();
                    mPlateConfig = _setter;
                }
            }
        }

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

        public PlateConfigDTO()
        {
            Name = "";
            InitialImportDate = DateTime.MaxValue;
            InitialExportDate = DateTime.MaxValue;
            ActualImportDate = DateTime.MaxValue;
            ActualExportDate = DateTime.MaxValue;
            PlanImportDate = DateTime.MaxValue;
            PlanExportDate = DateTime.MaxValue;
            IsDelayed = false;
            IsLocated = false;
            IsFinished = false;
            DelayedDays = 0;
            LeadTime = 0;
            RowCount = 0;
            ColCount = 0;

            mPreferAddressIndexList = new List<int>();
            mPreferWorkShopIndexList = new List<int>();
        }

        public PlateConfigDTO(string PlateName, int ConfigRow, int ConfigCol)
        {
            Name = PlateName;
            RowCount = ConfigRow;
            ColCount = ConfigCol;

            InitialImportDate = DateTime.MaxValue;
            InitialExportDate = DateTime.MaxValue;
            ActualImportDate = DateTime.MaxValue;
            ActualExportDate = DateTime.MaxValue;
            PlanImportDate = DateTime.MaxValue;
            PlanExportDate = DateTime.MaxValue;
            IsFinished = false;
            IsDelayed = false;
            IsLocated = false;
            DelayedDays = 0;
            LeadTime = 0;
            mPreferAddressIndexList = new List<int>();
            mPreferWorkShopIndexList = new List<int>();
        }

        public PlateConfigDTO Clone()
        {
            PlateConfigDTO ResultDTO = new PlateConfigDTO();

            ResultDTO.Name = this.Name;
            ResultDTO.RowCount = this.RowCount;
            ResultDTO.ColCount = this.ColCount;
            ResultDTO.IsDelayed = this.IsDelayed;
            ResultDTO.IsLocated = this.IsLocated;
            ResultDTO.IsFinished = this.IsFinished;
            ResultDTO.DelayedDays = this.DelayedDays;
            ResultDTO.LeadTime = this.LeadTime;

            //Period 추가
            ResultDTO.Period = this.Period;

            ResultDTO.CurrentLocatedWorkshopIndex = this.CurrentLocatedWorkshopIndex;
            ResultDTO.ActualLocatedWorkshopIndex = this.ActualLocatedWorkshopIndex;
            
            ResultDTO.InitialImportDate = this.InitialImportDate;
            ResultDTO.InitialExportDate = this.InitialExportDate;
            ResultDTO.ActualImportDate = this.ActualImportDate;
            ResultDTO.ActualExportDate = this.ActualExportDate;
            ResultDTO.PlanImportDate = this.PlanImportDate;
            ResultDTO.PlanExportDate = this.PlanExportDate;

            ResultDTO.PlateConfig = this.PlateConfig;

            for (int i = 0; i < this.PreferWorkShopIndexList.Count; i++) ResultDTO.PreferWorkShopIndexList.Add(this.PreferWorkShopIndexList[i]);
            for (int i = 0; i < this.PreferAddressIndexList.Count; i++) ResultDTO.PreferAddressIndexList.Add(this.PreferAddressIndexList[i]);

            return ResultDTO;
        }
    }

}
