using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerializedDataTypes.Components;

namespace SerializedDataTypes.RuleObjects
{
    [Serializable]
    [XmlRoot("RuleDataCollection")]
    public class SerializedRuleDataCollection : IDataCollection, ISheetObjectCollection
    {
        [XmlIgnore]
        public List<IObjectData> Objects
        {
            get
            {
                return new List<IObjectData>(ruleObjects);
            }
        }
        public string Name
        {
            get;
            set;
        }
        [XmlArray("RulesData")]
        [XmlArrayItem("RuleData")]
        public List<SerializedRuleData> ruleObjects = new List<SerializedRuleData>();
    }
}
