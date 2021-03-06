using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using JetBrains.Annotations;

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
        [NotNull]
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
        /// Registers an action factory with this <see cref="ActionManager"/>.
        /// </summary>
        /// <param name="factory">The action factory to be registered.</param>
        /// <remarks>
        ///  <para>
        ///   The action factory is used to provide a list of available actions for the
        ///   contexts in-scope.
        ///  </para>
        ///  <para>
        ///   Consider using <see cref="RegisterActionTemplate(IActionTemplate)"/>
        ///   or <see cref="RegisterActionTemplate(IMultiActionTemplate)"/> if actions
        ///   can be templated.
        ///  </para>
        /// </remarks>
        /// <seealso cref="GetActions"/>
        public void RegisterFactory([NotNull] IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            this.rwLock.EnterWriteLock();
            try
            {
                this.actionFactories.Add(factory);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an action factory.
        /// </summary>
        public void DeregisterFactory([NotNull] IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            this.rwLock.EnterWriteLock();
            try
            {
                this.actionFactories.Remove(factory);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }


        /// <summary>
        /// Registers an action template with this <see cref="ActionManager"/>.
        /// </summary>
        /// <param name="template">The action template to be registered.</param>
        /// <remarks>
        /// The template is used to create a list of available actions based on
        /// an in-scope context.
        /// </remarks>
        public void RegisterActionTemplate([NotNull] IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            this.rwLock.EnterWriteLock();
            try
            {
                if (this.templateActionFactory == null)
                {
                    this.templateActionFactory = new TemplateActionFactory();
                    this.actionFactories.Add(this.templateActionFactory);
                }

                this.templateActionFactory.RegisterActionTemplate(template);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Registers an action template with this <see cref="ActionManager"/>.
        /// </summary>
        /// <param name="template">The action template to be registered.</param>
        /// <remarks>
        ///  <para>
        ///   The template is used to create a list of available actions based on
        ///   a collection of in-scope contexts.
        ///  </para>
        ///  <para>
        ///   The <see cref="IMultiActionTemplate"/> differs from the <see cref="IActionTemplate"/>
        ///   in that an <see cref="IActionTemplate"/> will define an action if a
        ///   single context with a context name is in scope. <see cref="IMultiActionTemplate"/>
        ///   will define an action if multiple conexts are in-scope with the same context name.
        ///  </para>
        /// </remarks>
        public void RegisterActionTemplate([NotNull] IMultiActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            this.rwLock.EnterWriteLock();
            try
            {
                if (this.templateActionFactory == null)
                {
                    this.templateActionFactory = new TemplateActionFactory();
                    this.actionFactories.Add(this.templateActionFactory);
                }

                this.templateActionFactory.RegisterActionTemplate(template);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an action template.
        /// </summary>
        public void DeregisterActionTemplate([NotNull] IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            this.rwLock.EnterWriteLock();
            try
            {
                if (this.templateActionFactory == null)
                    return;

                this.templateActionFactory.DeregisterActionTemplate(template);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an action template.
        /// </summary>
        public void DeregisterActionTemplate([NotNull] IMultiActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            this.rwLock.EnterWriteLock();
            try
            {
                if (this.templateActionFactory == null)
                    return;

                this.templateActionFactory.DeregisterActionTemplate(template);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the actions available for a list of contexts.
        /// </summary>
        /// <param name="contexts">The list of contexts</param>
        /// <returns>
        /// A list of available actions for the given contexts.
        /// </returns>
        [NotNull, ItemNotNull]
        public IEnumerable<IAction> GetActions([NotNull, ItemNotNull] IEnumerable<ContextInfo> contexts)
        {
            IEnumerable<IActionFactory> factories;
            this.rwLock.EnterReadLock();
            try
            {
                //Copy the actionFactories to a new list to minimise the time required
                // within the lock.
                factories = this.actionFactories.ToList();
            }
            finally
            {
                this.rwLock.ExitReadLock();
            }

            var contextsByName = contexts
                .GroupBy(context => context.Name)
                .ToDictionary(group => group.Key, group => group.ToArray());

            var actions = factories
                .Select(factory => factory.GetActions(contextsByName))
                .Where(actionList => actionList != null)
                .SelectMany(action => action)
                .Where(action => action != null)
                .ToList();

            return actions;
        }

        #endregion
    }
}
