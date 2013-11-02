using System;
using System.Linq;
using System.Text.RegularExpressions;
using Farmi.Datasets;
using Farmi.Entities.Buildings;
using Farmi.Screens;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses.Managers;
using Khv.Maps.MapClasses.Processors;
using Microsoft.Xna.Framework;
using SerializedDataTypes.Components;

namespace Farmi.Entities
{
    internal sealed class Teleport : GameObject, ILoadableMapObject, ILoadableRepositoryObject<TeleportDataset>
    {
        #region Vars
        // Kartta johon teleportataan.
        private string mapToTeleport;
        // Kartta jossa teleportti asuu.
        private string mapContainedIn;

        // Value joka lisätään kun pelaaja teleporttaa tähän teleporttiin.
        private Vector2 positionOffSet;
        #endregion

        /// <summary>
        /// Luo uuden instanssin teleportista.
        /// </summary>
        /// <param name="game">Khv peli instanssi.</param>
        /// <param name="mapObjectArguments">Argumentit jotka saadaan kun karttaa parsitaan ja tämä olio alustetaan.</param>
        public Teleport(KhvGame game, MapObjectArguments mapObjectArguments)
            : base(game)
        {
            InitializeFromMapData(mapObjectArguments);
        }
        /// <summary>
        /// Alustaa uuden teleportin.
        /// </summary>
        /// <param name="game">Khv peli instanssi.</param>
        /// <param name="teleportDataset">Teleport dataset josta haetaan tiedot teleportille.</param>
        /// <param name="mapContainedIn">Kartta jossa teleportti asuu.</param>
        public Teleport(KhvGame game, TeleportDataset teleportDataset, string mapContainedIn)
            : base(game)
        {
            this.mapContainedIn = mapContainedIn;
            InitializeFromDataset(teleportDataset);
        }

        protected override void OnDestroy()
        {
            Collider.OnCollision -= Collider_OnCollision;
        }

        #region Event handlers
        private void Collider_OnCollision(object sender, CollisionEventArgs result)
        {
            if (result.CollidingObject is FarmPlayer)
            {
                Port();
            }
        }
        #endregion

        #region Value parsing methods
        // Hakee karttadatasta positionin offsetin.
        private Vector2 GetPositionOffSet(MapObjectArguments mapObjectArguments)
        {
            Vector2 position = Vector2.Zero;

            Valuepair valuepair = mapObjectArguments.SerializedData.valuepairs
                                  .Find(v => v.Name == "PositionOffSet");

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
        // Hakee serialisoidusta karttadatasta mapin minne tulisi teleportata.
        private string GetMapToTeleport(MapObjectArguments mapObjectArguments)
        {
            string mapName = string.Empty;

            Valuepair valuepair = mapObjectArguments.SerializedData.valuepairs
                                  .Find(v => v.Name == "To");

            if (valuepair != null)
            {
                mapName = valuepair.Value.Trim();
            }

            return mapName;
        }
        // Hakee serialisoidusta karttadatasta teleportin koon.
        private Size GetSize(MapObjectArguments mapObjectArguments)
        {
            Size size = new Size(0, 0);

            Valuepair valuePair = mapObjectArguments.SerializedData.valuepairs
                                  .Find(v => v.Name == "Size");

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
        // Hakee kartalta joka on vaihdettu vastakkaisen teleportin.
        private Teleport ResolveOpposite(MapManager mapManager)
        {
            // Hakee ensimmäisen objekti tason.
            GameObjectManager objectManager = mapManager.ActiveMap.ObjectManagers
                                             .AllManagers()
                                             .First();

            // Koittaa hakea teleportin aluksi suoraan kartan objekti listasta.
            Teleport teleport = objectManager.GetGameObject<Teleport>(
                o => o.mapToTeleport == this.mapContainedIn);

            // Koska teleportti on vielä null, sen on pakko olla ovessa.
            if (teleport == null)
            {
                // Hakee rakennuksen jossa teleportti on.
                teleport = objectManager.GetGameObject<Building>(
                    b => Array.Find<Door>(b.Doors,
                        d => d.Teleport.mapToTeleport == this.mapContainedIn) != null)
                        .Doors.First(d => d.Teleport.mapToTeleport == this.mapContainedIn).Teleport;
            }

            return teleport;
        }
        #endregion

        #region Initializers
        public void InitializeFromMapData(MapObjectArguments mapObjectArguments)
        {
            size = GetSize(mapObjectArguments);
            mapToTeleport = GetMapToTeleport(mapObjectArguments);
            positionOffSet = GetPositionOffSet(mapObjectArguments);
            position = mapObjectArguments.Origin;

            mapContainedIn = mapObjectArguments.MapContainedIn;

            Collider = new BoxCollider(null, this);
            Collider.OnCollision += new CollisionEventHandler(Collider_OnCollision);
        }
        public void InitializeFromDataset(TeleportDataset dataset)
        {
            TeleportDataset teleportDataset = dataset as TeleportDataset;

            size = teleportDataset.Size;
            mapToTeleport = teleportDataset.TeleportTo;
            positionOffSet = teleportDataset.PositionOffSet;

            Collider = new BoxCollider(null, this);
        }
        #endregion

        /// <summary>
        /// Teleporttaa playerin uudelle kartalle.
        /// </summary>
        public void Port()
        {
            FarmWorld world = (game.GameStateManager.States.First(
                c => c is GameplayScreen) as GameplayScreen).World;

            if (world != null)
            {
                MapChangeAction action;
                MapManager mapManager = world.MapManager;

                #region Warning flags
#warning Testi @ teleport, voi kusta tulevaisuudessa ku pitäs pitää karttoja muistissa.
#warning Testi @ teleport, vaihtaa vielä aika pseudona, ei osaa disabloida input eikä alottaa transition.
#warning Testi @ teleport, ei ole viel offset position joka annetaan teleporttaajalle kun hän porttaa eikä teleportin oikeaa kokoa.
                #endregion

                // Katotaan tässä kaikki mapit jotka tulisi pitää muistissa eikä disposata.
                // Jos kartta on "tärkeä" se tulee laittaa taustalle.
                if (mapManager.ActiveMap.Name == "farm" || Regex.IsMatch(mapManager.ActiveMap.Name, "playerhouseindoors[0-9]"))
                {
                    action = MapChangeAction.MoveCurrentToBackground;
                }
                else
                {
                    action = MapChangeAction.DisposeCurrent;
                }

                // Vaihtaa kartan ja resolvaa vastakkaisen teleportin.
                mapManager.ChangeMap(mapToTeleport, action);

                Teleport teleport = ResolveOpposite(mapManager);
                world.Player.Position = teleport.position + teleport.positionOffSet;
            }
        }
    }
}
