using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    public class TextInputProcessor
    {
        #region Vars
        protected TimeSpan delayTime;
        protected TimeSpan repeatTime;

        protected TimeSpan elapsedTime = TimeSpan.FromMilliseconds(0);

        private Keys[] lastKeys = new Keys[0];
        private bool shouldWait = false;

        protected int caretPosition = 0;
        protected StringBuilder text;

        protected string selectedText = "";
        #endregion

        #region Properties
        /// <summary>
        /// Palauttaa nykyisen tekstin
        /// Asettaa uuden tekstin, pyyhkii vanhan tekstin pois ja korjaa caretin paikan
        /// </summary>
        public string Text
        {
            get
            {
                return text.ToString();
            }
            set
            {
                if (value == null)
                    value = "";
                text.Clear();
                text.Append(value);
                caretPosition = text.Length;
            }
        }

        /// <summary>
        /// Lisää tekstiä joko loppuun tai nykyiseen caretin paikkaan
        /// </summary>
        /// <param name="text">Teksti mikä lisätään</param>
        /// <param name="toCurrentCaretPosition">False loppuun, true nykyisen caretin paikkaan</param>
        public void Append(string text, bool toCurrentCaretPosition = false)
        {
            if (toCurrentCaretPosition)
            {
                this.text.Insert(caretPosition, text);

            }
            else
            {
                this.text.Append(text);
            }

            caretPosition += text.Length;
        }

        /// <summary>
        /// Voidaanko liikkua nuolinäppäimillä tai homella ja endillä tekstissä
        /// </summary>
        public bool IsTravelsingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Jos voi valita (maalata) tekstiä
        /// </summary>
        public bool IsTextSelectionEnabled
        {
            get;
            set;
        }

        public string SelectedText
        {
            get { return selectedText; }
        }

        public int SelectionStartIndex
        {
            get;
            private set;
        }

        public int SelectionEndIndex
        {
            get;
            private set;
        }

        public int CaretPosition
        {
            get { return caretPosition; }
        }

        public bool IsTextSelected
        {
            get
            {
                return !selectedText.Equals("");
            }
        }

        public bool IsCopyingEnabled
        {
            get;
            set;
        }

        public bool IsPasteEnabled
        {
            get;
            set;
        }

        public bool IsCopyPasteEnabled
        {
            get
            {
                return IsCopyingEnabled && IsPasteEnabled;
            }
            set
            {
                IsCopyingEnabled = value;
                IsPasteEnabled = value;
            }
        }
        #endregion

        /// <summary>
        /// Luo uuden input prosessorin 500 millisekunnin delaylla ja 50 millisekunnin repeatilla
        /// </summary>
        public TextInputProcessor()
            : this("", 500, 50)
        {
        }
        /// <summary>
        /// Luo uuden input prosessorin valitulla tekstillä,
        /// 500 millisekunnin delaylla ja 50 millisekunnin repeatilla
        /// </summary>
        public TextInputProcessor(string text)
            : this(text, 500, 50)
        {
        }
        /// <summary>
        /// Luo uuden input prosessorin valituilla ajoilla millisekunneissa
        /// </summary>
        /// <param name="delay">Kuinka iso viive napin painalluksen jälkeen ennen kuin aletaan toistamaan</param>
        /// <param name="repeat">Kuinka iso viive napin toistamisen välillä</param>
        public TextInputProcessor(string text, double delay, double repeat)
        {
            delayTime = TimeSpan.FromMilliseconds(delay);
            repeatTime = TimeSpan.FromMilliseconds(repeat);
            this.text = new StringBuilder(text);
            caretPosition = text.Length;
            SelectionEndIndex = -1;
            SelectionStartIndex = -1;
        }

        #region Methods
        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            KeyboardState state = Keyboard.GetState();
            Keys[] newPressed = state.GetPressedKeys();
            bool firstPress = false;

            // jos ollaan vaihdettu nappia
            foreach (Keys newKey in newPressed)
            {
                if (!lastKeys.Contains(newKey))
                {
                    shouldWait = true;
                    firstPress = true;
                    elapsedTime = TimeSpan.FromMilliseconds(0);
                    break;
                }
            }

            // ollaan vaihdettu nappia 
            if (shouldWait)
            {
                // jos kuitenkin odotellaan ennen repeatin alkamista
                if (firstPress)
                {
                    HandlePress(state);
                }
                // ollaan odoteltu tarpeeksi, voidaan aloittaa repeat
                else if (elapsedTime >= delayTime)
                {
                    shouldWait = false;
                    elapsedTime = TimeSpan.FromMilliseconds(elapsedTime.TotalMilliseconds % delayTime.TotalMilliseconds);
                }
            }
            // repeat key
            else
            {
                // onko voidaanko jo lisätä
                if (elapsedTime >= repeatTime)
                {
                    HandlePress(state);
                    elapsedTime = TimeSpan.FromMilliseconds(elapsedTime.TotalMilliseconds % repeatTime.TotalMilliseconds);
                }
            }
            // otetaan vanhat napit talteen
            lastKeys = newPressed;
        }

        private void HandlePress(KeyboardState state)
        {
            bool shift = false;
            bool capslock = false;

            int previousCaretPosition = caretPosition;

            if (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
            {
                shift = true;
            }
            // jos on sallittu liikkuminen tekstissä
            if (IsTravelsingEnabled)
            {
                // halutaan päästä vasemmalle
                if (state.IsKeyDown(Keys.Left))
                {
                    caretPosition = (int)MathHelper.Clamp(caretPosition - 1, 0, int.MaxValue);

                }
                // halutaan oikealle
                else if (state.IsKeyDown(Keys.Right))
                {
                    caretPosition = (int)MathHelper.Clamp(caretPosition + 1, 0, text.Length);
                }
                // halutaan tekstin loppuun
                else if (state.IsKeyDown(Keys.End))
                {
                    caretPosition = text.Length;
                    if (caretPosition < 0)
                        caretPosition = 0;
                }
                // halutaan tekstin alkuun
                else if (state.IsKeyDown(Keys.Home))
                {
                    caretPosition = 0;
                }

                if (IsTextSelectionEnabled && !shift)
                {
                    if (previousCaretPosition != caretPosition)
                    {

                        // selectionia ei ole ennää
                        SelectionStartIndex = -1;
                        SelectionEndIndex = -1;
                        selectedText = "";
                    }
                }
            }

            if (IsTextSelectionEnabled)
            {
                if ((state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl)) && state.IsKeyDown(Keys.A))
                {
                    selectedText = text.ToString();
                    SelectionStartIndex = 0;
                    SelectionEndIndex = text.Length;
                    return;
                }
                else if (shift)
                {
                    if (state.IsKeyDown(Keys.Left))
                    {
                        // ei ole selectionia
                        if (SelectionStartIndex == -1)
                        {
                            SelectionStartIndex = caretPosition;
                            if (SelectionEndIndex == -1)
                            {
                                SelectionEndIndex = caretPosition + 1;
                            }
                        }
                        if (caretPosition < SelectionStartIndex)
                        {
                            SelectionStartIndex = caretPosition;
                        }
                        if (caretPosition > SelectionStartIndex)
                        {
                            SelectionEndIndex = caretPosition;
                        }
                    }
                    else if (state.IsKeyDown(Keys.Right))
                    {
                        if (SelectionEndIndex == -1)
                        {
                            SelectionStartIndex = caretPosition - 1;
                            SelectionEndIndex = caretPosition;
                            return;
                        }
                        if (caretPosition > SelectionEndIndex)
                        {
                            SelectionEndIndex = caretPosition;
                        }
                        if (caretPosition - 1 == SelectionStartIndex)
                        {
                            SelectionStartIndex = caretPosition;

                            if (SelectionStartIndex == SelectionEndIndex)
                            {
                                SelectionEndIndex = -1;
                                SelectionStartIndex = -1;
                                selectedText = "";
                            }
                        }
                    }
                    // selection nykyisestä kohdasta alkuun
                    else if (state.IsKeyDown(Keys.Home))
                    {
                        if (Text.Length != 0 && previousCaretPosition != 0)
                        {
                            SelectionStartIndex = 0;
                            SelectionEndIndex = previousCaretPosition;
                        }

                    }
                    // halutaan loppuun
                    else if (state.IsKeyDown(Keys.End))
                    {
                        if (Text.Length != 0 && previousCaretPosition != Text.Length)
                        {
                            SelectionStartIndex = previousCaretPosition;
                            SelectionEndIndex = caretPosition;
                        }
                    }
                    if (SelectionStartIndex != -1 && SelectionEndIndex != -1)
                    {
                        selectedText = Text.Substring(SelectionStartIndex, SelectionEndIndex - SelectionStartIndex);
                    }
                }
                HandleCopyPaste(state);
            }

            // halutaan pyyhkiä tekstiä
            if (state.IsKeyDown(Keys.Back))
            {
                if (text.Length != 0)
                {
                    if (IsTextSelectionEnabled)
                    {
                        // jos on valittu jotain
                        if (!SelectedText.Equals(""))
                        {
                            caretPosition = SelectionStartIndex;
                            text.Remove(SelectionStartIndex, selectedText.Length);
                            selectedText = "";
                            SelectionStartIndex = -1;
                            SelectionEndIndex = -1;
                            return;
                        }
                    }
                    // jos ollaan tekstin alussa ei tarvi tehä yhtään mitään
                    if (caretPosition == 0)
                    {
                        return;
                    }
                    text.Remove(caretPosition - 1, 1);
                    caretPosition--;
                }
                caretPosition = (int)MathHelper.Clamp(caretPosition, 0, text.Length);

            }
            else
            {
                Keys[] pressed = state.GetPressedKeys();
                List<Keys> pressedKeys = pressed.ToList();
                if (lastKeys.Length != pressed.Length)
                {
                    for (int i = pressed.Length - 1; i >= 0; i--)
                    {
                        if (lastKeys.Contains(pressed[i]))
                            pressedKeys.Remove(pressedKeys[i]);
                    }
                }
                foreach (Keys key in pressedKeys)
                {
                    string s = TranslateKey(key, shift, capslock);
                    if (s.Length != 0)
                    {
                        if (IsTextSelectionEnabled && IsTextSelected)
                        {
                            text.Remove(SelectionStartIndex, SelectionEndIndex - SelectionStartIndex);
                            text.Insert(SelectionStartIndex, s);
                            caretPosition = SelectionStartIndex + 1;
                            SelectionEndIndex = -1;
                            SelectionStartIndex = -1;
                            selectedText = "";

                        }
                        else
                        {
                            text.Insert(caretPosition, s);
                            caretPosition++;
                        }
                    }
                    break;
                }
            }
        }

        private void HandleCopyPaste(KeyboardState state)
        {
            bool control = state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);

            if (IsCopyingEnabled && control)
            {
                if (state.IsKeyDown(Keys.C) && IsTextSelected)
                {
                    // TODO: puuttuu copy - textinputprocessor.
                }
            }
            else if (IsPasteEnabled && control)
            {
                if (state.IsKeyDown(Keys.V))
                {
                    // TODO: puuttuu paste - textinputprocessor.
                }
            }

        }

        public string TranslateKey(Keys key, bool shift, bool capslock)
        {
            string keystr = key.ToString();

            bool shouldCapitalize = false;
            if (capslock && !shift)
                shouldCapitalize = false;
            else if (shift)
                shouldCapitalize = true;
            else if (capslock)
                shouldCapitalize = true;


            //  hyvin meni ja aakkonen tuli
            if (keystr.Length == 1)
            {
                return (shouldCapitalize) ? keystr.ToUpper() : keystr.ToLower();
            }

            // numerot on D\d+
            if (keystr.Length == 2)
            {
                if (keystr.StartsWith("D"))
                {

                    keystr = keystr.Substring(1, 1);
                    return keystr;
                }
            }

            if (keystr.StartsWith("NumPad"))
            {
                return keystr.Substring(6, 1);
            }

            if (key == Keys.Space)
                return " ";

            return "";
        }

        public void SelectText(string what)
        {
            string currentText = Text;

            if (currentText.Contains(what))
            {
                int start = currentText.IndexOf(what);
                int end = start + what.Length - 1;
                selectedText = what;
                SelectionStartIndex = start;
                SelectionEndIndex = end;
            }

        }
        #endregion
    }
}
