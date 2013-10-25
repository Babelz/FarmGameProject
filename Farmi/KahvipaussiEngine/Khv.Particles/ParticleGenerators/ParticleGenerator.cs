using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Particles
{
    public class ParticleGenerator
    {
        #region Vars
        protected readonly GenerateDelegate generateDelegate;
        protected readonly GeneratorAtributes generatorAtributes;
        protected readonly Random random;
        #endregion

        #region Properties
        public GeneratorAtributes GeneratorAttributes
        {
            get
            {
                return generatorAtributes;
            }
        }
        #endregion

        public ParticleGenerator(GeneratorAtributes generatorAtributes, GenerateDelegate generateDelegate)
        {
            this.generatorAtributes = generatorAtributes;
            this.generateDelegate = generateDelegate;
        }

        /// <summary>
        /// Metodi joka generoi uuden partikkelin annettujen atribuuttien
        /// ja delekaatin perusteella.
        /// </summary>
        public Particle Generate(Emitter owner)
        {
            return generateDelegate(owner, generatorAtributes);
        }
    }

    /// <summary>
    /// Delekaatti jota generaattorit kutsuvat Generate metodissaa.
    /// </summary>
    public delegate Particle GenerateDelegate(Emitter owner, GeneratorAtributes generatorAttributes);
}
