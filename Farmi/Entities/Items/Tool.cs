using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Repositories;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa työkalua jota voi käyttää
    /// </summary>
    internal sealed class Tool : Item
    {
        public Tool(KhvGame game, ToolDataset dataset)
            : base(game)
        {
            MakeFromData(dataset);
        }

        private void MakeFromData(ToolDataset dataset)
        {
            
            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
    
        }

        public override void DrawToInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 position, Khv.Engine.Structs.Size size)
        {
            throw new NotImplementedException();
        }
    }
}
