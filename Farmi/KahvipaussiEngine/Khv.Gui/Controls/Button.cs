using Khv.Gui.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Structs;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components.EventListeners;
using Khv.Gui.Components.EventDispatchers;

namespace Khv.Gui.Controls
{
    /// <summary>
    /// Normaali nappula jolla on button eventit.
    /// </summary>
    public class Button : Control, IButtonListener, IMouseListener
    {
        #region Vars
        private Label label;
        #endregion

        #region Properties
        public Texture2D BackgroundImage
        {
            set
            {
                BackgroundImage = value;
            }
        }
        public string Text
        {
            get
            {
                return label.Text;
            }
            set
            {
                label.Text = value;
            }
        }
        public override Colors Colors
        {
            set
            {
                base.Colors = value;
                label.Colors = value;
            }
        }
        public override bool Visible
        {
            set
            {
                base.Visible = value;
                label.Visible = value;
            }
        }
        public override bool Enabled
        {
            set
            {
                base.Enabled = value;
                label.Enabled = value;
            }
        }
        public override SpriteFont Font
        {
            set
            {
                base.Font = value;
                label.Font = value;
            }
        }
        public Point TextOffSet
        {
            get;
            set;
        }
        public Label Content
        {
            get
            {
                return label;
            }
        }
        #endregion
      
        #region Event listeners
        public MouseEventDispatcher MouseEventListener
        {
            get
            {
                return (MouseEventDispatcher)eventDispatchers.Find(d => d is MouseEventDispatcher);
            }
        }
        public ButtonEventDispatcher ButtonEventListener
        {
            get
            {
                return (ButtonEventDispatcher)eventDispatchers.Find(d => d is ButtonEventDispatcher);
            }
        }
        #endregion

        public Button()
            : base()
        {
            BaseInitialize();

            Size = new ControlSize(125, 65, SizeType.Fixed);
        }
        public Button(ControlPosition position, string text)
            : base()
        {
            BaseInitialize();

            Position = position;
            label.Text = text;

            Size = new ControlSize(125, 65, SizeType.Fixed);
        }
        public Button(ControlPosition position, Index focusIndex, string text = "")
            : base()
        {
            BaseInitialize();

            Position = position;
            this.FocusIndex = focusIndex;
            label.Text = text;

            Size = new ControlSize(125, 65, SizeType.Fixed);
        }
        public Button(ControlPosition position, Index focusIndex, ControlSize size, string text = "")
            : base()
        {
            BaseInitialize();

            Position = position;
            this.FocusIndex = focusIndex;
            Size = size;
            label.Text = text;
        }
        /// Suorittaa perus alustukset jotka tulee suorittaa
        /// jokaisessa muodostimessa.
        private void BaseInitialize()
        {
            // Ankkuroidaan labeli kontrolliin ja annetaan sen parentiksi
            // buttoni jossa se sijaitsee.
            label = new Label();
            label.Parent = this;

            Name = "Button";

            Enabled = true;
            Visible = true;

            label.Alingment = new Alingment()
            {
                Horizontal = HorizontalAlingment.Center,
                Vertical = VerticalAlingment.Center
            };
        }
        // Hoidetaan päivitykset jos kontrolli on enabloitus.
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);
                label.Update(gameTime);
            }
        }
        // Piirtää kontrollin ja mahdollisen tekstin.
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                base.Draw(spriteBatch);
            }
        }
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            if (label.HasText)
            {
                label.Draw(spriteBatch);
            }
        }
    }
}
