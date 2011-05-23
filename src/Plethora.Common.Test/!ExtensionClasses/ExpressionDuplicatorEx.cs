using System;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.Test.ExtensionClasses
{
    class ExpressionDuplicatorEx
    {
        private readonly MirrorClass mirrorClass;

        public ExpressionDuplicatorEx()
        {
            Type cachedExecutorType = typeof(CachedExecutor);
            string lambdaKeyerName = cachedExecutorType.Namespace + ".ExpressionDuplicator";

            this.mirrorClass = MirrorClass.Create(cachedExecutorType.Assembly, lambdaKeyerName);
        }

        public T Duplicate<T>(T expr)
            where T : LambdaExpression
        {
            return (T)mirrorClass.Exec(new[] { typeof(T) }, expr);
        }
    }
}
