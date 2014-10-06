using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Plethora.Context
{
    /// <summary>
    /// The pricipal manager of all elements related to contexts. 
    /// </summary>
    public class ContextManager
    {
        #region Singleton Instance

        private static readonly ContextManager defaultInstance = new ContextManager();

        /// <summary>
        /// The global instance of the <see cref="ContextManager"/>.
        /// </summary>
        /// <remarks>
        /// Implementations can utilise the global instacne or choose to define a local instance if preferred.
        /// </remarks>
        public static ContextManager DefaultInstance
        {
            get { return defaultInstance; }
        }

        #endregion

        #region Fields

        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private readonly ICollection<IContextProvider> activeProviders = new HashSet<IContextProvider>();
        private readonly Dictionary<string, ICollection<ContextAugmentor>> augmentors = new Dictionary<string, ICollection<ContextAugmentor>>();
        private readonly ICollection<IActionFactory> actionFactories = new List<IActionFactory>();
        private volatile TemplateActionFactory templateActionFactory;

        #endregion

        #region Events

        private readonly WeakEvent<EventHandler> contextChanged = new WeakEvent<EventHandler>();

        /// <summary>
        /// Occurs when the context list changes.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   This can be followed by a call to <see cref="GetContexts"/> to retrieve the context list.
        ///  </para>
        ///  <para>
        ///   This is a weak event, meaning that references passed to the event will not be kept 
        ///   alive during GC.
        ///  </para>
        /// </remarks>
        public event EventHandler ContextChanged
        {
            add { contextChanged.Add(value); }
            remove { contextChanged.Remove(value); }
        }

        protected virtual void OnContextChanged(object sender, EventArgs e)
        {
            foreach (var handler in contextChanged.GetInvocationList())
            {
                if (handler != null)
                    handler(sender, e);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an provider with this <see cref="ContextManager"/>.
        /// </summary>
        /// <param name="provider">The provider to be registered.</param>
        /// <remarks>
        /// The provider is used to supply contexts as the user navigates the user interface.
        /// </remarks>
        public void RegisterProvider(IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException("provider");

            provider.EnterContext += provider_EnterContext;
            provider.LeaveContext += provider_LeaveContext;
        }

        /// <summary>
        /// Deregisters an context provider.
        /// </summary>
        public void DeregisterProvider(IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException("provider");

            rwLock.EnterWriteLock();
            try
            {
                activeProviders.Remove(provider);

                provider.EnterContext -= provider_EnterContext;
                provider.LeaveContext -= provider_LeaveContext;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        /// <summary>
        /// Registers an augmentor with this <see cref="ContextManager"/>.
        /// </summary>
        /// <param name="augmentor">The augmentor to be registered.</param>
        /// <remarks>
        /// The augmentor is used to provide addition contexts, based on a current
        /// in-scope context.
        /// </remarks>
        public void RegisterAugmentor(ContextAugmentor augmentor)
        {
            //Validation
            if (augmentor == null)
                throw new ArgumentNullException("augmentor");

            rwLock.EnterWriteLock();
            try
            {
                ICollection<ContextAugmentor> list;
                if (!augmentors.TryGetValue(augmentor.ContextName, out list))
                {
                    list = new List<ContextAugmentor>();
                    augmentors.Add(augmentor.ContextName, list);
                }

                list.Add(augmentor);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an augmentor.
        /// </summary>
        public void DeregisterAugmentor(ContextAugmentor augmentor)
        {
            //Validation
            if (augmentor == null)
                throw new ArgumentNullException("augmentor");

            rwLock.EnterWriteLock();
            try
            {
                ICollection<ContextAugmentor> list;
                if (augmentors.TryGetValue(augmentor.ContextName, out list))
                    list.Remove(augmentor);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


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
        ///   or <see cref="RegisterActionTemplate(IMultiActionTemplate)"/> if actions
        ///   can be templated.
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
        /// Registers an action template with this <see cref="ContextManager"/>.
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
        public void RegisterActionTemplate(IMultiActionTemplate template)
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
        /// Deregisters an action template.
        /// </summary>
        public void DeregisterActionTemplate(IMultiActionTemplate template)
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
        /// Gets the list of contexts which are currently in-scope.
        /// </summary>
        /// <returns>
        /// The list of available contexts.
        /// </returns>
        public IEnumerable<ContextInfo> GetContexts()
        {
            // TODO: This could be more efficient by keeping a cache of the contexts, and only updating those per provider when the context changes

            rwLock.EnterReadLock();
            try
            {
                var contextSet = new Dictionary<ContextInfo, ContextInfo>(new ContextInfoComparer());

                var localContextsEnumerable = activeProviders
                    .Where(provider => provider != null)
                    .Select(provider => provider.Contexts)
                    .Where(contexts => contexts != null)
                    .SelectMany(contexts => contexts)
                    .Where(context => context != null);

                var contextsEnumerable = localContextsEnumerable;

                //Use the augmentors to augment the in-scope contexts
                bool newContexts = true;
                while (newContexts)
                {
                    var nextContextsEnumerable = Enumerable.Empty<ContextInfo>();

                    newContexts = false;
                    foreach (var contextInfo in contextsEnumerable)
                    {
                        ContextInfo setInfo;

                        //Add the context to the contextSet
                        bool isNewContext = false;
                        if (contextSet.TryGetValue(contextInfo, out setInfo))
                        {
                            //Store the one with the greater rank
                            if (contextInfo.Rank > setInfo.Rank)
                            {
                                contextSet.Remove(setInfo);
                                contextSet.Add(contextInfo, contextInfo);
                            }
                        }
                        else
                        {
                            isNewContext = true;
                            contextSet.Add(contextInfo, contextInfo);
                        }


                        if (isNewContext)
                        {
                            newContexts = true;

                            //Find augmented contexts (if any)
                            var augContexts = AugmentContext(contextInfo);
                            if (augContexts != null)
                                nextContextsEnumerable = nextContextsEnumerable.Concat(augContexts);
                        }
                    }

                    contextsEnumerable = nextContextsEnumerable;
                }

                return contextSet.Keys;
            }
            finally
            {
                rwLock.ExitReadLock();
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
                    .SelectMany(action => action)
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

        #region Private Methods

        /// <remarks>
        /// Must be called inside a lock.
        /// </remarks>
        private IEnumerable<ContextInfo> AugmentContext(ContextInfo context)
        {
            IEnumerable<ContextInfo> augmentedContexts = Enumerable.Empty<ContextInfo>();

            ICollection<ContextAugmentor> augmentorList;
            if (this.augmentors.TryGetValue(context.Name, out augmentorList))
            {
                foreach (var augmentor in augmentorList)
                {
                    var additionalContexts = augmentor.AugmentContext(context);
                    if (additionalContexts != null)
                        augmentedContexts = augmentedContexts.Concat(additionalContexts);
                }
            }

            return augmentedContexts;
        }

        private void provider_EnterContext(object sender, EventArgs e)
        {
            var provider = (IContextProvider)sender;
            provider.ContextChanged += provider_ContextChanged;

            rwLock.EnterWriteLock();
            try
            {
                if (!activeProviders.Contains(provider))
                    activeProviders.Add(provider);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            OnContextChanged();
        }

        private void provider_LeaveContext(object sender, EventArgs e)
        {
            var provider = (IContextProvider)sender;
            provider.ContextChanged -= provider_ContextChanged;

            rwLock.EnterWriteLock();
            try
            {
                activeProviders.Remove(provider);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }

            OnContextChanged();
        }

        private void provider_ContextChanged(object sender, EventArgs e)
        {
            OnContextChanged();
        }

        protected void OnContextChanged()
        {
            OnContextChanged(this, EventArgs.Empty);
        }

        #endregion

    }
}
