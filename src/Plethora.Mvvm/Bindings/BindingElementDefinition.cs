using System;
using System.Linq.Expressions;

namespace Plethora.Mvvm.Bindings
{
    /// <summary>
    /// Defines a single element in a binding.
    /// </summary>
    public abstract class BindingElementDefinition
    {
        public abstract IBindingObserverElement CreateObserver(IGetterProvider getterProvider);

        public Func<object, object> CreateGetter(Type observedType)
        {
            var parameterExpression = Expression.Parameter(typeof(object));
            var parameterConvertExpression = Expression.Convert(parameterExpression, observedType);
            var getterExpression = CreateGetterExpression(observedType, parameterConvertExpression);
            var bodyExpression = Expression.Convert(getterExpression, typeof(object));
            var lambdaExpression = Expression.Lambda<Func<object, object>>(bodyExpression, parameterExpression);

            var getter = lambdaExpression.Compile();

            return getter;
        }

        protected abstract Expression CreateGetterExpression(Type observedType, Expression observedExpression);

        public override abstract bool Equals(object obj);

        public override abstract int GetHashCode();
    }
}
