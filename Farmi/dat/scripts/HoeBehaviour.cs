using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farmi.Entities.Components;
using Farmi.Entities.Items;
using Farmi.Entities.Scripts;
using Khv.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.dat.scripts
{
    internal class HoeBehaviour : ToolBehaviourScript
    {
        public HoeBehaviour(KhvGame game, Tool owner) : base(game, owner)
        {
            owner.Components.Add(new HoeInteractionComponent());
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
