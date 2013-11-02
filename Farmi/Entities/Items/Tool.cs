using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa työkalua jota voi käyttää
    /// </summary>
    internal sealed class Tool : Item
    {
        public Tool(KhvGame game, string name) : base(game, name)
        {
        }
    }
}
