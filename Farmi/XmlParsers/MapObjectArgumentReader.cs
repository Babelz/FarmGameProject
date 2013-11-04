using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Maps.MapClasses.Processors;
using Khv.Engine.Structs;
using Microsoft.Xna.Framework;
using SerializedDataTypes.Components;

namespace Farmi.XmlParsers
{
    internal sealed class MapObjectArgumentReader
    {
        #region Vars
        private readonly MapObjectArguments mapObjectArguments;
        #endregion

        public MapObjectArgumentReader(MapObjectArguments mapObjectArguments)
        {
            this.mapObjectArguments = mapObjectArguments;
        }

        private Vector2 ReadVector(string keyName)
        {
            Vector2 position = Vector2.Zero;

            Valuepair valuepair = mapObjectArguments.SerializedData.valuepairs.Find(
                v => v.Name == keyName);

            if (valuepair != null)
            {
                string[] tokens = valuepair.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                string x = Array.Find<string>(tokens, s => s.Contains("X"));
                x = x.Trim().Substring(x.IndexOf("=") + 1).Trim();

                string y = Array.Find<string>(tokens, s => s.Contains("Y"));
                y = y.Trim().Substring(y.IndexOf("=") + 1).Trim();

                position = new Vector2(float.Parse(x), float.Parse(y));
            }

            return position;
        }

        public Size ReadSize()
        {
            Size size = new Size(0, 0);

            Valuepair valuePair = mapObjectArguments.SerializedData.valuepairs.Find(
                v => v.Name == "Size");

            if (valuePair != null)
            {
                string[] tokens = valuePair.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                string width = Array.Find<string>(tokens, s => s.Contains("Width"));
                width = width.Trim().Substring(width.IndexOf("=") + 1).Trim();

                string height = Array.Find<string>(tokens, s => s.Contains("Height"));
                height = height.Trim().Substring(height.IndexOf("=") + 1).Trim();

                size = new Size(int.Parse(width), int.Parse(height));
            }

            return size;
        }
        public Vector2 ReadPositionOffSet()
        {
            return ReadVector("PositionOffSet");
        }
        public Vector2 ReadPosition()
        {
            return ReadVector("Position");
        }
        public string ReadMapToTeleport()
        {
            string mapName = string.Empty;

            Valuepair valuepair = mapObjectArguments.SerializedData.valuepairs.Find(
                v => v.Name == "To");

            if (valuepair != null)
            {
                mapName = valuepair.Value.Trim();
            }

            return mapName;
        }
        public string ReadFeedType()
        {
            string feedType = string.Empty;

            Valuepair valuepair = mapObjectArguments.SerializedData.valuepairs.Find(
                v => v.Name == "FoodType");

            if (valuepair != null)
            {
                feedType = valuepair.Value.Trim();
            }

            return feedType;
        }
    }
}
