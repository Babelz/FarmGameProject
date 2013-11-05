using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Farmi.Entities.Components;
using System.IO;

namespace Farmi.Entities
{
    internal class FeedDispinserInformer : DrawableGameObject
    {
        #region Vars
        private readonly AnimalFeedDispenser owner;
        private readonly Texture2D texture;
        #endregion

        public FeedDispinserInformer(KhvGame game, AnimalFeedDispenser owner)
            : base(game)
        {
            this.owner = owner;

            size = new Size(32, 32);
            position = new Vector2(owner.Position.X + size.Width, owner.Position.Y);

            Components.Add(new DispenserInformerComponent(owner));

            texture = game.Content.Load<Texture2D>(Path.Combine("Entities", "info"));

            Components.Add(new DispenserInformerComponent(owner));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
