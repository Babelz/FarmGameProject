using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa työkalua jota voi käyttää
    /// </summary>
    internal sealed class Tool : Item
    {
        public Tool(KhvGame game)
            : base(game)
        {
        }

        public override void DrawToInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 position, Khv.Engine.Structs.Size size)
        {
            throw new NotImplementedException();
        }
    }
}
