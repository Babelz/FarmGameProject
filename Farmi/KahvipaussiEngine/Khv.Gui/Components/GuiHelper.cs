using Khv.Gui.Components.BaseComponents;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components
{
    public static class GuiHelper
    {
        public static Vector2 CalulateHorizontalPoint(ControlPosition currentPoint, ControlSize targetSize, ControlSize parentSize, ControlPosition parentPosition, HorizontalAlingment alingment)
        {
            Vector2 results = Vector2.Zero;
            switch (alingment)
            {
                case HorizontalAlingment.Right:
                    results = new Vector2((parentPosition.Relative.X + parentSize.Width) - targetSize.Width, currentPoint.Relative.Y);
                    break;
                case HorizontalAlingment.Left:
                    results = new Vector2(parentPosition.Relative.X, currentPoint.Relative.Y);
                    break;
                case HorizontalAlingment.Center:
                    results = new Vector2(parentSize.Width / 2 - targetSize.Width / 2, currentPoint.Relative.Y);
                    break;
            }

            return results;
        }
        public static Vector2 CalculateVerticalPoint(ControlPosition currentPoint, ControlSize targetSize, ControlSize parentSize, ControlPosition parentPosition, VerticalAlingment alingment)
        {
            Vector2 results = Vector2.Zero;
            switch (alingment)
            {
                case VerticalAlingment.Center:
                    results = new Vector2(currentPoint.Relative.X, parentSize.Height / 2 - targetSize.Height / 2);
                    break;
                case VerticalAlingment.Top:
                    results = new Vector2(currentPoint.Relative.X, 0);
                    break;
                case VerticalAlingment.Bottom:
                    results = new Vector2(currentPoint.Relative.X, parentSize.Height - targetSize.Height);
                    break;
            }
            return results;
        }
    }
}
