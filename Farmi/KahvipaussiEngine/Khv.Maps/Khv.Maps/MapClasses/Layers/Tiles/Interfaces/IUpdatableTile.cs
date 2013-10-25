using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Maps.MapClasses.Layers.Tiles.Interfaces
{
    /// <summary>
    /// interface jonka tilet joita voidaan päivittää perivät
    /// </summary>
    public interface IUpdatableTile
    {
        // kirjoitetaan päivittämään tile
        void Update(GameTime gameTime);
    }
}
