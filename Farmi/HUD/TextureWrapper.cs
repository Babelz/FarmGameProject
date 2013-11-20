using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components.BaseComponents;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components;
using Microsoft.Xna.Framework;

namespace Farmi.HUD
{
    public sealed class TextureWrapper : Control
    {
        #region Vars
        private Texture2D currentTexture;
        #endregion

        public TextureWrapper()
        {
            Colors = new Colors()
            {
                Foreground = Color.White,
                Background = Color.White
            };
        }

        public void ChangeTexture(Texture2D texture)
        {
            currentTexture = texture;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (currentTexture != null)
            {
                Rectangle rectangle = new Rectangle((int)Position.Real.X, (int)Position.Real.Y,
                    size.Width, size.Height);

                spriteBatch.Draw(currentTexture, rectangle, Colors.Background);
            }
        }
    }
}
