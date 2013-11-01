using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Farmi.Entities.Animals;
using Khv.Engine;

namespace Script
{
    internal sealed class DogBehaviour : AnimalBehaviourScript
    {
        #region Vars
        private Texture2D texture;
        #endregion

        public DogBehaviour(KhvGame game, Animal owner)
            : base(game, owner)
        {
        }

        public override void Initialize()
        {
            texture = game.Content.Load<Texture2D>(@"Entities\" + owner.Dataset.AssetName); 
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)owner.Position.X, (int)owner.Position.Y, 
                                                    owner.Size.Width, owner.Size.Height), Color.White);
        }
    }
}
