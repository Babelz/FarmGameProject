using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Buildings;
using Khv.Engine;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Farmi.Screens;

namespace Farmi.Entities
{
    class Door : DrawableGameObject
    {
        #region Vars
        private Texture2D texture;
        private Teleport teleport;
        #endregion

        #region Properties
        public Building OwningBuilding
        {
            get; 
            private set;
        }
        #endregion

        public Door(KhvGame game, Building owningBuilding, DoorDataset doorDataset)
            : base(game)
        {
            OwningBuilding = owningBuilding;
            Position = owningBuilding.Position + doorDataset.Position;
            Size = doorDataset.Size;

            GameplayScreen screen = game.GameStateManager.Current as GameplayScreen;
            teleport = new Teleport(game, doorDataset.TeleportDataset, screen.World.MapManager.ActiveMap.Name);

            Collider = new BoxCollider(null, this);
            Components.Add(new BasicInteractionComponent());
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height), Color.Black);
        }

    }
}
