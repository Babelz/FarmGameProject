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
using Khv.Game.Collision;

namespace Farmi.dat.scripts
{
    public sealed class CowBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private readonly Texture2D texture;
        private readonly CalendarSystem calendarSystem;
        private readonly WeatherSystem weatherSystem;
        private readonly MapManager mapManager;
        private readonly FarmWorld world;

        private readonly TimerWrapper timers;
        private readonly Random random;

        // Timer fields.
        #endregion

        public CowBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
            texture = game.Content.Load<Texture2D>(Path.Combine("Entities", owner.Dataset.AssetName));
            world = (game.GameStateManager.Current as GameplayScreen).World;

            calendarSystem = game.Components.GetGameComponent<CalendarSystem>();
            calendarSystem.OnDayChanged += new CalendarEventHandler(calendarSystem_OnDayChanged);

            weatherSystem = game.Components.GetGameComponent<WeatherSystem>();
            mapManager = (game.GameStateManager.Current as GameplayScreen).World.MapManager;

            timers = new TimerWrapper()
            {
                AutoCreateNewTimers = true
            };
            owner.Components.AddComponent(timers);

            random = new Random();

            brain.PushState(Walk);
            brain.PushState(ChangeDirection);

            owner.Collider.OnCollision += new CollisionEventHandler(Collider_OnCollision);
        }

        #region Brain states
        private void ChangeDirection()
        {
            if (random.Next(-5, 5) > 0)
            {
                while (owner.MotionEngine.GoalVelocityX == 0)
                {
                    owner.MotionEngine.GoalVelocityX = random.Next(-1, 1);
                }
                owner.MotionEngine.GoalVelocityY = 0;
            }
            else
            {
                while (owner.MotionEngine.GoalVelocityY == 0)
                {
                    owner.MotionEngine.GoalVelocityY = random.Next(-1, 1);
                }
                owner.MotionEngine.GoalVelocityX = 0;
            }

            brain.PopState();

            if (brain.CurrentState != Walk)
            {
                brain.PushState(Walk);
            }
        }
        private void Walk()
        {
            if (timers["walk"] > 1500)
            {
                brain.PushState(ChangeDirection);
                brain.PushState(Idle);

                timers.RemoveTimer("walk");
            }
        }
        private void Idle()
        {
            if (timers["idle"] > 5000)
            {
                brain.PopState();

                timers.RemoveTimer("idle");
                owner.MotionEngine.Enabled = true;
            }
            else
            {
                owner.MotionEngine.Enabled = false;
            }
        }
        #endregion

        #region Event handlers
        private void Collider_OnCollision(object sender, CollisionEventArgs result)
        {
            FarmPlayer player = result.CollidingObject as FarmPlayer;

            if (player == null)
            {
                timers.RemoveAllTimers();

                brain.PushState(Idle);
                brain.PushState(ChangeDirection);
            }
        }
        private void calendarSystem_OnDayChanged(object sender, CalendarEventArgs e)
        {
            UpdateWeatherState();

            if (owner.MapContainedIn.StartsWith("barnindoors"))
            {
                SearchFoodFromInDoors();
            }
            else
            {
                SearchFoodFromOutDoors();
            }
        }
        #endregion

        #region State updaters
        private void SearchFoodFromOutDoors()
        {
            if (calendarSystem.CurrentSeason != Season.Winter)
            {
                // TODO: saatiin ruokaa.
            }
            else
            {
                // TODO: ei saatu ruokaa.
            }
        }
        private void SearchFoodFromInDoors()
        {
            TileMap barn = mapManager.MapsInBackground().FirstOrDefault(m => m.Name == owner.MapContainedIn) ?? mapManager.ActiveMap;

            List<FeedingTray> traysWithFood = new List<FeedingTray>();
            foreach (GameObjectManager gameObjectManager in barn.ObjectManagers.AllManagers())
            {
                traysWithFood.AddRange(gameObjectManager.GameObjectsOfType<FeedingTray>(c => c.ContainsFeed));
            }

            if (traysWithFood.Count > 0)
            {
                // TODO: saatiin ruokaa.
            }
            else
            {
                // TODO: ei saatu ruokaa.
            }
        }
        private void UpdateWeatherState()
        {
            switch (weatherSystem.CurrentWeather)
            {
                case Weather.None:
                    break;
                case Weather.Snowy:
                    break;
                case Weather.Sunny:
                    break;
                case Weather.Rainy:
                    break;
                case Weather.Windy:
                    break;
                case Weather.Storm:
                    break;
                default:
                    break;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (owner.Collider.Collides(world.Player))
            {
                if (brain.CurrentState != Idle)
                {
                    timers.RemoveAllTimers();

                    brain.PushState(Idle);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle((int)owner.Position.X, (int)owner.Position.Y, owner.Size.Width, owner.Size.Height);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
