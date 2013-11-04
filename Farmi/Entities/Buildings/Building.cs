using System;
using System.Linq;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Screens;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.MapClasses.Processors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Farmi.Entities.Buildings
{
    public sealed class Building : DrawableGameObject, ILoadableMapObject, ILoadableRepositoryObject<BuildingDataset>
    {
        #region Vars
        private Texture2D texture;
        private Color color;
        private FarmWorld world;
        private string mapContainedIn;
        #endregion

        #region Properties
        public Door[] Doors
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Muodostin kun ladataan suoraan kartasta olio.
        /// </summary>
        public Building(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }
        public Building(KhvGame game, BuildingDataset buildingDataset)
            : base(game)
        {
            InitializeFromDataset(buildingDataset);
        }

        protected override void OnDestroy()
        {
            world.WorldObjects.SafelyRemove<Door>(Doors);
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, MapEventArgs e)
        {
            if (e.Current.Name == mapContainedIn)
            {
                world.WorldObjects.SafelyRemove<Door>(Doors);
            }
            else
            {
                world.WorldObjects.SafelyAddMany(Doors);
            }
        }
        #endregion

        #region Initializers
        private void GetWorld()
        {
            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;
        }
        private void Initialize(BuildingDataset dataset)
        {
            if (dataset != null)
            {
                texture = game.Content.Load<Texture2D>(Path.Combine(@"Buildings" ,dataset.AssetName));
                size = dataset.Size;
                color = Color.White;
                Doors = new Door[dataset.Doors.Length];

                for (int doorIndex = 0; doorIndex < dataset.Doors.Length; doorIndex++)
                {
                    var doorDataset = dataset.Doors[doorIndex];
                    Door door = new Door(game, this, doorDataset, mapContainedIn);
                    Doors[doorIndex] = door;
                }

                world.WorldObjects.AddGameObjects(Doors);
            }
            else
            {
                Doors = new Door[0];
                size = new Size(128, 64);
                texture = KhvGame.Temp;
                color = Color.Brown;
            }

            Collider = new BoxCollider(world, this);
        }

        public void InitializeFromDataset(BuildingDataset dataset)
        {
            GetWorld();

            mapContainedIn = world.MapManager.ActiveMap.Name;

            Initialize(dataset);
        }
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            GetWorld();

            mapContainedIn = mapObjectArguments.MapContainedIn;

            position = mapObjectArguments.Origin;

            BuildingDataset dataset = (game.Components.First(
                c => c is RepositoryManager) as RepositoryManager).GetDataSet<BuildingDataset>(
                d => d.Name == mapObjectArguments.SerializedData.valuepairs[1].Value);

            Initialize(dataset);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Collider.Update(gameTime);

            Array.ForEach<Door>(Doors, 
                d => d.Update(gameTime));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), color);

            Array.ForEach<Door>(Doors, 
                d => d.Draw(spriteBatch));
        }
    }
}
