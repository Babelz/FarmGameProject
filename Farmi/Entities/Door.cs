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

namespace Farmi.Entities
{
    class Door : DrawableGameObject
    {
        public Building Building
        {
            get; 
            private set;
        }
        private Texture2D texture;

        public Door(KhvGame game, Building building, DoorDataset dataset) : base(game)
        {
            Building = building;
            Position = building.Position + dataset.Position;
            Size = dataset.Size;
            Collider = new BoxCollider(null, this);
            DoorInteractionComponent c = new DoorInteractionComponent();
            c.OnInteraction += () => Console.WriteLine("OnInteraction");
            c.OnInteractionBegin += () => Console.WriteLine("OnInteractionBegin");
            c.OnInteractionFinished += () => Console.WriteLine("OnInteractionFinished");
            Components.Add(c);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height), Color.Black);
        }

    }
}
