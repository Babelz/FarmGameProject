using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Buildings;
using Farmi.Entities.Components;
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
        #endregion

        #region Properties
        public Building OwningBuilding
        {
            get; 
            private set;
        }
        public Teleport Teleport
        {
            get;
            private set;
        }
        #endregion

        public Door(KhvGame game, Building owningBuilding, DoorDataset doorDataset, string mapContainedIn)
            : base(game)
        {
            OwningBuilding = owningBuilding;
            Position = owningBuilding.Position + doorDataset.Position;
            Size = doorDataset.Size;

            GameplayScreen screen = game.GameStateManager.Current as GameplayScreen;

            Teleport = new Teleport(game, doorDataset.TeleportDataset, mapContainedIn);
            Teleport.Position = position;

            Collider = new BoxCollider(null, this);
            DoorInteractionComponent c = new DoorInteractionComponent(this);

            // Joku tekstuuri ois kiva.
            texture = KhvGame.Temp;

            Components.Add(c);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height), Color.Black);
        }

    }
}
