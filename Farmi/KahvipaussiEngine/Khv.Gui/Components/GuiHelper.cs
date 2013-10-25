using Khv.Gui.Components.BaseComponents;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components
{
    public static class GuiHelper
    {
        public static Point CalulateHorizontalPoint(ControlPosition currentPoint, ControlSize targetSize, ControlSize parentSize, ControlPosition parentPosition, HorizontalAlingment alingment)
        {
            Point results = Point.Zero;
            switch (alingment)
            {
                case HorizontalAlingment.Right:
                    results = new Point((parentPosition.Relative.X + parentSize.Width) - targetSize.Width, currentPoint.Relative.Y);
                    break;
                case HorizontalAlingment.Left:
                    results = new Point(parentPosition.Relative.X, currentPoint.Relative.Y);
                    break;
                case HorizontalAlingment.Center:
                    results = new Point(parentSize.Width / 2 - targetSize.Width / 2, currentPoint.Relative.Y);
                    break;
            }

            return results;
        }
        public static Point CalculateVerticalPoint(ControlPosition currentPoint, ControlSize targetSize, ControlSize parentSize, ControlPosition parentPosition, VerticalAlingment alingment)
        {
            Point results = Point.Zero;
            switch (alingment)
            {
                case VerticalAlingment.Center:
                    results = new Point(currentPoint.Relative.X, parentSize.Height / 2 - targetSize.Height / 2);
                    break;
                case VerticalAlingment.Top:
                    results = new Point(currentPoint.Relative.X, 0);
                    break;
                case VerticalAlingment.Bottom:
                    results = new Point(currentPoint.Relative.X, parentSize.Height - targetSize.Height);
                    break;
            }
            return results;
        }
    }
}
