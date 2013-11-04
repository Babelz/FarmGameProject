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

namespace Farmi.Entities
{
    internal sealed class FeedingTray : GameObject, ILoadableMapObject
    {
        #region Vars
        private FarmWorld world;
        private string mapContainedIn;
        private bool hasFeed;
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
                return hasFeed;
            }
        }
        #endregion

        public FeedingTray(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }

        protected override void OnDestroy()
        {
            world.MapManager.OnMapChanged -= MapManager_OnMapChanged;
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, MapEventArgs e)
        {
            if(e.Current.Name == mapContainedIn)
            {
                world.WorldObjects.SafelyAdd(this);
            }
            else 
            {
                world.WorldObjects.SafelyRemove(this);
            }
        }
        #endregion

        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            world = (game.GameStateManager.Current as GameplayScreen).World;
            world.MapManager.OnMapChanged += new MapEventHandler(MapManager_OnMapChanged);

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
