using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Args;

namespace Khv.Game.Collision
{
    public abstract class Collider
    {
        #region Events

        /// <summary>
        /// Kun törmäys tapahtuu
        /// </summary>
        public event CollisionEventHandler OnCollision;

        #endregion

        #region Virtuals

        /// <summary>
        /// Apuri että voidaan laukaista eventti yläluokista
        /// </summary>
        /// <param name="source">GameObject johon/joka törmäsi</param>
        protected virtual void FireOnCollision(GameObject me, object sender, CollisionEventArgs result)
        {
            if (me.Collider.OnCollision != null)
            {
                result.CollidingObject = sender;
                me.Collider.OnCollision(this, result);
            }
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Törmääkö johonkin
        /// </summary>
        /// <param name="other">Gameobject != itse</param>
        /// <returns>true jos törmää</returns>
        public abstract bool Collides(GameObject other, out CollisionEventArgs result);

        /// <summary>
        /// Päivittää colliderin toimintaa, etsii uusia
        /// mahdollisia törmättäviä juttuja
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        #endregion
    }

    public delegate void CollisionEventHandler(object sender, CollisionEventArgs result);
}
