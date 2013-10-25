using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapClasses.Layers.Tiles.Interfaces
{
    /// <summary>
    /// luokka jonka objektin omavaata tilet perivät
    /// </summary>
    public interface IObjectTile
    {
        // kirjoitetaan asettamaan olio
        void SetObject(object mapObject);
    }
}
