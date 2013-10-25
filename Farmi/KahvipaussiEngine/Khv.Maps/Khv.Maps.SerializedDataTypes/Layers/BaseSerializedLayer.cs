using System;
using Khv.Maps.SerializedDataTypes.Layers.SerializedComponents;

namespace Khv.Maps.SerializedDataTypes.Layers
{
    [Serializable]
    public class BaseSerializedLayer
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Visible { get; set; }
        public bool Transparent { get; set; }
        public int DrawOrder { get; set; }
        public bool IsMdiLayer { get; set; }
        public string SheetPath { get; set; }
        public SerializedMdiData MdiData { get; set; }
    }
}
