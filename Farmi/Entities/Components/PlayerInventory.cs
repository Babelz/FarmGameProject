using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Engine;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Components
{
    internal sealed class PlayerInventory : IDrawableObjectComponent
    {
        #region Vars
        private int itemIndex;
        private List<Item> items;

        private int toolIndex;
        private List<Tool> tools;

        private FarmPlayer player;
        #endregion

        #region Properties
        public Item ItemInHands
        {
            get;
            private set;
        }
        public bool HasItemInHands
        {
            get
            {
                return ItemInHands != null;
            }
        }
        public Tool SelectedTool
        {
            get;
            private set;
        }
        public bool HasToolSelected
        {
            get
            {
                return SelectedTool != null;
            }
        }
        #endregion

        public PlayerInventory(FarmPlayer player)
        {
            this.player = player;

            items = new List<Item>();
            tools = new List<Tool>();
        }
        public void NextItem()
        {
            itemIndex++;
            if (itemIndex >= items.Count)
            {
                itemIndex = 0;
            }

            ItemInHands = items[itemIndex];
        }
        public void PreviousItem()
        {
            itemIndex--;
            if (itemIndex < 0)
            {
                itemIndex = items.Count;
            }

            ItemInHands = items[itemIndex];
        }
        public void NextTool()
        {
            toolIndex++;
            if (toolIndex >= tools.Count)
            {
                toolIndex = 0;
            }

            SelectedTool = tools[toolIndex];
        }
        public void PreviousTool()
        {
            toolIndex--;
            if (toolIndex < tools.Count)
            {
                toolIndex = items.Count;
            }

            SelectedTool = tools[toolIndex];
        }
        public void AddToInventory(Item item)
        {
            if (HasItemInHands)
            {
                return;
            }

            Tool tool = item as Tool;

            if (tool != null)
            {
                tools.Add(tool);
            }
            else
            {
                items.Add(item);
                ItemInHands = item;
            }
        }
        public Item ThrowItem()
        {
            Item item = null;

            if (HasItemInHands)
            {
                items.Remove(ItemInHands);
                item = ItemInHands;
                ItemInHands = null;
            }

            return item;
        }

        public void Update(GameTime gametime)
        {
            if (HasToolSelected)
                SelectedTool.Update(gametime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasItemInHands)
            {
                ItemInHands.Position = new Vector2(player.Position.X, player.Position.Y - ItemInHands.Size.Height);
                ItemInHands.Draw(spriteBatch);
            }
        }
    }
}
