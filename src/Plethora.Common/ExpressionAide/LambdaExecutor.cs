using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Plethora.ExpressionAide
{
    public interface ILambdaExecutor
    {
        object Execute(LambdaExpression lambda, params object[] args);
    }

    public interface ILambdaExecutor<in TLambda> : ILambdaExecutor
        where TLambda : LambdaExpression
    {
        object Execute(TLambda lambda, params object[] args);
    }

    public class LambdaExecutorBase<TLambda> : ILambdaExecutor<TLambda>
        where TLambda : LambdaExpression
    {
        #region Delegates

        private delegate object InvokeMethodDelegate(object[] args);
        #endregion

        #region Fields

        private readonly Step[] path;
        private readonly Delegate dupeDelegate;
        private readonly InvokeMethodDelegate invokeMethod;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutorBase{TLambda}"/> class.
        /// </summary>
        internal LambdaExecutorBase(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
        {
            this.dupeDelegate = dupe.Compile();
            this.path = FindMagicParameterPath(parameters);
            this.invokeMethod = this.GetInvokeDelegateFor(this.dupeDelegate);
        }
        #endregion

        #region Protected Methods

        protected object Execute(TLambda lambda, params object[] args)
        {
            //Inject the additional parameter to override the constant expression
            object[] delegateArgs;
            if (this.path != null)
            {
                delegateArgs = new object[args.Length + 1];
                args.CopyTo(delegateArgs, 0);
                delegateArgs[args.Length] = this.GetMagicParameter(lambda);
            }
            else
            {
                delegateArgs = args;
            }

            return this.invokeMethod(delegateArgs);
        }
        #endregion

        #region Private Methods

        private object GetMagicParameter(TLambda lambda)
        {
            Expression tmp = lambda;
            foreach (var step in this.path)
            {
                tmp = Move(tmp, step);
            }
            return ((ConstantExpression)tmp).Value;
        }

        private static Step[] FindMagicParameterPath(IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
        {
            foreach (var pair in parameters)
            {
                ParameterExpression parameterExpression = pair.Key;
                if (parameterExpression.Name.StartsWith(ExpressionDuplicatorWithClosurePromotion.CLOSURE_CONSTANT_NAME))
                {
                    return pair.Value.ToArray();
                }
            }

            return null;
        }

        private static Expression Move(Expression expr, Step step)
        {
            switch (step.Direction)
            {
                case Direction.This:
                    return expr;
                case Direction.Left:
                    return ((BinaryExpression)expr).Left;
                case Direction.Right:
                    return ((BinaryExpression)expr).Right;
                case Direction.Operand:
                    return ((UnaryExpression)expr).Operand;
                case Direction.Object:
                    return ((MethodCallExpression)expr).Object;
                case Direction.Body:
                    return ((LambdaExpression)expr).Body;
                case Direction.Test:
                    return ((ConditionalExpression)expr).Test;
                case Direction.IfTrue:
                    return ((ConditionalExpression)expr).IfTrue;
                case Direction.IfFalse:
                    return ((ConditionalExpression)expr).IfFalse;
                case Direction.Expression:
                    if (expr is InvocationExpression)
                        return ((InvocationExpression)expr).Expression;
                    else if (expr is TypeBinaryExpression)
                        return ((TypeBinaryExpression)expr).Expression;
                    else if (expr is MemberExpression)
                        return ((MemberExpression)expr).Expression;
                    break;
                case Direction.NewExpression:
                    if (expr is ListInitExpression)
                        return ((ListInitExpression)expr).NewExpression;
                    else if (expr is MemberInitExpression)
                        return ((MemberInitExpression)expr).NewExpression;
                    break;

                case Direction.Arguments:
                    if (expr is MethodCallExpression)
                    {
                        IList<Expression> arguments = ((MethodCallExpression)expr).Arguments;
                        return arguments[step.Index];
                    }
                    else if (expr is NewExpression)
                    {
                        IList<Expression> arguments = ((NewExpression)expr).Arguments;
                        return arguments[step.Index];
                    }
                    else if (expr is InvocationExpression)
                    {
                        IList<Expression> arguments = ((InvocationExpression)expr).Arguments;
                        return arguments[step.Index];
                    }
                    break;
                case Direction.Expressions:
                    if (expr is NewArrayExpression)
                    {
                        IList<Expression> arguments = ((NewArrayExpression)expr).Expressions;
                        return arguments[step.Index];
                    }
                    break;
            }

            throw new ArgumentException(string.Format("Step {0} not recognised for Expression type {1}", step.Direction, expr.GetType().Name));
        }

        #region InvokeMethod Members

        /// <summary>
        /// Generates a known, named delegate which may be used to
        /// dynamically invoke <paramref name="delegate"/>
        /// </summary>
        /// <param name="delegate">The delegate for which the invoke delegate is required.</param>
        /// <returns></returns>
        /// <remarks>
        /// This provides an optimisation where, by creating a named delegate, the
        /// dupilcated delegate no longer need to use DynamicInvoke.
        /// </remarks>
        private InvokeMethodDelegate GetInvokeDelegateFor(Delegate @delegate)
        {
            InvokeMethodDelegate rtn = null;

            Type delegateType = @delegate.GetType();
            if (delegateType.IsGenericType)
            {
                Type[] genericArgs = delegateType.GetGenericArguments();

                MethodInfo methodInfo = typeof(LambdaExecutorBase<TLambda>)
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(method =>
                           method.Name == "InvokeMethod" &&
                           method.GetGenericArguments().Length == genericArgs.Length)
                    .SingleOrDefault();

                if (methodInfo != default(MethodInfo))
                {
                    MethodInfo genericMethod = methodInfo.MakeGenericMethod(genericArgs);
                    rtn = (InvokeMethodDelegate)Delegate.CreateDelegate(
                                                                   typeof(InvokeMethodDelegate),
                                                                   this,
                                                                   genericMethod, false);
                }
            }

            return rtn ?? this.InvokeMethod;
        }

        /// <remarks>
        /// Method referenced dynamically by <see cref="GetInvokeDelegateFor"/> method.
        /// DO NOT REMOVE.
        /// </remarks>
        [UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
        private object InvokeMethod<TResult>(object[] args)
        {
            if (args.Length != 0)
                throw new ArgumentException("Invalid number of arguments");

            var del = (Func<TResult>)this.dupeDelegate;
            return del.Invoke();
        }

        /// <remarks>
        /// Method referenced dynamically by <see cref="GetInvokeDelegateFor"/> method.
        /// DO NOT REMOVE.
        /// </remarks>
        [UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
        private object InvokeMethod<T1, TResult>(object[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Invalid number of arguments");

            T1 arg1 = (T1)args[0];

            var del = (Func<T1, TResult>)this.dupeDelegate;
            return del.Invoke(arg1);
        }

        /// <remarks>
        /// Method referenced dynamically by <see cref="GetInvokeDelegateFor"/> method.
        /// DO NOT REMOVE.
        /// </remarks>
        [UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
        private object InvokeMethod<T1, T2, TResult>(object[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException("Invalid number of arguments");

            T1 arg1 = (T1)args[0];
            T2 arg2 = (T2)args[1];

            var del = (Func<T1, T2, TResult>)this.dupeDelegate;
            return del.Invoke(arg1, arg2);
        }

        /// <remarks>
        /// Method referenced dynamically by <see cref="GetInvokeDelegateFor"/> method.
        /// DO NOT REMOVE.
        /// </remarks>
        [UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
        private object InvokeMethod<T1, T2, T3, TResult>(object[] args)
        {
            if (args.Length != 3)
                throw new ArgumentException("Invalid number of arguments");

            T1 arg1 = (T1)args[0];
            T2 arg2 = (T2)args[1];
            T3 arg3 = (T3)args[2];

            var del = (Func<T1, T2, T3, TResult>)this.dupeDelegate;
            return del.Invoke(arg1, arg2, arg3);
        }

        /// <remarks>
        /// Method referenced dynamically by <see cref="GetInvokeDelegateFor"/> method.
        /// DO NOT REMOVE.
        /// </remarks>
        [UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
        private object InvokeMethod<T1, T2, T3, T4, TResult>(object[] args)
        {
            if (args.Length != 4)
                throw new ArgumentException("Invalid number of arguments");

            T1 arg1 = (T1)args[0];
            T2 arg2 = (T2)args[1];
            T3 arg3 = (T3)args[2];
            T4 arg4 = (T4)args[3];

            var del = (Func<T1, T2, T3, T4, TResult>)this.dupeDelegate;
            return del.Invoke(arg1, arg2, arg3, arg4);
        }

        private object InvokeMethod(object[] args)
        {
            return this.dupeDelegate.DynamicInvoke(args);
        }
        #endregion
        #endregion

        #region Implementation of ILambdaExecutor<TLambda>

        object ILambdaExecutor.Execute(LambdaExpression lambda, params object[] args)
        {
            return this.Execute((TLambda)lambda, args);
        }

        object ILambdaExecutor<TLambda>.Execute(TLambda lambda, params object[] args)
        {
            return this.Execute(lambda, args);
        }
        #endregion
    }

    public class LambdaExecutor : LambdaExecutorBase<LambdaExpression>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public new object Execute(LambdaExpression lambda, params object[] args)
        {
            return base.Execute(lambda, args);
        }
        #endregion
    }

    public class LambdaExecutor<TResult> : LambdaExecutorBase<Expression<Func<TResult>>>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public TResult Execute(Expression<Func<TResult>> lambda)
        {
            return (TResult)base.Execute(lambda);
        }
        #endregion
    }

    public class LambdaExecutor<T, TResult> : LambdaExecutorBase<Expression<Func<T, TResult>>>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{T, TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public TResult Execute(Expression<Func<T, TResult>> lambda, T arg)
        {
            return (TResult)base.Execute(lambda, arg);
        }
        #endregion
    }

    public class LambdaExecutor<T1, T2, TResult> : LambdaExecutorBase<Expression<Func<T1, T2, TResult>>>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{T1, T2, TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public TResult Execute(Expression<Func<T1, T2, TResult>> lambda, T1 arg1, T2 arg2)
        {
            return (TResult)base.Execute(lambda, arg1, arg2);
        }
        #endregion
    }

    public class LambdaExecutor<T1, T2, T3, TResult> : LambdaExecutorBase<Expression<Func<T1, T2, T3, TResult>>>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{T1, T2, T3, TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public TResult Execute(Expression<Func<T1, T2, T3, TResult>> lambda, T1 arg1, T2 arg2, T3 arg3)
        {
            return (TResult)base.Execute(lambda, arg1, arg2, arg3);
        }
        #endregion
    }

    public class LambdaExecutor<T1, T2, T3, T4, TResult> : LambdaExecutorBase<Expression<Func<T1, T2, T3, T4, TResult>>>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaExecutor{T1, T2, T3, T4, TResult}"/> class.
        /// </summary>
        internal LambdaExecutor(LambdaExpression dupe, IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
            : base(dupe, parameters)
        {
        }
        #endregion

        #region Public Methods

        public TResult Execute(Expression<Func<T1, T2, T3, T4, TResult>> lambda, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return (TResult)base.Execute(lambda, arg1, arg2, arg3, arg4);
        }
        #endregion
    }
}
