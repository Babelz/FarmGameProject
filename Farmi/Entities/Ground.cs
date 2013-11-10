using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public class Ground
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

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch, CropSpot owner)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) owner.Position.X, (int) owner.Position.Y, 32, 32), Color.Peru );
            if (Seed == null)
                return;
            Seed.DrawToInventory(spriteBatch, owner.Position, new Size(16,16));
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
                State = GroundState.Planted;
            }

        }

        #endregion
    }
}
