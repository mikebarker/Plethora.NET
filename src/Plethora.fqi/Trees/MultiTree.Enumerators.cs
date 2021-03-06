using System;
using System.Collections;
using System.Collections.Generic;

using Plethora.Collections.Trees;

namespace Plethora.fqi.Trees
{
    partial class MultiTree<TKey, TValue>
    {
        public abstract class BaseEnumerator<T> : IKeyLimitedEnumerator<TKey, T>
        {
            #region Fields

            protected readonly IKeyLimitedEnumerator<TKey, KeyValuePair<TKey, List<TValue>>> treeEnumerator;
            protected IEnumerator<TValue> listEnumerator = null;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="BaseEnumerator{T}"/> class.
            /// </summary>
            /// <param name="multiTree">
            /// The <see cref="MultiTree{TKey, TValue}"/> tree to be enumerated.
            /// </param>
            protected BaseEnumerator(MultiTree<TKey, TValue> multiTree)
            {
                this.treeEnumerator = multiTree.innerTree.GetPairEnumerator();
            }
            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                this.Reset();
            }
            #endregion

            #region Implementation of IEnumerator

            public bool MoveNext()
            {
                bool result = false;

                if (this.listEnumerator != null)
                {
                    result = this.listEnumerator.MoveNext();
                }

                if (!result)
                {
                    result = this.treeEnumerator.MoveNext();
                    if (result)
                    {
                        this.listEnumerator = this.treeEnumerator.Current.Value.GetEnumerator();
                        result = this.listEnumerator.MoveNext();
                    }
                }

                return result;
            }

            public void Reset()
            {
                this.treeEnumerator.Reset();
                this.listEnumerator = null;
            }

            public abstract T Current
            {
                get;
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }
            #endregion

            #region Implementation of IKeyLimitedEnumerator<TKey,T>

            public TKey Min
            {
                get { return this.treeEnumerator.Min; }
                set { this.treeEnumerator.Min = value; }
            }

            public TKey Max
            {
                get { return this.treeEnumerator.Max; }
                set { this.treeEnumerator.Max = value; }
            }
            #endregion
        }

        public class InOrderValueEnumerator : BaseEnumerator<TValue>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderValueEnumerator"/> class.
            /// </summary>
            public InOrderValueEnumerator(MultiTree<TKey, TValue> multiTree)
                :base(multiTree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override TValue Current
            {
                get
                {
                    if (this.listEnumerator == null)
                        throw new InvalidOperationException("Enumeration either not started, or complete.");

                    return this.listEnumerator.Current;
                }
            }
            #endregion
        }

        public class InOrderKeyEnumerator : BaseEnumerator<TKey>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderValueEnumerator"/> class.
            /// </summary>
            public InOrderKeyEnumerator(MultiTree<TKey, TValue> multiTree)
                :base(multiTree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override TKey Current
            {
                get
                {
                    if (this.listEnumerator == null)
                        throw new InvalidOperationException("Enumeration either not started, or complete.");

                    return this.treeEnumerator.Current.Key;
                }
            }
            #endregion
        }

        public class InOrderPairEnumerator : BaseEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderValueEnumerator"/> class.
            /// </summary>
            public InOrderPairEnumerator(MultiTree<TKey, TValue> multiTree)
                :base(multiTree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override KeyValuePair<TKey, TValue> Current
            {
                get 
                {
                    if (this.listEnumerator == null)
                        throw new InvalidOperationException("Enumeration either not started, or complete.");

                    return new KeyValuePair<TKey, TValue>(this.treeEnumerator.Current.Key, this.listEnumerator.Current);
                }

            }
            #endregion
        }
    }
}
