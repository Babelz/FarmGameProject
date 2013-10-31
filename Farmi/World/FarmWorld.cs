using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Farmi.Entities.Components;
using Khv.Engine;
using Khv.Engine.Helpers;
using Khv.Game.GameObjects;
using Khv.Gui.Components.BaseComponents;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Khv.Gui.Components.BaseComponents.Containers.Components;

namespace Farmi.World
{
    public sealed class FarmWorld : Khv.Game.World
    {
        #region Properties

        public FarmPlayer Player
        {
            get;
            private set;
        }


        #endregion

        public FarmWorld(KhvGame game) : base(game)
        {

        }

        public override void Initialize()
        {
            MapManager = new MapManager(Game, Path.Combine("cfg", "mengine.cfg"));
            WorldObjects = new GameObjectManager(null);
            Player = new FarmPlayer(Game, this, PlayerIndex.One);
            Player.Initialize();
            WorldObjects.AddGameObject(Player);

            MapManager.ChangeMap("farm");
        }

        public override void Update(GameTime gameTime)
        {
            if (MapManager.ActiveMap != null)
            {
                MapManager.ActiveMap.Update(gameTime);
            }

            var gameobjects = WorldObjects.AllObjects();

            foreach (var gameobject in gameobjects)
            {
                gameobject.Update(gameTime);
            }
           
        }

        /// <summary>
        /// Hakee lähimmät gameobjectit joilla on interaction komponentti
        /// </summary>
        /// <param name="source">Kenestä katsottuna lähimmät.</param>
        /// <param name="radius">Säde kuinka laajalta alueelta etsitään.</param>
        /// <returns>Tyhjän listan jos ei löydy yhtään, muuten lähimmät objektit</returns>
        public List<GameObject> GetNearInteractables(GameObject source, Padding radius)
        {
            List<GameObject> gobs = GetNearGameObjects(source, radius);
            var objects = gobs.Where(
                 o => o.Components.ContainsComponent(c => c is IInteractionComponent))
                .ToList();

            return objects;
        }

        /// <summary>
        /// Hakee lähimmät gameobjectit
        /// </summary>
        /// <param name="source">Kenestä katsottuna lähimmät.</param>
        /// <param name="radius">Säde kuinka laajalta alueelta etsitään.</param>
        /// <returns></returns>
        public List<GameObject> GetNearGameObjects(GameObject source, Padding radius)
        {
            List<GameObject> gobs = new List<GameObject>();
            foreach (GameObjectManager gameobjectManager in MapManager.ActiveMap.ObjectManagers.AllManagers())
            {
                gobs.AddRange(gameobjectManager.AllObjects());
            }

            gobs.AddRange(WorldObjects.AllObjects());
           // Rectangle r = new Rectangle((int)(source.Position.X - radius.Left), (int)(source.Position.Y - radius.Top), source.Size.Width*2 + radius.Right, source.Size.Height * 2 + radius.Bottom);
            Rectangle r = new Rectangle((int)(source.Position.X - radius.Left), (int)(source.Position.Y - radius.Top), radius.Left + radius.Right, radius.Top + radius.Bottom * 2);

            var objects = gobs.Where(
                o => !ReferenceEquals(o, source) && r.Intersects(new Rectangle((int)o.Position.X, (int)o.Position.Y, o.Size.Width, o.Size.Height))).ToList();

            return objects;
        }

        /// <summary>
        /// Hakee lähimmän peliobjektin
        /// </summary>
        /// <param name="source">Kenestä katsottuna lähin.</param>
        /// <param name="radius">Säde kuinka laajalta alueelta etsitään.</param>
        /// <returns>Lähimmän peliobjektin, jos ei löydy niin palauttaa null</returns>
        public GameObject GetNearestGameObject(GameObject source, Padding radius)
        {
            return GetNearestGameObject(GetNearGameObjects(source, radius), source);
        }

        /// <summary>
        /// Hakee listasta lähimmän peliobjektin
        /// </summary>
        /// <param name="objects">Objektit joista etsitään lähintä.</param>
        /// <param name="source">Kenestä katsottuna lähi.</param>
        /// <returns>Null jos listan koko on 0, muuten lähimmän objektin</returns>
        public GameObject GetNearestGameObject(List<GameObject> objects, GameObject source)
        {

            if (objects.Count == 0)
            {
                return null;
            }
            if (objects.Count == 1)
            {
                return objects[0];
            }

            Rectangle search = new Rectangle((int)source.Position.X, (int)source.Position.Y, source.Size.Width, source.Size.Height);



            Vector2[] tocheck = new Vector2[objects.Count];
            for (int index = 0; index < objects.Count; index++)
            {
                var g = objects[index];
                Vector2 v = g.Position;

                if (g.Position.X + g.Size.Width <= source.Position.X ||
                    g.Position.X + g.Size.Width <= source.Position.X + source.Size.Width)
                {
                    v.X += g.Size.Width;
                }

                if (g.Position.Y + g.Size.Height <= source.Position.Y ||
                    g.Position.Y + g.Size.Height <= source.Position.X + source.Size.Height)
                {
                    v.Y += g.Size.Height;
                }

                tocheck[index] = v;
            }


            Vector2? closest = null;
            int closestindex = 0;
            float mindis = float.MaxValue;

            for (int index = 0; index < tocheck.Length; index++)
            {
                var pos = tocheck[index];
                float dis = VectorHelper.Distance(pos, source.Position);

                if (!closest.HasValue || dis < mindis)
                {
                    closest = pos;
                    mindis = dis;
                    closestindex = index;
                }
            }

            return objects[closestindex];
        }

        /// <summary>
        /// Hakee lähimmän interactablen
        /// </summary>
        /// <param name="source">Kenestä katsottuna lähin.</param>
        /// <param name="radius">Säde kuinka laajalta alueelta etsitään.</param>
        /// <returns>Null jos ei löydy läheltä, muuten lähimmän objektin</returns>
        public GameObject GetNearestInteractable(GameObject source, Padding radius)
        {
            List<GameObject> objects = GetNearInteractables(source, radius);

            GameObject nearest = GetNearestGameObject(objects, source);

            return nearest;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (MapManager.ActiveMap != null)
            {
                MapManager.ActiveMap.Draw(spriteBatch);
            }

            var gameobjects = WorldObjects.GameObjectsOfType<DrawableGameObject>(g => g is DrawableGameObject);

            foreach (var gameobject in gameobjects)
            {
               gameobject.Draw(spriteBatch);
            }

            gameobjects.First(o => o is FarmPlayer).Draw(spriteBatch);
        }
    }
}
