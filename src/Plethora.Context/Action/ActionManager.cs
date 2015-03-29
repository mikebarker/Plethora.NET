using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Plethora.Context.Action
{
    /// <summary>
    /// The pricipal manager of actions related to contexts. 
    /// </summary>
    public class ActionManager
    {
        #region Singleton Instance

        private static readonly ActionManager globalInstance = new ActionManager();

        /// <summary>
        /// The global instance of the <see cref="ActionManager"/>.
        /// </summary>
        /// <remarks>
        /// Implementations can utilise the global instance or choose to define a local instance if preferred.
        /// </remarks>
        public static ActionManager GlobalInstance
        {
            get { return globalInstance; }
        }

        #endregion

        #region Fields

        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private readonly ICollection<IActionFactory> actionFactories = new List<IActionFactory>(0);
        private volatile TemplateActionFactory templateActionFactory;

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an action factory with this <see cref="ContextManager"/>.
        /// </summary>
        /// <param name="factory">The action factory to be registered.</param>
        /// <remarks>
        ///  <para>
        ///   The action factory is used to provide a list of available actions for the
        ///   contexts in-scope.
        ///  </para>
        ///  <para>
        ///   Consider using <see cref="RegisterActionTemplate(IActionTemplate)"/>
        ///   if actions can be templated.
        ///  </para>
        /// </remarks>
        /// <seealso cref="GetActions"/>
        public void RegisterFactory(IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException("factory");

            rwLock.EnterWriteLock();
            try
            {
                this.actionFactories.Add(factory);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an action factory.
        /// </summary>
        public void DeregisterFactory(IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException("factory");

            rwLock.EnterWriteLock();
            try
            {
                this.actionFactories.Remove(factory);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        /// <summary>
        /// Registers an action template with this <see cref="ContextManager"/>.
        /// </summary>
        /// <param name="template">The action template to be registered.</param>
        /// <remarks>
        /// The template is used to create a list of available actions based on
        /// an in-scope context.
        /// </remarks>
        public void RegisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");

            rwLock.EnterWriteLock();
            try
            {
                if (templateActionFactory == null)
                {
                    templateActionFactory = new TemplateActionFactory();
                    actionFactories.Add(templateActionFactory);
                }

                templateActionFactory.RegisterActionTemplate(template);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an action template.
        /// </summary>
        public void DeregisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");

            rwLock.EnterWriteLock();
            try
            {
                if (templateActionFactory == null)
                    return;

                templateActionFactory.DeregisterActionTemplate(template);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the actions available for a list of contexts.
        /// </summary>
        /// <param name="contexts">The list of contexts</param>
        /// <returns>
        /// A list of available actions for the given contexts.
        /// </returns>
        public IEnumerable<IAction> GetActions(IEnumerable<ContextInfo> contexts)
        {
            rwLock.EnterReadLock();
            try
            {
                var contextsByName = contexts
                    .GroupBy(context => context.Name)
                    .ToDictionary(group => group.Key, group => group.ToArray());

                var actions = actionFactories
                    .Select(factory => factory.GetActions(contextsByName))
                    .Where(actionList => actionList != null)
                    .SelectMany(actionList => actionList)
                    .Where(action => action != null)
                    .ToList();

                return actions;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        #endregion
    }
}
