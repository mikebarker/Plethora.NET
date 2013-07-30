using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    public class ContextAugmentor
    {
        private readonly string contextName;
        private readonly Func<ContextInfo, IEnumerable<ContextInfo>> augmentationFunc;

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

        public IEnumerable<ContextInfo> AugmentContext(ContextInfo context)
        {
            return this.augmentationFunc(context);
        }
    }
}
