using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Engine;
using Farmi.Datasets;

namespace Farmi
{
    /// <summary>
    /// Olio joka hoitaa save filujen tietojen lukemisen
    /// ja runtime oliojen tietojen tallentamisen 
    /// kun peli halutaan tallentaa.
    /// </summary>
    internal sealed class GameDataExporter
    {
        // TODO: base määrittely vasta.

        #region Vars
        private readonly KhvGame game;
        private readonly List<IDataset> exportedDatasets;

        private readonly string saveFilePath;
        #endregion

        public GameDataExporter(KhvGame game, string saveFilePath)
        {
            this.game = game;
            this.saveFilePath = saveFilePath;

            exportedDatasets = new List<IDataset>();
        }

        public void ExportData(IDataset dataset)
        {
            if (!exportedDatasets.Contains(dataset))
            {
                exportedDatasets.Add(dataset);
            }
        }
        public List<IDataset> GetExportedDatasets()
        {
            List<IDataset> datasets = exportedDatasets.ToList();
            exportedDatasets.Clear();

            return datasets;
        }
    }
}
