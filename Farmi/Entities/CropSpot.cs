using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public sealed class CropSpot : DrawableGameObject
    {
        #region Vars
        private DrawableGameObject plantedItem = null;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa maaperän.
        /// Jos null, tarkoittaa sitä, että
        /// alue on ns. vapaata markkinaa eikä siinä kasva mitään
        /// </summary>
        public Ground Ground
        {
            get;
            private set;
        }

        /// <summary>
        /// Cropspotin state
        /// </summary>
        public CropSpotState State
        {
            get;
            private set;
        }
        #endregion

        public CropSpot(KhvGame game, MapObjectArguments args)
            : base(game)
        {
            Position = args.Origin;
            State = CropSpotState.Dirt;
        }
        public CropSpot(KhvGame game)
            : base(game)
        {
            State = CropSpotState.Dirt;
        }

        /// <summary>
        /// Asettaa maaperän. Jos null, tarkoittaa sitä, että
        /// alue on ns. vapaata markkinaa eikä siinä kasva mitään
        /// </summary>
        /// <param name="ground"></param>
        public void SetGround(Ground ground)
        {
            Ground = ground;
            if (ground == null)
            {
                State = CropSpotState.Dirt;
            }
            else
            {
                State = CropSpotState.Grounded;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Ground == null) return;

            Ground.Draw(spriteBatch, this);
        }
    }
}
