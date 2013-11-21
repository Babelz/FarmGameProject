using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Game.GameObjects;

namespace Farmi.Entities.Components
{
    public sealed class TimerWrapper : IUpdatableObjectComponent
    {
        #region Private Keypair class
        private class Keypair<TKey, TValue>
        {
            #region Properties
            public TKey Key
            {
                get;
                private set;
            }
            public TValue Value
            {
                get;
                set;
            }
            #endregion

            public Keypair(TKey key, TValue startValue)
            {
                Key = key;
                Value = startValue;
            }
        }
        #endregion

        #region Vars
        private readonly List<Keypair<string, int>> timers;
        #endregion

        #region Properties
        /// <summary>
        /// Jos arvo on true ja haluttua timeriä ei löydy, luo uuden timerin automaattisesti.
        /// </summary>
        public bool AutoCreateNewTimers
        {
            get;
            set;
        }
        /// <summary>
        /// Palauttaa timer listasta annetulla keyllä timerin.
        /// </summary>
        public int this[string key]
        {
            get
            {
                Keypair<string, int> keypair = TryGetKeypair(key);
                
                return keypair.Value;
            }
            set
            {
                Keypair<string, int> keypair = TryGetKeypair(key);

                keypair.Value = value;
            }
        }
        #endregion

        public TimerWrapper()
        {
            timers = new List<Keypair<string, int>>();
        }

        private Keypair<string, int> TryGetKeypair(string key)
        {
            Keypair<string, int> keypair = GetKeypair(key);

            if (keypair == null)
            {
                throw new KeyNotFoundException("Avaimella " + key + " ei löytynyt timeriä.");
            }
            else
            {
                return keypair;
            }
        }
        private Keypair<string, int> GetKeypair(string key)
        {
            Keypair<string, int> keypair = timers.FirstOrDefault(c => c.Key == key);

            if (keypair == null && AutoCreateNewTimers)
            {
                timers.Add(keypair = new Keypair<string, int>(key, 0));
            }

            return keypair;
        }

        public bool AddTimer(string key, int startValue = 0)
        {
            bool results = false;

            if((timers.FirstOrDefault(t => t.Key == key) == null))
            {
                timers.Add(new Keypair<string, int>(key, startValue));
                results = true;
            }

            return results;
        }
        public bool RemoveTimer(string key)
        {
            Keypair<string, int> keypair = timers.FirstOrDefault(c => c.Key == key);

            if (keypair != null)
            {
                timers.Remove(keypair);
            }

            return keypair != null;
        }
        public void RemoveAllTimers()
        {
            timers.Clear();
        }
       
        public int ReadTimerValue(string key)
        {
            return GetKeypair(key).Value;
        }
        public void ResetTimervalue(string key)
        {
            Keypair<string, int> keypair = TryGetKeypair(key);
            keypair.Value = 0;
        }
        public void ResetAllTimers()
        {
            timers.ForEach(t => t.Value = 0);
        }

        public void Update(GameTime gameTime)
        {
            timers.ForEach(c =>
                {
                    c.Value += gameTime.ElapsedGameTime.Milliseconds;
                });
        }
    }
}
