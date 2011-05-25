using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace Plethora.ExpressionAide
{
    /// <summary>
    /// Class which provides the ability to execute an <see cref="Expression"/>, using
    /// cached closure promotion.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   The compiled function of any expression encountered will be cached, ensuring
    ///   that re-execusion does not require re-compiling the expression.
    ///  </para>
    ///  <para>
    ///   Furthermore expressions which vary only the constant values are rewritten
    ///   to allow the constant to be passed in as an extra parameter to the expresison.
    ///   This allows multiple expressions to be represented by a single compiled
    ///   function, improving execution times.
    ///  </para>
    /// </remarks>
    public static class CachedExecutor
    {
        #region Fields

        private static readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private static readonly Dictionary<string, ILambdaExecutor> expressionCache =
            new Dictionary<string, ILambdaExecutor>();

        private static readonly ExpressionDuplicatorWithClosurePromotion duplicator =
            new ExpressionDuplicatorWithClosurePromotion();
        #endregion

        #region Public Methods

        public static object Execute<TLambda>(this TLambda lambdaExpression, params object[] args)
            where TLambda : LambdaExpression
        {
            return ExecuteInner(lambdaExpression, args);
        }

        public static TResult Execute<TResult>(this Expression<Func<TResult>> expression)
        {
            return (TResult)ExecuteInner(expression);
        }

        public static TResult Execute<T, TResult>(this Expression<Func<T, TResult>> expression, T arg)
        {
            return (TResult)ExecuteInner(expression, arg);
        }

        public static TResult Execute<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression, T1 arg1, T2 arg2)
        {
            return (TResult)ExecuteInner(expression, arg1, arg2);
        }

        public static TResult Execute<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression, T1 arg1, T2 arg2, T3 arg3)
        {
            return (TResult)ExecuteInner(expression, arg1, arg2, arg3);
        }

        public static TResult Execute<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return (TResult)ExecuteInner(expression, arg1, arg2, arg3, arg4);
        }
        #endregion

        #region Private Methods

        private static object ExecuteInner<TLambda>(TLambda lambdaExpression, params object[] args)
            where TLambda : LambdaExpression
        {
            ILambdaExecutor executor = GetExecutor(lambdaExpression);
            return executor.Execute(lambdaExpression, args);
        }

        private static ILambdaExecutor GetExecutor(LambdaExpression lambdaExpression)
        {
            var key = LambdaKeyer.GetKey(lambdaExpression);

            cacheLock.EnterUpgradeableReadLock();
            try
            {
                ILambdaExecutor executor;
                bool result = expressionCache.TryGetValue(key, out executor);
                if (!result)
                {
                    cacheLock.EnterWriteLock();
                    try
                    {
                        result = expressionCache.TryGetValue(key, out executor);
                        if (!result)
                        {
                            executor = duplicator.PromoteClosures(lambdaExpression);
                            expressionCache.Add(key, executor);
                        }
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }

                return executor;
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }
        #endregion
    }
}
