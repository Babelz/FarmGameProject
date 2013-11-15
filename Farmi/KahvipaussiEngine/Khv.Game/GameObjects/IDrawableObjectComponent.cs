using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Khv.Maps.MapClasses.Layers.Components;

namespace Khv.Game.GameObjects
{
    public interface IDrawableObjectComponent : IUpdatableObjectComponent
    {
        #region Properties
        int DrawOrder
        {
            get;
            set;
        }
        #endregion

        void Draw(SpriteBatch spriteBatch);
    }
}
