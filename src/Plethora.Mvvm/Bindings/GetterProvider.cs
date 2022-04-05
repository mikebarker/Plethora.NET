using Plethora.Collections;
using System;

namespace Plethora.Mvvm.Bindings
{
    public interface IGetterProvider
    {
        Func<object, object> AcquireGetter(Type observedType, BindingElementDefinition bindingElementDefinition);
    }

    public class CachedGetterProvider : IGetterProvider
    {
        private readonly MruDictionary<Tuple<Type, BindingElementDefinition>, Func<object, object>> gettersMap =
            new MruDictionary<Tuple<Type, BindingElementDefinition>, Func<object, object>>(maxEntries: 1024);
        private readonly IGetterProvider innerProvider;

        public CachedGetterProvider(
            IGetterProvider innerProvider)
        {
            this.innerProvider = innerProvider;
        }

        public Func<object, object> AcquireGetter(Type observedType, BindingElementDefinition bindingElementDefinition)
        {
            var key = Tuple.Create(observedType, bindingElementDefinition);

            if (!gettersMap.TryGetValue(key, out Func<object, object> getter))
            {
                getter = innerProvider.AcquireGetter(observedType, bindingElementDefinition);
                gettersMap.Add(key, getter);
            }

            return getter;
        }
    }

    public class GetterProvider : IGetterProvider
    {
        public Func<object, object> AcquireGetter(Type observedType, BindingElementDefinition bindingElementDefinition)
        {
            Func<object, object> getter = bindingElementDefinition.CreateGetter(observedType);

            return getter;
        }
    }
}
