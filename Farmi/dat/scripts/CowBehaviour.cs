using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Farmi.Entities.Animals;
using Farmi.Entities.Components;
using System.IO;
using Farmi.Calendar;
using Khv.Maps.MapClasses;
using Khv.Maps.MapClasses.Managers;
using Farmi.Screens;
using Farmi.Entities;

namespace Farmi.dat.scripts
{
    public sealed class CowBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private readonly Texture2D texture;
        private readonly CalendarSystem calendarSystem;
        private readonly WeatherSystem weatherSystem;
        private readonly MapManager mapManager;
        #endregion

        public CowBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
            texture = game.Content.Load<Texture2D>(Path.Combine("Entities", owner.Dataset.AssetName));

            calendarSystem = game.Components.GetGameComponent<CalendarSystem>();
            calendarSystem.OnDayChanged += new CalendarEventHandler(calendarSystem_OnDayChanged);

            weatherSystem = game.Components.GetGameComponent<WeatherSystem>();

            mapManager = (game.GameStateManager.Current as GameplayScreen).World.MapManager;
        }

        #region Event handlers
        private void calendarSystem_OnDayChanged(object sender, CalendarEventArgs e)
        {
            if (owner.MapContainedIn.StartsWith("barnindoors"))
            {
                TileMap barn = mapManager.MapsInBackground().FirstOrDefault(m => m.Name == owner.MapContainedIn) ?? mapManager.ActiveMap;

                List<FeedingTray> traysWithFood = new List<FeedingTray>();
                foreach (GameObjectManager gameObjectManager in barn.ObjectManagers.AllManagers())
                {
                    traysWithFood.AddRange(gameObjectManager.GameObjectsOfType<FeedingTray>(c => c.ContainsFeed));
                }

                if (traysWithFood.Count == 0)
                {
                    Console.WriteLine("Ladossa ei ole safkaa... :(((");
                }
                else
                {
                    Console.WriteLine("{0} lohkoa sisältää ruokaa... :)))", traysWithFood.Count);
                }
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, owner.Position, Color.White);
        }
    }
}
