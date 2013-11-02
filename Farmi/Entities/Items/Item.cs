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
    abstract class Item : DrawableGameObject
    {

        #region Properties

        public string Name { get; protected set; }
        public Texture2D Texture { get; set; }

        protected RepositoryManager RepositoryManager
        {
            get;
            private set;
        }

        #endregion

        protected Item(KhvGame game, string name) 
            : base(game)
        {
            Name = name;
            RepositoryManager = game.Components.First(c => c is RepositoryManager) as RepositoryManager;
        }

        public abstract void Import(GameDataImporter importer);
        public abstract void Export(GameDataExporter exporter);
    }
}
