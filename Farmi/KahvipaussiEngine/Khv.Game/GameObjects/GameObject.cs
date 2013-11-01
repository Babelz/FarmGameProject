using Khv.Engine.Structs;
using Khv.Game.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KhvGame = Khv.Engine.KhvGame;
using Game = Microsoft.Xna.Framework.Game;

namespace Khv.Game.GameObjects
{
    /// <summary>
    /// Kuvastaa abstraktia peliobjektia joka voi liikkua ja jolla on paikka
    /// </summary>
    public abstract class GameObject
    {
        #region Vars

        protected Vector2 position;
        protected Vector2 velocity;

        protected Size size;

        protected KhvGame game;
   
        #endregion

        #region Properties

        /// <summary>
        /// Palauttaa/asettaa nykyisen olinpaikan 
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Palauttaa/asettaa nykyisen nopeuden
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Palauttaa/asettaa colliderin. Jos ei ole collideria
        /// palauttaa null
        /// </summary>
        public Collider Collider
        {
            get;
            set;
        }

        /// <summary>
        /// Voiko objektiin törmätä
        /// </summary>
        public virtual bool IsCollidable
        {
            get { return Collider != null; }
        }

       
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        public ObjectComponentCollection Components
        {
            get;
            private set;
        }

        #endregion

        protected GameObject(KhvGame game)
        {
            this.game = game;

            Components = new ObjectComponentCollection();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (IObjectComponent objectComponent in Components.AllComponents())
            {
                objectComponent.Update(gameTime);
            }
        }
    }
}
