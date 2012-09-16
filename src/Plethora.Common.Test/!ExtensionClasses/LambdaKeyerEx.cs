using System;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.Test.ExtensionClasses
{
    static class LambdaKeyerEx
    {
        private static readonly MirrorClass mirrorClass;

        static LambdaKeyerEx()
        {
            Type cachedExecutorType = typeof(CachedExecutor);
            string lambdaKeyerName = cachedExecutorType.Namespace + ".LambdaKeyer";

            mirrorClass = MirrorClass.CreateStaticOnly(cachedExecutorType.Assembly, lambdaKeyerName);
        }

        public static string GetKey(Expression expression)
        {
            return (string)mirrorClass.Exec(new Type[0], expression);
        }
    }
}
