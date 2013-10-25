using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Game.Collision
{
    public struct CollisionResult
    {
        public bool Intersecting { get;  set; }
        public bool WillIntersect { get;  set; }
        public Vector2 Translation { get;  set; }
    }
}
