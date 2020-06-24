using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    public class ArrangementResultDTO
    {
        public List<UnitcellDTO[,]> ArrangementResultList { get; set; }
        public List<WorkshopDTO> ResultWorkShopInfo { get; set; }
        public List<BlockDTO> ResultBlockList { get; set; }

        //투입일을 고려한 날짜별 배치를 위한 변수
        public DateTime ArrangementDate { get; set; }
        public List<BlockDTO> ImportedBlockList { get; set; }
        public List<BlockDTO> ExportedBlockList { get; set; }



        public ArrangementResultDTO(List<UnitcellDTO[,]> _arrangementResult, List<WorkshopDTO> _resultWorkShopInfo,List<BlockDTO> _resultBlockList, DateTime _arrangementDate, List<BlockDTO> _importBlockList, List<BlockDTO> _exportBlockList)
        {
            ArrangementResultList = _arrangementResult;
            ResultWorkShopInfo = _resultWorkShopInfo;
            ResultBlockList = _resultBlockList;
            ArrangementDate = _arrangementDate;
            ImportedBlockList = _importBlockList;
            ExportedBlockList = _exportBlockList;
        }

        public ArrangementResultDTO()
        {
            
        }

        public ArrangementResultDTO Clone()
        {
            return new ArrangementResultDTO(ArrangementResultList, ResultWorkShopInfo, ResultBlockList,ArrangementDate,ImportedBlockList,ExportedBlockList);
        }
  
    }
}




             //List<UnitcellDTO[,]> CurrentArrangementMatrix = ArrangementMatrix;
             //List<BlockDTO> CurrentArrangementedBlockList = new List<BlockDTO>();
             //List<List<BlockDTO>> TotalDailyArragnementedBlockList = new List<List<BlockDTO>>();
             //List<List<UnitcellDTO[,]>> TotalArrangementResult = new List<List<UnitcellDTO[,]>>();
             //List<BlockDTO> BlockList = new List<BlockDTO>();
             //List<List<BlockDTO>> TotalBlockImportLogList = new List<List<BlockDTO>>();
             //List<List<BlockDTO>> TotalBlockExportLogList = new List<List<BlockDTO>>();
             //List<List<BlockDTO>> TotalNotArrangementBlockList = new List<List<BlockDTO>>();