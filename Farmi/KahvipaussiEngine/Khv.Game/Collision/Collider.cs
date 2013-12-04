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
        /// Kun törmäys tapahtuu.
        /// </summary>
        public event CollisionEventHandler OnCollision;

        // TODO: tarvis sen OnCollisionLeave ja OnCollisionEnter eventit

        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionLeave;
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

        protected void FireOnCollisionEnter(GameObject me, object sender, CollisionEventArgs args)
        {
            if (me.Collider.OnCollisionEnter == null) return;

            args.CollidingObject = sender;
            me.Collider.OnCollisionEnter(this, args);
        }

        protected void FireOnCollisionLeave(GameObject me, object sender, CollisionEventArgs args)
        {
            if (me.Collider.OnCollisionLeave == null) return;

            args.CollidingObject = sender;
            me.Collider.OnCollisionLeave(this, args);
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Törmääkö johonkin
        /// </summary>
        public abstract bool Collides(GameObject other, out CollisionEventArgs result);
        /// <summary>
        /// Katsoo törmätäänkö argumenttina syötettyyn olioon. 
        /// Samakuin Collides(GameObject other, out CollisionEventArgs result) 
        /// mutta ilman "out results" pakkoa.
        /// </summary>
        public bool Collides(GameObject other)
        {
            CollisionEventArgs results = new CollisionEventArgs();

            return Collides(other, out results);
        }

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
