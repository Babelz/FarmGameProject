using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    /// <summary>
    /// Kuvastaa siemenen komponenttia joka hoitaa sen kasvulogiikan
    /// Toisinsanoen, jos tätä komponenttia ei ole, ei siemen voi kasvaakkaan
    /// </summary>
    public class GrowComponent : IUpdatableObjectComponent
    {
        private readonly Seed owner;

        public GrowComponent(Seed owner)
        {
            this.owner = owner;
        }

        public void Update(GameTime gametime)
        {
            
        }
    }
}
