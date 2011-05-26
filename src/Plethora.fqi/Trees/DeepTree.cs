using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.fqi.Trees
{
    /// <summary>
    /// Interface defining a layer of a deep-tree.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///    A deep-tree is a tree, of trees, of trees... of data.
    ///  </para>
    ///  <para>
    ///    Each layer has a separate key, allowing the deep-tree to be used as a multi-columned index.
    ///  </para>
    /// </remarks>
    internal interface IDeepTreeLayer<T> : IIndexedCollection<T>
    {
    }

    /// <summary>
    /// Static class for the construction of deep-tree
    /// </summary>
    internal static partial class DeepTree
    {
        #region Public Static Methods

        public static IDeepTreeLayer<T> CreateDeepTree<T>(bool unique, IEnumerable<LambdaExpression> expressions)
        {
            Type typeTree = CreateDeepTreeType<T>(expressions);

            var indexNames = expressions.Select(r => ExpressionAnalyser.GetMemberName(r)).ToArray();
            var indexFuncs = expressions.Select(r => r.Compile()).ToArray();

            return CreateTreeInstance<T>(typeTree,
                unique, indexNames, indexFuncs);
        }
        #endregion

        #region Private Static Methods

        private static Type CreateDeepTreeType<T>(IEnumerable<LambdaExpression> expressions)
        {
            Type typeT = typeof(T);
            Type typeTree = null;
            //Reverse the list, to build the tree bottom up.
            foreach (var expression in expressions.Reverse())
            {
                var memberExp = expression.Body;

                Type typeTKey = memberExp.Type;

                typeTree = (typeTree == null)
                                    ? typeof(DeepTreeLeafLayer<,>).MakeGenericType(typeTKey, typeT)
                                    : typeof(DeepTreeMidLayer<,,>).MakeGenericType(typeTKey, typeT, typeTree);
            }

            return typeTree;
        }

        private static IDeepTreeLayer<T> CreateTreeInstance<T>(Type typeTree, bool unique, IEnumerable<string> indexNames, IEnumerable<Delegate> indexFuncs)
        {
            return (IDeepTreeLayer<T>)Activator.CreateInstance(
                                           typeTree,
                                           unique, indexNames, indexFuncs);
        }
        #endregion
    }

}
