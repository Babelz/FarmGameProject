using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Engine;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.World
{
    class FarmWorld : Khv.Game.World
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
            Player = new FarmPlayer(Game, PlayerIndex.One);
            Player.Initialize();
            WorldObjects.AddGameObject(Player);
        }

        public override void Update(GameTime gameTime)
        {
            var gameobjects = WorldObjects.AllObjects();
            foreach (var gameobject in gameobjects)
            {
                gameobject.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
