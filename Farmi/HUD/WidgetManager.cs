using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components.BaseComponents.Containers.Collections;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Gui.Components.BaseComponents;
using Farmi.Screens;

namespace Farmi.HUD
{
    public sealed class WidgetManager : IDrawableObjectComponent
    {
        #region Vars
        private readonly KhvGame khvGame;
        private readonly List<Widget> widgets;
        private readonly WidgetWindow window;
        private readonly GameObject owner;
        #endregion

        #region Properties
        public int DrawOrder
        {
            get;
            set;
        }
        /// <summary>
        /// Päivitetäänkö widgettejä.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return window.Enabled;
            }
            set
            {
                window.Enabled = value;
            }
        }
        /// <summary>
        /// Piiretäänkö widgettejä.
        /// </summary>
        public bool Visible
        {
            get
            {
                return window.Enabled;
            }
            set
            {
                window.Visible = value;
            }
        }
        public ControlSize WidgetArea
        {
            get
            {
                return window.Size;
            }
        }
        #endregion

        public WidgetManager(KhvGame khvGame, GameObject owner)
        {
            this.khvGame = khvGame;
            this.owner = owner;

            widgets = new List<Widget>();
            window = new WidgetWindow(khvGame);

            DrawOrder = 5;
        }

        #region Add methods
        public void AddWidgets(IEnumerable<Widget> widgets)
        {
            foreach (Widget widget in widgets)
            {
                if (!this.widgets.Contains(widget))
                {
                    this.widgets.Add(widget);
                    window.AddWidget(widget);
                }
            }
        }
        public void AddWidget(Widget widget)
        {
            if (!this.widgets.Contains(widget))
            {
                this.widgets.Add(widget);
                window.AddWidget(widget);
            }
        }
        #endregion

        #region Remove methods
        public void RemoveWidgets(IEnumerable<Widget> widgets)
        {
            foreach (Widget widget in widgets)
            {
                if (this.widgets.Contains(widget))
                {
                    this.widgets.Remove(widget);
                    window.RemoveWidget(widget);
                }
            }
        }
        public void RemoveWidget(Widget widget)
        {
            if (this.widgets.Contains(widget))
            {
                this.widgets.Remove(widget);
                window.RemoveWidget(widget);
            }
        }
        #endregion

        #region Query methods
        public bool ContainsWidget(Widget widget)
        {
            return widgets.Contains(widget);
        }
        public Window GetWidget(Predicate<Widget> predicate)
        {
            return widgets.Find(w => predicate(w));
        }
        #endregion

        #region IEnumerable methods
        public IEnumerable<Window> AllWidgets(Predicate<Widget> predicate = null)
        {
            if (predicate == null)
            {
                foreach (Widget widget in widgets)
                {
                    yield return widget;
                }
            }
            else
            {
                foreach (Widget widget in widgets.Where(w => predicate(w)))
                {
                    yield return widget;
                }
            }
        }
        #endregion

        public void Update(GameTime gametime)
        {
            GameplayScreen gameplayScreen = khvGame.GameStateManager.Current as GameplayScreen;

            if (gameplayScreen != null)
            {
                window.Position = new ControlPosition(
                    (int)gameplayScreen.Camera.Position.X,
                    (int)gameplayScreen.Camera.Position.Y);
            }

            window.Update(gametime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            window.Draw(spriteBatch);
        }
    }
}
