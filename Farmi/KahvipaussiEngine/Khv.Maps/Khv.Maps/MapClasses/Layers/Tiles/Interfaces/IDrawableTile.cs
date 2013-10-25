using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Engine.Structs;

namespace Khv.Maps.MapClasses.Layers.Tiles.Interfaces
{
    /// <summary>
    /// interface jonka tilet jotka voi piirtää perivät
    /// </summary>
    public interface IDrawableTile
    {
        #region Properties
        Index TextureIndex
        {
            get;
            set;
        }
        #endregion

        // kirjoitetaan piirtämään tile
        void Draw(SpriteBatch spriteBatch);
    }
}
