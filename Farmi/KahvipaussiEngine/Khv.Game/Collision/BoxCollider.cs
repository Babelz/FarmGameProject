using Khv.Engine;
﻿using Khv.Game.Collision;
using Khv.Engine.Structs;
using Khv.Game.GameObjects;
using Khv.Input;
using Khv.Maps.MapClasses.Layers;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace Khv.Game.Collision
{

    public class BoxCollider : PolygonCollider
    {
        #region Vars

        protected World world;

        

        #endregion

        #region Ctor

        /// <summary>
        /// Luo uuden colliderin
        /// </summary>
        /// <param name="world">Mistä kysellään ja etsitään gameobjecteja tai tilejä</param>
        /// <param name="Instance">Kenen gameobjectin collider tämä on</param>
        public BoxCollider(World world, GameObject Instance) : base(Instance)
        {
            this.world = world;

            Polygon = new Polygon();
            
            
            Polygon.Vertices.Add(new Vector2(0,0));
            Polygon.Vertices.Add(new Vector2(0 + Instance.Size.Width, 0));
            Polygon.Vertices.Add(new Vector2(0 + Instance.Size.Width, 0 + Instance.Size.Height));
            Polygon.Vertices.Add(new Vector2(0, 0 + Instance.Size.Height));
            Polygon.BuildEdges();
            // vertex == kärki
            // edge == viiva kärjestä A kärkeen B jne.
            // edgejä ja verticeitä ei tarvi päivittää muuten kuin jos laitetaan rotationia tms

            
        }

        #endregion

        #region Methods

        #region Overrides

        /// <summary>
        /// Päivittää collideria ja querittaa lähietäisyydeltä mahdollisia
        /// gameobjecteja ja tilejä joihin voi törmätä
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            #warning proto
            // oletetetaan että on ajettu läpi g => g.IsCollidable
            if (world == null)
                return;
            IEnumerable<GameObject> nearGameObjects = world.WorldObjects.AllObjects();

            nearGameObjects.Union(world.MapManager.ActiveMap.ObjectManagers.GetManager(g => g != null).AllObjects());
            Layer<RuleTile> rules = world.MapManager.ActiveMap.LayerManager.AllLayers().First(l => l is Layer<RuleTile>) as Layer<RuleTile>;
            Size tSize = world.MapManager.ActiveMap.TileEngine.TileSize;
            if (rules != null)
            {
                RuleTile[][] tiles = rules.GetSurroundingTiles(Instance.Position);
                for (int i = 0; i < tiles.Length; i++)
                {
                    for (int j = 0; j < tiles[i].Length; j++)
                    {
                        RuleTile t = tiles[i][j];
                        if (t == null)
                            continue;
                        
                        Vector2 v1 = (t.Position);
                        Vector2 v2 = new Vector2(v1.X + tSize.Width, v1.Y);
                        Vector2 v3 = new Vector2(v2.X, v2.Y + tSize.Height);
                        Vector2 v4 = new Vector2(t.Position.X, v3.Y);
                        Polygon polygon = new Polygon();
                        polygon.Vertices.Add(v1);
                        polygon.Vertices.Add(v2);
                        polygon.Vertices.Add(v3);
                        polygon.Vertices.Add(v4);
                        polygon.BuildEdges();
                        
                        CollisionResult r = PolygonCollision(this, polygon);

                        if (r.Intersecting || r.WillIntersect)
                        {
                            Instance.Position += r.Translation;
                            FireOnCollision(Instance, t, r);
                        }
                        
                    }
                }
            }

            foreach (GameObject gameObject in nearGameObjects)
            {
                CollisionResult r;
                if (!Collides(gameObject, out r)) continue;

                Asd(Instance, r);

                CollisionResult r2 = PolygonCollision(gameObject.Collider as PolygonCollider, this);
                Asd(gameObject, r2);



                FireOnCollision(Instance, gameObject, r);
                FireOnCollision(gameObject, Instance,
                    r2
                    );
            }
        }

        private void Asd(GameObject gameObject, CollisionResult r)
        {
            Vector2 translation = Vector2.Zero;
            if (Math.Abs(r.Translation.X) > 0.00001f)
            {

                gameObject.Velocity = new Vector2(0, gameObject.Velocity.Y);

                Console.WriteLine("jees");
            }
            if (Math.Abs(r.Translation.Y) > 0.00001f)
            {
                gameObject.Velocity = new Vector2(gameObject.Velocity.X, 0);
                Console.WriteLine("jees");
            }
            gameObject.Position += r.Translation + translation;
        }

        /// <summary>
        /// Tarkastaa törmääkö joku gameobject
        /// </summary>
        /// <param name="other">Toinen gameobject != owner</param>
        /// <returns>true jos törmää, false muuten</returns>
        public override bool Collides(GameObject other, out CollisionResult result)
        {
            result = new CollisionResult();
            // ei voida törmätä itseen tai olemattomaan ;)
            if (other.Collider == null || other == Instance || !other.IsCollidable)
                return false;

            PolygonCollider pCollider = other.Collider as PolygonCollider;
            if (pCollider == null)
                throw new NotImplementedException("Ei osata collidaita " + other.Collider.GetType().Name);


            result = PolygonCollision(this, pCollider);

            return result.Intersecting || result.WillIntersect;
        }


        private CollisionResult PolygonCollision(Polygon polygon, Polygon other, 
            Vector2 velocity = new Vector2() ,Vector2 aOffset = new Vector2(), Vector2 bOffset = new Vector2())
        {
            CollisionResult result = new CollisionResult();
            result.WillIntersect = true;
            result.Intersecting = true;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = Vector2.Zero;
            foreach (var edge in GetEdges(polygon, other))
            {
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                float minA, minB, maxA, maxB;
                minA = minB = maxA = maxB = 0;
                ProjectPolygon(axis, polygon, aOffset, ref minA, ref maxA);
                ProjectPolygon(axis, other, bOffset, ref minB, ref maxB);

                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersecting = false;


                // velocity projection
                float velocityProjection = DotProduct(axis, velocity);
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0)
                {
                    result.WillIntersect = false;
                }

                // polygonit ei tule törmään tässä loopissa niin lähetään kämpille
                if (!result.Intersecting && !result.WillIntersect) break;

                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;
                    Vector2 d = (aOffset + polygon.Center) - (other.Center + bOffset);
                    if (DotProduct(d, translationAxis) < 0) translationAxis = -translationAxis;
                }
            }
            if (result.WillIntersect) result.Translation = translationAxis*minIntervalDistance;
            return result;
        }

        /// <summary>
        /// Tarkastaa liikkuvan objektin collisionin liikkuvaan objectiin
        /// </summary>
        /// <param name="who">liikkuva objekti jonka törmäys katsotaan</param>
        /// <param name="other">keneen ollaan törmäämässä</param>
        /// <returns></returns>
        private CollisionResult PolygonCollision(PolygonCollider who, PolygonCollider other)
        {
            return PolygonCollision(
                who.Polygon,
                other.Polygon,
                who.Instance.Velocity - other.Instance.Velocity,
                who.Instance.Position,
                other.Instance.Position
                );
        }

        /// <summary>
        /// Tarkistaa liikkuvan esineen collisionin ei liikkuvaan polygoniin
        /// </summary>
        /// <param name="who">liikkuva objekti</param>
        /// <param name="other">staattinen polygon</param>
        /// <returns></returns>
        private CollisionResult PolygonCollision(PolygonCollider who, Polygon other)
        {
            return PolygonCollision(
                who.Polygon,
                other,
                who.Instance.Velocity,
                who.Instance.Position
                );
        }

        /// <summary>
        /// Palauttaa molempien polygoneiden edget parametrijärjestyksessä
        /// </summary>
        /// <param name="polygon">toinen polygon</param>
        /// <param name="other">toinen polygon</param>
        /// <returns>edget</returns>
        private IEnumerable<Vector2> GetEdges(Polygon polygon, Polygon other)
        {
            int myEdges = polygon.Edges.Count;
            int otherEdges = other.Edges.Count;
            for (int edge = 0; edge < myEdges + otherEdges; edge++)
            {
                if (edge < myEdges)
                {
                    yield return polygon.Edges[edge];
                }
                else
                {
                    yield return other.Edges[edge - myEdges];
                }

            }
        }


        /// <summary>
        /// Yli 0 == väli
        /// Tasan 0 == koskettaa reunoista
        /// Alle 0 == intersect
        /// laskee olioden välisen matkan
        /// </summary>
        /// <param name="minA"></param>
        /// <param name="maxA"></param>
        /// <param name="minB"></param>
        /// <param name="maxB"></param>
        /// <returns></returns>
        public float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            return minA - maxB;
            
        }


        /// <summary>
        /// Katsoo kuinka paljon collider on kohtisuoraan poikkiviivaan päin joka kulmasta
        /// </summary>
        private void ProjectPolygon(Vector2 axis, Polygon polygon, Vector2 offset, ref float min, ref float max)
        {
            float dot = DotProduct(axis, polygon.Vertices[0] + offset);
            max = min = dot;

            for (int i = 1; i < polygon.Vertices.Count; i++)
            {
                dot = DotProduct(polygon.Vertices[i] + offset, axis);
                if (dot < min)
                {
                    min = dot;
                }
                else if (dot > max)
                {
                    max = dot;
                }
            }
        }

        /// <summary>
        /// Pistetulo kahden vektorin välillä
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private float DotProduct(Vector2 v1, Vector2 v2)
        {
            return v1.X*v2.X + v1.Y*v2.Y;
        }

        #endregion

        #endregion
    }
}
