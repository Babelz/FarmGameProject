using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Repositories;
using Khv.Engine;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities.Items
{
    class Item : DrawableGameObject
    {

        #region Properties

        public string Name { get; protected set; }
        public Texture2D Texture { get; set; }

        #endregion

        public Item(KhvGame game, string name) 
            : base(game)
        {
            Name = name;
            var repositoryManager = game.Components.First(c => c is RepositoryManager) as RepositoryManager;
            ItemDataset dataset = repositoryManager.GetDataSet<ItemDataset>(d => d.Name == name);
            if (dataset == null)
            {
                throw new InvalidOperationException(String.Format("Ei löydy item datasettiä nimellä: {0}", name));
            }
            MakeFromData(dataset);
        }

        protected void MakeFromData(ItemDataset dataset)
        {
            Name = dataset.Name;
            Texture = game.Content.Load<Texture2D>(Path.Combine("Items", dataset.AssetName));
            //TODO muut
        }

        

        public virtual void Import(GameDataImporter importer)
        {
            throw new NotImplementedException();
        }

        public virtual void Export(GameDataExporter exporter)
        {
            throw new NotImplementedException();
        }
    }
}
