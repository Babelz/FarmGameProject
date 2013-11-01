using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Farmi.World;
using Farmi.Screens;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Entities.Scripts;
using Khv.Scripts.CSharpScriptEngine;
using Khv.Scripts.CSharpScriptEngine.Builders;
using Khv.Game.Collision;

namespace Farmi.Entities.Animals
{
    public sealed class Animal : DrawableGameObject
    {
        #region Vars
        private FarmWorld world;
        private string mapContainedIn;
        #endregion

        #region Vars
        public AnimalBehaviourScript Behaviour
        {
            get;
            private set;
        }
        public AnimalDataset Dataset
        {
            get;
            private set;
        }
        #endregion

        public Animal(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            Initialize(string.Empty, mapObjectArguments);
        }
        public Animal(KhvGame game, string typeName)
            : base(game)
        {
            Initialize(typeName, null);
        }
        public Animal(KhvGame game)
            : base(game)
        {
            Initialize(string.Empty, null);
        }

        private void Initialize(string typeName, MapObjectArguments mapObjectArguments)
        {
            if (mapObjectArguments != null)
            {
                typeName = mapObjectArguments.SerializedData.valuepairs
                    .First(v => v.Name == "Type").Value;

                mapContainedIn = mapObjectArguments.MapContainedIn;
            }

            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;

            RepositoryManager repositoryManager = game.Components
                .First(c => c is RepositoryManager) as RepositoryManager;

            Dataset = repositoryManager.GetDataSet<AnimalDataset>(
                d => d.Type == typeName && d.Name == "Sparky");

            size = Dataset.Size;
            Collider = new BoxCollider(world, this);

            ScriptEngine scriptEngine = game.Components
                .First(c => c is ScriptEngine) as ScriptEngine;

            Behaviour = scriptEngine.GetScript<AnimalBehaviourScript>
                (new ScriptBuilder("DogBehaviour", new object[] { game, this }));
            Behaviour.Initialize();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            Behaviour.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Behaviour.Draw(spriteBatch);
        }
    }
}
