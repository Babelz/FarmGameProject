using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Animals;
using Khv.Game.GameObjects;
using Microsoft.Xna.Framework;
using Farmi.Entities.Items;

namespace Farmi.Entities.Components
{
    public abstract class AnimalInteractionComponent : InteractionComponent
    {
        #region Vars
        protected readonly Animal owner;
        protected readonly string[] acceptedTools;
        #endregion

        public AnimalInteractionComponent(Animal owner, string[] acceptedTools)
        {
            this.owner = owner;
            this.acceptedTools = acceptedTools;
        }

        private bool IsAcceptedTool(Tool tool)
        {
            return acceptedTools.Contains(tool.Dataset.Name);
        }
    }
}
