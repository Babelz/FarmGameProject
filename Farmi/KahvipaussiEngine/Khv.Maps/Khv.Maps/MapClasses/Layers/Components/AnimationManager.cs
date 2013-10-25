using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Maps.MapClasses.Layers.Components
{
    /// <summary>
    /// hoitaa animaatio layereiden tilejen
    /// animaatioiden päivitykset
    /// </summary>
    public class AnimationManager
    {
        #region Vars
        private readonly double frameTime;
        private double elapsedTime;

        private int frameCount;
        private int currentFrame;
        private int nextFrame;
        #endregion

        #region Properties
        public int NextFrame
        {
            get
            {
                return nextFrame;
            }
        }
        public int FrameCount
        {
            get
            {
                return frameCount;
            }
        }
        #endregion

        public AnimationManager(int frameCount, double frameTime)
        {
            this.frameCount = frameCount;
            this.frameTime = frameTime;
        }

        /// <summary>
        /// päivittää animaatio framea
        /// </summary>
        /// <param name="gameTime">xna peli aika</param>
        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= frameTime)
            {
                if (currentFrame == frameCount - 1)
                {
                    currentFrame = 0;
                    nextFrame = -(frameCount - 1);
                }
                else
                {
                    currentFrame++;
                    nextFrame = 1;
                }
                elapsedTime = 0;
            }
            else
            {
                nextFrame = 0;
            }
        }
    }
}
