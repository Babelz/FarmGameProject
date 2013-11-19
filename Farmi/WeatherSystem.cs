using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Farmi.Calendar;

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
    public sealed class WeatherSystem : IGameComponent
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
        #endregion

        public WeatherSystem(KhvGame khvGame)
        {
            calendar = khvGame.Components.GetGameComponent<CalendarSystem>();
            CurrentWeather = Weather.None;
        }

        public void Initialize()
        {
        }
    }
}
