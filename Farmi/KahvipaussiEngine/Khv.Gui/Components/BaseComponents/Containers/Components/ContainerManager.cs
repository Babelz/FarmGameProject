using System;
using Khv.Gui.Components.BaseComponents.Containers.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Khv.Engine.Args;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    /// <summary>
    /// Luokka joka wräppää containereita itseensä ja hoitaa niiden vaihtamisen.
    /// </summary>
    public class ContainerManager<T> where T : Container
    {
        #region Vars
        private T maiContainer;
        private T[] childContainers;

        // Containerin vaihtoon liittyvät fieldit.
        private T next;
        private TransitionPlayer currentPlayer;
        #endregion

        #region Events
        public event ContainerEventHandler OnChange;
        public event ContainerEventHandler OnChanged;
        #endregion

        #region Properties
        public T Current
        {
            get;
            private set;
        }
        public bool IsInTransition
        {
            get
            {
                return currentPlayer != null && currentPlayer.IsPlaying;
            }
        }
        public bool Enabled
        {
            get;
            set;
        }
        #endregion

        public ContainerManager(IContainerBuilder<T> containerBuilder)
        {
            Enabled = true;

            maiContainer = containerBuilder.BuildMainContainer(this);
            childContainers = containerBuilder.BuildChildContainers(this);

            Current = maiContainer;
        }

        // Wrapperi currentin disabloinnille.
        private void DisableCurrent()
        {
            Current.Defocus();
            Current.Enabled = false;
            Current.Visible = false;
        }
        // Wrapperi currentin enabloinnille.
        private void EnableCurrent()
        {
            Current.Focus();
            Current.Enabled = true;
            Current.Visible = true;
        }
        
        /// <summary>
        /// Hakee containerin managerista jos se on olemassa.
        /// Palauttaa nullin jos containeria ei löydy.
        /// </summary>
        public T GetContainer(Predicate<T> predicate)
        {
            T results = null;

            // Jos container on main, palautetaan se suoriltaa,
            // muulloin pudotaan alempaa looppiin ja etsitään containeria
            // childeistä.
            if (predicate.Invoke(maiContainer))
            {
                results = maiContainer;
            }
            else
            {
                foreach (T container in childContainers)
                {
                    if (predicate.Invoke(container))
                    {
                        results = container;
                        break;
                    }
                }
            }

            return results;
        }
        /// <summary>
        /// Vaihtaa ikkunan ja aloittaa siirtymän piirtämisen.
        /// Hoitaa siirtymän toiston aikana ikkunoiden välisen focuksen vaihdon.
        /// Voidaan vaihtaa ikkuna myös ilman siirtymä efektiä.
        /// </summary>
        /// <param name="container">Container joka halutaan vaihtaa aktiiviseksi.</param>
        /// <param name="transitionPlayer">Toistaja joka toistaa siirtymä efektit.</param>
        /// <returns>Palauttaa truen jos ikkuna löytyi ja se vaihdetaan. Falsen jos ikkunaa ei löytynyt.</returns>
        public bool ChangeContainer(T container, TransitionPlayer transitionPlayer = null)
        {
            bool results = false;

            // Jos container ei kuulu tähän manageriin, palauttaa falsen suoraan.
            if (GetContainer(c => c == container) != null && currentPlayer == null)
            {
                next = container;
                currentPlayer = transitionPlayer;

                if (OnChange != null)
                {
                    OnChange(next, new ContainerEvetArgs(Current));
                }

                // Jossei playeriä syötetty, vaihdetaan ikkuna suoriltaan.
                if (currentPlayer == null)
                {
                    DisableCurrent();
                    Current = container;

                    EnableCurrent();
                    next = null;
                }
                else
                {
                    Current.Defocus();
                    Current.Enabled = false;

                    // Hookataan eventit joilla hallitaan ikkunoiden focusta siirtymä efektien välillä.
                    transitionPlayer.OnInTransitionFinished += new TransitionEventHandler(transitionPlayer_OnInTransitionFinished);
                    transitionPlayer.OnOutTransitionFinished += new TransitionEventHandler(transitionPlayer_OnOutTransitionFinished);
                    currentPlayer.Start();
                }

                results = true;
            }

            return results;
        }
       
        /// <summary>
        /// Päivittää tämän hetkistä siirtymää ja aktiivista ikkunaa.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                Current.Update(gameTime);
                if (currentPlayer != null)
                {
                    // Viimeinen steppi siirtymässä, jos ollaan
                    // toistettu koko siirtymä, asettaa sen nulliksi.
                    if (!currentPlayer.IsFinished)
                    {
                        currentPlayer.Update(gameTime);
                    }
                    else
                    {
                        currentPlayer = null;

                        if (OnChanged != null)
                        {
                            OnChanged(next, new ContainerEvetArgs(Current));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Piirtää tämän hetkisen siirtymän ja containerin.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled)
            {
                Current.Draw(spriteBatch);
                if (currentPlayer != null)
                {
                    currentPlayer.Draw(spriteBatch);
                }
            }
        }

        #region Event handlers
        private void transitionPlayer_OnOutTransitionFinished(object sender, GameEventArgs e)
        {
            Current = next;
            Current.Visible = true;
            Current.Enabled = false;
            Current.RefreshChildPositions();
            Current.DoAllActions();
            Current.UpdateOnce();
        }
        private void transitionPlayer_OnInTransitionFinished(object sender, GameEventArgs e)
        {
            EnableCurrent();
        }
        #endregion
    }
    public class ContainerEvetArgs : GameEventArgs
    {
        #region Properites
        public Container Current
        {
            get;
            private set;
        }
        #endregion

        public ContainerEvetArgs(Container current)
        {
            Current = current;
        }
    }

    public delegate void ContainerEventHandler(object sender, ContainerEvetArgs e);
}
