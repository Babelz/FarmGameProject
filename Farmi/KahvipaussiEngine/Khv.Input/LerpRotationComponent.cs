using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Input
{
    public class LerpRotationComponent : IRotationComponent
    {
        #region Properties
        public float Rotation { get; private set; }
        public float GoalRotation { get; set; }
        public float Step { get; set; }
        #endregion

        public LerpRotationComponent(float step)
        {
            Step = step;
        }

        public void Update(GameTime gametime)
        {
            Rotation = CurveAngle(Rotation, GoalRotation, Step);
        }

        private float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            Vector2 fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            Vector2 toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));

            Vector2 currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        private Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            double theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            double sinTheta = Math.Sin(theta);
            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }
    }
}
