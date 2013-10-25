using System;
using System.Linq;
using Khv.Engine.Args;
using Khv.Engine.Structs;
using Khv.Gui.Components.BaseComponents.Containers.Collections;
using Khv.Gui.Components.BaseComponents.Containers.Components.Layout;
using Khv.Gui.Components.EventDispatchers;
using Khv.Gui.Components.EventListeners;
using Khv.Gui.Components.Navigation;
using Khv.Gui.Controls;
using Microsoft.Xna.Framework;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    public class KeyboardButton : Control, IButtonListener
    {
        #region Properties
        public String Button
        {
            get;
            private set;
        }
        public KeyTrigger Trigger
        {
            get;
            set;
        }
        public bool ListenButtonEvents
        {
            get;
            set;
        }
        #endregion

        #region Event listeners
        public ButtonEventDispatcher ButtonEventListener
        {
            get
            {
                return (ButtonEventDispatcher)eventDispatchers.Find(d => d is ButtonEventDispatcher);
            }
        }
        #endregion

        public KeyboardButton(string value)
            : base()
        {
            Button = value;
            Visible = true;
            Enabled = true;
            Trigger = new KeyTrigger(Microsoft.Xna.Framework.Input.Keys.Enter);

            ListenButtonEvents = true;
        }
    }
    public class OnScreenKeyboard : LayoutPanel
    {
        #region Vars
        private readonly static string atoz = new String(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToArray());
        private KeyboardButton[][] cells;
        private int cellsPerRow;

        private IndexNavigator navigator;
        private FocusManager focusManager;

        private NavigationTriggers triggers;

        private TextField textField;
        #endregion

        #region Properties
        public override Microsoft.Xna.Framework.Graphics.SpriteFont Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                textField.Font = value;
            }
        }
        public int CellsPerRow
        {
            get
            {
                return cellsPerRow;
            }
            set
            {
                if (value != cellsPerRow && value != 0)
                {
                    cellsPerRow = value;
                    int rows = Content.Length / value;

                    if (Content.Length % value != 0)
                    {
                        rows++;
                    }

                    cells = new KeyboardButton[rows][];

                    int temp = Content.Length;
                    for (int i = 0; i < rows; i++)
                    {
                        temp -= value;
                        int count = value;
                        if (temp < 0)
                        {
                            count = temp + value;
                        }
                        cells[i] = new KeyboardButton[count];

                        int offset = (i * value);
                        for (int j = 0; j < count && j < value; j++)
                        {
                            KeyboardButton button = new KeyboardButton(Content[offset + j].ToString());
                            cells[i][j] = button;

                            navigator.AddControl(button, new Index(j, i));
                            button.ButtonEventListener.OnKeyPressed += OnKeyboardButtonPressed;
                        }
                    }
                    focusManager.ChangeFocus(cells[0][0]);
                }
            }
        }
        /// <summary>
        /// Helpperi funktio, palauttaa aakkoset A-Z isona
        /// </summary>
        public static string UpperAToZ
        {
            get
            {
                return atoz;
            }
        }
        /// <summary>
        /// Helpperi funktio, palauttaa aakkoset a-z pienenä
        /// </summary>
        public static string LowerAToZ
        {
            get
            {
                return atoz.ToLower();
            }
        }
        public String Content
        {
            get;
            set;
        }
        #endregion

        public OnScreenKeyboard(string content, int cellsPerRow)
            : base(new BorderLayout())
        {
            focusManager = new FocusManager();
            navigator = new IndexNavigator(focusManager);
            triggers = new NavigationTriggers();

            navigator.OnNavigate += OnNavigate;

            textField = new TextField("kirjota");
            textField.CurrentProcessor.IsTravelsingEnabled = false;
            textField.CurrentProcessor.IsCopyPasteEnabled = false;
            textField.CurrentProcessor.IsTextSelectionEnabled = false;
            textField.Size = new ControlSize(0, 50);

            AddControl(textField, BorderLayout.Down);
            textField.Focus();

            Content = content;
            CellsPerRow = cellsPerRow;

            Enabled = true;
            Visible = true;
        }

        #region Event handlers
        private void OnNavigate(object sender, NavigationEventArgs e)
        {
            if (sender != null)
            {
                focusManager.ChangeFocus((Control)sender);
            }
        }
        private void OnKeyboardButtonPressed(object sender, GameEventArgs e)
        {
            if (sender != null && sender is KeyboardButton)
            {
                KeyboardButton button = sender as KeyboardButton;

                textField.Append(button.Button);
            }
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            textField.Update(gameTime);
            triggers.Update();
            if (triggers.Up.IsPressed())
            {
                navigator.Navigate(NavigationDirection.Up);
            }
            if (triggers.Down.IsPressed())
            {
                navigator.Navigate(NavigationDirection.Down);
            }
            if (triggers.Left.IsPressed())
            {
                navigator.Navigate(NavigationDirection.Left);
            }
            if (triggers.Right.IsPressed())
            {
                navigator.Navigate(NavigationDirection.Right);
            }

            for (int y = 0; y < cells.Length; y++)
            {
                for (int x = 0; x < cells[y].Length; x++)
                {
                    cells[y][x].Update(gameTime);
                }
            }

        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for (int y = 0; y < cells.Length; y++)
            {
                float margin = 25f;
                Vector2 destination = new Vector2(Position.Real.X, Position.Real.Y + y * 25); ;
                for (int x = 0; x < cells[y].Length; x++)
                {
                    KeyboardButton button = cells[y][x];
                    destination.X = Position.Real.X + x * margin;
                    if (button.HasFocus)
                    {
                        Vector2 size = Font.MeasureString(button.Button);
                        spriteBatch.Draw(Khv.Engine.KhvGame.Temp,
                            new Rectangle((int)destination.X - (int)((margin - size.X) / 2), (int)destination.Y,
                                (int)margin,
                                (int)size.Y),
                                Color.Green);
                    }
                    spriteBatch.DrawString(Font, button.Button, destination, Color.Azure);
                }
            }
        }
        #endregion
    }
}
