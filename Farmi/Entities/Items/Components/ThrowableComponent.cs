using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Items.Components
{
    /// <summary>
    /// Kuvaa komponenttia jonka omistajaa voi heittää
    /// </summary>
    internal sealed class ThrowableComponent : IUpdatableObjectComponent
    {
        #region Properties
        /// <summary>
        /// Kuka omistaa tämän komponentin
        /// </summary>
        public Item Item { get; private set; }
        #endregion
        public ThrowableComponent(Item item)
        {
            Item = item;
        }

        public void Update(GameTime gametime)
        {
            
        }
    }
}
