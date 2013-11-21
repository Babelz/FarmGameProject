using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Khv.Maps.MapClasses.Processors;

namespace Farmi.Entities
{
    public interface ISavable
    {
        void Import(IDataset dataset);
        IDataset Export();
    }
    public interface ILoadableRepositoryObject<T> where T : IDataset
    {
        void InitializeFromDataset(T dataset);
    }
    public interface ILoadableMapObject
    {
        void InitializeFromMapData(MapObjectArguments mapObjectArguments);
    }
}
