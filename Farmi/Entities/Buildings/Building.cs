using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Engine;
using SerializedDataTypes.MapObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Processors;

namespace Farmi.Entities.Buildings
{
    internal sealed class Building : DrawableGameObject
    {
        /// <summary>
        /// Muodostin kun ladataan suoraan kartasta olio.
        /// </summary>
        public Building(KhvGame game, MapObjectArguments args)
            : base(game)
        {
            // Pitäs ladata db:stä tietoja jo tässä

            TestInitialize(args);
        }
        /// <summary>
        /// Konsu kun luodaan olio suoraan lennosta.
        /// </summary>
        public Building(KhvGame game)
            : base(game)
        {
            // Pitäs ladata db:stä tietoja jo tässä

            TestInitialize(null);
        }

        // Testi metodi initille.
        private void TestInitialize(MapObjectArguments args)
        {
            size = new Size(192, 128);
            Collider = new BoxCollider(null, this);
            if (args == null)
            {
                position = Vector2.Zero;
            }
            else
            {
                position = args.Origin;
            }
        }

        public void InitializeFromData(string datasetName)
        {
            // Alustaa otuksen datasta tässä.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Collider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), Color.Brown);   
        }
    }
}
