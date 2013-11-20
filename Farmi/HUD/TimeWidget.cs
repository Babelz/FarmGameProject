using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Calendar;
using Khv.Engine;
using Khv.Game.GameObjects;
using Khv.Gui.Components.BaseComponents;
using Khv.Gui.Components;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Khv.Gui.Controls;

namespace Farmi.HUD
{
    public sealed partial class TimeWidget : Widget
    {
        #region Vars
        private readonly CalendarSystem calendar;
        private readonly WeatherSystem weatherSystem;
        #endregion

        public TimeWidget(KhvGame khvGame, GameObject owner, WidgetManager owningManager, string name)
            : base(khvGame, owner, owningManager, name)
        {
            calendar = khvGame.Components.GetGameComponent<CalendarSystem>();
            weatherSystem = khvGame.Components.GetGameComponent<WeatherSystem>();

            Initialize();
        }
    }

    public sealed partial class TimeWidget : Widget
    {
        #region Vars
        private int elapsed;
        private Label timeLabel;
        #endregion

        protected override void Initialize()
        {
            int screenWidth = khvGame.GraphicsDeviceManager.PreferredBackBufferWidth;
            int screenHeight = khvGame.GraphicsDeviceManager.PreferredBackBufferHeight;

            BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "widgetbg"));
            Size = new ControlSize(26, 16, SizeType.Percent);
            Size.Transform(new ControlSize(screenWidth, screenHeight, SizeType.Fixed));

            Colors = new Colors()
            {
                Background = Color.White,
                Foreground = Color.White
            };

            Position = new ControlPosition(screenWidth - size.Width, 0);
            Position.Margin = new Margin(-15, 0, 15, 0);

            #region Controls init
            TextureWrapper weatherTextureWrapper = new TextureWrapper();
            weatherTextureWrapper.ChangeTexture(weatherSystem.CorrespondingWeatherTexture);
            weatherTextureWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));
            this.controlManager.AddControl(weatherTextureWrapper);
            weatherTextureWrapper.Size = new ControlSize(35, 65, SizeType.Percent);
            weatherTextureWrapper.Size.Transform(this.size);
            weatherTextureWrapper.Position = new ControlPosition(size.Width - weatherTextureWrapper.Size.Width, 0);
            weatherTextureWrapper.Position.Margin = new Margin(-10, 0, 10, 0);

            TextureWrapper bgWrapper = new TextureWrapper();
            bgWrapper.BackgroundImage = khvGame.Content.Load<Texture2D>(Path.Combine("Gui", "slot"));
            this.controlManager.AddControl(bgWrapper);
            bgWrapper.Size = new ControlSize(weatherTextureWrapper.Size.Width, weatherTextureWrapper.Size.Height / 3, SizeType.Fixed);
            bgWrapper.Size.Transform(this.size);
            bgWrapper.Position = new ControlPosition(weatherTextureWrapper.Position.Relative.X, 
                                                     weatherTextureWrapper.Position.Relative.Y + weatherTextureWrapper.Size.Height);
            bgWrapper.Position.Margin = new Margin(0, 0, 5, 0);


            Label seasonLabel = new Label();
            this.controlManager.AddControl(seasonLabel);
            seasonLabel.Font = khvGame.Content.Load<SpriteFont>("arial");
            seasonLabel.Text = calendar.CurrentSeason.ToString().Substring(0, 3);
            seasonLabel.Position = new ControlPosition(bgWrapper.Position.Relative.X + bgWrapper.Size.Width / 2 - seasonLabel.Size.Width / 2,
                                                       bgWrapper.Position.Relative.Y);

            Label dateLabel = new Label();
            this.controlManager.AddControl(dateLabel);
            dateLabel.Font = khvGame.Content.Load<SpriteFont>("arial");
            dateLabel.Text = "DateLabel";
            dateLabel.Position = new ControlPosition(0, 0);
            dateLabel.Position.Margin = new Margin(10, 0, 10, 0);
            dateLabel.UpdateActions.Add((c) =>
                {
                    dateLabel.Text = calendar.GetDateDisplayString();
                });
            dateLabel.FontScale = 0.85f;


            timeLabel = new Label();
            this.controlManager.AddControl(timeLabel);
            timeLabel.Font = khvGame.Content.Load<SpriteFont>("arial");
            timeLabel.Text = "TimeLabel";
            timeLabel.Position = new ControlPosition(0, size.Height - timeLabel.Size.Height);
            timeLabel.Position.Margin = new Margin(10, 0, -10, 0);
            timeLabel.FontScale = 1.25f;
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed < 400)
            {
                timeLabel.Text = calendar.GetTimeDisplayString().Replace(":", " ");
            }
            else if (elapsed < 800)
            {
                timeLabel.Text = calendar.GetTimeDisplayString();
            }
            else
            {
                elapsed = 0;
            }
        }
    }
}
