using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine;

namespace Farmi
{
    internal sealed class GameDataManager : IGameComponent
    {
        #region Vars
        private readonly KhvGame game;

        private readonly string savefileName;
        private readonly string saveDirectory;
        #endregion

        #region Properties
        public GameDataImporter GameDataImporter
        {
            get;
            private set;
        }
        public GameDataExporter GameDataExporter
        {
            get;
            private set;
        }
        #endregion

        public GameDataManager(KhvGame game, string savefileName, string saveDirectory)
        {
            this.game = game;

            this.savefileName = savefileName;
            this.saveDirectory = saveDirectory;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
