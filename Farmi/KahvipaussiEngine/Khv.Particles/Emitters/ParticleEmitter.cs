using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Particles
{
    public class ParticleEmitter : Emitter
    {
        #region Properties
        /// <summary>
        /// Boolean arvo siitä, tuottaako emitter partikkeleita.
        /// </summary>
        public bool IsEmitting
        {
            get;
            set;
        }
        /// <summary>
        /// Kuinka monta partikkelia luodaan per update looppi.
        /// </summary>
        public int ParticlesPerLoop
        {
            get;
            set;
        }
        #endregion

        public ParticleEmitter(ParticleGenerator particleGenerator, Vector2 position)
            : base(particleGenerator, position)
        {
            IsEmitting = false;

            ParticlesPerLoop = 0;
        }
        public ParticleEmitter(ParticleGenerator particleGenerator)
            : base(particleGenerator)
        {
            IsEmitting = false;

            ParticlesPerLoop = 0;
        }

        protected virtual void MakeNewParticles()
        {
            for (int i = 0; i < ParticlesPerLoop; i++)
            {
                particles.Add(particleGenerator.Generate(this));
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsEmitting)
            {
                MakeNewParticles();
            }
            particles.ForEach(p => p.Update(gameTime));
            particles.RemoveAll(p => !p.IsAlive());
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            particles.ForEach(p => p.Draw(spriteBatch));
        }
    }
}
