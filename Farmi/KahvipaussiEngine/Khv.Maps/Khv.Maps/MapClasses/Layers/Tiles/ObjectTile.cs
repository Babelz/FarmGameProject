using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;
using Microsoft.Xna.Framework;
using Khv.Game.GameObjects;


namespace Khv.Maps.MapClasses.Layers.Tiles
{
    /// <summary>
    /// tile joka siältää kartta objektin ja 
    /// sen tiedot
    /// </summary>
    public class ObjectTile : BaseTile, IObjectTile
    {
        #region Vars
        private GameObject objectOwned;
        #endregion

        #region Properties
        public GameObject ObjectOwned
        {
            get
            {
                return objectOwned;
            }
            set
            {
                if (objectOwned == null)
                {
                    objectOwned = value;
                }
            }
        }
        #endregion

        public ObjectTile(Vector2 position)
            : base(position)
        {
        }

        public override bool IsEmpty()
        {
            return objectOwned == null;
        }
        public override void Clear()
        {
            objectOwned = null;
        }
        public void SetObject(object mapObject)
        {
            objectOwned = (GameObject)mapObject;
        }
    }
}
