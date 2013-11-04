using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Khv.Game.Collision
{
    public class Polygon
    {
        #region Properties
        public List<Vector2> Vertices
        {
            get;
            set;
        }
        public List<Vector2> Edges
        {
            get;
            set;
        }
        public Vector2 Center
        {
            get
            {
                float x = 0;
                float y = 0;
                Vertices.ForEach(vertex =>
                {
                    x += vertex.X;
                    y += vertex.Y;
                });
                return new Vector2(x / (float)Vertices.Count, y / (float)Vertices.Count);
            }
        }
        #endregion

        public Polygon()
        {
            Vertices = new List<Vector2>();
            Edges = new List<Vector2>();
        }

        public void BuildEdges()
        {
            Vector2 v1;
            Vector2 v2;
            for (int i = 0; i < Vertices.Count; i++)
            {
                v1 = Vertices[i];
                if (i + 1 >= Vertices.Count)
                {
                    // viimisestä ekaan
                    v2 = Vertices[0];
                }
                else
                {
                    v2 = Vertices[i + 1];
                }
                Edges.Add(v2 - v1);   
            }
        }
        
    }
}
