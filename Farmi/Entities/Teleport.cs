using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Maps.MapClasses.Processors;
using Khv.Game.GameObjects;
using Farmi.Repositories;
using Khv.Engine.Structs;
using Khv.Game.Collision;
using Farmi.World;
using Farmi.Screens;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;

namespace Farmi.Entities
{
    internal sealed class Teleport : GameObject
    {
        #region Vars
        private string mapToTeleport;
        private string mapContainedIn;
        #endregion

        public Teleport(KhvGame game, MapObjectArguments args)
            : base(game)
        {
            TestInitialize(args);
        }
        public Teleport(KhvGame game)
            : base(game)
        {
            TestInitialize(null);
        }

        private void TestInitialize(MapObjectArguments args)
        {
            if (args != null)
            {
                position = args.Origin;
                position.X -= 32;

                size = new Size(96, 32);

                Collider = new BoxCollider(null, this);
                Collider.OnCollision += new CollisionEventHandler(Collider_OnCollision);

                mapToTeleport = args.SerializedData.valuepairs[0].Value;
                mapContainedIn = args.MapContainedIn;
            }
        }

        private void Collider_OnCollision(object sender, CollisionResult result)
        {
            GameplayScreen screen = (game.Components.First(c => c is GameStateManager) as GameStateManager).Current as GameplayScreen;

            if (screen != null)
            {
                MapManager mapManager = screen.World.MapManager;
                MapChangeAction action;

                #warning Testi @ teleport, voi kusta tulevaisuudessa ku pitäs pitää karttoja muistissa.
                #warning Testi @ teleport, vaihtaa vielä aika pseudona, ei osaa disabloida input eikä alottaa transition.
                #warning Testi @ teleport, ei ole viel offset position joka annetaan teleporttaajalle kun hän porttaa eikä teleportin oikeaa kokoa.

                if (mapManager.ActiveMap.Name == "farm")
                {
                    action = MapChangeAction.MoveCurrentToBackground;
                }
                else
                {
                    action = MapChangeAction.DisposeCurrent;
                }

                mapManager.ChangeMap(mapToTeleport, action);

                GameObjectManager objectManager = mapManager.ActiveMap.ObjectManagers
                                                 .AllManagers()
                                                 .First();

                Teleport teleport = objectManager
                                    .GetGameObject<Teleport>(
                                    o => o.mapToTeleport == this.mapContainedIn);

                screen.World.Player.Position = teleport.position;

                if (teleport.position.Y == 0)
                {
                    screen.World.Player.Position = new Vector2(screen.World.Player.Position.X, screen.World.Player.Position.Y + 64);
                }
                else
                {
                    screen.World.Player.Position = new Vector2(screen.World.Player.Position.X, screen.World.Player.Position.Y - 64);
                }
            }
        }
    }
}
