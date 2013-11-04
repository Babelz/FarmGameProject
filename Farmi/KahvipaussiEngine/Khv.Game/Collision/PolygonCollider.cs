using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Khv.Game.Collision
{
    public abstract class PolygonCollider : Collider
    {
        protected PolygonCollider(GameObject gameObject)
        {
            Instance = gameObject;
        }

        #region Properties

        public Polygon Polygon
        {
            get;
            protected set;
        }


        public GameObject Instance
        {
            get;
            private set;
        }

        #endregion
    }
}
