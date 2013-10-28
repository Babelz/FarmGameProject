using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Game.GameObjects;

namespace Farmi.Entities
{
    internal sealed class CropSpot : DrawableGameObject
    {
        public CropSpot(KhvGame game, MapObjectArguments args)
            : base(game)
        {
        }
        public CropSpot(KhvGame game)
            : base(game)
        {
        }
    }
}
