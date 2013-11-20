using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using BrashMonkeySpriter;
using Farmi.Entities.Components;
using Farmi.Entities.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farmi.Entities
{
    public sealed class FarmPlayerAnimator : CharaterAnimator
    {
        #region Vars
        private readonly FarmPlayer player;
        #endregion

        public FarmPlayerAnimator(FarmPlayer player, CharacterModel model, string entity) : base(model, entity)
        {
            this.player = player;
        }

        public override void Update(GameTime p_gameTime)
        {
            Location = player.Position + new Vector2(player.Size.Width / 2, player.Size.Height);
            base.Update(p_gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (player.Inventory.HasToolSelected)
            {
                Tool selectedTool = player.Inventory.SelectedTool;
                IInteractionComponent interactionComponent = selectedTool.Components.GetComponent(
                    c => c is IInteractionComponent) as IInteractionComponent;

                // ei pitäs olla null ikinä
                if (interactionComponent != null && interactionComponent.IsInteracting)
                {
                    foreach (RenderMatrix l_render in m_renderList)
                    {
                        Texture2D texture = m_tx[l_render.Folder];
                        Rectangle? source = m_rect[l_render.Folder][l_render.File];
                        
                        if (l_render.Folder == 2)
                        {
                            texture = selectedTool.Texture ?? texture;
                            source = null;
                        }

                        spriteBatch.Draw(
                            texture,
                            l_render.Location,
                            source,
                            m_color*l_render.Alpha,
                            l_render.Rotation,
                            l_render.Pivot,
                            l_render.Scale,
                            l_render.Effects,
                            /*(float)l_render.ZOrder*/0.0f
                            );
                    }
                    return;
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
