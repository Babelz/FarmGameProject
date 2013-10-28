using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.World
{
    class FarmWorld : Khv.Game.World
    {
        public FarmWorld(KhvGame game) : base(game)
        {

        }

        public override void Initialize()
        {
            MapManager = new MapManager(Game, Path.Combine("cfg", "mengine.cfg"));
            WorldObjects = new GameObjectManager(null);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
