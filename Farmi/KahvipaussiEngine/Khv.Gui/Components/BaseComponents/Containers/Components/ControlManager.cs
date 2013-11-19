using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components;
using System.Linq;
using System;
using System.Collections;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    /// <summary>
    /// luokka joka sisältää kontrolleja ja helpottaa
    /// niiden managointia
    /// </summary>
    public class ControlManager
    {
        #region Vars
        private readonly List<Control> controls;
        private readonly Control owner;
        #endregion

        public ControlManager(Control owner)
        {
            this.owner = owner;
            controls = new List<Control>();
        }
        // Lisää kontrollin manageriin ja antaa sille tarittavat viitteet.
        public void AddControl(Control control)
        {
            if (control == owner)
            {
                return;
            }

            controls.Add(control);
            control.Parent = owner;
        }
        public void RemoveControl(Control control)
        {
            if (control == owner)
            {
                return;
            }

            controls.Remove(control);
        }
        public Control GetControl(Predicate<Control> predicate)
        {
            return controls.First(c => predicate(c));
        }

        public IEnumerable<Control> AllControls(Predicate<Control> predicate = null)
        {
            if (predicate == null)
            {
                foreach (Control control in controls)
                {
                    yield return control;
                }
            }
            else
            {
                foreach (Control control in controls.Where(c => predicate(c)))
                {
                    yield return control;
                }
            }
        }

        /// <summary>
        /// Päivittää kaikki kontrollit jotka ovat enabloitu.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (Control control in controls)
            {
                control.Update(gameTime);
            }
        }
        /// <summary>
        /// Piirtää kaikki näkyvät kontrollit.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control control in controls)
            {
                control.Draw(spriteBatch);
            }
        }

        // Wräppäys metodit.
        #region Wrappers
        // Disabloi kaikki kontrollit.
        public void DisableAll()
        {
            foreach (Control control in controls)
            {
                control.Enabled = false;
            }
        }
        // Piilottaa kaikki kontrollit.
        public void HideAll()
        {
            foreach (Control control in controls)
            {
                control.Visible = false;
            }
        }
        // Enabloi kaikki kontrollit.
        public void EnableAll()
        {
            foreach (Control control in controls)
            {
                control.Enabled = true;
            }
        }
        // Näyttää kaikki kontrollit.
        public void ShowAll()
        {
            foreach (Control control in controls)
            {
                control.Visible = true;
            }
        }
        #endregion
    }
}
