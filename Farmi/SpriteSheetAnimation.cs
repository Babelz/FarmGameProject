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
        private readonly List<SpriteAnimationSet> sets;
        private int currentFrame;
        private int elapsed;
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
        public Texture2D Texture
        {
            get;
            private set;
        }
        #endregion

        public SpriteSheetAnimation(Texture2D texture)
        {
            Texture = texture;
            sets = new List<SpriteAnimationSet>();
        }
        public void ChangeSet(string setname)
        {
            Reset();

            CurrentSet = sets.Find(s => s.Setname == setname);
        }
        public void AddSets(IEnumerable<SpriteAnimationSet> sets)
        {
            this.sets.AddRange(sets);
        }
        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsed > CurrentSet.FrameTime)
            {
                NextFrame();

                elapsed = 0;
            }
        }
        public void NextFrame()
        {
            if (currentFrame < CurrentSet.Sources.Length - 1)
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
        public int FrameTime
        {
            get;
            set;
        }
        #endregion


        public SpriteAnimationSet(string setname, Size sourceSize, int frames, int y)
        {
            Setname = setname;

            Sources = new Rectangle[frames];

            for (int i = 0; i < Sources.Length; i++)
            {
                Sources[i] = new Rectangle(i * sourceSize.Width, y, 
                    sourceSize.Width, sourceSize.Height);
            }

            FrameTime = 0;
        }
    }
}
