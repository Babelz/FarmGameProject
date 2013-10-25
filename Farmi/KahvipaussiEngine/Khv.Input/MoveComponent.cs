using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Input
{
    public class BaseMoveComponent : IMoveComponent
    {
        #region Vars
        private Vector2 goalVelocity;
        #endregion

        #region Properties
        public float GoalVelocityX
        {
            get
            {
                return goalVelocity.X;
            }
            set
            {
                goalVelocity.X = value;
            }
        }

        public float GoalVelocityY
        {
            get
            {
                return goalVelocity.Y;
            }
            set
            {
                goalVelocity.Y = value;
            }
        }

        public Vector2 GoalVelocity
        {
            get { return goalVelocity; }
            set { goalVelocity = value; }
        }
        #endregion

        protected BaseMoveComponent()
        {
            goalVelocity = Vector2.Zero;
        }
        public void Update(GameTime gametime)
        {
            
        }
    }
}
