using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Items;
using Khv.Engine;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public sealed class Seed : Tool
    {
        public Seed(KhvGame game, ToolDataset dataset) 
            : base(game, dataset)
        {
        }
    }
}
