using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Input
{
    public delegate void KeyboardInputCallback(Keys triggered, InputEventArgs args);
    public delegate void GamepadInputCallback(Buttons triggered, GamepadInputEventArgs args);
    public delegate void MouseInputCallback(MouseButtons triggered, InputEventArgs args);
}
