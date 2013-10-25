using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Transition;

using KhvGame = Khv.Engine.KhvGame;
using Game = Microsoft.Xna.Framework.Game;
using Khv.Engine.Args;

namespace Khv.Engine.State
{

    /// <summary>
    /// Kuvastaa peliruutua
    /// TODO: jokasella gamestatella vois olla oma ContentManager että on helppo unloadia?
    /// </summary>
    public abstract class GameState : IStateElement
    {

        #region Vars

        private GameStateManager gameStateManager;

        private ScreenState state = ScreenState.Hidden;

        private Rectangle area;

        #endregion

        #region Events

        public event StateChanged OnStateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// TODO: Voisko tehdä järkevämmin=
        /// Peittääkö kokonainen ruutu tämän ruudun
        /// </summary>
        public bool CoveredByOtherScreen
        {
            get;
            set;
        }

        /// <summary>
        /// TODO: Voisko tehdä järkevämmin?
        /// Peittääkö popup tämän ruudun
        /// </summary>
        public bool IsCoveredByPopUp
        {
            get;
            set;
        }

        /// <summary>
        /// TODO: Voisko tehdä järkevämmin?
        /// Onko jollakin muulla ruudulla focus
        /// </summary>
        public bool OtherHasFocus
        {
            get;
            set;
        }

        /// <summary>
        /// Viekö tämä ruutu koko tilaa?
        /// </summary>
        /// <returns>Jos ruutu vie koko tilan niin false, muuten true</returns>
        public virtual bool IsPopUp
        {
            get;
            set;
        }

        /// <summary>
        /// Onko ruutu häviämässä kokonaan
        /// </summary>
        public bool IsExiting
        {
            get;
            set;
        }

        /// <summary>
        /// Nykyinen ruudun tilanne
        /// </summary>
        public ScreenState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    ScreenState previous = state;
                    state = value;
                    if (OnStateChanged != null)
                    {
                        OnStateChanged(previous, new GameStateEventArgs(state));
                    }
                }
            }
        }

        /// <summary>
        /// Onko tällä ruudulla focus
        /// </summary>
        public bool HasFocus
        {
            get;
            set;
        }


        /// <summary>
        /// Onko tälle ruudulle annettu GameStateManager ja Content referenssi
        /// </summary>
        public bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Palauttaa tai asettaa GameStateManagerin johon tämä ruutu kuuluu
        /// </summary>
        public GameStateManager Manager
        {
            get { return gameStateManager; }
            set
            {
                if (gameStateManager == null)
                    gameStateManager = value;
            }
        }

        /// <summary>
        /// TODO: Pitäskö olla oma ContentManager?
        /// Palauttaa tai asettaa ContentManagerin jota tämä ruutu käyttää
        /// </summary>
        public ContentManager Content
        {
            get { return Game.Content; }
        }

        /// <summary>
        /// Viite peliin
        /// </summary>
        public KhvGame Game
        {
            get;
            set;
        }


        /// <summary>
        /// Palauttaa SpriteBatchin
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return Manager.SpriteBatch; }
        }



        #endregion

        #region Constructor

        /// <summary>
        /// Debug
        /// </summary>
        ~GameState()
        {
        }

        public GameState()
        {
            HasFocus = false;
            IsInitialized = false;
            HasFocus = false;
            IsExiting = false;
            IsPopUp = false;
            OtherHasFocus = false;
            IsCoveredByPopUp = false;
            CoveredByOtherScreen = false;

        }

        #endregion


        #region Methods


        /// <summary>
        /// Kutsutaan ennen kuin piirretään mitään
        /// </summary>
        public virtual void PreRender()
        {

        }

        /// <summary>
        /// Kutsutaan kun kaikki mahdolliset transitionit on tehty
        /// </summary>
        public virtual void PostRender()
        {

        }



        /// <summary>
        /// Kutsuu LoadContentia
        /// Tässä metodissa tulisi alustaa kaikki tarpeellinen
        /// </summary>
        public virtual void Initialize()
        {
            
            LoadContent();
            area = SpriteBatch.GraphicsDevice.Viewport.Bounds;
            IsInitialized = true;
        }

        /// <summary>
        /// Kutsutaan ennen Initializeä
        /// Tässä metodissa tulisi ladata kaikki tarpeellinen
        /// </summary>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Kutsutaan kun halutaan piirtää ruutu
        /// Jokaisen staten tulisi kutsua SpriteBatch.Start ja End metodia
        /// </summary>
        public virtual void Draw()
        {

        }

        /// <summary>
        /// Kutsutaan kun halutaan päivittää ruutu
        /// </summary>
        /// <param name="t">Kauan aikaa viime framesta</param>
        public virtual void Update(GameTime t)
        {
        }

        /// <summary>
        /// Täällä tulisi ladata kaikki content pois
        /// Kutsutaan kun ruutu hävitetään lopullisesti
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion

        /// <summary>
        /// Siirtymä kun tullaan tähän ruutuun
        /// </summary>
        /// <value>null jos ei ole määritelty, siirtymä olio muuten</value>
        public virtual TransitionEffect EnterTransition
        {
            get;
            set;
        }
        /// <summary>
        /// Siirtymä kun lähdetään tästä ruudusta
        /// </summary>
        /// <value>null jos ei ole määritelty, siirtymä olio muuten</value>
        public virtual TransitionEffect LeaveTransition
        {
            get;
            set;
        }

        /// <summary>
        /// Transitionin vaikutus alue
        /// </summary>
        public virtual Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

    }
    public class GameStateEventArgs : GameEventArgs
    {
        #region Properties
        public ScreenState Current
        {
            get;
            private set;
        }
        #endregion

        public GameStateEventArgs(ScreenState current)
        {
            Current = current;
        }
    }

    public delegate void StateChanged(object sender, GameStateEventArgs e);
}

