using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Plethora
{
    /// <summary>
    /// Helper class for creating delegates containing a weak reference to the target.
    /// </summary>
    public static class WeakDelegate
    {
        private class DelegateReference<TDelegate>
        {
            public TDelegate _delegate;  //Do not delete, used by reflection
        }

        #region Public Methods

        /// <summary>
        /// Create a wrapper around a delegate to maintain only a weak reference to the invocation target.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate to be wrapped in a weak reference.</typeparam>
        /// <param name="delegate">The delegate to be wrapped.</param>
        /// <param name="onTargetCollected">
        /// Action to be called when it is noticed that the target has been garbage collected.
        /// </param>
        /// <remarks>
        /// It is intended that the method be used in a manner similar to:
        /// <example>
        /// <![CDATA[
        ///   Subscriber subscriber = new Subscriber();
        ///   Publisher publisher = new Publisher();
        ///   
        ///   //The reference to subscriber is held in a weak reference, allow for the subscriber to be garbage collected.
        ///   // When it is detected that the subscriber has been garbage collected, the weak delegate will remove itself from the
        ///   // publisher's Event invocationList.
        ///   publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(subscriber.Callback, handler => publisher.Event -= handler);
        /// ]]>
        /// </example>
        /// </remarks>
        public static TDelegate CreateWeakDelegate<TDelegate>(TDelegate @delegate, Action<TDelegate> onTargetCollected)
        {
            //Validation
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (!typeof(Delegate).IsAssignableFrom(typeof(TDelegate)))
                throw new ArgumentException("Type parameter TDelegate must be a subclass of Delegate.", "delegate");

            if (onTargetCollected == null)
                throw new ArgumentNullException("onTargetCollected");

            var del = (Delegate)(object)@delegate;

            //For static method calls, return the delegate without wrapping
            if (del.Target == null)
                return @delegate;

            if (del.Method.ReturnType != typeof(void))
                throw new ArgumentException(string.Format("The return type of {0} is not 'void'. Use CreateWeakDelegate<{0}, TResult>({0}, Func<{0}, TResult>)",
                    typeof(TDelegate).Name));


            /* ****************************
             * Encoding as an expression:
             * ****************************
             * 
             *   var target = weakReference.Target;
             *   if (target != null)
             *       method.Invoke(target, parameters);
             *   else
             *       onTargetCollected(delegateReference._delegate);
             * 
             * */

            var weakDelegate = InternalCreateWeakDelegate(
                @delegate,
                onTargetCollected,
                delegate(BinaryExpression targetIsNotNullExp, MethodCallExpression callExp, MethodCallExpression onTargetNull)
                {
                    var ifNotNullExp = Expression.IfThenElse(targetIsNotNullExp,
                        callExp,
                        onTargetNull);

                    return ifNotNullExp;
                });

            return weakDelegate;
        }

        /// <summary>
        /// Create a wrapper around a delegate to maintain only a weak reference to the invocation target.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate to be wrapped in a weak reference.</typeparam>
        /// <typeparam name="TResult">The return type of the <typeparamref name="TDelegate"/> delagte type.</typeparam>
        /// <param name="delegate">The delegate to be wrapped.</param>
        /// <param name="onTargetCollected">
        /// Function to be called when it is noticed that the target has been garbage collected, to return a default value.
        /// </param>
        /// <remarks>
        /// It is intended that the method be used in a manner similar to:
        /// <example>
        /// <![CDATA[
        ///   HandleObject obj = new HandleObject();
        ///   
        ///   //The reference to obj is held in a weak reference, allow for obj to be garbage collected.
        ///   // When it is detected that the obj has been garbage collected, the default value [0] will be returned.
        ///   Func<int> weakGetHandle = WeakDelegate.CreateWeakDelegate<Func<int>, int>(obj.GetHandle, handler => 0);
        /// ]]>
        /// </example>
        /// </remarks>
        public static TDelegate CreateWeakDelegate<TDelegate, TResult>(TDelegate @delegate, Func<TDelegate, TResult> onTargetCollected)
        {
            //Validation
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (!typeof(Delegate).IsAssignableFrom(typeof(TDelegate)))
                throw new ArgumentException("Type parameter TDelegate must be a subclass of Delegate.", "delegate");

            if (onTargetCollected == null)
                throw new ArgumentNullException("onTargetCollected");

            var del = (Delegate)(object)@delegate;

            //For static method calls, return the delegate without wrapping
            if (del.Target == null)
                return @delegate;

            if (del.Method.ReturnType == typeof(void))
                throw new ArgumentException(string.Format("The return type of {0} is 'void'. Use CreateWeakDelegate<{0}>({0}, Action<{0}>)",
                    typeof(TDelegate).Name));

            if (!del.Method.ReturnType.IsAssignableFrom(typeof(TResult)))
                throw new ArgumentException(string.Format("The return type of the {0} is {1}, but this is not assignable from the type of TResult specified, {2}.",
                    typeof(TDelegate).Name,
                    del.Method.ReturnType.Name,
                    typeof(TResult).Name));


            /* ****************************
             * Encoding as an expression:
             * ****************************
             * 
             *   var target = weakReference.Target;
             *   TResult result;
             *   if (target != null)
             *       result = (TResult)method.Invoke(target, parameters);
             *   else
             *       result = onTargetCollected(delegateReference._delegate);
             * 
             *   return result;
             * */

            var weakDelegate = InternalCreateWeakDelegate(
                @delegate,
                onTargetCollected,
                delegate(BinaryExpression targetIsNotNullExp, MethodCallExpression callExp, MethodCallExpression onTargetNull)
                {
                    var resultExp = Expression.Variable(typeof(TResult), "result");

                    var ifNotNullExp = Expression.IfThenElse(targetIsNotNullExp,
                        Expression.Assign(resultExp, callExp),
                        Expression.Assign(resultExp, onTargetNull));

                    var body = Expression.Block(new[] { resultExp }, ifNotNullExp, resultExp);

                    return body;
                });

            return weakDelegate;
        }

        #endregion

        #region Private Methods

        private static TDelegate InternalCreateWeakDelegate<TDelegate>(
            TDelegate @delegate,
            Delegate onTargetCollected,
            Func<BinaryExpression, MethodCallExpression, MethodCallExpression, Expression> generateBody)
        {
            var del = (Delegate)(object)@delegate;

            //For static method calls, return the delegate without wrapping
            if (del.Target == null)
                return @delegate;

            WeakReference weakTarget = new WeakReference(del.Target);
            MethodInfo method = del.Method;

            /* ****************************
             * Encoding as an expression:
             * ****************************
             * 
             *   var target = weakReference.Target;
             *   TResult result;
             *   if (target != null)
             *       result = (TResult)method.Invoke(target, parameters);
             *   else
             *       result = onTargetCollected(delegateReference._delegate);
             * 
             *   return result;
             * */


            DelegateReference<TDelegate> delegateReference = new DelegateReference<TDelegate>();

            var delegateReferenceExp = Expression.Constant(delegateReference);
            var thisDelegateExp = Expression.Field(delegateReferenceExp, "_delegate");

            MethodCallExpression onTargetCollectedCallExp = (onTargetCollected.Target == null)
                ? Expression.Call(onTargetCollected.Method, thisDelegateExp)
                : Expression.Call(Expression.Constant(onTargetCollected.Target), onTargetCollected.Method, thisDelegateExp);



            var weakTargetExp = Expression.Constant(weakTarget);

            var targetExp = Expression.Convert(Expression.Property(weakTargetExp, "target"), del.Target.GetType());



            var parameterExps = method.GetParameters()
                .Select(param => Expression.Parameter(param.ParameterType, param.Name))
                .ToArray();

            var callExp = Expression.Call(targetExp, method, parameterExps);


            var onTargetNull = onTargetCollectedCallExp;

            var nullExp = Expression.Constant(null);
            var targetIsNotNullExp = Expression.ReferenceNotEqual(targetExp, nullExp);

            var body = generateBody(targetIsNotNullExp, callExp, onTargetNull);

            var lambda = Expression.Lambda<TDelegate>(body, parameterExps);

            var weakDelegate = lambda.Compile();

            delegateReference._delegate = weakDelegate;

            return weakDelegate;
        }

        #endregion
    }
}
