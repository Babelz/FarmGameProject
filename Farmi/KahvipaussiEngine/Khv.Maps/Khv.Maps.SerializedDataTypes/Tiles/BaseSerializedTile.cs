using System;

namespace Khv.Maps.SerializedDataTypes.Tiles
{
    [Serializable]
    public class BaseSerializedTile
    {
        public int PositionIndexX { get; set; }
        public int PositionIndexY { get; set; }
    }
}
