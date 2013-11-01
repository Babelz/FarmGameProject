using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Khv.Engine;
using Farmi.Datasets;

namespace Farmi
{
    /// <summary>
    /// Olio joka tallentaa pelin olioja ja niiden statet 
    /// save fileen.
    /// </summary>
    internal sealed class GameDataImporter
    {
        // TODO: base määrittely vasta.

        #region Vars
        private readonly KhvGame game;
        private readonly List<IDataset> importedDatasets;

        private readonly string saveFilePath;
        #endregion

        public GameDataImporter(KhvGame game, string saveFilePath)
        {
            this.game = game;
            this.saveFilePath = saveFilePath;

            importedDatasets = new List<IDataset>();
        }

        public IDataset ResolveDataset(Predicate<IDataset> predicate)
        {
            IDataset dataset = importedDatasets.Find(
                d => predicate(d));

            if (dataset != null)
            {
                importedDatasets.Remove(dataset);
            }

            return dataset;
        }
        public List<IDataset> GetImportedDatasets()
        {
            List<IDataset> datasets = importedDatasets.ToList();
            importedDatasets.Clear();

            return datasets;
        }
    }
}
