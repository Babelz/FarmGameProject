using System.Collections.Generic;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Gui.Components.EventDispatchers;
using Khv.Gui.Components.EventListeners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Khv.Gui.Effects;
using System.Linq;

// Alias using statementit.
using Effect = Khv.Gui.Effects.Effect;
using DrawableEffect = Khv.Gui.Effects.DrawableEffect;

namespace Khv.Gui.Components.BaseComponents
{
    /// <summary>
    /// Kaikkien kontrollien äiti. Uusi kontrolli tulee johtaa tästä luokasta,
    /// tämä luokka on abstracti.
    /// </summary>
    public abstract class Control : IControlListener
    {
        #region Vars
        // muuttujat joita kontrolloidaan propertyn kautta
        private ControlPosition position;
        private Colors colors;
        private Alingment alingment;
        private bool hasFocus;
    
        // protected muuttujat jotka ovat muokattavissa perinnässä
        protected List<EventDispatcher> eventDispatchers;
        protected Texture2D backgroundImage;
        protected ControlSize size;

        // lista toiminnoista jotka suoritetaan joka updatessa.
        protected List<Action<Control>> updateActions;
        #endregion 

        #region Event listeners
        public ControlEventDispatcher ControlEventListener
        {
            get
            {
                return (ControlEventDispatcher)eventDispatchers.Find(d => d is ControlEventDispatcher);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Asettaa tai palauttaa kontrollin nimen.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Kontrollin koko.
        /// </summary>
        public virtual ControlSize Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value == null)
                {
                    size = ControlSize.Default();
                }
                else
                {
                    size = value;
                }
            }
        }
        /// <summary>
        /// Asettaa tai palauttaa kontrollin sijainnin.
        /// </summary>
        public ControlPosition Position
        {
            get
            {
                return position;
            }
            set
            {
                if (value == null)
                {
                    position = ControlPosition.Default();
                }
                else
                {
                    position = value;
                }
            }
        }
        /// <summary>
        /// Palauttaa booleanin onko kontrollilla focusta.
        /// </summary>
        public bool HasFocus
        {
            get
            {
                return hasFocus;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin parentin.
        /// </summary>
        public Control Parent
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin fontin.
        /// </summary>
        public virtual SpriteFont Font
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin värit.
        /// </summary>
        public virtual Colors Colors
        {
            get
            {
                return colors;
            }
            set
            {
                colors = value;
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin alingmentin.
        /// </summary>
        public virtual Alingment Alingment
        {
            get
            {
                return alingment;
            }
            set
            {
                alingment = value;
            }
        }
        /// <summary>
        /// Palauttaa booleanin onko kontrolli enabled.
        /// </summary>
        public virtual bool Enabled
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa tai asettaa onko kontrolli visible.
        /// </summary>
        public virtual bool Visible
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa tai asettaa kontrollin focus indeksin.
        /// </summary>
        public Index FocusIndex
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa rectanglen joka koostuu kontrollin oikeasta sijainnista.
        /// ja koosta.
        /// </summary>
        public Rectangle ClientArea
        {
            get
            {
                return new Rectangle(Position.Real.X, Position.Real.Y, Size.Width, Size.Height);
            }
        }
        /// <summary>
        /// Lista toiminnoista jotka suoritetaan updatessa.
        /// </summary>
        public List<Action<Control>> UpdateActions
        {
            get
            {
                return updateActions;
            }
        }
        public EffectContainer Effects
        {
            get;
            protected set;
        }
        #endregion 

        public Control()
        {
            // Asettaa vakio alingmentin.
            alingment = new Alingment()
            {
                Horizontal = HorizontalAlingment.None,
                Vertical = VerticalAlingment.None
            };
            // Asettaa vakio värit.
            colors = new Colors()
            {
                Foreground = Color.Black,
                Background = Color.White
            };
            FocusIndex = Index.Empty;

            size = ControlSize.Default();
            position = ControlPosition.Default();

            updateActions = new List<Action<Control>>();
            Effects = new EffectContainer();

            MakeDispatchers();
        }
        /// <summary>
        /// Ylikirjoitetaan jos halutaan lisätä lisää dispathcereita 
        /// alustus vaiheessa. Alustaa kolme vakio listeneriä (Control, Mouse ja Button)
        /// jos luokka perii jonkin liistä.
        /// </summary>
        protected virtual void MakeDispatchers()
        {
            eventDispatchers = new List<EventDispatcher>();

            eventDispatchers.Add(new ControlEventDispatcher(this));

            if (this is IMouseListener)
            {
                eventDispatchers.Add(new MouseEventDispatcher(this));
            }
            if (this is IButtonListener)
            {
                eventDispatchers.Add(new ButtonEventDispatcher(this));
            }
        }

        /// <summary>
        /// Asettaa kontrollille focuksen.
        /// </summary>
        public virtual void Focus()
        {
            hasFocus = true;
        }
        /// <summary>
        /// Poistaa focuksen kontrollilta ja sen mahdollisilta listenereitlä 
        /// ja childeiltä.
        /// </summary>
        public virtual void Defocus()
        {
            if (this is IButtonListener)
            {
                (this as IButtonListener).ButtonEventListener.Trigger.CurrentState = PressedState.None;
            }

            hasFocus = false;
        }
        /// <summary>
        /// Suorittaa kontrollin päivittämisen. Vakiona kuuntelee
        /// eventit ja ankkuroi sen mahdolliseen parenttiin.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Effects.AllEffects.ForEach(e => e.Update(gameTime));

            eventDispatchers.ForEach(d =>
            {
                if (d.IsListening)
                {
                    d.ListenOnce();
                }
            });

            if (Parent != null)
            {
                Size.Update(Parent.Size);
                Position.Relative = Alingment.Calculate(this);
            }
            Position.AnchorTo(Parent);

            updateActions.ForEach(a => a.Invoke(this));
        }
        /// <summary>
        /// Piirtää kontrollin ja sen efektit. 
        /// Jos jokin efekti ylikirjoittaa drawin, ei piirrtä kontrollia.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Effects.ContainsOverridingEffects)
            {
                DrawEffects(spriteBatch);
            }
            else
            {
                DrawControl(spriteBatch);
                DrawEffects(spriteBatch);
            }
        }
        /// <summary>
        /// Piirtää kontrollin. Vakiona piirtää 
        /// taustakuvan halutulla värillä.
        /// </summary>
        protected virtual void DrawControl(SpriteBatch spriteBatch)
        {
            // jos taustakuva on null, piirretään default texture
            // tausta värillä
            Texture2D textureToDraw = (backgroundImage == null) ? KhvGame.Temp : backgroundImage;
            spriteBatch.Draw(textureToDraw, new Rectangle(Position.Real.X, Position.Real.Y, size.Width, size.Height), Colors.Background);
        }
        /// <summary>
        /// Piirtää kaikki efektit. Vakiona järjestys on draworder.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected virtual void DrawEffects(SpriteBatch spriteBatch)
        {
            foreach(DrawableEffect effect in Effects.DrawableEffects.OrderBy(e => e.DrawOrder))
            {
                effect.Draw(spriteBatch);
            }
        }
    }
}
