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
        private Point position;
        private Point realPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Kontrollin sijanti ilman marginia.
        /// </summary>
        public Point NormalPosition
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
        public Point Relative
        {
            get
            {
                return new Point(margin.Left + position.X + margin.Right,
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
        public Point Real
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

        public ControlPosition(int x, int y)
        {
            Relative = new Point(x, y);
            margin = Margin.Default();
        }

        /// <summary>
        /// Asettaa kontrollin positionin että se ankkuroituu parametreinä saatavaan kontrolliin.
        /// Yleensä tämä kontrolli on kontrollin parentti.
        /// </summary>
        public void AnchorTo(Control parent)
        {
            if (parent != null)
            {
                realPosition = new Point(Relative.X + parent.Position.Real.X, Relative.Y + parent.Position.Real.Y);
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
