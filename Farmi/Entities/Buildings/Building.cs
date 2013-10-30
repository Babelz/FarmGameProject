using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Farmi.Screens;
using Farmi.World;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Managers;
using SerializedDataTypes.MapObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Processors;
using Farmi.Repositories;
using Farmi.Datasets;

namespace Farmi.Entities.Buildings
{
    internal sealed class Building : DrawableGameObject
    {
        #region Vars
        private Texture2D texture;
        private Color color;
        private FarmWorld world;

        #endregion

        /// <summary>
        /// Muodostin kun ladataan suoraan kartasta olio.
        /// </summary>
        public Building(KhvGame game, MapObjectArguments args)
            : base(game)
        {
            // Pitäs ladata db:stä tietoja jo tässä
            var gameplayScreen = game.GameStateManager.States.Find(c => c is GameplayScreen) as GameplayScreen;
            if (gameplayScreen == null)
                throw new InvalidAsynchronousStateException("Gameplayta ei ole stateissa @ Building.cs consu");
            world = gameplayScreen.World;
            //world.MapManager.OnMapChanged += MapManager_OnMapChanged;
            
            MakeFromMapData(args);
            //Components.Add(new BasicInteractionComponent());
        }

        // Testi metodi initille.
        private void MakeFromMapData(MapObjectArguments args)
        {
            if (args == null)
            {
                position = Vector2.Zero;
            }
            else
            {
                position = args.Origin;
            }

            // Hakee tiedot repoista.
            RepositoryManager repositoryManager = game.Components.First(c => c is RepositoryManager) as RepositoryManager;
            BuildingDataset dataset = repositoryManager.GetDataSet<BuildingDataset>(s => s.Name == args.SerializedData.valuepairs[1].Value);
            
            if (dataset != null)
            {
                texture = game.Content.Load<Texture2D>(@"Buildings\" + dataset.AssetName);
                size = dataset.Size;
                color = Color.White;
                Doors = new Door[dataset.Doors.Length];
                for (int doorIndex = 0; doorIndex < dataset.Doors.Length; doorIndex++)
                {
                    var doorDataset = dataset.Doors[doorIndex];
                    Door door = new Door(game, this, doorDataset);
                    Doors[doorIndex] = door;
                    
                    
                }
                world.WorldObjects.AddGameObjects(Doors);
                // heitetään buildingin collision boxeista pois ovet
                
            }
            else
            {
                Doors = new Door[0];
                size = new Size(128, 64);
                texture = KhvGame.Temp;
                color = Color.Brown;
            }

            Collider = new BoxCollider(null, this);
        }

        public void InitializeFromData(string datasetName)
        {
            // Alustaa otuksen datasta tässä.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Collider.Update(gameTime);
            Array.ForEach(Doors, door => door.Update(gameTime));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), color);
            foreach (var door in Doors)
            {
                door.Draw(spriteBatch);
            }
        }

        public Door[] Doors
        {
            get;
            private set;
        }
    }
}
