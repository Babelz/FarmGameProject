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
        private List<Control> controls;
        private Control owner;
        #endregion

        #region Properties
        public List<Control> Controls
        {
            get
            {
                return controls;
            }
        }
        #endregion

        public ControlManager(Control owner)
        {
            this.owner = owner;
            controls = new List<Control>();
        }
        // Lisää kontrollin manageriin ja antaa sille tarittavat viitteet.
        public void AddControl(Control control)
        {
            controls.Add(control);
            control.Parent = owner;
            control.Font = owner.Font;
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
