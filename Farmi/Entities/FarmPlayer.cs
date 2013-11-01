using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Components;
using Farmi.KahvipaussiEngine.Khv.Game.Collision;
using Farmi.World;
using Khv.Engine;
using Khv.Engine.Helpers;
using Khv.Engine.Structs;
using Khv.Game;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Gui.Components.BaseComponents;
using Khv.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenTK.Input;

namespace Farmi.Entities
{
    public sealed class FarmPlayer : Player
    {
        #region Vars
        private readonly FarmWorld world;
        private const float speed = 5f;


        private InputController controller;
        private InputControlSetup defaultInputSetup;
        private InputControlSetup shopInputSetup;
        private Texture2D texture;

        private ViewComponent viewComponent;

        #endregion

        #region Properties
        public bool CouldInteract
        {
            get
            {
                return ClosestInteractable != null;
            }
        }
        public GameObject ClosestInteractable
        {
            get;
            set;
        }
        #endregion

        public FarmPlayer(KhvGame game, FarmWorld world, PlayerIndex index = PlayerIndex.One)
            : base(game, index)
        {
            defaultInputSetup = new InputControlSetup();
            shopInputSetup = new InputControlSetup();
            
            this.world = world;
            Position = new Vector2(500, 200);

            Size = new Size(32, 32);

            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());

            Collider.OnCollision += Collider_OnCollision;
            viewComponent = new ViewComponent(new Vector2(0, 1));
            Components.Add(viewComponent);
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

            Components.Add(new ExclamationMarkDrawer(game, this));
            texture = game.Content.Load<Texture2D>("ukko");
        }


        private void InitDefaultSetup()
        {
            var keymapper = defaultInputSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("Move left", Keys.A, Keys.Left), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, -speed));
            keymapper.Map(new KeyTrigger("Move right", Keys.D, Keys.Right), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("Move up", Keys.W, Keys.Up), (triggered, args) => MotionEngine.GoalVelocityY = VelocityFunc(args, -speed));
            keymapper.Map(new KeyTrigger("Move down", Keys.S, Keys.Down), (triggered, args) => MotionEngine.GoalVelocityY = VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("Interact", Keys.Space), (triggered, args) => TryInteract(args));

            var padmapper = defaultInputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("Move left", Buttons.LeftThumbstickLeft, Buttons.DPadLeft), (triggered, args) => MotionEngine.GoalVelocityX = -speed);
            padmapper.Map(new ButtonTrigger("Move right", Buttons.LeftThumbstickRight, Buttons.DPadRight), (triggered, args) => MotionEngine.GoalVelocityX = speed);
            padmapper.Map(new ButtonTrigger("Move up", Buttons.LeftThumbstickUp, Buttons.DPadUp), (triggered, args) => MotionEngine.GoalVelocityY = -speed);
            padmapper.Map(new ButtonTrigger("Move down", Buttons.LeftThumbstickDown, Buttons.DPadDown), (triggered, args) => MotionEngine.GoalVelocityX = speed);
        }

        private void TryInteract(InputEventArgs args)
        {
            if (args.State != InputState.Released)
            {
                return;
            }

            if (ClosestInteractable == null)
            {
                return;
            }

            (ClosestInteractable.Components.GetComponent(c => c is IInteractionComponent) as IInteractionComponent).Interact(this);

        }

        public void CanSee(GameObject g)
        {
            Vector2 v2 = g.Position;
            v2.Normalize();
            Console.WriteLine(
                    1 - VectorHelper.DotProduct(viewComponent.ViewVector, v2)
                );
            Console.WriteLine(viewComponent.ViewVector);
            Console.WriteLine(g);
        }

        private float VelocityFunc(InputEventArgs args, float src)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }

            return src;
        }

        private float VerticalVelocityFunc(InputEventArgs args, float velY)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }
            if (velY != 0 && (Velocity.X > 0 || Velocity.X < 0))
            {
                return 0;
            }
            if (velY < 0)
                viewComponent.ViewVector = new Vector2(0, -1);
            else if (velY > 0)
                viewComponent.ViewVector = new Vector2(0, 1);
            return velY;
        }

        private float HorizontalVelocityFunc(InputEventArgs args, float velX)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }
            if (velX != 0 && (Velocity.Y > 0 || Velocity.Y < 0))
                return 0;
            if (velX < 0)
                viewComponent.ViewVector = new Vector2(-1, 0);
            else if (velX > 0)
                viewComponent.ViewVector = new Vector2(1, 0);
            return velX;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);
            ClosestInteractable = world.GetNearestInteractable(this, new Padding(10, 5));
            //if (ClosestInteractable != null)
            //    CanSee(ClosestInteractable);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            #region test
            /*  GameObject source = this;
            Padding radius = new Padding(10, 5);

            Rectangle r = new Rectangle((int)(source.Position.X - radius.Left), (int)(source.Position.Y - radius.Top), radius.Left + radius.Right , radius.Top + radius.Bottom * 2);
            spriteBatch.Draw(KhvGame.Temp, r, Color.Red);*/
            #endregion
            //spriteBatch.Draw(KhvGame.Temp, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), Color.Turquoise);
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero,1f, viewComponent.Effects ,1f);
            base.Draw(spriteBatch);
        }
    }
}
