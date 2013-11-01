using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Game.GameObjects;
using Khv.Maps.MapClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    internal sealed class Camera
    {
        #region Vars
        private float zoom;
        private Vector2 position;
        #endregion

        #region Properties
        // palauttaa kameran sijainnin
        public Vector2 Position
        {
            get { return position;  }
            set { position = value;  }
        }

        public Viewport Viewport
        {
            get;
            private set;
        }

        public GameObject FollowedObject
        {
            get;
            private set;
        }

        public bool IsFollowing
        {
            get { return FollowedObject != null;  }
        }
        // palauttaa transformaation viewportista
        public Matrix TransFormation
        {
            get
            {
                /*return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                Matrix.CreateRotationY(0.0f) *
                                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f,
                                                                     Viewport.Height * 0.5f, 0));*/
                // Matrix matrix = Matrix.CreateScale(zoom)*Matrix.CreateTranslation(new Vector3(-Position, 0f));

                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                                           Matrix.CreateRotationZ(0.0f) *
                                           Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                           Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }

        #endregion

        public Camera(Vector2 position, Viewport viewport)
        {
            Position = position;
            Viewport = viewport;
            zoom = 1.0f;
        }

        #region Methods

        public void Update(TileMap activeMap)
        {
            if (FollowedObject == null)
                return;
            
            Vector2 v = new Vector2();
            v.X = MathHelper.Clamp(FollowedObject.Position.X - Viewport.Width / 2, 0, activeMap.TileEngine.MapSizeInPixels.Width - Viewport.Width);
            v.Y = MathHelper.Clamp(FollowedObject.Position.Y - Viewport.Height / 2, 0, activeMap.TileEngine.MapSizeInPixels.Height - Viewport.Height);
            Position = v;

        }
        public void Follow(GameObject who)
        {
            FollowedObject = who;
        }
        public void StopFollowing()
        {
            FollowedObject = null;
        }

        #endregion
    }
}