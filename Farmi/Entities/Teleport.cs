using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Game.GameObjects;

namespace Farmi.Entities
{
    internal sealed class Teleport : GameObject
    {
        public Teleport(KhvGame game, MapObjectArguments args)
            : base(game)
        {
        }
        public Teleport(KhvGame game)
            : base(game)
        {
        }
    }
}
