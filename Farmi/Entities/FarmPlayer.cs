using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BrashMonkeyContentPipelineExtension;
using BrashMonkeySpriter;
using BrashMonkeySpriter.Content;
using BrashMonkeySpriter.Spriter;
using Farmi.Calendar;
using Farmi.Datasets;
using Farmi.Entities.Animals;
using Farmi.Entities.Components;
using Farmi.Entities.Items;
using Farmi.Repositories;
using Khv.Engine;
using Khv.Engine.Structs;
using Khv.Game;
using Khv.Game.Collision;
using Khv.Game.GameObjects;
using Khv.Gui.Components.BaseComponents;
using Khv.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Farmi.HUD;

namespace Farmi.Entities
{
    public sealed class FarmPlayer : Player
    {
        #region Vars
        private readonly FarmWorld world;
        private const float speed = 2.5f;

        private InputController controller;
        private readonly InputControlSetup defaultInputSetup;

        private ViewComponent viewComponent;
        private AnimationComponent animationComponent;

        
        #endregion

        #region Properties
        public bool CouldInteract
        {
            get
            {
                if (ClosestInteractable == null)
                {
                    return false;
                }
                InteractionComponent component = ClosestInteractable.Components.GetComponent<IUpdatableObjectComponent>(
                    c => c is InteractionComponent) as InteractionComponent;

                return component.CanInteract(this);
            }
        }
        public GameObject ClosestInteractable
        {
            get;
            private set;
        }
        public PlayerInventory Inventory
        {
            get;
            private set;
        }

        public FarmPlayerAnimator Animator
        {
            get; 
            private set;
        }

        #endregion

        public FarmPlayer(KhvGame game, FarmWorld world, PlayerIndex index = PlayerIndex.One)
            : base(game, index)
        {
            defaultInputSetup = new InputControlSetup();
            
            this.world = world;
            Position = new Vector2(500, 200);

            Size = new Size(30, 47);

            Collider = new BoxCollider(world, this,
                new BasicObjectCollisionQuerier(),
                new BasicTileCollisionQuerier());
        }

        #region Init

        public void Initialize()
        {
            AddComponents();

            controller = new InputController(game.InputManager);
            controller.ChangeSetup(defaultInputSetup);
            InitDefaultSetup();            
        }

        private void AddComponents()
        {
            SpriterReader reader = new SpriterReader();
            SpriterImporter importer = new SpriterImporter();
            var model = reader.Read(importer.Import(Path.Combine("Content\\Spriter", "player.scml")), null, game.Content,
                game.GraphicsDevice);
            Animator = new FarmPlayerAnimator(this, model, "player");
            //Animator.AnimationEnded += Animator_AnimationEnded;
            Animator.Scale = 1.0f;
            animationComponent = new AnimationComponent(this, Animator, "idle_down");

            Components.AddComponent(animationComponent);
            Components.AddComponent(new ExclamationMarkDrawer(game, this));
            Components.AddComponent(new MessageBoxComponent(game, this));
            Components.AddComponent(Inventory = new PlayerInventory(this));
            Components.AddComponent(viewComponent = new ViewComponent(new Vector2(0, 1)));
            CreateTools();

            WidgetManager widgets = new WidgetManager(game, this);
            widgets.AddWidget(new ItemWidget(game, this, widgets, "Tool widget"));
            widgets.AddWidget(new TimeWidget(game, this, widgets, "Time and date widget"));
            Components.AddComponent(widgets);
        }

        private void CreateTools()
        {
            RepositoryManager r = game.Components.First(c => c is RepositoryManager) as RepositoryManager;
            Inventory.AddToInventory(new Tool(game, r.GetDataSet<ToolDataset>(t => t.Name == "Hoe")));
            Inventory.AddToInventory(new Tool(game, r.GetDataSet<ToolDataset>(t => t.Name == "Pick")));
            Inventory.AddToInventory(new Seed(game, r.GetDataSet<SeedDataset>(t => t.Name == "Turnip seeds")));
            Inventory.NextTool();
            Inventory.OnToolChanging += Inventory_OnToolChanging;
        }

        #region Init input

        private void InitDefaultSetup()
        {
            var keymapper = defaultInputSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
           
            #region Move
            keymapper.Map(new KeyTrigger("Move left", Keys.A, Keys.Left), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, -speed));
            keymapper.Map(new KeyTrigger("Move right", Keys.D, Keys.Right), (triggered, args) => MotionEngine.GoalVelocityX = VelocityFunc(args, speed));
            keymapper.Map(new KeyTrigger("Move up", Keys.W, Keys.Up), (triggered, args) =>
            {
                MotionEngine.GoalVelocityY = VelocityFunc(args, -speed);
                if (Equals(Animator.CurrentAnimation, Animator.GetAnimation("walk_up"))) return;
                Animator.ChangeAnimation("walk_up");
                Animator.FlipX = false;
                Animator.FlipY = false;
            });
            keymapper.Map(new KeyTrigger("Move down", Keys.S, Keys.Down), (triggered, args) =>
            {
                MotionEngine.GoalVelocityY = VelocityFunc(args, speed);

                if (Equals(Animator.CurrentAnimation, Animator.GetAnimation("walk_down"))) return;
                Animator.ChangeAnimation("walk_down");
                Animator.FlipX = false;
                Animator.FlipY = false;
            });
            #endregion

            keymapper.Map(new KeyTrigger("Flip left", Keys.A, Keys.Left), (triggered, args) =>
            {
                // joudutaan flippaan
                Animator.FlipX = true;
                if (Equals(Animator.CurrentAnimation, Animator.GetAnimation("walk_right")))
                {
                    return;
                }
                Animator.ChangeAnimation("walk_right");
            }, InputState.Pressed | InputState.Down);

            keymapper.Map(new KeyTrigger("Flip right", Keys.D, Keys.Right), (triggered, args) =>
            {
                Animator.FlipX = false;
                if (Equals(Animator.CurrentAnimation, Animator.GetAnimation("walk_right")))
                {
                    return;
                }
                Animator.ChangeAnimation("walk_right");

            }, InputState.Pressed | InputState.Down);


            keymapper.Map(new KeyTrigger("Interact", Keys.Space), 
                (triggered, args) => TryInteract(args));

            keymapper.Map(new KeyTrigger("Next day", Keys.F1), (triggered, args) =>
            {
                if (args.State == InputState.Pressed)
                {
                    CalendarSystem calendar = game.Components.First(
                        c => c is CalendarSystem) as CalendarSystem;

                    calendar.SkipDay(23, 45);
                }
            });
            keymapper.Map(new KeyTrigger("Previous tool", Keys.Q), (triggered, args) => Inventory.PreviousTool(), InputState.Released);
            keymapper.Map(new KeyTrigger("Next tool", Keys.E), (triggered, args) => Inventory.NextTool(), InputState.Released);
            
            
            
            keymapper.Map(new KeyTrigger("Spawn dog", Keys.F2), (triggered, args) =>
            {
                if (args.State == InputState.Pressed)
                {
                    AnimalDataset dataset = (game.Components.First(
                        c => c is RepositoryManager) as RepositoryManager).GetDataSet<AnimalDataset>(p => p.Type == "Dog");

                    Animal dog = new Animal(game, dataset);
                    dog.Position = position;

                    world.WorldObjects.AddGameObject(dog);
                }
            });
            keymapper.Map(new KeyTrigger("Spawn cow", Keys.F3), (triggered, args) =>
                {
                    if(args.State == InputState.Pressed)
                    {
                        AnimalDataset dataset = game.Components.GetGameComponent<RepositoryManager>()
                            .GetDataSet<AnimalDataset>(d => d.Type == "Cow");

                        Animal cow = new Animal(game, dataset);
                        cow.Position = position;

                        world.WorldObjects.AddGameObject(cow);
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

        #endregion

        #region Events

        void Inventory_OnToolChanging(object sender, PlayerInventoryEventArgs e)
        {
            /*
            Animator.AnimationEnded -= tool_AnimationEnded;
            var interactionComponent = e.CurrentItem.Components.GetComponent<IInteractionComponent>();
            // jos ei ole komponenttia tai komponentti ei interaktaa kenenkään kanssa
            if (interactionComponent == null || !interactionComponent.IsInteracting)
            {
                return;
            }
            interactionComponent.CancelInteract();*/
        }

        #endregion

        #region Input callbacks
        /// <summary>
        /// Interactaa työkalulla callback inputtiin
        /// </summary>
        /// <param name="triggered"></param>
        /// <param name="args"></param>
        private void InteractWithTool(Keys triggered, InputEventArgs args)
        {
            if (!Inventory.HasToolSelected)
            {
                return;
            }


            var powerUpComponent = Inventory.SelectedTool.Components.GetComponent(c => c is PowerUpComponent) as PowerUpComponent;
            if (powerUpComponent != null)
            {
                powerUpComponent.Disable();
            }

            var interactionComponent = Inventory.SelectedTool.Components.GetComponent(c => c is IInteractionComponent) as IInteractionComponent;
            // jotain meni vikaan, jokaisella työkalulla PITÄISI olla interaktion komponentti
            // otetaan myös huomioon että jos se lyö jotain tällä hetkellä
            if (interactionComponent == null || interactionComponent.IsInteracting)
            {
                return;
            }
            GameObject nearestObject = world.GetNearestGameObject(this, new Padding(32));
            // jos ei oo lähellä ketää, älä tee mitään
            if (nearestObject == null)
            {
                return;
            }
            Animator.ChangeAnimation("use_tool_right");
            interactionComponent.Interact(nearestObject);
            interactionComponent.OnInteractionFinished += interactionComponent_OnInteractionFinished; 
        }

        void interactionComponent_OnInteractionFinished(object sender, InteractionEventArgs e)
        {
            var component = sender as IInteractionComponent;
            component.OnInteractionFinished -= interactionComponent_OnInteractionFinished;
            Animator.ChangeAnimation("idle_right");

        }

        /// <summary>
        /// PowerUp työkaluun callback inputtiin
        /// </summary>
        /// <param name="triggered"></param>
        /// <param name="args"></param>
        private void PowerUpTool(Keys triggered, InputEventArgs args)
        {
            if (!Inventory.HasToolSelected)
            {
                return;
            }

            var powerUpComponent = Inventory.SelectedTool.Components.GetComponent(c => c is PowerUpComponent) as PowerUpComponent;

            // jos ei tarvi poweruppia niin lähetään pois
            if (powerUpComponent == null)
            {
                return;
            }

            // jos power up on päällä ja päivittämässä poweria niin ei tarvi resetoida
            if (powerUpComponent.Enabled || powerUpComponent.IsMaximumMet) return;
            powerUpComponent.Disable();
            powerUpComponent.Enable();
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
        private float VelocityFunc(InputEventArgs args, float src)
        {
            if (args.State == InputState.Released)
            {
                return 0;
            }

            return src;
        }
        #endregion

        #region Events
        void Animator_AnimationEnded(Animation sender)
        {
            // näyttää niin vitun tyhmältä :D
            /*if (sender.Name == "use_tool_right")
                Animator.ChangeAnimation("idle");*/
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MotionEngine.Update(gameTime);
            Collider.Update(gameTime);

            ClosestInteractable = world.GetNearestInteractable(this, new Padding(10, 5));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            #region Draw closest
            /*if (ClosestInteractable != null)
            {
                Rectangle r = new Rectangle((int)ClosestInteractable.Position.X, (int)ClosestInteractable.Position.Y, ClosestInteractable.Size.Width, ClosestInteractable.Size.Height);

                spriteBatch.Draw(KhvGame.Temp, r, Color.Red);
            }*/
            #endregion
        }
    }
}
