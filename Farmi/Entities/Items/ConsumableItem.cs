using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using Farmi.Entities.Items.Components;
using Khv.Engine;

namespace Farmi.Entities.Items
{
    /// <summary>
    /// Kuvaa itemiä jonka voi syödä. Jokaista consumable
    /// itemiä voidaan heittää.
    /// </summary>
    internal sealed class ConsumableItem : Item
    {

        public ConsumableItem(KhvGame game, string name) : base(game, name)
        {
            Components.Add(new ThrowableComponent(this));
            RepositoryManager.GetDataSet<ConsumableItemDataset>(d => d.Name == name);
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
