using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components
{
    /// <summary>
    /// Enum nappulan statesta.
    /// </summary>
    public enum PressedState
    {
        None,
        Pressed,
        Released
    }
    /// <summary>
    /// Enum navigointi suunnasta.
    /// </summary>
    public enum NavigationDirection
    {
        Up,
        Down,
        Left,
        Right, 
    }
    /// <summary>
    /// Enum horisontaalisesta sijoittelusta.
    /// </summary>
    public enum HorizontalAlingment
    {
        Left,
        Right,
        Center,
        None
    }
    /// <summary>
    /// Enum vertikaalisesta sijoittelusta.
    /// </summary>
    public enum VerticalAlingment
    {
        Center,
        Top,
        Bottom,
        None
    }
    /// <summary>
    /// Enum kokojen tyypeistä.
    /// </summary>
    public enum SizeType
    {
        Fixed,
        Percent
    }
}
