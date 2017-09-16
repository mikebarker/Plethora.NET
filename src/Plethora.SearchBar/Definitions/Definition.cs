using System;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public abstract class Definition
    {
        private readonly string name;

        protected Definition([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));


            this.name = name;
        }

        [NotNull]
        public string Name
        {
            get { return this.name; }
        }
    }
}
