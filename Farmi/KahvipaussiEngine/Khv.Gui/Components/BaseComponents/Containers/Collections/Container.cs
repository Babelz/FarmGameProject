using System.Linq;
using Khv.Gui.Components.BaseComponents.Containers.Components;
using Khv.Gui.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


// TODO: REFACTOR
namespace Khv.Gui.Components.BaseComponents.Containers.Collections
{
    /// <summary>
    /// Pohjoaluokka kontrolleja omistaville kontrolleille
    /// kuten Windowille.
    /// </summary>
    public abstract class Container : Control
    {
        #region Vars
        protected float fontScale = 1.0f;
        protected ControlManager controlManager;
        protected FocusManager focusManager;
        protected IndexNavigator indexNavigator;
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa ja asettaa groupin enabledin.
        /// </summary>
        public override bool Enabled
        {
            set
            {
                base.Enabled = value;
                if (Enabled)
                {
                    controlManager.EnableAll();
                }
                else
                {
                    if (FocusFirstOnChange)
                    {
                        FocusFirst();
                    }
                    controlManager.DisableAll();
                }
            }
        }
        /// <summary>
        /// Palauttaa ja asettaa groupin visibledin.
        /// </summary>
        public override bool Visible
        {
            set
            {
                base.Visible = value;
                if (Visible)
                {
                    controlManager.ShowAll();
                }
                else
                {
                    if (FocusFirstOnChange)
                    {
                        FocusFirst();
                    }
                    controlManager.HideAll();
                }
            }
        }
        /// <summary>
        /// Jos true, focusaa ensimmäisen kontrollin
        /// kun enabled tai visible proprty asetetaan falseksi.
        /// Vakiona value on false.
        /// </summary>
        public bool FocusFirstOnChange
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa ja asettaa groupin fontin.
        /// </summary>
        public override SpriteFont Font
        {
            set
            {
                base.Font = value;
                if (Font != null)
                {
                    foreach (Control control in controlManager.AllControls().Where(c => c.Font == null))
                    {
                        control.Font = Font;
                    }
                }
            }
            get
            {
                return base.Font;
            }
        }
        /// <summary>
        /// Palauttaa ja settaa groupin fontin skaalauksen tason.
        /// </summary>
        public virtual float FontScale
        {
            get
            {
                return fontScale;
            }
            set
            {
                fontScale = MathHelper.Clamp(value, 0.0f, 100.0f);
                foreach (Control control in controlManager.AllControls())
                {
                    Label label = control as Label;
                    if (label != null)
                    {
                        label.FontScale = fontScale;
                    }
                }
            }
        }
        #endregion

        public Container()
        {
            controlManager = new ControlManager(this);
            focusManager = new FocusManager();
            indexNavigator = new IndexNavigator(focusManager);

            Colors = new Colors()
            {
                Background = Color.Black,
                Foreground = Color.White
            };

            FocusFirstOnChange = false;
        }
        /// <summary>
        /// Pakottaa jokaisen kontrollin ankkuroitumaan uudelleen.
        /// </summary>
        public void RefreshChildPositions()
        {
            controlManager.AllControls().ToList().ForEach(c => c.Position.Transform(c.Parent));
        }
        /// <summary>
        /// Asettaa focuksen kontrollille jolla on pienin focusindex.
        /// </summary>
        public void FocusFirst()
        {
            List<Control> allControls = controlManager.AllControls()
                .ToList();

            focusManager.ChangeFocus(allControls.First(p => p.FocusIndex.X == allControls.Min(c => c.FocusIndex.X) &&
                                                            p.FocusIndex.Y == allControls.Min(c => c.FocusIndex.Y)));
        }
        /// <summary>
        /// Oakottaa jokaisen childin päivittämään itseään kerran,
        /// passaa updateen null gametimen.
        /// 
        /// Hyvä kutsua kun päivityksiä tarvitaan vain yksi esim focuksen vaihdon kanssa
        /// kontrollien positioni ei päivity
        /// </summary>
        public void UpdateOnce()
        {
            if (!Enabled)
            {
                controlManager.EnableAll();
                controlManager.AllControls()
                    .ToList()
                    .ForEach(c => c.Update(null));

                controlManager.DisableAll();
            }
        }
        public void DoAllActions()
        {
            controlManager.AllControls()
                .ToList()
                .ForEach(c => c.UpdateActions.ForEach(a => a.Invoke(c)));
        }
        /// <summary>
        /// Päivittää containerin ja kaikki sen enabloidut childit.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            controlManager.Update(gameTime);
        }
        /// <summary>
        /// Piirtää containerin ja kaikki sen visible childit.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            controlManager.Draw(spriteBatch);
        }
    }
}
