using Khv.Scripts.CSharpScriptEngine.Builders;
using Khv.Scripts.CSharpScriptEngine.Containers;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Khv.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Resolver joka hoitaa resolvauksen kutsuvassa säikeessä.
    /// </summary>
    public class BlockingScriptResolver : ScriptResolver
    {
        public BlockingScriptResolver(ScriptPathContainer scriptPathContainer, ScriptDepencyContainer scriptDepencyContainer, ScriptAssemblyContainer scriptAssemblyContainer)
            : base(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer)
        {
        }

        public T Resolve<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            return StartResolving<T>(scriptBuilder);
        }
    }
}
