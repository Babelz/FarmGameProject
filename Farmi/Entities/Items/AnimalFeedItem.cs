using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;
using Farmi.Datasets;
using Khv.Engine;
using System.IO;

namespace Farmi.Entities.Items
{
    internal sealed class AnimalFeedItem : Item, ILoadableRepositoryObject<FeedDataset>
    {
        #region Vars
        public string Type
        {
            get;
            private set;
        }
        #endregion

        public AnimalFeedItem(KhvGame game, FeedDataset feedDataset)
            : base(game)
        {
            InitializeFromDataset(feedDataset);
        }

        public void InitializeFromDataset(FeedDataset dataset)
        {
            Name = dataset.Name;
            Type = dataset.Type;
            Description = dataset.Type;

            size = new Size(32, 32);

            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
        }
        public override void DrawToInventory(SpriteBatch spriteBatch, Vector2 position, Size size)
        {
            throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}
