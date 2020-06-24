using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eoba.Shipyard.ArrangementSimulator.DataTransferObject;

namespace Eoba.Shipyard.ArrangementSimulator.BusinessComponent.Interface
{
    public interface IDataManagement
    {
        //작업장 정보, 블록 정보 불러오기 
        void PrintWorkshopDataOnGrid(DataGridView _datagrid, List<WorkshopDTO> _workshopList);
        void PrintBlockDataOnGrid(DataGridView _datagrid, List<BlockDTO> _blockList);
        void PrintPlateDataOnFrid(DataGridView _datagrid, List<PlateConfigDTO> _plaeList);
        List<BlockDTO> RefineInputBlockData(List<string[]> _inputStringList, List<WorkshopDTO> _workshopList, double _UnitGridLength);

        //작업장 정보, 블록 정보 갱신하기
        void UpdateWorkshopDataOfGrid(DataGridView _datagrid, List<WorkshopDTO> _workshopList);
        void UpdateBlockDataOfGrid(DataGridView _datagrid, List<BlockDTO> _blockList);
    }
}
