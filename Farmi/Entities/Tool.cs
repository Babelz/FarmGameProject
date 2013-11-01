using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;

namespace Farmi.Entities
{
    internal sealed class Tool : Item
    {
        public Tool(KhvGame game, string name) : base(game, name)
        {
        }

        public override void Export(GameDataExporter exporter)
        {
            base.Export(exporter);
        }

        public override void Import(GameDataImporter importer)
        {
            base.Import(importer);
        }
    }
}
