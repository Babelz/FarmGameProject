using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities;
using Farmi.Entities.Components;
using Farmi.Entities.Items;
using Farmi.Entities.Scripts;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.dat.scripts
{
    public class SeedBehaviour : ToolBehaviourScript
    {
        internal SeedInteractionComponent seedInteractionComponent;
        public SeedBehaviour(KhvGame game, Seed owner) : base(game, owner)
        {
            Console.WriteLine(owner.Dataset.Name);
            seedInteractionComponent = new SeedInteractionComponent(owner);
            owner.Components.AddComponent(seedInteractionComponent);
        }

        protected override void HookEvents()
        {
            base.HookEvents();
        }


        public override void Update(GameTime gameTime)
        {
                
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
