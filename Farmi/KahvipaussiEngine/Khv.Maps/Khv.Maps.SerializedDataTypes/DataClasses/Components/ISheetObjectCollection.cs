using System.Collections.Generic;

namespace SerializedDataTypes.Components
{
    public interface ISheetObjectCollection
    {
        List<IObjectData> Objects
        {
            get;
        }
    }
}
