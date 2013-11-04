using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine.Args;

namespace Khv.Game.Collision
{
    public class CollisionEventArgs : GameEventArgs
    {
        #region Properties
        public object CollidingObject
        {
            get;
            set;
        }
        public bool IsCanceled
        {
            get;
            set;
        }
        public bool Intersecting
        {
            get;
            set;
        }
        public bool WillIntersect
        {
            get;
            set;
        }
        public Vector2 Translation
        {
            get;
            set;
        }
        #endregion
    }
}
