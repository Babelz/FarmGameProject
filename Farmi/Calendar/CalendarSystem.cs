using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine.Args;
using Farmi.Entities;
using Khv.Engine;

namespace Farmi.Calendar
{
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    internal sealed class CalendarSystem : GameComponent
    {
        #region Vars
        // Kaikki tämän kuukauden kalenteri merkinnät.
        private readonly List<Calendarentry> calendarentries;
        private readonly int clockTickFrequency;
        private const int daysInSeason = 30;

        private double elapsed;
        private int nextDayOfMonth;
        #endregion

        #region Events
        /// <summary>
        /// Laukaistaan kun konsussa annettu tick arvo tulee vastaan.
        /// </summary>
        public event CalendarEventHandler OnClockTick;

        /// <summary>
        /// Laukaistaan sillon kun päivä vaihtuu.
        /// </summary>
        public event CalendarEventHandler OnDayChanged;

        /// <summary>
        /// Laukaistaan sillon kun vuosi vaihtuu.
        /// </summary>
        public event CalendarEventHandler OnYearChanged;

        /// <summary>
        /// Laukaistaan kun season vaihtuu.
        /// </summary>
        public event CalendarEventHandler OnSeasonChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Tämän hetkinen aika.
        /// </summary>
        public DateTime CurrentTime
        {
            get;
            private set;
        }
        /// <summary>
        /// Tämän hetkinen päivä.
        /// </summary>
        public DayOfWeek CurrentDay
        {
            get;
            private set;
        }
        /// <summary>
        /// Tämän hetkinen season.
        /// </summary>
        public Season CurrentSeason
        {
            get;
            private set;
        }
        /// <summary>
        /// Monesko päivä kuukaudesta nyt on.
        /// </summary>
        public int CurrentDayOfMonth
        {
            get;
            private set;
        }
        /// <summary>
        /// Monesko vuosi nyt on.
        /// </summary>
        public int CurrentYear
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Alustaa uuden kalenteri järjestelmän.
        /// </summary>
        /// <param name="game">Khv game instanssi.</param>
        /// <param name="clockTickFrequency">Aika sekunneissa kuinka usein kellon aikaa päivitetään.</param>
        public CalendarSystem(KhvGame game, int clockTickFrequency)
            : base(game)
        {
            this.clockTickFrequency = clockTickFrequency;
        }

        // Päivittää seasonin ja vuoden valueita.
        // Laukoo myös eventit.
        private void UpdateSeason()
        {
            if (CurrentDayOfMonth == 1)
            {
                // Jos ei olla vuoden viimeisessä seasonissa niin lisätään seasonia yhdellä.
                if (CurrentSeason < Enum.GetValues(typeof(Season)).Cast<Season>().Max())
                {
                    CurrentSeason++;
                }
                else
                {
                    // Aloitetaan uusi vuosi.
                    CurrentYear++;
                    CurrentSeason = 0;

                    if (OnYearChanged != null)
                    {
                        OnYearChanged(this, new CalendarEventArgs());
                    }
                }
                if (OnSeasonChanged != null)
                {
                    OnSeasonChanged(this, new CalendarEventArgs());
                }
            }
        }

        // Päivittää viikonpäivän valueta.
        // Laukoo myös eventit.
        private void UpdateDay()
        {
            // Jos nyt on viikon viimeinen päivä, aloittaa uuden viikon.
            if (CurrentDay < Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Max())
            {
                CurrentDay++;
            }
            else
            {
                CurrentDay = 0;
            }

            if (OnDayChanged != null)
            {
                OnDayChanged(this, new CalendarEventArgs());
            }

            // Koska vaihdettiin päivää, alustetaan kello uudelleen.
            CurrentTime = new DateTime();
        }

        // Päivittää kuukauden päivän valueita.
        private void UpdateMonth()
        {
            // Jos nyt on kuun viimeinen päivä, aloittaa uuden kuukauden.
            if (CurrentDayOfMonth == daysInSeason)
            {
                CurrentDayOfMonth = 1;
                nextDayOfMonth = 2;
            }
            else
            {
                CurrentDayOfMonth++;
                nextDayOfMonth++;
            }
        }

        /// <summary>
        /// Alustaa kalenteri järjestelmän.
        /// </summary>
        public override void Initialize()
        {
            CurrentYear = 1;
            CurrentDay = 0;
            CurrentTime = new DateTime().AddHours(7);

            CurrentDayOfMonth = 1;
            nextDayOfMonth = 2;

            base.Initialize();
        }
        /// <summary>
        /// Päivittää kalenteri järsejtelmän aikaa, päivää ja seasonia
        /// jos komponentti on enabloitu.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                elapsed += gameTime.ElapsedGameTime.TotalSeconds;

                // Jos ollaan mitattu tarpeeksi aikaa, lisätään 
                // se kelloon laukaistaan eventti.
                if (elapsed >= clockTickFrequency)
                {
                    CurrentTime = CurrentTime.AddMinutes(elapsed);
                    elapsed = 0;

                    if (OnClockTick != null)
                    {
                        OnClockTick(this, new CalendarEventArgs());
                    }
                }

                // Jos päivä on muuttunut, päivitetään kalenterin arvot.
                if (CurrentTime.Day == 2)
                {
                    UpdateDay();
                    UpdateMonth();
                    UpdateSeason();
                }
                
                base.Update(gameTime);
            }
        }
        /// <summary>
        /// Menee seuraavaan päivään ja haluttuun kellon aikaan.
        /// </summary>
        public void SkipDay(int hours, int minutes)
        {
            elapsed = 0;

            UpdateDay();
            UpdateMonth();
            UpdateSeason();

            CurrentTime = new DateTime().AddHours(hours).AddMinutes(minutes);
        }
        /// <summary>
        /// Palauttaa ajasta display stringin.
        /// </summary>
        /// <returns></returns>
        public string GetTimeDisplayString()
        {
            return CurrentTime.ToShortTimeString();
        }
        /// <summary>
        /// Palauttaa päivästä, seasonista ja vuodesta display stringin.
        /// </summary>
        /// <returns></returns>
        public string GetDateDisplayString()
        {
            return string.Format("{0} : d: {1} - y: {2} - dow: {3}", CurrentSeason, CurrentDayOfMonth, CurrentYear, CurrentDay);
        }

        public void Import(GameDataImporter gameDataImporter)
        {
            throw new NotImplementedException();
        }
        public void Export(GameDataExporter gameDataExporter)
        {
            throw new NotImplementedException();
        }
    }
    
    public delegate void CalendarEventHandler(object sender, CalendarEventArgs e);

    public class CalendarEventArgs : GameEventArgs
    {
    }
}
