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
using Farmi.Entities.Components;

namespace Farmi.Entities.Animals
{
    public sealed class Animal : DrawableGameObject, ILoadableMapObject, ILoadableRepositoryObject<AnimalDataset>
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
            InitializeFromMapData(mapObjectArguments);
        }
        public Animal(KhvGame game, AnimalDataset animalDataset)
            : base(game)
        {
            Dataset = animalDataset;

            InitializeFromDataset(animalDataset);
        }

        protected override void OnDestroy()
        {
            world.MapManager.OnMapChanged -= MapManager_OnMapChanged;
        }

        #region Event handlers
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
        #endregion

        #region Initializers
        private void Initialize(AnimalDataset dataset)
        {
            MotionEngine = new MotionEngine(this);

            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;

            world.MapManager.OnMapChanged += new MapEventHandler(MapManager_OnMapChanged);

            RepositoryManager repositoryManager = game.Components
                .First(c => c is RepositoryManager) as RepositoryManager;

            size = Dataset.Size;
            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());

            ScriptEngine scriptEngine = game.Components
                .First(c => c is ScriptEngine) as ScriptEngine;

            scriptEngine.MakeScript<AnimalBehaviourScript>(
                new ParallelScriptBuilder(
                    "DogBehaviour",
                    new object[] { game, this },
                    this,
                    (script, builder) =>
                    {
                        Behaviour = script as AnimalBehaviourScript;

                        Behaviour.Initialize();
                    }));
        }
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            string typeName = mapObjectArguments.SerializedData.valuepairs
                .First(v => v.Name == "Type").Value;

            Dataset = (game.Components.First(c => c is RepositoryManager) as RepositoryManager)
                .GetDataSet<AnimalDataset>(d => d.Type == typeName);

            Initialize(Dataset);

            MapContainedIn = mapObjectArguments.MapContainedIn;
        }
        public void InitializeFromDataset(AnimalDataset dataset)
        {
            Initialize(dataset);

            MapContainedIn = world.MapManager.ActiveMap.Name;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);
            
            if (Behaviour != null)
            {
                Behaviour.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Behaviour != null)
            {
                Behaviour.Draw(spriteBatch);
            }
        }
    }
}
