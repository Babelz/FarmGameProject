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

namespace Farmi.Entities
{
    internal sealed class FeedingTray : GameObject, ILoadableMapObject
    {
        #region Vars
        private FeedDataset feedDataset;
        private FarmWorld world;

        private string mapContainedIn;
        private bool hasFeed;
        private int feedCount;
        #endregion

        #region Properties
        public string FeedType
        {
            get;
            private set;
        }
        #endregion

        public FeedingTray(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }

        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            world = (game.GameStateManager.Current as GameplayScreen).World;

            mapContainedIn = mapObjectArguments.MapContainedIn;
            size = reader.ReadSize();
            position = mapObjectArguments.Origin;
            FeedType = reader.ReadFeedType();

            Collider = new BoxCollider(world, this);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collider.Update(gameTime);
        }
    }
}
