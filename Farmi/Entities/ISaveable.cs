using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Khv.Maps.MapClasses.Processors;

namespace Farmi.Entities
{
    internal interface ISaveable
    {
        void Import(IDataset dataset);
        IDataset Export();
    }
    internal interface ILoadableRepositoryObject<T> where T : IDataset
    {
        void InitializeFromDataset(T dataset);
    }
    internal interface ILoadableMapObject
    {
        void InitializeFromMapData(MapObjectArguments mapObjectArguments);
    }
}
