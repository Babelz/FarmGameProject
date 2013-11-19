using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Engine.Structs;
using Farmi.XmlParsers;
using Khv.Game.Collision;
using Farmi.Screens;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Managers;
using Farmi.Entities.Items;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Entities.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    internal sealed class FeedingTray : DrawableGameObject, ILoadableMapObject
    {
        #region Vars
        private FarmWorld world;
        private AnimalFeedItem feed;

        private string mapContainedIn;
        #endregion

        #region Properties
        public string FeedType
        {
            get;
            private set;
        }
        public bool ContainsFeed
        {
            get
            {
                return feed != null;
            }
        }
        #endregion

        public FeedingTray(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }

        #region Initializers
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            world = (game.GameStateManager.Current as GameplayScreen).World;

            mapContainedIn = mapObjectArguments.MapContainedIn;
            size = reader.ReadSize();
            position = mapObjectArguments.Origin;
            FeedType = reader.ReadFeedType();

            Collider = new BoxCollider(world, this);

            Components.AddComponent(new FeedingTrayInteractionComponent(this));
        }
        #endregion

        public void InsertFeed(AnimalFeedItem feedItem)
        {
            feed = feedItem;
            feedItem.Size = new Size(feedItem.Size.Width, feedItem.Size.Height / 2);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collider.Update(gameTime);

            if (feed != null)
            {
                Vector2 feedPosition = new Vector2(this.position.X + feed.Size.Width / 2,
                                                   this.position.Y);
                feed.Position = feedPosition;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (feed != null)
            {
                feed.Draw(spriteBatch);
            }
        }
    }
}
