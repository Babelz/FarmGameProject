using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Farmi.Calendar;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi
{
    public enum Weather
    {
        None,
        Snowy,
        Sunny,
        Rainy,
        Windy,
        Storm
    }

    // TODO: varhainen toteutus.
    public sealed class WeatherSystem : GameComponent
    {
        #region Vars
        private readonly CalendarSystem calendar;
        #endregion

        #region Properties
        public Weather CurrentWeather
        {
            get;
            private set;
        }
        public Texture2D CorrespondingWeatherTexture
        {
            get;
            private set;
        }
        #endregion

        public WeatherSystem(KhvGame khvGame)
            : base(khvGame)
        {
            calendar = khvGame.Components.GetGameComponent<CalendarSystem>();
            CurrentWeather = Weather.None;

            ChangeCorrespondingWeatherTexture();
        }
        private void ChangeCorrespondingWeatherTexture()
        {
            // TODO: testi
            CorrespondingWeatherTexture = Game.Content.Load<Texture2D>("sun");
        }
        public void Initialize()
        {
            // TODO: alustustu, lataa tekstuurit tässä.
        }
        public override void Update(GameTime gameTime)
        {
            // TODO: vaihtaa säätä aina ehdon x jälkeen.
            base.Update(gameTime);
        }
    }
}
