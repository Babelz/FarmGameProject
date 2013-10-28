using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.State;
using Microsoft.Xna.Framework;

namespace Farmi.Screens
{
    class StartScreen : GameState
    {
        public override void Draw()
        {
            SpriteBatch.GraphicsDevice.Clear(Color.Yellow);
        }
    }
}
