using System;

namespace Khv.Maps.SerializedDataTypes.Tiles
{
    [Serializable]
    public class SerializedBaseTile : BaseSerializedTile
    {
        public int TextureIndexX { get; set; }
        public int TextureIndexY { get; set; }
    }
}
