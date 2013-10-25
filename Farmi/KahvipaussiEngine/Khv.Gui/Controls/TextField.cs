using Khv.Engine.Structs;
using Khv.Gui.Components;
using Microsoft.Xna.Framework;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components.BaseComponents.Containers.Components;

namespace Khv.Gui.Controls
{
    public class TextField : Control
    {
        #region Vars

        protected TextInputProcessor textProcessor;

        private string caret = "|";


        private Vector2 caretPositionInPixels = Vector2.Zero;
        private Color caretColor = Color.Black;

        #endregion

        #region Properties
        public TextInputProcessor CurrentProcessor
        {
            get
            {
                return textProcessor;
            }
        }

        public Size TextSize
        {
            get
            {
                Vector2 textSize = Font.MeasureString(textProcessor.Text);
                return new Size((int)textSize.X, (int)textSize.Y);
            }
        }
        #endregion

        /// <summary>
        /// Luo uuden text fieldin tyhjällä tekstillä
        /// Laittaa kentän näkyviin ja enabloi sen
        /// </summary>
        public TextField() : this("")
        {
            Name = "TextField";
        }

        /// <summary>
        /// Luo uuden text fieldin valitulla tekstillä
        /// Laittaa kentän näkyviin ja enabloi sen
        /// </summary>
        /// <param name="text"></param>
        public TextField(string text)
            : base()
        {
            textProcessor = new TextInputProcessor(text);
            textProcessor.IsTravelsingEnabled = true;
            textProcessor.IsTextSelectionEnabled = true;
            textProcessor.IsCopyPasteEnabled = true;
            Visible = true;
            Enabled = true;

        }

        #region Methods
        private Vector2 CalculateCaretPosition()
        {
            string text = textProcessor.Text;
            int caretIndex = textProcessor.CaretPosition;

            Vector2 caretOffset = Font.MeasureString(text.Substring(0, caretIndex));
            Vector2 caretPosition = new Vector2(caretOffset.X + Position.Real.X, Position.Real.Y);
            if (caretIndex < text.Length)
            {
                Vector2 caretSecondOffset = Font.MeasureString(text.Substring(caretIndex, 1));
                caretPosition.X -= (caretSecondOffset.X / 2);
            }
            
            else if (caretIndex != 0 && caretIndex == text.Length)
            {
                Vector2 caretSecondOffset = Font.MeasureString(text.Substring(caretIndex - 1, 1));
                caretPosition.X -= (caretSecondOffset.X / 2);
            }

            return caretPosition;
        }
        public void Append(string text)
        {
            textProcessor.Append(text, true);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (HasFocus)
            {
               // textProcessor.SelectText("kel");
                caretPositionInPixels = CalculateCaretPosition();
                textProcessor.Update(gameTime);
            }

            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            string text = textProcessor.Text;

            if (textProcessor.IsTextSelectionEnabled)
            {
                string selectedText = textProcessor.SelectedText;

                // on valittua tekstiä
                if (!selectedText.Equals(""))
                {
                    int startIndex = textProcessor.SelectionStartIndex;
                    int endIndex = textProcessor.SelectionEndIndex;

                    Vector2 size = Font.MeasureString(selectedText);
                    Vector2 offset = Vector2.Zero;
                    if (startIndex != 0)
                    {
                        offset = Font.MeasureString(text.Substring(0, startIndex));
                    }

                    Rectangle selectionPosition = new Rectangle(Position.Real.X + (int)offset.X, Position.Real.Y, (int)size.X, (int)size.Y);
                    spriteBatch.Draw(Khv.Engine.KhvGame.Temp, selectionPosition, Color.Green);
                }
            }
            Vector2 position = new Vector2(Position.Real.X, Position.Real.Y);
            
            spriteBatch.DrawString(Font, text, position, Colors.Foreground, 0.0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, caret, caretPositionInPixels, caretColor); 
        }
        #endregion
    }
}
