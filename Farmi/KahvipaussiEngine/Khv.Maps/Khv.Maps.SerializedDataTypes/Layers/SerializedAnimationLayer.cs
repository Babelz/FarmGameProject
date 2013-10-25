using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Khv.Maps.SerializedDataTypes.Layers.SerializedComponents;
using Khv.Maps.SerializedDataTypes.Tiles;

namespace Khv.Maps.SerializedDataTypes.Layers
{
    [XmlRoot("Animation layer")]
    [Serializable]
    public class SerializedAnimationLayer : BaseSerializedLayer
    {
        public SerializedAnimationManager AnimationManager { get; set; }
        [XmlArray("Animation tiles")]
        public List<SerializedBaseTile> Tiles { get; set; }
    }
}
