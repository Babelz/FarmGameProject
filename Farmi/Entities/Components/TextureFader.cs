using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Components
{
    public sealed class TextureFader
    {
        #region Vars
        private readonly Texture2D textureToFade;
        private readonly int endAlpha;
        private readonly int alphaStep;
        private readonly int stepTime;

        private int elapsed;
        private int currentAlpha;
        #endregion

        #region Properties
        public bool IsFading
        {
            get
            {
                return currentAlpha > endAlpha;
            }
        }
        public Rectangle Destination
        {
            get;
            set;
        }
        #endregion

        public TextureFader(Texture2D textureToFade, int startAlpha, int endAlpha, int alphaStep, int stepTime)
        {
            this.textureToFade = textureToFade;
            this.endAlpha = endAlpha;
            this.alphaStep = alphaStep;
            this.stepTime = stepTime;

            currentAlpha = startAlpha;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFading)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > stepTime)
                {
                    currentAlpha -= alphaStep;
                    elapsed = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsFading)
            {
                spriteBatch.Draw(textureToFade, Destination, new Color(255, 255, 255, currentAlpha));
            }
        }
    }
}
