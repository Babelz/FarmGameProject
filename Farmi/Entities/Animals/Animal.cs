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

        #region Properties
        public AnimalDataset Dataset
        {
            get;
            private set;
        }
        public MotionEngine MotionEngine
        {
            get;
            private set;
        }
        public ScriptObserver<AnimalBehaviourScript> BehaviourObserver
        {
            get;
            private set;
        }
        public AnimalBehaviourScript Behaviour
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

        #region Event handlers
        private void mapLocator_ContainingMapActive(object sender, MapLocatorEventArgs e)
        {
            world.WorldObjects.MoveToForeground(this);
        }
        private void mapLocator_ContainingMapChanged(object sender, MapLocatorEventArgs e)
        {
            world.WorldObjects.MoveToBackground(this);
        }
        #endregion

        #region Initializers
        private void Initialize(AnimalDataset dataset)
        {
            MotionEngine = new MotionEngine(this);

            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;

            RepositoryManager repositoryManager = game.Components
                .First(c => c is RepositoryManager) as RepositoryManager;

            size = Dataset.Size;
            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());

            ScriptEngine scriptEngine = game.Components
                .First(c => c is ScriptEngine) as ScriptEngine;

            ScriptBuilder builder = new ScriptBuilder(dataset.Behaviours.First(), new object[] { game, this });
            Behaviour = scriptEngine.GetScript<AnimalBehaviourScript>(builder);
            BehaviourObserver = new ScriptObserver<AnimalBehaviourScript>(Behaviour, builder);
        }
        private void InitializeLocator()
        {
            MapLocator mapLocator = new MapLocator(world, this, MapContainedIn);
            mapLocator.ContainingMapActive += new MapLocatorEventHandler(mapLocator_ContainingMapActive);
            mapLocator.ContainingMapChanged += new MapLocatorEventHandler(mapLocator_ContainingMapChanged);
            Components.AddComponent(mapLocator);
        }
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            string typeName = mapObjectArguments.SerializedData.valuepairs
                .First(v => v.Name == "Type").Value;

            Dataset = (game.Components.First(c => c is RepositoryManager) as RepositoryManager)
                .GetDataSet<AnimalDataset>(d => d.Type == typeName);

            Initialize(Dataset);

            MapContainedIn = mapObjectArguments.MapContainedIn;

            InitializeLocator();
        }
        public void InitializeFromDataset(AnimalDataset dataset)
        {
            Initialize(dataset);

            MapContainedIn = world.MapManager.ActiveMap.Name;

            InitializeLocator();
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);

            if (BehaviourObserver.HasScript)
            {
                BehaviourObserver.Script.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (BehaviourObserver.HasScript)
            {
                BehaviourObserver.Script.Draw(spriteBatch);
            }
        }
    }
}
