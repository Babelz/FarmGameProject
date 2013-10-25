using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerializedDataTypes.Components;

namespace SerializedDataTypes.MapObjects
{
    [Serializable]
    public class SerializedMapObjectCollection : IDataCollection, ISheetObjectCollection
    {
        [XmlIgnore]
        public List<IObjectData> Objects
        {
            get
            {
                return new List<IObjectData>(mapObjects);
            }
        }
        public string Name
        {
            get;
            set;
        }
        [XmlArray("MapObjects")]
        [XmlArrayItem("MapObject")]
        public List<SerializedMapObject> mapObjects = new List<SerializedMapObject>();
    }
}
