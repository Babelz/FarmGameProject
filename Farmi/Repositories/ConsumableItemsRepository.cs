using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Datasets;
using System.Xml.Linq;

namespace Farmi.Repositories
{
    internal sealed class ConsumableItemsRepository : Repository<ConsumableItemDataset>
    {
        public ConsumableItemsRepository(string name)
            : base(name)
        {
        }

        private IEnumerable<XElement> GetConsumableItemElemens(XDocument repository)
        {
            var itemElements = from items in repository.Descendants("Items")
                               from item in items.Descendants()
                               where item.Name == "Item"
                               select item;

            return itemElements;
        }

        public override void Load(XDocument repository)
        {
            var itemElements = GetConsumableItemElemens(repository);

            foreach (var itemElement in itemElements)
            {
                ConsumableItemDataset consumableItemDataset = new ConsumableItemDataset();
                consumableItemDataset.ParseValuesFrom(itemElement);

                items.Add(consumableItemDataset);
            }
        }
    }
}
