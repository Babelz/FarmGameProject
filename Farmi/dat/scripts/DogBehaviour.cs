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

namespace Script
{
    internal sealed class DogBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private SpriteSheetAnimation animation;
        #endregion

        public DogBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
        }

        public override void Initialize()
        {
            Texture2D texture = game.Content.Load<Texture2D>(@"Entities\" + owner.Dataset.AssetName); 
            animation = new SpriteSheetAnimation(texture);

            animation.AddSets(new SpriteAnimationSet[]
            {
                new SpriteAnimationSet("idle", new Size(32, 32), 4, 0)
            });

            animation.ChangeSet("idle");
        }
        public override void Update(GameTime gameTime)
        {
            animation.NextFrame();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle((int)owner.Position.X, (int)owner.Position.Y, owner.Size.Width, owner.Size.Height);

            spriteBatch.Draw(animation.Texture, destination, animation.CurrentSource, Color.White);
        }
    }
}
