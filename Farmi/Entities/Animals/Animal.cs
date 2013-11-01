using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Farmi.Screens;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Entities.Scripts;
using Khv.Scripts.CSharpScriptEngine;
using Khv.Scripts.CSharpScriptEngine.Builders;
using Khv.Game.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Managers;
using Khv.Game;
using Farmi.KahvipaussiEngine.Khv.Game.Collision;
using Farmi.Entities.Components;

namespace Farmi.Entities.Animals
{
    public sealed class Animal : DrawableGameObject
    {
        #region Vars
        private FarmWorld world;
        #endregion

        #region Vars
        public MotionEngine MotionEngine
        {
            get;
            private set;
        }
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
        public string MapContainedIn
        {
            get;
            set;
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

                MapContainedIn = mapObjectArguments.MapContainedIn;
            }

            MotionEngine = new MotionEngine(this);

            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;

            world.MapManager.OnMapChanged += new Khv.Maps.MapClasses.Managers.MapEventHandler(MapManager_OnMapChanged);

            RepositoryManager repositoryManager = game.Components
                .First(c => c is RepositoryManager) as RepositoryManager;

            Dataset = repositoryManager.GetDataSet<AnimalDataset>(
                d => d.Type == typeName && d.Name == "Sparky");

            size = Dataset.Size;
            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());

            ScriptEngine scriptEngine = game.Components
                .First(c => c is ScriptEngine) as ScriptEngine;

            Behaviour = scriptEngine.GetScript<AnimalBehaviourScript>
                (new ScriptBuilder("DogBehaviour", new object[] { game, this }));

            Behaviour.Initialize();
        }

        private void MapManager_OnMapChanged(object sender, MapEventArgs e)
        {
            if (e.Current.Name == MapContainedIn)
            {
                world.WorldObjects.SafelyAdd(this);
            }
            else
            {
                world.WorldObjects.SafelyRemove(this);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collider.Update(gameTime);
            MotionEngine.Update(gameTime);
            Behaviour.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Behaviour.Draw(spriteBatch);
        }
    }
}
