using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Khv.Particles
{
    /// <summary>
    /// Luokka joka pitää sisällään partikkeleiden generoitiin
    /// tarvittavat tiedot ja apu funktiot.
    /// </summary>
    public class GeneratorAtributes
    {
        #region Vars
        private readonly Random random;
        #endregion

        #region Propeties
        /// <summary>
        /// Lista tekstuureista joita käytetään partikkeleissa.
        /// </summary>
        public List<Texture2D> Textures
        {
            get;
            set;
        }
        /// <summary>
        /// Lista väreistä joita käytetään partikkeleissa.
        /// </summary>
        public List<Color> Colors
        {
            get;
            set;
        }

        #region Scale properties
        /// <summary>
        /// Partikkelin skaalan kerroin. 
        /// Tämä arvo määrittää partikkelin aloitus skaalan.
        /// </summary>
        public float ScaleMultiplier
        {
            get;
            set;
        }
        /// <summary>
        /// Partikkelin skaalan kasvu nopeuden kerroin.
        /// </summary>
        public float ScaleVelocityMultiplier
        {
            get;
            set;
        }
        /// <summary>
        /// Määrä joka lisätään skaalan kasvu nopeuteen.
        /// </summary>
        public float AddedScaleVelocity
        {
            get;
            set;
        }
        #endregion

        #region Position properties
        /// <summary>
        /// Määrät joilla velocity kerrotaan.
        /// </summary>
        public Vector2 VelocityMultiplier
        {
            get;
            set;
        }
        /// <summary>
        /// Määrä joka lisätään velocityyn.
        /// </summary>
        public Vector2 AddedVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Aloitus kohdan multiplier arvo.
        /// </summary>
        public Vector2 StartPositionMultiplier
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Value jolla generoidaan randomin avulla 
        /// aika jonka partikkeli on elossa. Tämä arvo voi 
        /// kuvastaa aikaa, alphaa tai vaikka looppi kertaa.
        /// </summary>
        public int TimeToKeepAlive
        {
            get;
            set;
        }
        /// <summary>
        /// Aika joka lisätään generoituun aikaan.
        /// </summary>
        public int AddedTime
        {
            get;
            set;
        }
        /// <summary>
        /// Rotationin kiihtyvyys.
        /// </summary>
        public float RotationVelocityMultiplier
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Alustaa kaikki auto propit default valueilla.
        /// </summary>
        public GeneratorAtributes()
        {
            Textures = new List<Texture2D>();
            Colors = new List<Color>();

            ScaleMultiplier = 0.0f;
            ScaleVelocityMultiplier = 0.0f;

            VelocityMultiplier = Vector2.Zero;
            AddedVelocity = Vector2.Zero;
            StartPositionMultiplier = Vector2.Zero;
            
            TimeToKeepAlive = 0;
            RotationVelocityMultiplier = 0.0f;


            random = new Random();
        }

        /// <summary>
        /// Hakee tekstuurin partikkelia varten.
        /// </summary>
        public virtual Texture2D GetTexture()
        {
            return this.Textures.Count == 0 ? null : this.Textures[random.Next(this.Textures.Count)];
        }
        /// <summary>
        /// Hakee tai generoi värin partikkelia varten.
        /// </summary>
        public virtual Color GetColor()
        {
            return this.Colors.Count == 0 ? GenerateRandomColor() : this.Colors[random.Next(this.Colors.Count)];
        }
        /// <summary>
        /// Laskee projektilen skaalan velocityn joka on 
        /// </summary>
        public virtual float CalculateScaleVelocity()
        {
            return this.ScaleVelocityMultiplier * ((float)random.NextDouble() * this.AddedScaleVelocity - 1);
        }
        /// <summary>
        /// Laskee alotus skaalan.
        /// </summary>
        public virtual float CalculateScale()
        {
            return this.ScaleMultiplier * (float)random.NextDouble();
        }
        /// <summary>
        /// Laskee partikkelin velocityn joka on multiplier * random + added X ja Y komponenteille.
        /// </summary>
        public virtual Vector2 CalculateVelocity()
        {
            return new Vector2(this.VelocityMultiplier.X * ((float)random.NextDouble() * this.AddedVelocity.X - 1),
                               this.VelocityMultiplier.Y * ((float)random.NextDouble() * this.AddedVelocity.Y - 1));
        }
        /// <summary>
        /// Laskee partikkelin random aloitus positionin.
        /// </summary>
        public virtual Vector2 CalculateStartPosition()
        {
            return new Vector2(this.StartPositionMultiplier.X * ((float)random.NextDouble() * 2 - 1),
                               this.StartPositionMultiplier.Y * ((float)random.NextDouble() * 2 - 1));
        }
        /// <summary>
        /// Laskee ajan jonka partikkeli on elossa.
        /// </summary>
        public virtual int CalculateTimeAlive()
        {
            return AddedTime + random.Next(this.TimeToKeepAlive);
        }
        /// <summary>
        /// Laskee rotation velocityn.
        /// </summary>
        /// <returns></returns>
        public virtual float CalculcateRotatioVelocity()
        {
            return RotationVelocityMultiplier * (float)(random.NextDouble() * 2 - 1);
        }
        /// <summary>
        /// Generoi randomin värin.
        /// </summary>
        public virtual Color GenerateRandomColor()
        {
            return new Color((float)random.NextDouble(),
                             (float)random.NextDouble(),
                             (float)random.NextDouble(),
                             (float)random.NextDouble());
        }
    }
}
