using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmi.Entities
{
    internal interface ISaveable
    {
        /// <summary>
        /// Hakee importterista tiedot itselleen.
        /// </summary>
        void Import(GameDataImporter gameDataImporter);

        /// <summary>
        /// Antaa exportterille tiedot itsestään.
        /// </summary>
        void Export(GameDataExporter gameDataExporter);
    }
}
