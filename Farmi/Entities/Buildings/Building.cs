using System;
using System.Linq;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Screens;
using Farmi.World;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.MapClasses.Processors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Buildings
{
    internal sealed class Building : DrawableGameObject
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
            Initialize(mapObjectArguments);
        }
        public Building(KhvGame game)
            : base(game)
        {
            Initialize(null);
        }

        // Testi metodi initille.
        private void Initialize(MapObjectArguments mapObjectArguments)
        {
            world = (game.GameStateManager.States
                .First(c => c is GameplayScreen) as GameplayScreen).World;

            if (mapObjectArguments == null)
            {
                position = Vector2.Zero;
            }
            else
            {
                position = mapObjectArguments.Origin;
            }

            world.MapManager.OnMapChanged += new MapEventHandler(MapManager_OnMapChanged);

            // Hakee tiedot repoista.
            RepositoryManager repositoryManager = game.Components
                .First(c => c is RepositoryManager) as RepositoryManager;

            BuildingDataset dataset = repositoryManager.GetDataSet<BuildingDataset>(
                s => s.Name == mapObjectArguments.SerializedData.valuepairs[1].Value);
            
            if (dataset != null)
            {
                texture = game.Content.Load<Texture2D>(@"Buildings\" + dataset.AssetName);
                size = dataset.Size;
                color = Color.White;
                Doors = new Door[dataset.Doors.Length];

                for (int doorIndex = 0; doorIndex < dataset.Doors.Length; doorIndex++)
                {
                    var doorDataset = dataset.Doors[doorIndex];
                    Door door = new Door(game, this, doorDataset, mapObjectArguments.MapContainedIn);
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
