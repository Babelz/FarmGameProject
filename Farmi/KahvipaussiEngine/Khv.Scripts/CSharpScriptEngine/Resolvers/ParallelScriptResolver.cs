using System;
using System.Collections.Generic;
using System.Linq;
using Khv.Scripts.CSharpScriptEngine.Builders;
using Khv.Scripts.CSharpScriptEngine.Containers;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;

namespace Khv.Scripts.CSharpScriptEngine.Resolvers
{
    /// <summary>
    /// Resolver joka avaa uusia säikeitä resolvausta varten.
    /// </summary>
    public class ParallelScriptResolver : ScriptResolver
    {
        #region Vars 
        private List<ParallelWorkItem> pendingResolves;
        #endregion

        #region Properties
        /// <summary>
        /// Onko kesken olevia resolvauksia.
        /// </summary>
        public bool HasPendingResolves
        {
            get
            {
                return pendingResolves.Count > 0;
            }
        }
        #endregion

        public ParallelScriptResolver(ScriptPathContainer scriptPathContainer, ScriptDepencyContainer scriptDepencyContainer, ScriptAssemblyContainer scriptAssemblyContainer)
            : base(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer)
        {
            pendingResolves = new List<ParallelWorkItem>();
        }

        // Ottaa resolvatun scriptin sisään ja scriptbuilderin jota käytettiin resolvauksessa.
        // Suorittaa scriptin callback metodin.
        private void ExecuteCallback(ParallelScriptBuilder parallelScriptBuilder, IScript script)
        {
            if (parallelScriptBuilder.ScriptResolvedCallback != null)
            {
                parallelScriptBuilder.ScriptResolvedCallback(script, parallelScriptBuilder);
            }
        }

        public void Update()
        {
            foreach (ParallelWorkItem parallelWorkItem in pendingResolves.Where(i => i.AsyncResult.IsCompleted))
            {
                IScript script = parallelWorkItem.ParallelResolveDelegate.EndInvoke(parallelWorkItem.AsyncResult);

                if (script != null)
                {
                    ExecuteCallback(parallelWorkItem.ParallelScriptBuilder, script);
                }
            }

            pendingResolves.RemoveAll(i => i.AsyncResult.IsCompleted);
        }
        public void BeginResolve<T>(ParallelScriptBuilder scriptBuilder) where T : IScript
        {
            ParallelResolverDelegate<T> resolverDelegate = StartResolving<T>;

            ParallelWorkItem parallelScriptWorker = new ParallelWorkItem()
            {
                ParallelScriptBuilder = scriptBuilder,
                ParallelResolveDelegate = resolverDelegate as ParallelResolverDelegate<IScript>,
                AsyncResult = resolverDelegate.BeginInvoke(scriptBuilder, null, null)
            };

            pendingResolves.Add(parallelScriptWorker);
        }

        private delegate T ParallelResolverDelegate<T>(ScriptBuilder scriptBuilder) where T : IScript;

        /// <summary>
        /// Luokka joka wräppää parallel resolvauksen aikana tarvittavia tietoja itseensä.
        /// </summary>
        private class ParallelWorkItem
        {
            #region Properties
            public ParallelScriptBuilder ParallelScriptBuilder
            {
                get;
                set;
            }
            public ParallelResolverDelegate<IScript> ParallelResolveDelegate
            {
                get;
                set;
            }
            public IAsyncResult AsyncResult
            {
                get;
                set;
            }
            #endregion
        }
    }
}
