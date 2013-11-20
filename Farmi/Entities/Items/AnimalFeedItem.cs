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
    public sealed class AnimalFeedItem : Item, ILoadableRepositoryObject<FeedDataset>
    {
        #region Properties
        public FeedDataset Dataset
        {
            get;
            private set;
        }
        public string FeedType
        {
            get;
            private set;
        }
        #endregion

        public AnimalFeedItem(KhvGame game, FeedDataset feedDataset)
            : base(game)
        {
            Dataset = feedDataset;

            InitializeFromDataset(feedDataset);
        }

        #region Initializers
        public void InitializeFromDataset(FeedDataset dataset)
        {
            Name = dataset.Name;
            FeedType = dataset.Type;
            Description = dataset.Type;

            size = new Size(32, 32);

            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}
