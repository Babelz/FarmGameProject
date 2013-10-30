using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.KahvipaussiEngine.Khv.Game.Collision;
using Farmi.World;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game;
using Khv.Game.Collision;
using Khv.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farmi.Entities
{
    public sealed class FarmPlayer : Player
    {
        private readonly FarmWorld world;
        private const float speed = 5f;
        private InputController controller;

        private InputControlSetup defaultInputSetup = new InputControlSetup();
        private InputControlSetup shopInputSetup = new InputControlSetup();

        public FarmPlayer(KhvGame game, FarmWorld world, PlayerIndex index = PlayerIndex.One) : base(game, index)
        {
            this.world = world;
            Position = new Vector2(500, 200);

            Size = new Size(32, 32);

            Collider = new BoxCollider(world, 
                this, 
                new BasicObjectCollisionQuerier(), 
                new BasicTileCollisionQuerier());
            Collider.OnCollision += Collider_OnCollision;
            
        }

        void Collider_OnCollision(object sender, CollisionEventArgs result)
        {
            //Console.WriteLine(sender);
        }

        public void Initialize()
        {
            controller = new InputController(game.InputManager);
            controller.ChangeSetup(defaultInputSetup);
            InitDefaultSetup();
            
            
            //controller.ChangeSetup(shopInputSetup);
        }


        private void InitDefaultSetup()
        {
            var keymapper = defaultInputSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("Move left", Keys.A, Keys.Left), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, -speed));
            keymapper.Map(new KeyTrigger("Move right", Keys.D, Keys.Right), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("Move up", Keys.W, Keys.Up), (triggered, args) => MotionEngine.GoalVelocityY = VelocityFunc(args, -speed));
            keymapper.Map(new KeyTrigger("Move down", Keys.S, Keys.Down), (triggered, args) => MotionEngine.GoalVelocityY = VelocityFunc(args, speed));

            var padmapper = defaultInputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("Move left", Buttons.LeftThumbstickLeft, Buttons.DPadLeft), (triggered, args) => MotionEngine.GoalVelocityX = -speed);
            padmapper.Map(new ButtonTrigger("Move right", Buttons.LeftThumbstickRight, Buttons.DPadRight), (triggered, args) => MotionEngine.GoalVelocityX = speed);
            padmapper.Map(new ButtonTrigger("Move up", Buttons.LeftThumbstickUp, Buttons.DPadUp), (triggered, args) => MotionEngine.GoalVelocityY = -speed);
            padmapper.Map(new ButtonTrigger("Move down", Buttons.LeftThumbstickDown, Buttons.DPadDown), (triggered, args) => MotionEngine.GoalVelocityX = speed);
        }

        private float VelocityFunc(InputEventArgs args, float src)
        {
            if (args.State == InputState.Released)
                return 0;
            return src;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(KhvGame.Temp, new Rectangle((int) position.X, (int) position.Y, size.Width, size.Height), Color.Turquoise );
        }
    }
}
