using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Particles
{
    public abstract class Particle
    {
        #region Vars
        private Texture2D texture;
        private Vector2 origin;
        private Rectangle sourceRectangle;
        #endregion

        #region Properties
        /// <summary>
        /// Teksuuri jonka partikkeli piirtää.
        /// Laskee samalla sourcen ja origin vectorin uudelleen.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                CalculateSource();
                CalculateOrigin();
            }
        }
        /// <summary>
        /// Tämän hetkinen sijainti.
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }
        /// <summary>
        /// Sijainnin kiihtyvyys arvo.
        /// </summary>
        public Vector2 Velocity
        {
            get;
            set;
        }
        /// <summary>
        /// Tämän hetkinen kierron arvo.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }
        /// <summary>
        /// Rotation kiihtyvyys.
        /// </summary>
        public float RotationVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Tämän hetkinen skaala arvo.
        /// </summary>
        public float Scale
        {
            get;
            set;
        }
        /// <summary>
        /// Skaalana kiihtyvyys.
        /// </summary>
        public float ScaleVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Väri jolla partikkeli piirretään.
        /// </summary>
        public Color Color
        {
            get;
            set;
        }
        /// <summary>
        /// Aika jonka partikkeli pidetään elossa.
        /// </summary>
        public int TimeToKeepAlive
        {
            get;
            set;
        }
        /// <summary>
        /// Aika joka partikkeli on ollut elossa.
        /// </summary>
        public virtual int TimeAlive
        {
            get;
            protected set;
        }
        #endregion

        #region Protected properties
        protected Rectangle SourceRectangle
        {
            get
            {
                return sourceRectangle;
            }
        }
        protected Vector2 Origin
        {
            get
            {
                return origin;
            }
        }
        #endregion

        /// <summary>
        /// Alustaa kaikki auto propit default arvoilla.
        /// </summary>
        public Particle()
            : this(null, Vector2.Zero, Vector2.Zero, 0.0f, 0.0f, 1.0f, 0.0f, Color.White, 0)
        {
        }

        /// <summary>
        /// Laskee source rectanglen.
        /// </summary>
        private void CalculateSource()
        {
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        /// <summary>
        /// Laskee origin verktorin.
        /// </summary>
        private void CalculateOrigin()
        {
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        /// <summary>
        /// Alustaa kaikki arvot suoraan muodostimessa.
        /// </summary>
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float rotation, float rotationVelocity,
                        float scale, float scaleVelocity, Color color, int timeToKeepAlive)
        {
            Texture = texture;

            Position = position;
            Velocity = velocity;

            Rotation = rotation;
            RotationVelocity = rotationVelocity;

            Color = color;

            Scale = scale;
            ScaleVelocity = scaleVelocity;

            TimeToKeepAlive = timeToKeepAlive;
        }

        #region Abstract members
        /// <summary>
        /// Palauttaa booleanin onko partikkeli vielä elossa.
        /// </summary>
        public abstract bool IsAlive();

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        #endregion
    }
}
