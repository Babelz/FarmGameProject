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
        private float speed;
        private float minZoom;
        private float maxZoom;
        private const float minSpeed = 1.0f;
        private const float maxSpeed = 100.0f;
        private int newVal;
        private int oldVal;
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
                Matrix matrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                Matrix.CreateRotationY(0.0f) *
                                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                Matrix.CreateTranslation(new Vector3(viewport.Height * 0.5f,
                                                                     viewport.Width * 0.5f, 0));
                return matrix;
            }
        }

        #endregion

        public Camera(Vector2 position, Viewport viewport)
        {
            this.position = position;
            this.viewport = viewport;
            zoom = 1.0f;
            speed = 5.0f;
        }
    }
}