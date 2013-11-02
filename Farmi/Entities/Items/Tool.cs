using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Repositories;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa työkalua jota voi käyttää
    /// </summary>
    internal sealed class Tool : Item
    {
        public Tool(KhvGame game, string name) : base(game, name)
        {
            ToolDataset dataset = RepositoryManager.GetDataSet<ToolDataset>(d => d.Name == name);
            MakeFromData(dataset);
        }

        private void MakeFromData(ToolDataset dataset)
        {
            Name = dataset.Name;
            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
    
        }

        public override void Import(GameDataImporter importer)
        {
            throw new NotImplementedException();
        }

        public override void Export(GameDataExporter exporter)
        {
            throw new NotImplementedException();
        }
    }
}
