using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using JetBrains.Annotations;

namespace Plethora.Context
{
    // TODO: Memory leaks: Should the ContextManager use weak delegates to subscribe to the providers' events?

    /// <summary>
    /// The pricipal manager of all elements related to contexts. 
    /// </summary>
    public class ContextManager
    {
        #region Singleton Instance

        private static readonly ContextManager globalInstance = new ContextManager();

        /// <summary>
        /// The global instance of the <see cref="ContextManager"/>.
        /// </summary>
        /// <remarks>
        /// Implementations can utilise the global instance or choose to define a local instance if preferred.
        /// </remarks>
        [NotNull]
        public static ContextManager GlobalInstance
        {
            get { return globalInstance; }
        }

        #endregion

        #region Fields

        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private readonly ICollection<IContextProvider> activeProviders = new HashSet<IContextProvider>();
        private readonly Dictionary<string, ICollection<IContextAugmenter>> augmenters = new Dictionary<string, ICollection<IContextAugmenter>>(0);

        private IEnumerable<ContextInfo> cachedContexts = null;

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
            add { this.contextChanged.Add(value); }
            remove { this.contextChanged.Remove(value); }
        }

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(object sender, EventArgs e)
        {
            this.cachedContexts = null;

            foreach (var handler in this.contextChanged.GetInvocationList())
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
        public void RegisterProvider([NotNull] IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider.EnterContext += this.provider_EnterContext;
            provider.LeaveContext += this.provider_LeaveContext;
        }

        /// <summary>
        /// Deregisters an context provider.
        /// </summary>
        public void DeregisterProvider([NotNull] IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            this.rwLock.EnterWriteLock();
            try
            {
                this.activeProviders.Remove(provider);

                provider.EnterContext -= this.provider_EnterContext;
                provider.LeaveContext -= this.provider_LeaveContext;
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }


        /// <summary>
        /// Registers an augmenter with this <see cref="ContextManager"/>.
        /// </summary>
        /// <param name="augmenter">The augmenter to be registered.</param>
        /// <remarks>
        /// The augmenter is used to provide addition contexts, based on a current
        /// in-scope context.
        /// </remarks>
        public void RegisterAugmenter([NotNull] IContextAugmenter augmenter)
        {
            //Validation
            if (augmenter == null)
                throw new ArgumentNullException(nameof(augmenter));

            this.rwLock.EnterWriteLock();
            try
            {
                if (!this.augmenters.TryGetValue(augmenter.ContextName, out var list))
                {
                    list = new List<IContextAugmenter>();
                    this.augmenters.Add(augmenter.ContextName, list);
                }

                list.Add(augmenter);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregisters an augmenter.
        /// </summary>
        public void DeregisterAugmenter([NotNull] IContextAugmenter augmenter)
        {
            //Validation
            if (augmenter == null)
                throw new ArgumentNullException(nameof(augmenter));

            this.rwLock.EnterWriteLock();
            try
            {
                if (this.augmenters.TryGetValue(augmenter.ContextName, out var list))
                {
                    list.Remove(augmenter);

                    if (list.Count == 0)
                    {
                        this.augmenters.Remove(augmenter.ContextName);
                    }
                }
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the list of contexts which are currently in-scope.
        /// </summary>
        /// <returns>
        /// The list of available contexts.
        /// </returns>
        [NotNull]
        public IEnumerable<ContextInfo> GetContexts()
        {
            // Keep a cache of the contexts, and only updating those when the context changes.
            var localCachedContexts = this.cachedContexts;
            if (localCachedContexts != null)
                return localCachedContexts;


            this.rwLock.EnterReadLock();
            try
            {
                var contextSet = new Dictionary<ContextInfo, ContextInfo>(new ContextInfoIgnoreRankComparer());

                var localContextsEnumerable = this.activeProviders
                    .Where(provider => provider != null)
                    .Select(provider => provider.Contexts)
                    .Where(contexts => contexts != null)
                    .SelectMany(contexts => contexts)
                    .Where(context => context != null);

                IEnumerable<ContextInfo> contextsEnumerable = localContextsEnumerable;

                //Use the augmenters to augment the in-scope contexts
                bool newContexts = true;
                while (newContexts)
                {
                    var nextContextsEnumerable = Enumerable.Empty<ContextInfo>();

                    newContexts = false;
                    foreach (var contextInfo in contextsEnumerable)
                    {
                        if (contextInfo == null)
                            continue;

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
                            var augContexts = this.AugmentContext(contextInfo);
                            if (augContexts != null)
                                nextContextsEnumerable = nextContextsEnumerable.Concat(augContexts);
                        }
                    }

                    contextsEnumerable = nextContextsEnumerable;
                }

                this.cachedContexts = contextSet.Keys;
                return contextSet.Keys;
            }
            finally
            {
                this.rwLock.ExitReadLock();
            }
        }

        #endregion

        #region Private Methods

        /// <remarks>
        /// Must be called inside a lock.
        /// </remarks>
        [CanBeNull, ItemCanBeNull]
        private IEnumerable<ContextInfo> AugmentContext(ContextInfo context)
        {
            IEnumerable<ContextInfo> augmentedContexts = null;

            if (this.augmenters.TryGetValue(context.Name, out var augmenterList))
            {
                foreach (var augmenter in augmenterList)
                {
                    IEnumerable<ContextInfo> additionalContexts = augmenter.Augment(context);
                    if (additionalContexts != null)
                    {
                        if (augmentedContexts == null)
                            augmentedContexts = additionalContexts;
                        else
                            augmentedContexts = augmentedContexts.Concat(additionalContexts);
                    }
                }
            }

            return augmentedContexts;
        }

        private void provider_EnterContext(object sender, EventArgs e)
        {
            var provider = (IContextProvider)sender;
            provider.ContextChanged += this.provider_ContextChanged;

            this.rwLock.EnterWriteLock();
            try
            {
                if (!this.activeProviders.Contains(provider))
                    this.activeProviders.Add(provider);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }

            this.OnContextChanged();
        }

        private void provider_LeaveContext(object sender, EventArgs e)
        {
            var provider = (IContextProvider)sender;
            provider.ContextChanged -= this.provider_ContextChanged;

            this.rwLock.EnterWriteLock();
            try
            {
                this.activeProviders.Remove(provider);
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }

            this.OnContextChanged();
        }

        private void provider_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }

        protected void OnContextChanged()
        {
            this.OnContextChanged(this, EventArgs.Empty);
        }

        #endregion

    }
}
