using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Farmi.Entities.Animals;
using Khv.Engine;
using Farmi;
using Khv.Engine.Structs;
using Farmi.Entities.Components;
using Khv.Game.Collision;
using Khv.Maps.MapClasses.Layers.Tiles;
using Microsoft.Xna.Framework.Audio;
using Farmi.Entities;

namespace Script
{
    internal sealed class DogBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private SpriteSheetAnimation animation;
        private FiniteStateMachine brain;

        private int followTime;
        private int runawayTime;
        private int idleTime;

        private Random random;
        private double currentModifier;
        private Vector2 oldvelocity;

        SoundEffect effect;
        #endregion

        public DogBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
        }

        private void InitializeBrain()
        {
            brain = new FiniteStateMachine();
            brain.PushState(Move);

            random = new Random();
            currentModifier = random.NextDouble();

            RandomizeSpeed();
        }

        #region Brain states
        private void Move()
        {
        }
        private void AvoidPlayer()
        {
        }
        private void FollowAndBark()
        {
            if (followTime > 3500 * currentModifier)
            {
                brain.PopState();
                brain.PushState(Move);

                followTime = 0;

                currentModifier = random.NextDouble();
            }
        }
        private void RunAwayFromTiles()
        {
            if (runawayTime > 2500 * 2 * currentModifier)
            {
                brain.PopState();
                brain.PushState(Idle);

                runawayTime = 0;

                currentModifier = random.NextDouble();
            }
            else
            {
                owner.MotionEngine.GoalVelocityX += 0.0001f;
                owner.MotionEngine.GoalVelocityY += 0.0001f;
            }
        }
        private void Idle()
        {
            if (owner.MotionEngine.GoalVelocityX > 0) 
            {
                oldvelocity = owner.MotionEngine.GoalVelocity;
                owner.MotionEngine.GoalVelocity = Vector2.Zero;
            }
            if (idleTime > 2500 * 2 * currentModifier)
            {
                RandomizeSpeed();

                brain.PopState();
                brain.PushState(Bark);

                idleTime = 0;

                currentModifier = random.NextDouble();
            }
            else
            {
            }
        }
        private void Bark()
        {
            effect.Play();

            brain.PopState();
            brain.PushState(RunAwayFromTiles);
        }
        #endregion

        public override void Initialize()
        {
            InitializeBrain();

            Texture2D texture = game.Content.Load<Texture2D>(@"Entities\" + owner.Dataset.AssetName);
            animation = new SpriteSheetAnimation(texture);

            effect = game.Content.Load<SoundEffect>(@"Sounds\woof");

            animation.AddSets(new SpriteAnimationSet[]
            {
                new SpriteAnimationSet("idle", new Size(32, 32), 2, 0)
                {
                    FrameTime = 100 * 3
                }
            });

            animation.ChangeSet("idle");

            owner.Collider.OnCollision += new CollisionEventHandler(Collider_OnCollision);
        }
        public override void Update(GameTime gameTime)
        {
            brain.Update();

            if (brain.CurrentState == FollowAndBark)
            {
                followTime += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (brain.CurrentState == RunAwayFromTiles)
            {
                runawayTime += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (brain.CurrentState == Idle)
            {
                idleTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle((int)owner.Position.X, (int)owner.Position.Y, owner.Size.Width, owner.Size.Height);

            spriteBatch.Draw(animation.Texture, destination, animation.CurrentSource, Color.White);
        }

        private void RandomizeSpeed()
        {
            owner.MotionEngine.GoalVelocityX = (2.0f * ((float)random.NextDouble() * 2 - 1));
            owner.MotionEngine.GoalVelocityY = (2.0f * ((float)random.NextDouble() * 2 - 1));
        }

        #region Event handlers
        private void Collider_OnCollision(object sender, CollisionEventArgs result)
        {
            brain.PopState();

            if (result.CollidingObject is BaseTile)
            {
                owner.MotionEngine.GoalVelocityX = -owner.MotionEngine.GoalVelocityX;
                owner.MotionEngine.GoalVelocityY = -owner.MotionEngine.GoalVelocityY;

                brain.PushState(RunAwayFromTiles);
            }
            else if (result.CollidingObject is Animal)
            {
                RandomizeSpeed();

                brain.PushState(RunAwayFromTiles);
            }
            else if (result.CollidingObject is FarmPlayer)
            {
                RandomizeSpeed();

                brain.PushState(Bark);
            }
            
        }
        #endregion
    }
}
