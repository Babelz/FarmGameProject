using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KhvGame = Khv.Engine.KhvGame;
using Game = Microsoft.Xna.Framework.Game;

namespace Khv.Game.GameObjects
{
    public abstract class DrawableGameObject : GameObject
    {
        protected DrawableGameObject(KhvGame game)
            : base(game)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (IDrawableObjectComponent drawableComponent in Components.DrawableComponents())
            {
                drawableComponent.Draw(spriteBatch);
            }
        }
    }
}
