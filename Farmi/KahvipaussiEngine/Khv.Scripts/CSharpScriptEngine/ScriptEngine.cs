﻿using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Khv.Scripts.CSharpScriptEngine.Builders;
using Khv.Scripts.CSharpScriptEngine.Containers;
using Khv.Scripts.CSharpScriptEngine.Resolvers;
using Khv.Scripts.CSharpScriptEngine.ScriptClasses;
using Microsoft.Xna.Framework;
using Khv.Engine;

namespace Khv.Scripts.CSharpScriptEngine
{
    public class ScriptEngine : GameComponent
    {
        #region Vars
        private ScriptPathContainer scriptPathContainer;
        private ScriptDepencyContainer scriptDepencyContainer;
        private ScriptAssemblyContainer scriptAssemblyContainer;
        private ScriptObserverContainer observerContainer;

        private BlockingScriptResolver blockingScriptResolver;
        private ParallelScriptResolver parallelScriptResolver;

        private string configurationFilePath;
        #endregion

        #region Properties
        public ScriptObserverContainer ScriptObservers
        {
            get
            {
                return observerContainer;
            }
        }
        public ScriptAssemblyContainer ScriptAssemblyContainer
        {
            get
            {
                return scriptAssemblyContainer;
            }
        }
        public ScriptPathContainer ScriptPathContainer
        {
            get
            {
                return scriptPathContainer;
            }
        }
        /// <summary>
        /// Miten errorit logataan.
        /// </summary>
        public LoggingMethod LoggingMethod
        {
            get;
            set;
        }
        /// <summary>
        /// Onko scriptejä vielä resolvaamatta.
        /// </summary>
        public bool HasPendingResolves
        {
            get
            {
                return parallelScriptResolver.HasPendingResolves;
            }
        }
        #endregion

        public ScriptEngine(KhvGame game, string configurationFilePath)
            : base(game)
        {
            this.configurationFilePath = configurationFilePath;
            LoggingMethod = LoggingMethod.None;
        }

        // Alustaa kaikki tarvittavat containerit.
        private void InitializeContainers()
        {
            XDocument configurationFile = XDocument.Load(configurationFilePath);

            scriptPathContainer = new ScriptPathContainer(configurationFile);
            scriptDepencyContainer = new ScriptDepencyContainer(configurationFile);

            scriptAssemblyContainer = new ScriptAssemblyContainer();
            observerContainer = new ScriptObserverContainer();
        }
        // Alustaa kaikki resolverit.
        private void InitializeResolvers()
        {
            blockingScriptResolver = new BlockingScriptResolver(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer);
            parallelScriptResolver = new ParallelScriptResolver(scriptPathContainer, scriptDepencyContainer, scriptAssemblyContainer);
        }

        /// <summary>
        /// Alustaa enginen uudelleen uudella configurationfile pathilla.
        /// </summary>
        public void ReInitialize(string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
            Initialize();
        }
        /// <summary>
        /// Alustaa engine uudelleen.
        /// </summary>
        public void ReInitialize()
        {
            Initialize();
        }
        /// <summary>
        /// Alustaa enginen.
        /// </summary>
        public override void Initialize()
        {
            if (File.Exists(configurationFilePath))
            {
                InitializeContainers();
                InitializeResolvers();
            }
            else
            {
                throw new FileNotFoundException("Configuration file was not found at given path." + Environment.NewLine +
                                                "Path is: " + configurationFilePath);
            }

            base.Initialize();
        }
        /// <summary>
        /// Päivittää assembly containerin ja parallel script resolverin.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            scriptAssemblyContainer.Update();
            parallelScriptResolver.Update();

            if (scriptAssemblyContainer.HasModifiedAssemblies)
            {
                observerContainer.Notify(this, scriptAssemblyContainer.ModifiedAssemblies);
            }
            if (observerContainer.ShouldNotifyNullScripts)
            {
                observerContainer.NotifyNullScripts(this);
            }

            base.Update(gameTime);
        }
        
        /// <summary>
        /// Yrittää luoda halutun scriptin annetuilla tiedoilla ja palauttaa sen userille.
        /// Tämä metodi ajetaan suoraan kutsuvalla säikeellä joten se on blokkaava.
        /// Kutsu voi aiheuttaa viivettä, varsinkin jos scripti pitää kääntää.
        /// </summary>
        /// <typeparam name="T">Halutun scriptin tyyppi.</typeparam>
        /// <param name="scriptBuilder">Builder joka sisältää tarvittavat tiedot scriptin luomiseen.</param>
        public T GetScript<T>(ScriptBuilder scriptBuilder) where T : IScript
        {
            blockingScriptResolver.LoggingMethod = LoggingMethod;
            return blockingScriptResolver.Resolve<T>(scriptBuilder);
        }
        /// <summary>
        /// Yrittää luoda halutun scriptin annetuilla tiedoilla ja luo siitä instanssin.
        /// Tätä resolvausta varten avataan uusi säije, joten se ei ole blokkaava.
        /// Kutsu ei aiheuta viivettä.
        /// </summary>
        /// <typeparam name="T">Halutun scriptin tyyppi.</typeparam>
        /// <param name="parallelScriptBuilder">Builder joka sisältää tarvittavat tiedot scriptin luomiseen.</param>
        public void MakeScript<T>(ParallelScriptBuilder parallelScriptBuilder) where T : IScript
        {
            parallelScriptResolver.LoggingMethod = LoggingMethod;
            parallelScriptResolver.BeginResolve<T>(parallelScriptBuilder);
        }

        /// <summary>
        /// Nukuttaa kutsuvan säikeen siksi aikaa että 
        /// resolverit saavat työnsä tehtyä.
        /// </summary>
        public void WaitForPendingResolves()
        {
            while (HasPendingResolves)
            {
                Thread.Sleep(5);
            }
        }
    }
}
