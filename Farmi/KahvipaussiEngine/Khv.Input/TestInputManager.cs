using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Khv.Engine;
using Microsoft.Xna.Framework;

namespace Khv.Input
{
    public class TestInputManager : GameComponent
    {
        #region Vars

        private readonly Dictionary<Type, IInputStateProvider> stateProviders; 

        #endregion

        #region Properties

        public InputMapper Mapper { get; private set;  }

        #endregion

        public TestInputManager(KhvGame game) : base(game)
        {
            stateProviders = new Dictionary<Type, IInputStateProvider>();
            Mapper = new InputMapper();
        }

        public void AddStateProvider(Type type, IInputStateProvider provider)
        {
            if (!stateProviders.ContainsKey(type))
            {
                stateProviders[type] = provider;
            }
        }

        public override void Initialize()
        {

            Mapper.StateProviders = stateProviders;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var inputStateProvider in stateProviders)
            {
                inputStateProvider.Value.Update();
            }
            

            InvokeAll(gameTime, Mapper.Update(gameTime));
        }

        private void InvokeAll(GameTime gameTime, IEnumerable<IInputCallbacker> invokes)
        {
            foreach (var binding in invokes)
            {
                binding.Fire();
            }
        }
    }
}
