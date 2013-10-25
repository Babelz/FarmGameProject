using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Particles
{
    public class TimedParticle : Particle
    {
        #region Vars
        private int timeAlive;
        #endregion

        #region Properties
        public override int TimeAlive
        {
            get
            {
                return timeAlive;
            }
            protected set
            {
                timeAlive = value;
            }
        }
        #endregion

        public TimedParticle()
            : base()
        {
        }
        public TimedParticle(Texture2D texture, Vector2 position, Vector2 velocity, float rotation, float rotationVelocity,
                        float scale, float scaleVelocity, Color color, int timeToKeepAlive)
            : base(texture, position, velocity, rotation, rotationVelocity, scale, scaleVelocity, color, timeToKeepAlive)
        {
        }

        public override bool IsAlive()
        {
            return TimeToKeepAlive >= TimeAlive;
        }
        public override void Update(GameTime gameTime)
        {
            TimeAlive += gameTime.ElapsedGameTime.Seconds;

            Position += Velocity;
            Rotation += RotationVelocity;
            Scale += ScaleVelocity;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
