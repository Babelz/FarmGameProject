using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Repositories;
using Khv.Engine;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;

namespace Farmi.Entities.Items
{
    public abstract class Item : DrawableGameObject
    {
        #region Properties
        public string Name
        {
            get;
            protected set;
        }
        public string Description
        {
            get;
            protected set;
        }
        public Texture2D Texture
        {
            get;
            set;
        }
        #endregion

        protected Item(KhvGame game, string name)
            : base(game)
        {
            Name = name;
        }
        public Item(KhvGame game)
            : this(game, "")
        {
        }

        public virtual void DrawToInventory(SpriteBatch spriteBatch, Vector2 position, Size size)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}
