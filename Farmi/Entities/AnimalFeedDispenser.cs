using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Farmi.XmlParsers;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Game.Collision;
using Farmi.Screens;
using Farmi.Entities.Components;
using Farmi.Repositories;
using Farmi.Datasets;
using Farmi.Entities.Items;

namespace Farmi.Entities
{
    internal sealed class AnimalFeedDispenser : DrawableGameObject, ILoadableMapObject
    {
        #region Vars
        private FeedDataset feedDataset;
        private int feedCount;
        #endregion

        #region Properties
        public int FeedContained
        {
            get;
            private set;
        }
        public string FeedType
        {
            get;
            private set;
        }
        public bool HasFeed
        {
            get
            {
                return feedCount > 0;
            }
        }
        #endregion

        public AnimalFeedDispenser(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }

        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            size = new Size(32, 32);
            FeedType = reader.ReadFeedType();
            position = mapObjectArguments.Origin;

            FarmWorld world = (game.GameStateManager.Current as GameplayScreen).World;

            Components.Add(new FeedDispenserComponent(this));

            feedDataset = (game.Components.First(c => c is RepositoryManager)
                as RepositoryManager).GetDataSet<FeedDataset>(d => d.Type == FeedType);

            InsertFeed(100);
        }

        public AnimalFeedItem GetFeed()
        {
            AnimalFeedItem feed = null;

            if (feedCount > 0)
            {
                feed = new AnimalFeedItem(game, feedDataset);
                feedCount--;
            }

            return feed;
        }
        public void InsertFeed(int amount)
        {
            feedCount += amount;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);

            spriteBatch.Draw(KhvGame.Temp, rectangle, Color.Black);
        }
    }
}
