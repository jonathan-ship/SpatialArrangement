using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    public class ArrangementResultWithDateDTO
    {

        //날짜별 작업장 정보의 저장을 위한 변수 (ex. 면적점유율)
        public List<List<WorkshopDTO>> ResultWorkShopInfo { get; set; }

        //블록정보 또는 주판정보을 위한 리스트
        public List<BlockDTO> BlockResultList { get; set; }
        

        //시작일 및 종료일을 나타내는 변수
        public DateTime ArrangementStartDate { get; set; }
        public DateTime ArrangementFinishDate { get; set; }


        //날짜별 투입 블록, 반출 블록, 미배치 블록 등 블록 목록을 위한 변수

        public List<List<BlockDTO>> TotalDailyArragnementedBlockList { get; set; }
        public List<List<BlockDTO>> TotalBlockImportLogList { get; set; }
        public List<List<BlockDTO>> TotalBlockExportLogList { get; set; }
        public List<List<BlockDTO>> TotalDelayedBlockList { get; set; }


        


        public ArrangementResultWithDateDTO(List<List<WorkshopDTO>> _resultWorkShopInfo, List<BlockDTO> _blockResultList, DateTime _arrangementStartDate, DateTime _arrangementFinishDate, List<List<BlockDTO>> _totalDailyArragnementedBlockList, List<List<BlockDTO>> _totalBlockImportLogList, List<List<BlockDTO>> _totalBlockExportLogList, List<List<BlockDTO>> _totalDelayedBlockList)
        {
            ResultWorkShopInfo = _resultWorkShopInfo;
            BlockResultList = _blockResultList;
            TotalDailyArragnementedBlockList = _totalDailyArragnementedBlockList;
            TotalBlockImportLogList = _totalBlockImportLogList;
            TotalBlockExportLogList = _totalBlockExportLogList;
            TotalDelayedBlockList = _totalDelayedBlockList;
            ArrangementStartDate = _arrangementStartDate;
            ArrangementFinishDate = _arrangementFinishDate;
        }


        



        public ArrangementResultWithDateDTO Clone()
        {
            return new ArrangementResultWithDateDTO(ResultWorkShopInfo, BlockResultList, ArrangementStartDate, ArrangementFinishDate, TotalDailyArragnementedBlockList, TotalBlockImportLogList, TotalBlockExportLogList, TotalDelayedBlockList);
        }


        public ArrangementResultWithDateDTO()
        {
        }
  
    }
}
