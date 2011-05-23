using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.Test.ExtensionClasses
{
    class ExpressionDuplicatorWithClosurePromotionEx
    {
        private readonly MirrorClass mirrorClass;

        public ExpressionDuplicatorWithClosurePromotionEx()
        {
            Type cachedExecutorType = typeof(CachedExecutor);
            string lambdaKeyerName = cachedExecutorType.Namespace + ".ExpressionDuplicatorWithClosurePromotion";

            this.mirrorClass = MirrorClass.Create(cachedExecutorType.Assembly, lambdaKeyerName);
        }

        #region Promote Closures

        public LambdaExecutor PromoteClosures(LambdaExpression expr)
        {
            return (LambdaExecutor)mirrorClass.Exec(
                new Type[0],
                expr);
        }

        public LambdaExecutor<TResult> PromoteClosures<TResult>(Expression<Func<TResult>> expr)
        {
            return (LambdaExecutor<TResult>)mirrorClass.Exec(
                new[] { typeof(TResult) },
                expr);
        }

        public LambdaExecutor<T, TResult> PromoteClosures<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            return (LambdaExecutor<T, TResult>)mirrorClass.Exec(
                new[] { typeof(T), typeof(TResult) },
                expr);
        }

        public LambdaExecutor<T1, T2, TResult> PromoteClosures<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expr)
        {
            return (LambdaExecutor<T1, T2, TResult>)mirrorClass.Exec(
                new[] { typeof(T1), typeof(T2), typeof(TResult) },
                expr);
        }

        public LambdaExecutor<T1, T2, T3, TResult> PromoteClosures<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expr)
        {
            return (LambdaExecutor<T1, T2, T3, TResult>)mirrorClass.Exec(
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(TResult) },
                expr);
        }

        public LambdaExecutor<T1, T2, T3, T4, TResult> PromoteClosures<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expr)
        {
            return (LambdaExecutor<T1, T2, T3, T4, TResult>)mirrorClass.Exec(
                new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(TResult) },
                expr);
        }
        #endregion

        #region DuplicateWithClosurePromotion

        public LambdaExpression DuplicateWithClosurePromotion(
            LambdaExpression expr,
            out IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
        {
            return (LambdaExpression)mirrorClass.Exec(new Type[0], expr, parameters);
        }
        #endregion
    }
}
