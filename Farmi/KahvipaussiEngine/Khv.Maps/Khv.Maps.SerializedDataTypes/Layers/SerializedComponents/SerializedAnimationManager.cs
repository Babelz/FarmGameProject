using System;
using System.Xml.Serialization;

namespace Khv.Maps.SerializedDataTypes.Layers.SerializedComponents
{
    [XmlRoot("Animation manager")]
    [Serializable]
    public class SerializedAnimationManager
    {
        public int FrameCount { get; set; }
        public double FrameTime { get; set; }
    }
}
