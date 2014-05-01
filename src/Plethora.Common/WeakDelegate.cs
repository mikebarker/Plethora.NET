using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Plethora
{
    /// <summary>
    /// Helper class for creating delegates containing a weak reference to the target.
    /// </summary>
    public static class WeakDelegate
    {
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
                new DelegateReference_Action<TDelegate>(onTargetCollected),
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
        /// <typeparam name="TResult">The return type of the <typeparamref name="TDelegate"/> delegate type.</typeparam>
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
                new DelegateReference_Func<TDelegate, TResult>(onTargetCollected),
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
            DelegateReference<TDelegate> delegateReference,
            Func<BinaryExpression, MethodCallExpression, MethodCallExpression, Expression> generateBody)
        {
            var del = (Delegate)(object)@delegate;

            //For static method calls, return the delegate without wrapping
            if (del.Target == null)
                return @delegate;

            WeakReference weakTarget = new WeakReference(del.Target);
            MethodInfo method = del.Method;

            delegateReference._weakTarget = weakTarget;

            /* ****************************
             * Encoding as an expression:
             * ****************************
             * 
             *   var target = weakReference.Target;
             *   TResult result;
             *   if (target != null)
             *       result = (TResult)method.Invoke(target, parameters);
             *   else
             *       result = delegateReference.OnTargetCollected();
             * 
             *   return result;
             * */


            Type type = delegateReference.GetType();
            MethodInfo onTargetCollected = type.GetMethod("OnTargetCollected", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodCallExpression onTargetCollectedCallExp = Expression.Call(Expression.Constant(delegateReference), onTargetCollected);



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

        #region DelegateReference

        internal abstract class DelegateReference
        {
            public WeakReference _weakTarget;

            public bool IsTargetAlive
            {
                get
                {
                    var isTargetAlive = this._weakTarget.IsAlive;
                    if (!isTargetAlive)
                    {
                        this.CallOnTargetCollected();
                    }

                    return isTargetAlive;
                }
            }

            protected abstract void CallOnTargetCollected();
        }

        private abstract class DelegateReference<TDelegate> : DelegateReference
        {
            public TDelegate _delegate;
        }

        private class DelegateReference_Action<TDelegate> : DelegateReference<TDelegate>
        {
            private readonly Action<TDelegate> onTargetCollected;
            private bool onTargetCollectedCalled = false;

            public DelegateReference_Action(Action<TDelegate> onTargetCollected)
            {
                this.onTargetCollected = onTargetCollected;
            }

            protected override void CallOnTargetCollected()
            {
                OnTargetCollected();
            }

            private void OnTargetCollected()    // Do not change signature, used by reflection
            {
                if (this.onTargetCollectedCalled)
                    return;

                this.onTargetCollected(this._delegate);
                this.onTargetCollectedCalled = true;
            }
        }

        private class DelegateReference_Func<TDelegate, TResult> : DelegateReference<TDelegate>
        {
            private readonly Func<TDelegate, TResult> onTargetCollected;
            private bool onTargetCollectedCalled = false;

            public DelegateReference_Func(Func<TDelegate, TResult> onTargetCollected)
            {
                this.onTargetCollected = onTargetCollected;
            }

            protected override void CallOnTargetCollected()
            {
                OnTargetCollected();
            }

            private TResult OnTargetCollected() // Do not change signature, used by reflection
            {
                if (this.onTargetCollectedCalled)
                    return default(TResult);

                var result = this.onTargetCollected(this._delegate);
                this.onTargetCollectedCalled = true;
                return result;
            }
        }

        #endregion
    }


    public static class WeakDelegateHelper
    {
        public static bool IsTargetAlive(this Delegate @delegate)
        {
            Closure closure = @delegate.Target as Closure;
            if (closure != null)
            {
                WeakDelegate.DelegateReference delegateReference = closure.Constants
                    .OfType<WeakDelegate.DelegateReference>()
                    .SingleOrDefault();

                if (delegateReference != null)
                    return delegateReference.IsTargetAlive;
            }

            return true;
        }
    }
}
