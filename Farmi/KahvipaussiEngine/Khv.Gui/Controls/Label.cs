using Khv.Gui.Components;
using Khv.Gui.Components.BaseComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Khv.Gui.Controls
{
    /// <summary>
    /// Kontrolli joka sisältää tekstiä.
    /// </summary>
    public class Label : Control
    {
        #region vars
        protected float fontScale;
        protected string text;
        #endregion

        #region Properties
        public override ControlSize Size
        {
            get
            {
                if (text == null || text.Length == 0)
                {
                    return new ControlSize(0, 0);
                }
                else
                {
                    return new ControlSize((int)Font.MeasureString(text).X, (int)Font.MeasureString(text).Y);
                }
            }
        }
        public override SpriteFont Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }
        public float FontScale
        {
            get
            {
                return fontScale;
            }
            set
            {
                fontScale = MathHelper.Clamp(value, 0.0f, 100.0f);
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value == null)
                {
                    text = "";
                }
                else
                {
                    text = value;
                }
            }
        }
        public bool HasText
        {
            get
            {
                return text.Length != 0 && text != null;
            }
        }
        #endregion

        public Label() :
            base()
        {
            text = "";

            BaseInitialize();
        }
        public Label(ControlPosition position, string text = "")
            : base()
        {
            Position = position;
            this.text = text;

            BaseInitialize();
        }
        /// Suorittaa perus alustukset jotka tulee suorittaa
        /// jokaisessa muodostimessa.
        protected virtual void BaseInitialize()
        {
            Colors.Background = Color.Transparent;
            fontScale = 1.0f;
            Name = "Label";

            Visible = true;
            Enabled = true;
        }
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                base.Draw(spriteBatch);
            }
        }
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Khv.Engine.KhvGame.Temp, new Rectangle(Position.Real.X, Position.Real.Y, Size.Width, Size.Height), Colors.Background);
            spriteBatch.DrawString(Font, text, new Vector2((float)Position.Real.X, (float)Position.Real.Y), Colors.Foreground, 0.0f, Vector2.Zero, fontScale, SpriteEffects.None, 0.0f);
        }
    }
}
