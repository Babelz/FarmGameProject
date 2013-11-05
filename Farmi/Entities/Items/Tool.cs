using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Components;
using Farmi.Repositories;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Structs;
using Farmi.Entities.Scripts;
using Khv.Scripts.CSharpScriptEngine;
using Khv.Scripts.CSharpScriptEngine.Builders;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa työkalua jota voi käyttää
    /// </summary>
    public sealed class Tool : Item, ILoadableRepositoryObject<ToolDataset>
    {
        #region Properties
        public ToolBehaviourScript Behaviour
        {
            get;
            private set;
        }
        public ToolDataset Dataset
        {
            get;
            private set;
        }
        #endregion

        public Tool(KhvGame game, ToolDataset dataset)
            : base(game)
        {
            Dataset = dataset;

            InitializeFromDataset(dataset);
        }

        public void InitializeFromDataset(ToolDataset dataset)
        {
           // Texture = game.Content.Load<Texture2D>(Path.Combine("Tools", dataset.AssetName));

            PowerUpComponent powComponent = new PowerUpComponent(this,
                dataset.MinPow,
                dataset.MaxPow,
                dataset.PowTimestep);
            //TODO temp
            Size = new Size(32,32);
            Components.Add(powComponent);

            ScriptEngine scriptEngine = game.Components.First(
                c => c is ScriptEngine) as ScriptEngine;

            Behaviour = scriptEngine.GetScript<ToolBehaviourScript>(
                new ScriptBuilder(dataset.Behaviour, new object[] { game, this }));
            Behaviour.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Behaviour.Update(gameTime);
        }

        public override void DrawToInventory(SpriteBatch spriteBatch, Vector2 position, Size size)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);

            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            return;
            base.Draw(spriteBatch);
            Behaviour.Draw(spriteBatch);
        }
    }
}
