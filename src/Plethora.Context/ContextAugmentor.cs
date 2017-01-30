﻿using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    /// <summary>
    /// A class which allows additional context to be derived from a given context.
    /// </summary>
    /// <example>
    /// <para>
    ///  Imagine a trading system. When the user enters the context of a trade this will
    ///  implicitly imply additional context. A trade is made on an asset. It is made in
    ///  a certain currency. It is made on an exchange. Therefore, a "trade" context also
    ///  implies an asset, currency, and exchange. Furthermore, an asset has properties
    ///  such as the country of domicile.
    /// </para>
    /// <para>
    ///  A <see cref="ContextAugmentor"/> can therefore be defined to create the
    ///  additional asset, currency and exchange contexts from a trade context.
    ///  A second <see cref="ContextAugmentor"/> can be defined to create the country context
    ///  from the asset context.
    /// </para>
    /// </example>
    public class ContextAugmentor : IContextAugmentor
    {
        private readonly string contextName;
        private readonly Func<ContextInfo, IEnumerable<ContextInfo>> augmentationFunc;

        /// <summary>
        /// Initializes an instance of the <see cref="ContextAugmentor"/> class.
        /// </summary>
        /// <param name="contextName">
        /// The name of the base context.
        /// </param>
        /// <param name="augmentationFunc">
        /// The augmentation function which takes the base context and returns the derived contexts.
        /// </param>
        public ContextAugmentor(string contextName, Func<ContextInfo, IEnumerable<ContextInfo>> augmentationFunc)
        {
            //Validation
            if (contextName == null)
                throw new ArgumentNullException("contextName");

            if (augmentationFunc == null)
                throw new ArgumentNullException("augmentationFunc");


            this.contextName = contextName;
            this.augmentationFunc = augmentationFunc;
        }

        public string ContextName
        {
            get { return this.contextName; }
        }

        public IEnumerable<ContextInfo> Augment(ContextInfo context)
        {
            return this.augmentationFunc(context);
        }
    }
}
