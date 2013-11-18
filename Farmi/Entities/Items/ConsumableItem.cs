using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Items.Components;
using Khv.Engine;
using Microsoft.Xna.Framework.Graphics;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;
using Farmi.Entities.Scripts;
using Khv.Scripts.CSharpScriptEngine;
using Khv.Scripts.CSharpScriptEngine.Builders;
using Microsoft.Xna.Framework;
using Khv.Engine.Structs;
using System.IO;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa itemiä jonka voi syödä. Jokaista consumable
    /// itemiä voidaan heittää.
    /// </summary>
    public sealed class ConsumableItem : Item, ILoadableRepositoryObject<ConsumableItemDataset>
    {
        #region Properties
        public ItemScript Script
        {
            get;
            private set;
        }
        public int MaxStamina
        {
            get;
            private set;
        }
        public int Stamina
        {
            get;
            private set;
        }
        #endregion

        public ConsumableItem(KhvGame game, ConsumableItemDataset itemDataset) 
            : base(game)
        {
            Components.AddComponent(new ThrowableComponent(this));
            InitializeFromDataset(itemDataset);
        }

        #region Initializers
        public void InitializeFromDataset(ConsumableItemDataset dataset)
        {
            Name = dataset.Name;

            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
            Description = dataset.Description;

            MaxStamina = dataset.AddedStamina;
            Stamina = dataset.RecoveredStamina;

            if (dataset.Size.Width == 0 && dataset.Size.Height == 0)
            {
                size = new Size(32, 32);
            }
            else
            {
                size = dataset.Size;
            }

            if (!string.IsNullOrEmpty(dataset.Script))
            {
                ScriptEngine scriptEngine = game.Components.First(
                    c => c is ScriptEngine) as ScriptEngine;

                Script = scriptEngine.GetScript<ItemScript>(
                    new ScriptBuilder(dataset.Script, new object[] { game, this }));
            }
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}
