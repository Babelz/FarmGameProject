using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Gui.Components;
using Khv.Engine.Structs;
using Khv.Engine.Args;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    public class IndexNavigator
    {
        #region Vars
        private List<Control> controls;
        private FocusManager focusManager;
        private bool autoCorrectOnNull;
        #endregion

        #region Events
        public event NavigationEventHandler OnNavigate;
        public event NavigationEventHandler OnNullNavigate;
        #endregion

        #region Properties
        public int MaxFocusIndex_X
        {
            get
            {
                return controls.Max((Control c) => c.FocusIndex.X);
            }
        }
        public int MinFocusIndex_X
        {
            get
            {
                return controls.Min((Control c) => c.FocusIndex.X);
            }
        }
        public int MaxFocusIndex_Y
        {
            get
            {
                return controls.Max((Control c) => c.FocusIndex.Y);
            }
        }
        public int MinFocusIndex_Y
        {
            get
            {
                return controls.Min((Control c) => c.FocusIndex.Y);
            }
        }
        public List<Control> Controls
        {
            get
            {
                return controls;
            }
        }
        // Boolean siitä onko autocorrect päällä.
        public bool AutoCorrectOnNull
        {
            get
            {
                return autoCorrectOnNull;
            }
            set
            {
                autoCorrectOnNull = value;
            }
        }
        #endregion

        // Alustaa navigaattorin, asettaa viitteen, autocorrect on vakiona true eli käytössä.
        public IndexNavigator(FocusManager focusManager)
        {
            this.focusManager = focusManager;
            controls = new List<Control>();
            autoCorrectOnNull = true;
        }
        // Lisää kontrollin ja asettaa indeksin sen lisäyksen yhteydessä.
        public void AddControl(Control control, Index index)
        {
            if (index == Index.Empty)
            {
                throw new Exception("Both indexes must be greater than zero.");
            }
            control.FocusIndex = new Index(index.X, index.Y);
            controls.Add(control);
        }
        // Lisää kontrollin.
        public void AddControl(Control control)
        {
            if (control.FocusIndex == Index.Empty)
            {
                throw new Exception("Control with zero index (x: -1 || y: -1) cant be added.");
            }
            Controls.Add(control);
        }
        /// <summary>
        /// Kutustaan kun halutaan navigoida kontrollien välillä eli vaihtaa focusta,
        /// ennen kutsua tulee alustaa OnNavigate eventti joka saa metodista seuraavan focusable
        /// kontrollin parametriksi ja suunnan minne haluttiin liikkua, ilman eventtiä kutsut ovat turhia
        /// koska koodarin tulee kertoa eventissä miten niihin halutaan reagoida.
        /// </summary>
        /// <param name="direction"></param>
        public void Navigate(NavigationDirection direction)
        {
            // Metodi muuttujat, candidates saa viitteet kontrolleista
            // metodi välityksellä.
            Control controlNextInFocus;
            List<Control> candidates = GetCandidates(direction);

            // Jos kandidaatteja on nolla, laukaisee onnullnavigate eventti ja jos 
            // autocorrect on käytössä, kutsutaan sitä myös.
            if (candidates.Count == 0)
            {
                if (OnNullNavigate != null)
                {
                    OnNullNavigate(focusManager.CurrentFocused, new NavigationEventArgs(direction));
                }
                if (autoCorrectOnNull)
                {
                    AutoCorrect(direction);
                }
                return;
            }

            controlNextInFocus = GetNextInFocus(direction, candidates);
        }
        // Hakee seuraavan kontrollin siitä suunnasta mihin halutaan mennä
        // ja lauakisee lopuksi onnavigate eventin.
        private Control GetNextInFocus(NavigationDirection direction, List<Control> candidates)
        {
            Control controlNextInFocus = null;
            Predicate<Control> nextControlPredicate = GetNextControlPredicate(direction, candidates);

            controlNextInFocus = candidates.Find(c => nextControlPredicate(c));

            OnNavigate(controlNextInFocus, new NavigationEventArgs(direction));

            return controlNextInFocus;
        }
        // Hakee kontrollit joihin voidaan mahdollisesti asettaa focus directionin
        // perusteella, otetaan kiinni lista jonka metodi palauttaa navigate eventissä.
        private List<Control> GetCandidates(NavigationDirection direction)
        {
            List<Control> candidates = new List<Control>();
            Predicate<Control> candidatePredicate = GetCandidatePredicate(direction);

            controls.ForEach(c =>
                {
                    if (candidatePredicate(c))
                    {
                        candidates.Add(c);
                    }
                });

            return candidates;
        }
        // Kutsutaan jos autocorrect on päällä, korjaa focuksen 
        // sen mukaan mihin haluttiin liikkua.
        private void AutoCorrect(NavigationDirection direction)
        {
            Control focusable = null;
            Predicate<Control> autoCorrectPredicate = GetAutoCorrectPredicate(direction);

            controls.Find(c => autoCorrectPredicate(c));

            OnNavigate(focusable, new NavigationEventArgs(direction));
        }

        #region Predicate prosessing methods
        // Palauttaa predicaatin jolla pyritään autokorjaamaan focus.
        private Predicate<Control> GetAutoCorrectPredicate(NavigationDirection direction)
        {
            Predicate<Control> autoCorrectPredicate = null;

            switch (direction)
            {
                case NavigationDirection.Up:
                    // Ottaa alimman kontrollin samasta x indeksistä.
                    autoCorrectPredicate = new Predicate<Control>(c => c.FocusIndex.Y == MaxFocusIndex_Y && c.FocusIndex.X == focusManager.CurrentFocused.FocusIndex.X);
                    break;
                case NavigationDirection.Down:
                    // Ottaa korkeimman kontrollin samasta x indeksistä.
                    autoCorrectPredicate = new Predicate<Control>(c => c.FocusIndex.Y == MinFocusIndex_Y && c.FocusIndex.X == focusManager.CurrentFocused.FocusIndex.X);
                    break;
                case NavigationDirection.Left:
                    // Ottaa kontrollin jonka indeksi x on pienenin.
                    autoCorrectPredicate = new Predicate<Control>(c => c.FocusIndex.X == MaxFocusIndex_X && c.FocusIndex.Y == focusManager.CurrentFocused.FocusIndex.Y);
                    break;
                case NavigationDirection.Right:
                    // Otta kontrollin jonka indeksi x on suurin.
                    autoCorrectPredicate = new Predicate<Control>(c => c.FocusIndex.X == MinFocusIndex_X && c.FocusIndex.Y == focusManager.CurrentFocused.FocusIndex.Y);
                    break;
            }

            return autoCorrectPredicate;
        }
        // Palauttaa predicaatin jolla haetaan seruaava focuksen saava control.
        private Predicate<Control> GetNextControlPredicate(NavigationDirection direction, List<Control> candidates)
        {
            Predicate<Control> nextControlPredicate = null;

            switch (direction)
            {
                case NavigationDirection.Up:
                    nextControlPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y == candidates.Max((Control n) => n.FocusIndex.Y));
                    break;
                case NavigationDirection.Down:
                    nextControlPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y == candidates.Min((Control n) => n.FocusIndex.Y));
                    break;
                case NavigationDirection.Left:
                    nextControlPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y == focusManager.CurrentFocused.FocusIndex.Y && c.FocusIndex.X == candidates.Max((Control n) => n.FocusIndex.X));
                    break;
                case NavigationDirection.Right:
                    nextControlPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y == focusManager.CurrentFocused.FocusIndex.Y && c.FocusIndex.X == candidates.Min((Control n) => n.FocusIndex.X));
                    break;
                default:
                    break;
            }

            return nextControlPredicate;
        }
        // Palauttaa predicaatin jolla haetaan mahdolliset nextinfocus controllit.
        private Predicate<Control> GetCandidatePredicate(NavigationDirection direction)
        {
            Predicate<Control> resultPredicate = null;

            switch (direction)
            {
                case NavigationDirection.Up:
                    resultPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y < focusManager.CurrentFocused.FocusIndex.Y && c.FocusIndex.X == focusManager.CurrentFocused.FocusIndex.X);
                    break;
                case NavigationDirection.Down:
                    resultPredicate = new Predicate<Control>(
                        c => c.FocusIndex.Y > focusManager.CurrentFocused.FocusIndex.Y && c.FocusIndex.X == focusManager.CurrentFocused.FocusIndex.X);
                    break;
                case NavigationDirection.Left:
                    resultPredicate = new Predicate<Control>(
                        c => c.FocusIndex.X < focusManager.CurrentFocused.FocusIndex.X);
                    break;
                case NavigationDirection.Right:
                    resultPredicate = new Predicate<Control>(
                        c => c.FocusIndex.X > focusManager.CurrentFocused.FocusIndex.X);
                    break;
                default:
                    break;
            }

            return resultPredicate;
        }
        #endregion
    }
    public class NavigationEventArgs : GameEventArgs
    {
        #region Properties
        public NavigationDirection Direction
        {
            get;
            private set;
        }
        #endregion

        public NavigationEventArgs(NavigationDirection direction)
        {
            Direction = direction;
        }
    }

    public delegate void NavigationEventHandler(object sender, NavigationEventArgs e);
}
