using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Items;
using Khv.Engine;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public class Seed : Tool
    {
        public Seed(KhvGame game, ToolDataset dataset) : base(game, dataset)
        {
        }

#warning proto | temp
        public override void DrawToInventory(SpriteBatch spriteBatch, Vector2 position, Size size)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(KhvGame.Temp, rectangle, Color.Red);
        }
    }
}
