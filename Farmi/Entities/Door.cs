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
    public sealed class Door : DrawableGameObject, ILoadableRepositoryObject<DoorDataset>
    {
        #region Vars
        private Texture2D texture;
        private string mapContainedIn;
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
            this.mapContainedIn = mapContainedIn;
            OwningBuilding = owningBuilding;

            InitializeFromDataset(doorDataset);

            GameplayScreen screen = game.GameStateManager.Current as GameplayScreen;

            Collider = new BoxCollider(null, this);
            DoorInteractionComponent doorInteractionComponent = new DoorInteractionComponent(this);
            Components.AddComponent(doorInteractionComponent);

            texture = KhvGame.Temp;
        }

        #region Initializers
        public void InitializeFromDataset(DoorDataset dataset)
        {
            position = OwningBuilding.Position + dataset.Position;
            size = dataset.Size;

            Teleport = new Teleport(game, dataset.TeleportDataset, mapContainedIn);
            Teleport.Position = position;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), Color.Black);
        }
    }
}
