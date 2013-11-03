using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Items;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Farmi.Entities.Components
{
    class PowerUpComponent : IObjectComponent
    {
        public float MinPower { get; private set; }
        public float MaxPower { get; private set; }
        public float CurrentPower { get; private set; }
        public bool Enabled { get; private set; }
        public float PowerStep { get; private set; }


        

        private readonly Tool tool;

        public PowerUpComponent(Tool tool, float minPower, float maxPower)
        {
            MinPower = minPower;
            MaxPower = maxPower;
            this.tool = tool;
            PowerStep = 1f;
            Reset();
        }

        public void Reset()
        {
            CurrentPower = 0f;
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Update(GameTime gametime)
        {
            if (!Enabled)
                return;
            if (CurrentPower < MaxPower)
                CurrentPower += PowerStep;
            else
                Enabled = false;
        }

        public bool IsMinimiumMet
        {
            get
            {
                return CurrentPower >= MinPower;
            }
        }

        public bool IsMaximumMet
        {
            get
            {
                return CurrentPower >= MaxPower;
            }
        }
    }
}
