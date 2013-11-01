using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Structs;

namespace Farmi
{
    public sealed class SpriteSheetAnimation
    {
        #region Vars
        private readonly Texture2D texture;
        private readonly List<SpriteAnimationSet> sets;
        private int currentFrame;
        #endregion

        #region Properties
        public SpriteAnimationSet CurrentSet
        {
            get;
            private set;
        }
        public Rectangle CurrentSource
        {
            get
            {
                if (CurrentSet == null)
                {
                    return Rectangle.Empty;
                }
                else
                {
                    return CurrentSet.Sources[currentFrame];
                }
            }
        }
        #endregion

        public SpriteSheetAnimation(Texture2D texture)
        {
            this.texture = texture;
            sets = new List<SpriteAnimationSet>();
        }
        public void ChangeSet(string setname)
        {
            Reset();

            CurrentSet = sets.Find(s => s.Setname == setname);
        }
        public void NextFrame()
        {
            if (currentFrame < CurrentSet.Sources.Length)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = 0;
            }
        }
        public void Reset()
        {
            currentFrame = 0;
        }
    }
    public class SpriteAnimationSet
    {
        #region Properties
        public string Setname
        {
            get;
            private set;
        }        
        public Rectangle[] Sources
        {
            get;
            private set;
        }
        #endregion


        public SpriteAnimationSet(string setname, Size sourceSize, int frames, int y)
        {
            Sources = new Rectangle[sourceSize.Width * frames];

            for (int i = 0; i < Sources.Length; i++)
            {
                Sources[i] = new Rectangle(i * sourceSize.Width, y, 
                    sourceSize.Width, sourceSize.Height);
            }
        }
    }
}
