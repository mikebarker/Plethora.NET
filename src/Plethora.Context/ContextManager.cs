using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Context
{
    // TODO: This can be more efficient by keeping a cache of the contexts, and only updating those per provider when the context changes
    public class ContextManager
    {
        #region Singleton Instance

        private static readonly ContextManager defaultInstance = new ContextManager();
        public static ContextManager DefaultInstance
        {
            get { return defaultInstance; }
        }
        #endregion

        #region Fields

        private readonly object lockObj = new object();
        private readonly ICollection<IContextProvider> activeProviders = new HashSet<IContextProvider>();
        private readonly Dictionary<string, ICollection<ContextAugmentor>> augmentors = new Dictionary<string, ICollection<ContextAugmentor>>();
        private readonly ICollection<IActionFactory> actionFactories = new List<IActionFactory>();
        private TemplateActionFactory templateActionFactory;

        #endregion

        #region Events

        public event EventHandler ContextChanged;
        #endregion

        #region Public Methods

        public void RegisterProvider(IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException("provider");

            provider.EnterContext += provider_EnterContext;
            provider.LeaveContext += provider_LeaveContext;
        }

        public void DeregisterProvider(IContextProvider provider)
        {
            //Validation
            if (provider == null)
                throw new ArgumentNullException("provider");

            lock (lockObj)
            {
                activeProviders.Remove(provider);

                provider.EnterContext -= provider_EnterContext;
                provider.LeaveContext -= provider_LeaveContext;
            }
        }

        public void RegisterAugmentor(ContextAugmentor augmentor)
        {
            //Validation
            if (augmentor == null)
                throw new ArgumentNullException("augmentor");

            lock (lockObj)
            {
                ICollection<ContextAugmentor> list;
                if (!augmentors.TryGetValue(augmentor.ContextName, out list))
                {
                    list = new List<ContextAugmentor>();
                    augmentors.Add(augmentor.ContextName, list);
                }

                list.Add(augmentor);
            }
        }

        public void DeregisterAugmentor(ContextAugmentor augmentor)
        {
            //Validation
            if (augmentor == null)
                throw new ArgumentNullException("augmentor");

            lock (lockObj)
            {
                ICollection<ContextAugmentor> list;
                if (augmentors.TryGetValue(augmentor.ContextName, out list))
                    list.Remove(augmentor);
            }
        }

        public void RegisterFactory(IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException("factory");

            lock (lockObj)
            {
                this.actionFactories.Add(factory);
            }
        }

        public void DeregisterFactory(IActionFactory factory)
        {
            //Validation
            if (factory == null)
                throw new ArgumentNullException("factory");

            lock (lockObj)
            {
                this.actionFactories.Remove(factory);
            }
        }

        public void RegisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");

            lock (lockObj)
            {
                if (templateActionFactory == null)
                {
                    templateActionFactory = new TemplateActionFactory();
                    actionFactories.Add(templateActionFactory);
                }

                templateActionFactory.RegisterActionTemplate(template);
            }
        }

        public void RegisterActionTemplate(IMultiActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");

            lock (lockObj)
            {
                if (templateActionFactory == null)
                {
                    templateActionFactory = new TemplateActionFactory();
                    actionFactories.Add(templateActionFactory);
                }

                templateActionFactory.RegisterActionTemplate(template);
            }
        }

        public IEnumerable<ContextInfo> GetContexts()
        {
            lock (lockObj)
            {
                var contextSet = new Dictionary<ContextInfo, ContextInfo>(new ContextInfoComparer());

                var localContextsEnumerable = activeProviders
                    .Where(provider => provider != null)
                    .Select(provider => provider.Contexts)
                    .Where(contexts => contexts != null)
                    .SelectMany(contexts => contexts)
                    .Where(context => context != null);

                var contextsEnumerable = localContextsEnumerable;

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
        }

        public IEnumerable<IAction> GetActions(IEnumerable<ContextInfo> contexts)
        {
            lock (lockObj)
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

            lock (lockObj)
            {
                if (!activeProviders.Contains(provider))
                    activeProviders.Add(provider);
            }

            OnContextChanged();
        }

        private void provider_LeaveContext(object sender, EventArgs e)
        {
            var provider = (IContextProvider)sender;
            provider.ContextChanged -= provider_ContextChanged;

            lock (lockObj)
            {
                activeProviders.Remove(provider);
            }

            OnContextChanged();
        }

        private void provider_ContextChanged(object sender, EventArgs e)
        {
            OnContextChanged(sender, e);
        }

        protected void OnContextChanged()
        {
            OnContextChanged(this, EventArgs.Empty);
        }

        protected virtual void OnContextChanged(object sender, EventArgs e)
        {
            var handler = this.ContextChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion
    }
}
