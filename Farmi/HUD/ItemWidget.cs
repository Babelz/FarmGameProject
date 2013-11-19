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

            BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "widgetbg"));
            Size = new ControlSize(15, 15, SizeType.Percent);
            Size.Transform(new ControlSize(screenWidth, screenHeight, SizeType.Fixed));

            Colors = new Colors()
            {
                Background = Color.White,
                Foreground = Color.White
            };

            Position = new ControlPosition(screenWidth - size.Width,
                                           screenHeight - size.Height);
            Position.Margin = new Margin(-15, 0, -15, 0);

            #region Controls init
            itemWrapper = new ItemWrapper();
            this.controlManager.AddControl(itemWrapper);
            inventory.OnItemChanging += new PlayerInventoryEventHandler(inventory_OnItemChanging);
            itemWrapper.Size = new ControlSize(50, 50, SizeType.Percent);
            itemWrapper.Size.Transform(this.size);
            itemWrapper.Position = new ControlPosition(this.size.Width - itemWrapper.Size.Width, 0);
            itemWrapper.Position.Margin = new Margin(-10, 0, 10, 0);
            itemWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));
            
            Label itemLabel = new Label();
            itemLabel.Font = khvGame.Content.Load<SpriteFont>("arial");
            this.controlManager.AddControl(itemLabel);
            itemLabel.Text = "Item";
            itemLabel.Position = new ControlPosition(itemWrapper.Position.Relative.X + itemWrapper.Size.Width / 2 - (int)itemLabel.Font.MeasureString(itemLabel.Text).X / 2,
                                                     itemWrapper.Position.Relative.Y + (int)itemLabel.Font.MeasureString(itemLabel.Text).Y * 2);
            itemLabel.Position.Margin = new Margin(0, 0, 5, 0);

            toolWrapper = new ItemWrapper();
            this.controlManager.AddControl(toolWrapper);
            inventory.OnToolChanging += new PlayerInventoryEventHandler(inventory_OnToolChanging);
            toolWrapper.Size = new ControlSize(50, 50, SizeType.Percent);
            toolWrapper.Size.Transform(this.size);
            toolWrapper.Position = new ControlPosition(0, 0);
            toolWrapper.Position.Margin = new Margin(10, 0, 10, 0);
            toolWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));

            Label toolLabel = new Label();
            toolLabel.Font = khvGame.Content.Load<SpriteFont>("arial");
            this.controlManager.AddControl(toolLabel);
            toolLabel.Text = "Tool";
            toolLabel.Position = new ControlPosition(toolWrapper.Position.Relative.X + toolWrapper.Size.Width / 2 - (int)toolLabel.Font.MeasureString(toolLabel.Text).X / 2,
                                                     toolWrapper.Position.Relative.Y + (int)toolLabel.Font.MeasureString(toolLabel.Text).Y * 2);
            toolLabel.Position.Margin = new Margin(0, 0, 5, 0);
            #endregion

            Owner.OnDestroyed += new GameObjectEventHandler(Owner_OnDestroyed);
        }

        protected override void OnDestory()
        {
            Owner.OnDestroyed -= Owner_OnDestroyed;
            inventory.OnItemChanging -= inventory_OnItemChanging;
            inventory.OnToolChanging -= inventory_OnToolChanging;

            base.OnDestory();
        }

        #region Event handlers
        private void Owner_OnDestroyed(object sender, Khv.Engine.Args.GameEventArgs e)
        {
            Destroy();
        }
        private void inventory_OnToolChanging(object sender, PlayerInventoryEventArgs e)
        {
            toolWrapper.ChangeItem(e.NextItem);
        }
        private void inventory_OnItemChanging(object sender, PlayerInventoryEventArgs e)
        {
            itemWrapper.ChangeItem(e.NextItem);
        }
        #endregion

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2((float)size.Width / (float)BackgroundImage.Width, (float)size.Height / (float)BackgroundImage.Height);
            spriteBatch.Draw(BackgroundImage, Position.Real, null, Colors.Background, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
