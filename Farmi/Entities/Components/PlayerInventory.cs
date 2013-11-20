using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Engine;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Args;

namespace Farmi.Entities.Components
{
    public sealed class PlayerInventory : IDrawableObjectComponent
    {
        #region Vars
        private int itemIndex;
        private readonly List<Item> items;

        private int toolIndex;
        private readonly List<Tool> tools;

        private readonly FarmPlayer player;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun aktiivinen tooli ollaan vaihtamassa.
        /// </summary>
        public event PlayerInventoryEventHandler OnToolChanging;

        /// <summary>
        /// Laukaistaan kun aktiivinen itemi ollaan vaihtamassa.
        /// </summary>
        public event PlayerInventoryEventHandler OnItemChanging;
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
        public int DrawOrder
        {
            get;
            set;
        }
        #endregion

        public PlayerInventory(FarmPlayer player)
        {
            this.player = player;

            DrawOrder = 0;

            items = new List<Item>();
            tools = new List<Tool>();
        }

        #region Event callers
        private void CallOnItemChaning(Item nextItem)
        {
            if (OnItemChanging != null)
            {
                OnItemChanging(this, new PlayerInventoryEventArgs(ItemInHands, nextItem));
            }
        }
        private void CallOnToolChanging(Tool nextTool)
        {
            if (OnToolChanging != null)
            {
                OnToolChanging(this, new PlayerInventoryEventArgs(SelectedTool, nextTool));
            }
        }
        #endregion

        public void NextItem()
        {
            itemIndex++;
            if (itemIndex >= items.Count)
            {
                itemIndex = 0;
            }

            Item nextItem = items[itemIndex];
            CallOnItemChaning(nextItem);

            ItemInHands = nextItem;
        }
        public void PreviousItem()
        {
            itemIndex--;
            if (itemIndex < 0)
            {
                itemIndex = items.Count - 1;
            }

            Item nextItem = items[itemIndex];
            CallOnItemChaning(nextItem);

            ItemInHands = nextItem;
        }

        public void NextTool()
        {
            toolIndex++;
            if (toolIndex >= tools.Count)
            {
                toolIndex = 0;
            }

            Tool nextTool = tools[toolIndex];
            CallOnToolChanging(nextTool);

            SelectedTool = nextTool;
        }
        public void PreviousTool()
        {
            toolIndex--;
            if (toolIndex < 0)
            {
                toolIndex = tools.Count - 1;
            }

            Tool nextTool = tools[toolIndex];
            CallOnToolChanging(nextTool);

            SelectedTool = nextTool;
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
                CallOnItemChaning(item);

                items.Add(item);
                ItemInHands = item;
            }
        }
        public Item ThrowItem()
        {
            Item item = null;

            if (HasItemInHands)
            {
                CallOnItemChaning(null);

                items.Remove(ItemInHands);
                item = ItemInHands;
                ItemInHands = null;
            }

            return item;
        }

        public void Update(GameTime gametime)
        {
            if (HasToolSelected)
            {
                SelectedTool.Update(gametime);
            }
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

    public delegate void PlayerInventoryEventHandler(object sender, PlayerInventoryEventArgs e);

    public class PlayerInventoryEventArgs : GameEventArgs
    {
        #region Properties
        public Item CurrentItem
        {
            get;
            private set;
        }
        public Item NextItem
        {
            get;
            private set;
        }
        #endregion

        public PlayerInventoryEventArgs(Item currentItem, Item nextItem)
        {
            CurrentItem = currentItem;
            NextItem = nextItem;
        }
    }
}
