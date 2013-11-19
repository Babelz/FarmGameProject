using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Gui.Components.BaseComponents;
using Farmi.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;
using Khv.Gui.Components;

namespace Farmi.HUD
{
    public sealed class ItemWrapper : Control
    {
        #region Vars
        private Item currentItem;
        #endregion

        public ItemWrapper()
        {
            Colors = new Colors()
            {
                Foreground = Color.White,
                Background = Color.White
            };
        }
        public void ChangeItem(Item next)
        {
            currentItem = next;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (currentItem == null)
            {
                spriteBatch.Draw(KhvGame.Temp, ClientArea, Color.Transparent);
            }
            else
            {
                currentItem.DrawToInventory(spriteBatch, new Vector2(Position.Real.X, Position.Real.Y),
                                                         new Size(size.Height, size.Width));
            }
        }
    }
}
