using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components.BaseComponents
{
    public class ControlPosition
    {
        #region Vars
        private Margin margin;
        private Vector2 position;
        private Vector2 realPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Kontrollin sijanti ilman marginia.
        /// </summary>
        public Vector2 NormalPosition
        {
            get
            {
                return position;
            }
        }
        /// <summary>
        /// Kontrollin sijainti. Relatiivinen parenttiin jos kontrollilla
        /// on parentti tai se ankkuroidaan johonkin muuhun AnchorTo metodilla.
        /// </summary>
        public Vector2 Relative
        {
            get
            {
                return new Vector2(margin.Left + position.X + margin.Right,
                                   margin.Top + position.Y + margin.Bottom);
            }
            set
            {
                position = value;
            }  
        }
        /// <summary>
        /// Kontrollin oikea sijainti näytöllä.
        /// </summary>
        public Vector2 Real
        {
            get
            {
                return realPosition;
            }
        }
        public Margin Margin
        {
            get
            {
                return margin;
            }
            set
            {
                if (value == null)
                {
                    margin = Margin.Default();
                }
                else
                {
                    margin = value;
                }
            }
        }
        #endregion

        public ControlPosition(float x, float y)
        {
            Relative = new Vector2(x, y);
            margin = Margin.Default();
        }

        /// <summary>
        /// Asettaa kontrollin positionin että se ankkuroituu parametreinä saatavaan kontrolliin.
        /// Yleensä tämä kontrolli on kontrollin parentti.
        /// </summary>
        public void Transform(Control parent)
        {
            if (parent != null)
            {
                realPosition = new Vector2(Relative.X + parent.Position.Real.X, Relative.Y + parent.Position.Real.Y);
            }
            else
            {
                realPosition = Relative;
            }
        }

        public static ControlPosition Default()
        {
            return new ControlPosition(0, 0);
        }
    }
}
