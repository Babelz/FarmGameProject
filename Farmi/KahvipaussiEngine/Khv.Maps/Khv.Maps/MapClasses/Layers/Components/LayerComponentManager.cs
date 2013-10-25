using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Maps.MapClasses.Layers.Components
{
    public class LayerComponentManager
    {
        #region Vars
        private List<LayerComponent> components;
        private List<DrawingLayerComponent> drawingComponents;
        #endregion

        public LayerComponentManager()
        {
            components = new List<LayerComponent>();
            drawingComponents = new List<DrawingLayerComponent>();
        }

        public void AddComponents(IEnumerable<LayerComponent> layerComponents)
        {
            components.AddRange(layerComponents);
            foreach (LayerComponent layerComponent in layerComponents)
            {
                DrawingLayerComponent drawing;
                if ((drawing = layerComponent as DrawingLayerComponent) != null)
                {
                    drawingComponents.Add(drawing);
                }
            }
        }
        public void AddComponent(LayerComponent layerComponent)
        {
            components.Add(layerComponent);

            DrawingLayerComponent drawing;
            if ((drawing = layerComponent as DrawingLayerComponent) != null)
            {
                drawingComponents.Add(drawing);
            }
        }
        public IEnumerable<LayerComponent> DrawingComponents()
        {
            foreach (DrawingLayerComponent drawingComponent in drawingComponents)
            {
                yield return drawingComponent;
            }
        }
        public IEnumerable<LayerComponent> AllComponents()
        {
            foreach (LayerComponent component in components)
            {
                yield return component;
            }
        }
    }
}
