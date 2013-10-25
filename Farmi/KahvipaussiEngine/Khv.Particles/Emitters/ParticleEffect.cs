using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Particles
{
    public class ParticleEffect : Emitter
    {
        #region Properties
        /// <summary>
        /// Boolean siitä toistetaanko efektiä tällä hetkellä.
        /// </summary>
        public bool IsPlaying
        {
            get;
            private set;
        }
        /// <summary>
        /// Jos playta kutsutaan kun efektiä toistetaan
        /// ja tämä value on true, resetoi efektin ja
        /// alkaa toistamaan sitä uudestaan. Muulloin jättää kaikki play kutstut
        /// huomioimatta jos toistetaan efektiä.
        /// </summary>
        public bool SkipCurrentEffect
        {
            get;
            set;
        }
        /// <summary>
        /// Kuinka monta partikkelia tulisi lisätä kun efektiä altetaan
        /// toistamaan.
        /// </summary>
        public int ParticlesPerEffect
        {
            get;
            set;
        }
        #endregion

        public ParticleEffect(ParticleGenerator particleGenerator, Vector2 position)
            : base(particleGenerator, position)
        {
            IsPlaying = false;
            SkipCurrentEffect = false;

            ParticlesPerEffect = 0;
        }
        public ParticleEffect(ParticleGenerator particleGenerator)
            : base(particleGenerator)
        {
            IsPlaying = false;
            SkipCurrentEffect = false;

            ParticlesPerEffect = 0;
        }

        protected virtual void BeginPlaying()
        {
            particles.Clear();

            for (int i = 0; i < ParticlesPerEffect; i++)
            {
                particles.Add(particleGenerator.Generate(this));
            }

            IsPlaying = true;
        }

        public virtual void Play()
        {
            if (IsPlaying && SkipCurrentEffect)
            {
                BeginPlaying();
            }
            else if(!IsPlaying)
            {
                BeginPlaying();
            }
        }
        public override void Update(GameTime gameTime)
        {
            particles.ForEach(p => p.Update(gameTime));
            particles.RemoveAll(p => !p.IsAlive());

            if (particles.Count == 0)
            {
                IsPlaying = false;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            particles.ForEach(p => p.Draw(spriteBatch));
        }
    }
}
