using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components.BaseComponents.Containers.Collections;
using Khv.Engine;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Farmi.Screens;
using Farmi.Entities;
using System.IO;

namespace Farmi.HUD
{
    public sealed partial class WidgetWindow : Window
    {
        public WidgetWindow(KhvGame khvGame)
            : base(khvGame)
        {

            size = new ControlSize(khvGame.GraphicsDeviceManager.PreferredBackBufferWidth,
                                   khvGame.GraphicsDeviceManager.PreferredBackBufferHeight,
                                   Khv.Gui.Components.SizeType.Fixed);

            Initialize();
        }
        public void AddWidget(Widget widget)
        {
            controlManager.AddControl(widget);
        }
        public void RemoveWidget(Widget widget)
        {
            controlManager.RemoveControl(widget);
        }
    }
    public sealed partial class WidgetWindow : Window
    {
        protected override void Initialize()
        {
            Colors = new Colors()
            {
                Background = Color.Transparent,
                Foreground = Color.Transparent
            };
        }
    }
}
