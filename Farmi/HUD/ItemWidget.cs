using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Game.GameObjects;
using Farmi.Calendar;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components;
using Farmi.Entities.Components;
using Farmi.Entities;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Khv.Gui.Controls;

namespace Farmi.HUD
{
    public sealed partial class ItemWidget : Widget
    {
        #region Vars
        private readonly PlayerInventory inventory;
        #endregion

        public ItemWidget(KhvGame khvGame, GameObject owner, WidgetManager owningManager, string name)
            : base(khvGame, owner, owningManager, name)
        {
            FarmPlayer player = owner as FarmPlayer;

            inventory = player.Components.GetComponent<PlayerInventory>();

            Initialize();
        }
    }
    public sealed partial class ItemWidget : Widget
    {
        #region Vars
        private ItemWrapper itemWrapper;
        private ItemWrapper toolWrapper;
        #endregion

        protected override void Initialize()
        {
            int screenWidth = khvGame.GraphicsDeviceManager.PreferredBackBufferWidth;
            int screenHeight = khvGame.GraphicsDeviceManager.PreferredBackBufferHeight;

            Font = khvGame.Content.Load<SpriteFont>("arial2");
            BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "widgetbg"));
            Size = new ControlSize(15, 15, SizeType.Percent);

            Colors = new Colors()
            {
                Background = Color.White,
                Foreground = Color.White
            };

            Position = new ControlPosition(
                screenWidth - screenWidth / 100 * 15,
                screenHeight - screenHeight / 100 * 15);

            Position.Margin = new Margin(
                left: -15,
                right: 0,
                top: -15,
                bottom: 0);

            itemWrapper = new ItemWrapper();
            this.controlManager.AddControl(itemWrapper);
            inventory.OnItemChanging += new PlayerInventoryEventHandler(inventory_OnItemChanging);
            itemWrapper.Size = new ControlSize(50, 50, SizeType.Percent);
            updateActions.Add((c) =>
                {
                    itemWrapper.Position = new ControlPosition(this.size.Width - itemWrapper.Size.Width, 0);
                    itemWrapper.Position.Margin = new Margin(-5, 0, 10, 0);
                });
            itemWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));
            
            Label itemLabel = new Label();
            this.controlManager.AddControl(itemLabel);
            itemLabel.Text = "Item";
            updateActions.Add((c) =>
                {
                    itemLabel.Position = new ControlPosition(
                        itemWrapper.Position.Relative.X + itemWrapper.Size.Width / 2 - (int)itemLabel.Font.MeasureString(itemLabel.Text).X / 2,
                        itemWrapper.Position.Relative.Y + (int)itemLabel.Font.MeasureString(itemLabel.Text).Y * 2);
                    itemLabel.Position.Margin = new Margin(0, 0, 5, 0);
                });

            toolWrapper = new ItemWrapper();
            this.controlManager.AddControl(toolWrapper);
            inventory.OnToolChanging += new PlayerInventoryEventHandler(inventory_OnToolChanging);
            toolWrapper.Size = new ControlSize(50, 50, SizeType.Percent);
            updateActions.Add((c) =>
                {
                    toolWrapper.Position = new ControlPosition(0, 0);
                    toolWrapper.Position.Margin = new Margin(5, 0, 10, 0);
                });
            toolWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));

            Label toolLabel = new Label();
            this.controlManager.AddControl(toolLabel);
            toolLabel.Text = "Tool";
            updateActions.Add((c) =>
            {
                toolLabel.Position = new ControlPosition(
                    toolWrapper.Position.Relative.X + toolWrapper.Size.Width / 2 - (int)toolLabel.Font.MeasureString(toolLabel.Text).X / 2,
                    toolWrapper.Position.Relative.Y + (int)toolLabel.Font.MeasureString(toolLabel.Text).Y * 2);
                toolLabel.Position.Margin = new Margin(0, 0, 5, 0);
            });
        }
        private void inventory_OnToolChanging(object sender, PlayerInventoryEventArgs e)
        {
            toolWrapper.ChangeItem(e.NextItem);
        }
        private void inventory_OnItemChanging(object sender, PlayerInventoryEventArgs e)
        {
            itemWrapper.ChangeItem(e.NextItem);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
