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

namespace Farmi.Entities
{
    internal sealed class AnimalFeedDispenser : DrawableGameObject, ILoadableMapObject
    {
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
