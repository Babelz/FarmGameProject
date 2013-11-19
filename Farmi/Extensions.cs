using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Farmi
{
    public static class Extensions
    {
        public static T GetGameComponent<T>(this GameComponentCollection gameComponents, Predicate<T> predicate = null) where T : class, IGameComponent
        {
            T results = default(T);

            if (predicate == null)
            {
                foreach (IGameComponent gameComponent in gameComponents)
                {
                    if ((results = gameComponent as T) != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach (IGameComponent gameComponent in gameComponents)
                {
                    if ((results = gameComponent as T) != null && predicate(results))
                    {
                        break;
                    }
                }
            }

            return results;
        }
    }
}
