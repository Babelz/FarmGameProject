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

        #endregion

        #region Properties
        // palauttaa kameran sijainnin
        public Vector2 Position
        {
            get; 
            set;
        }

        public Viewport Viewport
        {
            get;
            private set;
        }
        // palauttaa transformaation viewportista
        public Matrix TransFormation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                Matrix.CreateRotationY(0.0f) *
                                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f,
                                                                     Viewport.Height * 0.5f, 0));
                // Matrix matrix = Matrix.CreateScale(zoom)*Matrix.CreateTranslation(new Vector3(-Position, 0f));

               /* return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                                           Matrix.CreateRotationZ(0.0f) *
                                           Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                           Matrix.CreateTranslation(new Vector3(0, 0, 0));*/
            }
        }

        #endregion

        public Camera(Vector2 position, Viewport viewport)
        {
            Position = position;
            Viewport = viewport;
            zoom = 1.0f;
        }
    }
}