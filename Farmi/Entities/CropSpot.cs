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
    internal sealed class CropSpot : DrawableGameObject
    {
        private DrawableGameObject plantedItem = null;
        public CropSpot(KhvGame game, MapObjectArguments args)
            : base(game)
        {
            Position = args.Origin;
        }
        public CropSpot(KhvGame game)
            : base(game)
        {
        }


        public void Plant(DrawableGameObject seed)
        {
            plantedItem = seed;
            size = seed.Size;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsOccupied)
                return;
            //TODO päivitä kasvamista

        }

        public bool IsOccupied
        {
            get { return plantedItem != null; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsOccupied) return;

            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height), Color.RoyalBlue );
        }
    }
}
