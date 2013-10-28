using Khv.Engine.Transition;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khv.Engine.State
{
    public interface IStateElement : ITransition
    {
        void PreRender();

        ScreenState State { get; set; }

        void PostRender();

        void Draw();

        bool IsExiting { get; set; }

        bool HasFocus { get; set; }

        bool IsPopUp { get; set; }

        void Update(GameTime gameTime);

        bool IsInitialized { get; set; }

        void Dispose();
    }
}
