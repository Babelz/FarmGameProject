using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Audio.OpenAL;

namespace Farmi.Entities.Components
{
    class ViewComponent : IObjectComponent
    {
        private Vector2 viewVector = Vector2.Zero;
        public Vector2 ViewVector
        {
            get { return viewVector;  }
            set
            {
                if (value.X < 0 && Default.X > 0)
                    Effects = SpriteEffects.FlipHorizontally;
                else if (value.X > 0 && Default.X < 0)
                    Effects = SpriteEffects.FlipHorizontally;
                else
                    Effects = SpriteEffects.None;
                if (value.Y < 0 && Default.Y > 0)
                    Effects = SpriteEffects.FlipVertically;
                else if (value.Y > 0 && Default.Y < 0)
                    Effects = SpriteEffects.FlipVertically;
                else
                    Effects = SpriteEffects.None;
            }
        }

        public Vector2 Default
        {
            get;
            private set;
        }

        public ViewComponent(Vector2 defaultView)
        {
            ViewVector = Vector2.Zero;
            Default = defaultView;
        }

        public void Update(GameTime gametime)
        {
 /*           if (ViewVector.X < 0 && Default.X > 0)
                Effects = SpriteEffects.FlipHorizontally;
            else if (ViewVector.X > 0 && Default.X < 0)
                Effects = SpriteEffects.FlipHorizontally;

            if (ViewVector.Y < 0 && Default.Y > 0)
                Effects = SpriteEffects.FlipVertically;
            else if (ViewVector.Y > 0 && Default.Y < 0)
                Effects = SpriteEffects.FlipVertically;*/
        }

        public SpriteEffects Effects { get; private set; }
    }
}
