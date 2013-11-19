using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine.Structs;

namespace Khv.Gui.Components.BaseComponents
{
    public class ControlSize
    {
        #region Vars
        private int heightPercent;
        private int widthPercent;
        private Size size;

        // Pidetään "tyhjä" padding mukana koko ajan jotta
        // säästytään turhilta null tarkistuksilla joka
        // property kustuissa.
        private Padding padding;
        #endregion

        #region Properties
        public Padding Padding
        {
            get
            {
                return padding;
            }
            set
            {
                if (value == null)
                {
                    padding = Padding.Empty();
                }
                else
                {
                    padding = value;
                }
            }
        }
        /// <summary>
        /// Palauttaa leveyden pikseleissä.
        /// </summary>
        public int Width
        {
            get
            {
                return padding.Left + size.Width + padding.Right;
            }
            set
            {
                if (Type == SizeType.Fixed)
                {
                    size = new Size(value, size.Height);
                }
            }
        }
        /// <summary>
        /// Palauttaan korkeuden pikseleissä.
        /// </summary>
        public int Height
        {
            get
            {
                return padding.Top + size.Height + padding.Bottom;
            }
            set
            {
                if (Type == SizeType.Fixed)
                {
                    size = new Size(size.Width, value);
                }
            }
        }
        /// <summary>
        /// Palauttaa prosentuaalisen arvon leveydestä verrattuna parenttiin,
        /// jos koko on prosentuaalinen, asettaa prosentuaalisen arvon leveydelle.
        /// </summary>
        public int WidthPercent
        {
            get
            {
                return widthPercent;
            }
            set
            {
                if (Type == SizeType.Percent)
                {
                    widthPercent = value;
                }
            }
        }
        /// <summary>
        /// Palauttaa prosentuaalisen arvon korkeudesta verrattuna parenttiin,
        /// jos koko on prosentuaalinen, asettaa prosentuaalisen arvon korkeudelle.
        /// </summary>
        public int HeightPercent
        {
            get
            {
                return heightPercent;
            }
            set
            {
                if (Type == SizeType.Percent)
                {
                    heightPercent = value;
                }
            }
        }
        /// <summary>
        /// Koon tyyppi.
        /// </summary>
        public SizeType Type
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Alustaa uuden koko olion halutulla toiminnallisuudella.
        /// </summary>
        /// <param name="widthValue">Leveyden arvo, fixed koossa pikseleitä, percentissä prosentteja.</param>
        /// <param name="heightValue">Korkeuden arvo, fixed koossa pikseleitä, percentissä prosentteja.</param>
        /// <param name="type">Koko olion tyypi.</param>
        public ControlSize(int widthValue, int heightValue, SizeType type = SizeType.Fixed)
        {
            // Pidetään tyhjää padding mukana koko ajan jotta 
            // vältytään turhilta null checkeiltä joka propertyssä.
            padding = Padding.Empty();

            switch (type)
            {
                case SizeType.Fixed:
                    size = new Size(widthValue, heightValue);
                    break;
                case SizeType.Percent:
                    widthPercent = widthValue;
                    heightPercent = heightValue;
                    break;
            }

            Type = type;
        }
        /// <summary>
        /// Päivittää koon valueita. Jos koko on fixed, päivittää
        /// percent valueita, jos koko on percent, päivittää sizen 
        /// valueta.
        /// </summary>
        /// <param name="parentSize">Parentin koko jota tarvitaan päivityksien tekemisessä.</param>
        public void Transform(ControlSize parentSize)
        {
            switch (Type)
            {
                case SizeType.Fixed:
                    heightPercent = size.Height / parentSize.Height * 100;
                    widthPercent = size.Width / parentSize.Width * 100;
                    break;
                case SizeType.Percent:
                    float onePercent_Width = parentSize.Width / 100;
                    onePercent_Width = (onePercent_Width == 0 ? 1 : onePercent_Width);
                    float width = onePercent_Width * widthPercent;

                    float onePercent_Height = parentSize.Height / 100;
                    onePercent_Height = (onePercent_Height == 0 ? 1 : onePercent_Height);
                    float height = onePercent_Height * heightPercent;

                    size = new Size((int)width, (int)height);
                    break;
            }
        }

        public static ControlSize Default()
        {
            return new ControlSize(0, 0, SizeType.Fixed);
        }
    }
}
