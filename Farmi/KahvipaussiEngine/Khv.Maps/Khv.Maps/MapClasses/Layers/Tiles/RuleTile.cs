using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Components;
using Microsoft.Xna.Framework;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;

namespace Khv.Maps.MapClasses.Layers.Tiles
{
    /// <summary>
    /// tile joka sisältää säännön kuten
    /// damaging ja blocked
    /// </summary>
    public class RuleTile : BaseTile, IObjectTile
    {
        #region Vars
        private Rules rule;
        #endregion

        #region Properties
        public Rules Rule
        {
            get
            {
                return rule;
            }
        }
        #endregion

        public RuleTile(Vector2 position, Rules rule)
            : base(position)
        {
            this.rule = rule;
        }
        public RuleTile(Vector2 position)
            : base(position)
        {
            rule = Rules.None;
        }

        public void SetObject(object mapObject)
        {
            rule = (Rules)mapObject;
        }
        public override bool IsEmpty()
        {
            return rule == Rules.None;
        }
        public override void Clear()
        {
            rule = Rules.None;
        }
    }
}
