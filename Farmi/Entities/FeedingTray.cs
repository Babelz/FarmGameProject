using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Engine.Structs;
using Farmi.XmlParsers;
using Khv.Game.Collision;
using Farmi.Screens;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Managers;
using Farmi.Entities.Items;
using Farmi.Datasets;
using Farmi.Repositories;
using Farmi.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Farmi.Calendar;
using Khv.Maps.MapClasses;
using Farmi.Entities.Animals;

namespace Farmi.Entities
{
    public sealed class FeedingTray : DrawableGameObject, ILoadableMapObject
    {
        #region Vars
        private FarmWorld world;
        private AnimalFeedItem feed;
        private TextureFader fader;

        private string mapContainedIn;
        #endregion

        #region Properties
        public string FeedType
        {
            get;
            private set;
        }
        public bool ContainsFeed
        {
            get
            {
                return feed != null;
            }
        }
        #endregion

        public FeedingTray(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            CalendarSystem calendar = game.Components.GetGameComponent<CalendarSystem>();
            calendar.OnDayChanged += new CalendarEventHandler(calendar_OnDayChanged);
            InitializeFromMapData(mapObjectArguments);
        }

        protected override void OnDestroy()
        {
            game.Components.GetGameComponent<CalendarSystem>().OnDayChanged -= calendar_OnDayChanged;
        }

        #region Event handlers
        private void calendar_OnDayChanged(object sender, CalendarEventArgs e)
        {
            if (ContainsFeed)
            {
                // Jos pelaaja on tällä kartalla, aloitetaan faderien toisto.
                // Jos faderia ei toisteta, ruoka vain katoaa.
                if (world.MapManager.ActiveMap.Name == mapContainedIn)
                {
                    // Kun fader on saanut itsensä toistettua, ruoka dispostataan jos 
                    // sitä ei ole disposattu aikaisemmin.
                    fader = new TextureFader(feed.Texture, 255, 0, 5, 15);
                    fader.Destination = new Rectangle(
                        (int)feed.Position.X,
                        (int)feed.Position.Y,
                        feed.Size.Width,
                        feed.Size.Height);
                }
                else
                {
                    // Haetaan lato bg mapeista koska se ei ole aktiivisena.
                    TileMap barn = (game.GameStateManager.Current as GameplayScreen)
                        .World.MapManager.MapsInBackground().First(c => c.Name == mapContainedIn);

                    int traysWithFood = 0;
                    int consumers = 0;
                    foreach (GameObjectManager gameObjectManager in barn.ObjectManagers.AllManagers())
                    {
                        // Lasketaan jokaisesta managerista ruokinta astioiden määrä jotka sisältävät ruokaa.
                        traysWithFood += gameObjectManager.GameObjectsOfType<FeedingTray>(
                            f => f.ContainsFeed).Count();

                        // Lasketaan ladossa olevien eläinten määrä jotka voivat syödä tämän astian sisältämää ruokaa.
                        consumers += gameObjectManager.GameObjectsOfType<Animal>(
                            a => a.Dataset.FeedTable.Contains(FeedType)).Count();
                    }

                    // Jos ruokaa on enemmän kuin syöjiä, disposataan ruoka.
                    if (traysWithFood > consumers)
                    {
                        feed = null;
                    }
                }
            }
        }
        #endregion

        #region Initializers
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            MapObjectArgumentReader reader = new MapObjectArgumentReader(mapObjectArguments);

            world = (game.GameStateManager.Current as GameplayScreen).World;

            mapContainedIn = mapObjectArguments.MapContainedIn;
            size = reader.ReadSize();
            position = mapObjectArguments.Origin;
            FeedType = reader.ReadFeedType();

            Collider = new BoxCollider(world, this);

            Components.AddComponent(new FeedingTrayInteractionComponent(this));
        }
        #endregion

        public void InsertFeed(AnimalFeedItem feedItem)
        {
            feed = feedItem;
            feedItem.Size = new Size(feedItem.Size.Width, feedItem.Size.Height / 2);
        }
        public AnimalFeedItem GetFeed()
        {
            AnimalFeedItem feedItem = feed;
            feed = null;

            return feedItem;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collider.Update(gameTime);

            if (fader != null)
            {
                fader.Update(gameTime);
                if (!fader.IsFading)
                {
                    feed = null;
                    fader = null;
                }
            }

            if (feed != null)
            {
                Vector2 feedPosition = new Vector2(this.position.X + feed.Size.Width / 2, this.position.Y);
                feed.Position = feedPosition;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (fader != null)
            {
                fader.Draw(spriteBatch);
            }
            else if (feed != null)
            {
                feed.Draw(spriteBatch);
            }
        }
    }
}
