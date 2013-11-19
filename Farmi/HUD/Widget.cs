using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components.BaseComponents.Containers.Collections;
using Khv.Game.GameObjects;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Args;

namespace Farmi.HUD
{
    public abstract class Widget : Window
    {
        #region Vars
        private bool destoyed;
        #endregion

        #region Events
        public event WidgetEventHandler OnDestoryed;
        #endregion

        #region Properties
        public GameObject Owner
        {
            get;
            private set;
        }
        public WidgetManager OwningManager
        {
            get;
            private set;
        }
        #endregion

        public Widget(KhvGame khvGame, GameObject owner, WidgetManager owningManager, string name)
            : base(khvGame)
        {
            Owner = owner;
            OwningManager = owningManager;
            Name = name;
        }

        /// <summary>
        /// Suoritetaan widgetin disposaus tässä metodissa.
        /// </summary>
        protected virtual void OnDestory()
        {
        }
        public void Destroy()
        {
            if (!destoyed)
            {
                OnDestory();
                destoyed = true;

                if (OnDestoryed != null)
                {
                    OnDestoryed(this, new WidgetEventArgs());
                }
            }
        }
    }

    public delegate void WidgetEventHandler(object sender, WidgetEventArgs e);

    public class WidgetEventArgs : GameEventArgs { }
}
