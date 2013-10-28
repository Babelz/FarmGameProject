using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Khv.Game;
using Khv.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Farmi.Entities
{
    internal sealed class FarmPlayer : Player
    {
        private const float speed = 5f;
        private InputController controller;

        private InputControlSetup defaultInputSetup = new InputControlSetup();
        private InputControlSetup shopInputSetup = new InputControlSetup();

        public FarmPlayer(KhvGame game, PlayerIndex index = PlayerIndex.One) : base(game, index)
        {
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
            keymapper.Map(new KeyTrigger("Move left", Keys.A, Keys.Left), (triggered, args) => MotionEngine.GoalVelocityX = -speed);
            keymapper.Map(new KeyTrigger("Move right", Keys.D, Keys.Right), (triggered, args) => MotionEngine.GoalVelocityX = speed);
            keymapper.Map(new KeyTrigger("Move up", Keys.W, Keys.Up), (triggered, args) => MotionEngine.GoalVelocityY = -speed);
            keymapper.Map(new KeyTrigger("Move down", Keys.S, Keys.Down), (triggered, args) => MotionEngine.GoalVelocityY = -speed);

            var padmapper = defaultInputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("Move left", Buttons.LeftThumbstickLeft, Buttons.DPadLeft), (triggered, args) => MotionEngine.GoalVelocityX = -speed);
            padmapper.Map(new ButtonTrigger("Move right", Buttons.LeftThumbstickRight, Buttons.DPadRight), (triggered, args) => MotionEngine.GoalVelocityX = speed);
            padmapper.Map(new ButtonTrigger("Move up", Buttons.LeftThumbstickUp, Buttons.DPadUp), (triggered, args) => MotionEngine.GoalVelocityY = -speed);
            padmapper.Map(new ButtonTrigger("Move down", Buttons.LeftThumbstickDown, Buttons.DPadDown), (triggered, args) => MotionEngine.GoalVelocityX = speed);
        }
    }
}
