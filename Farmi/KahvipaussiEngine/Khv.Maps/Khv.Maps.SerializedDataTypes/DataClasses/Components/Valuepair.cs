using System;

namespace SerializedDataTypes.Components
{
    [Serializable]
    public class Valuepair
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
        #endregion

        public void SetData(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
