using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Particles
{
    public abstract class Emitter
    {
        #region Vars
        protected readonly ParticleGenerator particleGenerator;
        protected List<Particle> particles;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get;
            set;
        }
        public ParticleGenerator CurrentGenerator
        {
            get
            {
                return particleGenerator;
            }
        }
        #endregion

        public Emitter(ParticleGenerator particleGenerator, Vector2 position)
            : this(particleGenerator)
        {
            Position = position;
        }
        public Emitter(ParticleGenerator particleGenerator)
        {
            this.particleGenerator = particleGenerator;

            particles = new List<Particle>();
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Suorittaa kaikilla partikkeleilla notifyActionin.
        /// </summary>
        public virtual void NotifyParticles(Action<Particle> notifyAction)
        {
            particles.ForEach(p => notifyAction(p));
        }
        /// <summary>
        /// Poistaa kaikki partikkelit jotka täyttävät edhon.
        /// </summary>
        public void RemoveParticles(Predicate<Particle> predicate)
        {
            particles.RemoveAll(p => predicate(p));
        }

        #region Abstract members
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        #endregion
    }
}
