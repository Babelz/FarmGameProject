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
using Khv.Maps.MapClasses.Managers;

namespace Farmi.Entities
{
    internal sealed class AnimalFeedDispenser : DrawableGameObject, ILoadableMapObject
    {
        #region Vars
        private FarmWorld world;
        private FeedDispinserInformer informer;
        private FeedDataset feedDataset;
        private string mapContainedIn;
        #endregion

        #region Properties
        public string FeedType
        {
            get;
            private set;
        }
        public bool HasFeed
        {
            get
            {
                return FeedContained > 0;
            }
        }
        public int FeedContained
        {
            get;
            private set;
        }
        #endregion

        public AnimalFeedDispenser(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            mapContainedIn = mapObjectArguments.MapContainedIn;
            InitializeFromMapData(mapObjectArguments);
        }

        protected override void OnDestroy()
        {
            world.MapManager.OnMapChanged -= MapManager_OnMapChanged;
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, Khv.Maps.MapClasses.Managers.MapEventArgs e)
        {
            if (e.Current.Name == mapContainedIn)
            {
                world.WorldObjects.SafelyAdd(informer);
            }
            else
            {
                world.WorldObjects.SafelyRemove(informer);
            }
        }
        #endregion

        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            size = new Size(32, 32);
            FeedType = reader.ReadFeedType();
            position = mapObjectArguments.Origin;

            informer = new FeedDispinserInformer(game, this);

            world = (game.GameStateManager.Current as GameplayScreen).World;
            world.MapManager.OnMapChanged += new MapEventHandler(MapManager_OnMapChanged);

            Components.Add(new FeedDispenserComponent(this));

            feedDataset = (game.Components.First(c => c is RepositoryManager)
                as RepositoryManager).GetDataSet<FeedDataset>(d => d.Type == FeedType);

            InsertFeed(100);
        }
        public AnimalFeedItem GetFeed()
        {
            AnimalFeedItem feed = null;

            if (FeedContained > 0)
            {
                feed = new AnimalFeedItem(game, feedDataset);
                FeedContained--;
            }

            return feed;
        }
        public void InsertFeed(int amount)
        {
            FeedContained += amount;
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
