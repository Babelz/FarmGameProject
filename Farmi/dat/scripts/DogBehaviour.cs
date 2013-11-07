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
using System.IO;
using Khv.Game.GameObjects;

namespace Script
{
    internal sealed class DogBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private SpriteSheetAnimation animation;
        private SoundEffect effect;
        private SpriteEffects flip;
        private Random random;

        private BaseTile lastTileCollidedWith;
        private GameObject lastObjectCollidedWith;
        private Animal lastDogCollidedWith;

        private int stateTime;
        #endregion

        public DogBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
            random = new Random();

            Texture2D texture = game.Content.Load<Texture2D>(Path.Combine("Entities", owner.Dataset.AssetName));
            effect = game.Content.Load<SoundEffect>(Path.Combine("Sounds", "woof"));

            animation = new SpriteSheetAnimation(texture);
            animation.AddSets(new SpriteAnimationSet[]
            {
                new SpriteAnimationSet("idle", new Size(32, 32), 2, 0)
                {
                    FrameTime = 100 * 3
                }
            });
            animation.ChangeSet("idle");

            owner.Collider.OnCollision += new CollisionEventHandler(Collider_OnCollision);

            brain.PushState(Walking);
        }

        #region Brain states
        private void Idle()
        {
            if (stateTime > 3500)
            {
                stateTime = 0;
                brain.PopState();

                RandomizeGoalVector();

                brain.PushState(Walking);
            }
            else
            {
                owner.MotionEngine.GoalVelocityX = 0;
                owner.MotionEngine.GoalVelocityY = 0;
            }
        }
        private void Walking()
        {
            if (stateTime > 2000)
            {
                stateTime = 0;
                brain.PopState();

                brain.PushState(Idle);
            }
            else
            {
                if (stateTime % 250 == 0)
                {
                    RandomizeGoalVector();
                }
            }
        }
        private void RunAwayFromTile()
        {
            if (stateTime > 3000)
            {
                stateTime = 0;
                brain.PopState();
            }
            else
            {
                if (owner.Position.X > lastTileCollidedWith.Position.X)
                {
                    owner.MotionEngine.GoalVelocityX = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityX = -1.0f;
                }

                if (owner.Position.Y > lastTileCollidedWith.Position.Y)
                {
                    owner.MotionEngine.GoalVelocityY = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityY = -1.0f;
                }
            }
        }
        private void RunAwayFromOtherObject()
        {
            if (stateTime > 1250)
            {
                stateTime = 0;
                brain.PopState();
            }
            else
            {
                if (owner.Position.X > lastObjectCollidedWith.Position.X)
                {
                    owner.MotionEngine.GoalVelocityX = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityX = -1.0f;
                }

                if (owner.Position.Y > lastObjectCollidedWith.Position.Y)
                {
                    owner.MotionEngine.GoalVelocityY = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityY = -1.0f;
                }
            }
        }
        private void PlayWithOtherDog()
        {
            if (stateTime > 2000)
            {
                stateTime = 0;
                brain.PopState();
            }
            else
            {
                DogBehaviour behaviour = lastDogCollidedWith.Behaviours.First() as DogBehaviour;

                if (behaviour.brain.CurrentState != RunAwayFromOtherObject && brain.CurrentState == PlayWithOtherDog)
                {
                    behaviour.lastDogCollidedWith = owner;

                    behaviour.brain.PopState();
                    behaviour.brain.PushState(RunAwayFromOtherObject);
                }

                if (owner.Position.X > lastObjectCollidedWith.Position.X)
                {
                    owner.MotionEngine.GoalVelocityX = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityX = -1.0f;
                }

                if (owner.Position.Y > lastObjectCollidedWith.Position.Y)
                {
                    owner.MotionEngine.GoalVelocityY = 1.0f;
                }
                else
                {
                    owner.MotionEngine.GoalVelocityY = -1.0f;
                }
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            stateTime += gameTime.ElapsedGameTime.Milliseconds;
            flip = owner.MotionEngine.GoalVelocityX > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (brain.HasStates)
            {
                brain.CurrentState();
            }
            else
            {
                brain.PushState(Idle);
                effect.Play();
            }

            animation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle((int)owner.Position.X, (int)owner.Position.Y, owner.Size.Width, owner.Size.Height);

            spriteBatch.Draw(animation.Texture, owner.Position, animation.CurrentSource, Color.White, 0.0f, Vector2.Zero, 1.0f, flip, 0.0f);
        }

        #region Movement methods
        private void RandomizeGoalVector()
        {
            owner.MotionEngine.GoalVelocityX = 0;
            owner.MotionEngine.GoalVelocityY = 0;

            owner.MotionEngine.GoalVelocityX = random.Next(-1, 1);

            if (owner.MotionEngine.GoalVelocityX == 0)
            {
                while (owner.MotionEngine.GoalVelocityY == 0)
                {
                    owner.MotionEngine.GoalVelocityY = random.Next(-1, 1);
                }
            }
        }
        #endregion

        #region Event handlers
        private void Collider_OnCollision(object sender, CollisionEventArgs result)
        {
            BaseTile tile = result.CollidingObject as BaseTile;
            GameObject gameObject = result.CollidingObject as GameObject;

            if (tile != null && brain.CurrentState != RunAwayFromTile)
            {
                stateTime = 0;
                lastTileCollidedWith = tile;

                brain.PushState(RunAwayFromTile);
            }
            else if (gameObject != null)
            {
                stateTime = 0;
                lastObjectCollidedWith = gameObject;

                Animal animal = gameObject as Animal;

                if (animal != null)
                {
                    if (animal.Dataset.Type == "Dog" && brain.CurrentState != PlayWithOtherDog)
                    {
                        lastDogCollidedWith = animal;
                        brain.PushState(PlayWithOtherDog);
                    }
                }
                else if (brain.CurrentState != RunAwayFromOtherObject)
                {
                    brain.PushState(RunAwayFromOtherObject);
                }
            }
        }
        #endregion
    }
}
