using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eoba.Shipyard.ArrangementSimulator.DataTransferObject
{
    public class UnitcellDTO
    {
        public int BlockIndex { get; set; }
        public bool IsOccupied { get; set; }
        public int Address { get; set; }
        public UnitcellDTO(int _blockIndex, bool _isOccupied, int _address)
        {
            BlockIndex = _blockIndex;
            IsOccupied = _isOccupied;
            Address = _address;
        }

        public UnitcellDTO Clone()
        {
            return new UnitcellDTO(BlockIndex, IsOccupied, Address);
        }
    }
}
