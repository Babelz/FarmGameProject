using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;

namespace Khv.Input
{
    public interface IMoveComponent
    {
        float GoalVelocityX
        {
            get;
            set;
        }

        float GoalVelocityY
        {
            get;
            set;
        }

        Vector2 GoalVelocity
        {
            get;
            set;
        }
    }
}
