using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Khv.Engine.Helpers
{
    public static class VectorHelper
    {
        public static float Distance(Vector2 a, Vector2 b)
        {
            double results = Math.Pow((double)(a.X - b.X), 2.0) + Math.Pow((double)(a.Y - b.Y), 2.0);
            return (float)Math.Sqrt(results);
        }
    }
}
