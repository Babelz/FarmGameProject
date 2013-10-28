using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Khv.Engine;
using Microsoft.Xna.Framework.Graphics;
using Khv.Maps.MapClasses.Layers.Tiles.Interfaces;
using Khv.Maps.MapClasses.Layers.Tiles;
using Khv.Maps.MapClasses.Layers.Components;
using Khv.Maps.MapClasses.MapComponents.Layers.Sheets;
using Khv.Maps.MapClasses.Managers;
using Khv.Game.GameObjects;

namespace Khv.Maps.MapClasses.Layers.Components
{
    #region Base classes and interfaces
    /// <summary>
    /// Pohjaluokka layer komponenteille. Tästä tyypistä
    /// suoraan johdetun komponentit omaavat jonkin asian 
    /// päivittämisen.
    /// </summary>
    public abstract class LayerComponent
    {
        #region Vars
        private readonly KhvGame game;
        private readonly ILayer owner;
        #endregion

        public LayerComponent(KhvGame game, ILayer layer)
        {
            this.game = game;

            owner = layer;
        }

        public abstract void Update(GameTime gameTime);
    }

    /// <summary>
    /// Pohjaluokka komponentille joka voi piirtää.
    /// </summary>
    public abstract class DrawingLayerComponent : LayerComponent
    {
        public DrawingLayerComponent(KhvGame game, ILayer layer)
            : base(game, layer)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }

    /// <summary>
    /// Rajapinta komponentille joka voi suorittaa tausta päivityksiä
    /// kun layeri ei ole näkyvissä.
    /// </summary>
    public interface IBackGroundUpdatableComponent
    {
        void DoBackgroundUpdates(GameTime gameTime);
    }
    #endregion

    #region Drawing components
    /// <summary>
    /// Komponentti jonka omistavat kaikki layerit joiden tiletyyppi perii
    /// rajapinnan IDrawableTile. Piirtää kaikki tilet ja laskee updatessa
    /// rangen mistä minne tilet tulisi piirtää.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TileDrawingComponent<T> : DrawingLayerComponent where T : Tile
    {
        #region Vars
        private readonly Layer<T> genericOwner;
        #endregion

        public TileDrawingComponent(KhvGame game, ILayer layer)
            : base(game, layer)
        {
            genericOwner = layer as Layer<T>;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: lasketaan range drawille täällä.
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Array.ForEach(genericOwner.Tiles, row =>
                Array.ForEach(row.Where(t => t != null).ToArray(), t =>
                    t.Draw(spriteBatch)));
        }
    }

    /// <summary>
    /// Komponentti jonka omistavat objecti layerit. Päivittää 
    /// object manageria ja piirtää sen objektit. 
    /// Suorittaa taustalla myös objektien päivittämisen.
    /// </summary>
    public class GameObjectComponent : DrawingLayerComponent
    {
        #region Vars
        private readonly GameObjectManager gameObjectManager;
        #endregion

        public GameObjectComponent(KhvGame game, ILayer layer, GameObjectManager gameObjectManager)
            : base(game, layer)
        {
            this.gameObjectManager = gameObjectManager;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjectManager.GameObjectsOfType<GameObject>())
            {
                gameObject.Update(gameTime);
            }

            // TODO: lasketaan range drawille täällä.
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (DrawableGameObject drawableGameobject in gameObjectManager.GameObjectsOfType<DrawableGameObject>())
            {
                drawableGameobject.Draw(spriteBatch);
            }
        }
    }
    #endregion

    #region Updating components
    /// <summary>
    /// Komponentti jonka omistavat animation layerit. Päivittää
    /// layerin tilejen animaatiot sekä animation manageria.
    /// </summary>
    public class AnimationComponent : LayerComponent
    {
        #region Vars
        private readonly AnimationManager animationManager;
        private readonly Layer<AnimationTile> genericOwner;
        #endregion

        public AnimationComponent(KhvGame game, ILayer layer)
            : base(game, layer)
        {
            genericOwner = layer as Layer<AnimationTile>;
            animationManager = (layer.Sheet as AnimationTileSheet).AnimationManager;
        }

        public override void Update(GameTime gameTime)
        {
            animationManager.Update(gameTime);
            Array.ForEach(genericOwner.Tiles, row =>
                Array.ForEach(row, t =>
                    t.Update(gameTime)));
        }
    }
    #endregion
}
