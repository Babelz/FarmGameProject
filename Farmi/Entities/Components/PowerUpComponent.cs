using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    internal sealed class PowerUpComponent : IObjectComponent
    {
        #region Vars
        private int elapsed;
        private readonly Tool tool;
        #endregion

        #region Properties
        public float MinPow
        {
            get;
            private set;
        }
        public int MaxPow
        {
            get;
            private set;
        }
        public int CurrentPow
        {
            get;
            private set;
        }
        public bool Enabled
        {
            get;
            private set;
        }
        public int PowTimestep
        {
            get;
            private set;
        }
        public bool IsMinimiumMet
        {
            get
            {
                return CurrentPow >= MinPow;
            }
        }
        public bool IsMaximumMet
        {
            get
            {
                return CurrentPow >= MaxPow;
            }
        }
        #endregion

        public PowerUpComponent(Tool tool, int minPower, int maxPower, int powTimestep)
        {
            this.tool = tool;

            MinPow = minPower;
            MaxPow = maxPower;
            PowTimestep = powTimestep;
            
            Disable();
        }

        public void Disable()
        {
            CurrentPow = 0;
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Update(GameTime gametime)
        {
            if (!Enabled)
            {
                return;
            }

            if (CurrentPow < MaxPow)
            {
                elapsed += gametime.ElapsedGameTime.Milliseconds;

                if (elapsed > PowTimestep)
                {
                    CurrentPow++;
                    elapsed = 0;
                }
            }
            else
            {
                Enabled = false;
            }
        }
    }
}
