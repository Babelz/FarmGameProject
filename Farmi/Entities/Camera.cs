using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    internal sealed class Camera
    {
        #region Vars
        private float zoom;
        private Viewport viewport;

        private Vector2 position;
        #endregion

        #region Properties
        // palauttaa kameran sijainnin
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        // palauttaa transformaation viewportista
        public Matrix TransFormation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                           Matrix.CreateRotationZ(0.0f) *
                                           Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                           Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }

        #endregion

        public Camera(Vector2 position, Viewport viewport)
        {
            this.position = position;
            this.viewport = viewport;
            zoom = 1.0f;
        }
    }
}