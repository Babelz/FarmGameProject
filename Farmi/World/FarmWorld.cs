using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Engine;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.World
{
    public sealed class FarmWorld : Khv.Game.World
    {
        #region Properties

        public FarmPlayer Player
        {
            get;
            private set;
        }


        #endregion

        public FarmWorld(KhvGame game) : base(game)
        {

        }

        public override void Initialize()
        {
            MapManager = new MapManager(Game, Path.Combine("cfg", "mengine.cfg"));
            WorldObjects = new GameObjectManager(null);
            Player = new FarmPlayer(Game, this, PlayerIndex.One);
            Player.Initialize();
            WorldObjects.AddGameObject(Player);

            MapManager.ChangeMap("farm");
        }

        public override void Update(GameTime gameTime)
        {
            if (MapManager.ActiveMap != null)
            {
                MapManager.ActiveMap.Update(gameTime);
            }

            var gameobjects = WorldObjects.AllObjects();
            foreach (var gameobject in gameobjects)
            {
                gameobject.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (MapManager.ActiveMap != null)
            {
                MapManager.ActiveMap.Draw(spriteBatch);
            }
            var gameobjects = WorldObjects.GameObjectsOfType<DrawableGameObject>(g => g is DrawableGameObject);
            foreach (var gameobject in gameobjects)
            {
               gameobject.Draw(spriteBatch);
            }
        }
    }
}
