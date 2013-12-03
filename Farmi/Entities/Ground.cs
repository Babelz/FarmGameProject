using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Components;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public sealed class Ground
    {
        #region Properties

        /// <summary>
        /// Maaperän state
        /// </summary>
        public GroundState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Siemen mikä maaperässä kasvaa, jos null
        /// ei kasva mitään
        /// </summary>
        public Seed Seed
        {
            get; 
            private set;
        }

        /// <summary>
        /// Kasvaako tässä maaperässä joku
        /// </summary>
        public bool IsOccupied
        {
            get
            {
                return Seed != null;
            }
        }

        #endregion

        public Ground()
        {
            State = GroundState.Hoed;
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (Seed == null)
                return;
            Seed.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, CropSpot owner)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) owner.Position.X, (int) owner.Position.Y, 32, 32), Color.Peru );
            if (Seed == null)
                return;
            //Seed.DrawToInventory(spriteBatch, owner.Position, new Size(16,16));
        }

        /// <summary>
        /// Kylvee siemenen maaperään. 
        /// Jos null, ei kasva mitään ja maaperän state on Hoed
        /// </summary>
        /// <param name="seed">Siemen joka istutetaan</param>
        public void Plant(Seed seed)
        {
            Seed = seed;

            if (seed == null)
            {
                State = GroundState.Hoed;
            }
            else
            {
                // jotta ei päivitetä turhaan playerille liittyviä komponentteja
                seed.Components.SafelyRemoveComponents<IObjectComponent>(
                    seed.Components.AllComponents
                    );
                // lisätään siemenelle olennaiset komponentit
                seed.Components.SafelyAddComponents<IUpdatableObjectComponent>(
                    new GrowComponent(seed)
                    );
                State = GroundState.Planted;
            }
        }

        #endregion
    }
}
