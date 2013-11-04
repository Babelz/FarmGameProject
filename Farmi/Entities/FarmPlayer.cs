using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Farmi.Entities.Components;
using Farmi.Entities.Items;
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
using Farmi.Calendar;
using Farmi.Entities.Animals;
using Farmi.Repositories;
using Farmi.Datasets;

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

        public Item ItemInHand { get; private set; }

        #endregion

        #region Ctor
        public FarmPlayer(KhvGame game, FarmWorld world, PlayerIndex index = PlayerIndex.One)
            : base(game, index)
        {
            defaultInputSetup = new InputControlSetup();
            shopInputSetup = new InputControlSetup();
            
            this.world = world;
            Position = new Vector2(500, 200);

            Size = new Size(29, 37);

            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());
            viewComponent = new ViewComponent(new Vector2(0, 1));
            Components.Add(viewComponent);

            ItemInHand = new Tool(game, null);
        }
        #endregion

        #region Methods

        #region Init
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
            keymapper.Map(new KeyTrigger("Next day", Keys.F1), (triggered, args) =>
            {
                if (args.State == InputState.Pressed)
                {
                    CalendarSystem calendar = game.Components.First(
                        c => c is CalendarSystem) as CalendarSystem;

                    calendar.SkipDay(23, 45);
                }
            });
            keymapper.Map(new KeyTrigger("Spawn dog", Keys.F2), (triggered, args) =>
            {
                if (args.State == InputState.Pressed)
                {
                    AnimalDataset dataset = (game.Components.First(
                        c => c is RepositoryManager) as RepositoryManager).GetDataSet<AnimalDataset>(p => p.Type == "Dog");

                    Animal dog = new Animal(game, dataset);
                    dog.Position = position;
                    dog.MapContainedIn = world.MapManager.ActiveMap.Name;

                    world.WorldObjects.AddGameObject(dog);
                }
            });
            keymapper.Map(new KeyTrigger("Power tool", Keys.Z), PowerUpTool, InputState.Pressed | InputState.Down);
            keymapper.Map(new KeyTrigger("Interact with tool", Keys.Z), InteractWithTool, InputState.Released);

            var padmapper = defaultInputSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("Move left", Buttons.LeftThumbstickLeft, Buttons.DPadLeft), (triggered, args) => MotionEngine.GoalVelocityX = -speed);
            padmapper.Map(new ButtonTrigger("Move right", Buttons.LeftThumbstickRight, Buttons.DPadRight), (triggered, args) => MotionEngine.GoalVelocityX = speed);
            padmapper.Map(new ButtonTrigger("Move up", Buttons.LeftThumbstickUp, Buttons.DPadUp), (triggered, args) => MotionEngine.GoalVelocityY = -speed);
            padmapper.Map(new ButtonTrigger("Move down", Buttons.LeftThumbstickDown, Buttons.DPadDown), (triggered, args) => MotionEngine.GoalVelocityX = speed);
        }

        #endregion

        #region Input callbacks

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

        private float VelocityFunc(InputEventArgs args, float src)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }

            return src;
        }

        /// <summary>
        /// Interactaa työkalulla callback inputtiin
        /// </summary>
        /// <param name="triggered"></param>
        /// <param name="args"></param>
        private void InteractWithTool(Keys triggered, InputEventArgs args)
        {
            Tool toolInHand = ItemInHand as Tool;
            // jos ei ole työkalu kädessä niin lähetään kotia
            if (toolInHand == null)
                return;

            var powerUpComponent = toolInHand.Components.GetComponent(c => c is PowerUpComponent) as PowerUpComponent;
            Console.WriteLine("interact with " + powerUpComponent.CurrentPow + " power");
            powerUpComponent.Disable();

            var interactionComponent = toolInHand.Components.GetComponent(c => c is IInteractionComponent) as IInteractionComponent;
            // jotain meni vikaan, jokaisella työkalulla PITÄISI olla interaktion komponentti
            if (interactionComponent == null)
                return;

            GameObject nearestObject = world.GetNearestGameObject(this, new Padding(100));
            // jos ei ole lähellä mitään
            if (nearestObject == null)
                return;

            interactionComponent.Interact(nearestObject);
        }

        /// <summary>
        /// PowerUp työkaluun callback inputtiin
        /// </summary>
        /// <param name="triggered"></param>
        /// <param name="args"></param>
        private void PowerUpTool(Keys triggered, InputEventArgs args)
        {
            Tool toolInHand = ItemInHand as Tool;
            // jos ei ole työkalu kädessä niin lähetään kotia
            if (toolInHand == null)
                return;
            var powerUpComponent = toolInHand.Components.GetComponent(c => c is PowerUpComponent) as PowerUpComponent;
            // jos ei tarvi poweruppia niin lähetään pois
            if (powerUpComponent == null)
                return;

            if (!powerUpComponent.Enabled && !powerUpComponent.IsMaximumMet)
            {
                powerUpComponent.Disable();
                powerUpComponent.Enable();
            }
        }

        #endregion

        #region Overrides

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);
            ClosestInteractable = world.GetNearestInteractable(this, new Padding(10, 5));
            if (ItemInHand != null) ItemInHand.Update(gameTime);
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
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height), Color.White);
            base.Draw(spriteBatch);
        }

        #endregion

        #endregion

    }
}
